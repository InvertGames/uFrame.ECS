using System;
using System.Collections.Generic;
using System.Linq;
using Invert.IOC;
using uFrame.Kernel;
using UniRx;
using UnityEngine;

namespace uFrame.ECS
{
    public class EcsComponentService : EcsSystem, IComponentSystem
    {
        public static EcsComponentService Instance { get; set; }

        public EcsComponentService()
        {
            Instance = this;
        }

        public override void Setup()
        {
            base.Setup();
            Instance = this;
        
        }

        public override void Loaded()
        {
            base.Loaded();
            var array = ComponentManagers.Where(p => typeof (GroupItem).IsAssignableFrom(p.Key)).ToArray();
            foreach (var item in array)
            {
                var reactiveGroup = item.Value as IReactiveGroup;
                if (reactiveGroup != null)
                {
                    foreach (var observable in reactiveGroup.Install(this))
                    {
                        observable.Subscribe(reactiveGroup.UpdateItem)
                            .DisposeWith(this);
                    }
                }
            }
        }

        private Dictionary<Type, IEcsComponentManager> _componentManager;

        public LinkedList<Component> Components { get; set; }

        public Dictionary<Type, IEcsComponentManager> ComponentManagers
        {
            get { return _componentManager ?? (_componentManager = new Dictionary<Type, IEcsComponentManager>()); }
            set { _componentManager = value; }
        }

        public IEcsComponentManager RegisterComponent(Type componentType)
        {
            IEcsComponentManager existing;
            if (!ComponentManagers.TryGetValue(componentType, out existing))
            {
                throw new Exception(string.Format("Component {0} not registered correctly.", componentType.Name));

            }
            return existing;
        }

        public void RegisterComponentInstance(Type componentType, IEcsComponent instance)
        {
            IEcsComponentManager existing;
            if (!ComponentManagers.TryGetValue(componentType, out existing))
            {
                var type = typeof(EcsComponentManagerOf<>).MakeGenericType(componentType);
                existing = Activator.CreateInstance(type) as EcsComponentManager;
                ComponentManagers.Add(componentType, existing);
            }
            existing.RegisterComponent(instance);
        }
        public void DestroyComponentInstance(Type componentType, IEcsComponent instance)
        {
            IEcsComponentManager existing;
            if (!ComponentManagers.TryGetValue(componentType, out existing))
            {
                return;
            }
            existing.UnRegisterComponent(instance);

        }

        public void AddComponent(int entityId, Type componentType)
        {
            //var entityManager = RegisterComponent<Entity>();
            //var entity = entityManager.Components.FirstOrDefault(p => p.EntityId == entityId);
            throw new Exception("Not Implemented use gameObject.AddComponent");
             
        }

        public void AddComponent<TComponentType>(int entityId) where TComponentType : class,  IEcsComponent
        {

        }

        public IEcsComponentManagerOf<TComponent> RegisterComponent<TComponent>() where TComponent : class, IEcsComponent
        {
            IEcsComponentManager existing;
            if (!ComponentManagers.TryGetValue(typeof(TComponent), out existing))
            {
                existing = new EcsComponentManagerOf<TComponent>();
                ComponentManagers.Add(typeof(TComponent), existing);
                return (IEcsComponentManagerOf<TComponent>)existing;
            }
            else
            {
                return (IEcsComponentManagerOf<TComponent>)existing;
            }

        }
        public TGroupType RegisterGroup<TGroupType, TComponent>() where TComponent : GroupItem, new() where TGroupType : ReactiveGroup<TComponent>, new()
        {
            IEcsComponentManager existing;
            if (!ComponentManagers.TryGetValue(typeof(TComponent), out existing))
            {
                existing = new TGroupType();
                ComponentManagers.Add(typeof(TComponent), existing);
                return (TGroupType)existing;
            }
            else
            {
                return (TGroupType)existing;
            }
        }

        public bool TryGetComponent<TComponent>(int[] entityIds, out TComponent[] components) where TComponent : class, IEcsComponent
        {
            var list = new List<TComponent>();
            foreach (var entityid in entityIds)
            {
                TComponent component;
                if (!TryGetComponent(entityid, out component))
                {
                    components = null;
                    return false;
                }
                list.Add(component);
            }
            components = list.ToArray();
            return true;
        }

        public bool TryGetComponent<TComponent>(List<int> entityIds, out TComponent[] components) where TComponent : class, IEcsComponent
        {
            var list = new List<TComponent>();
            foreach (var entityid in entityIds)
            {
                TComponent component;
                if (!TryGetComponent(entityid, out component))
                {
                    components = null;
                    return false;
                }
                list.Add(component);
            }
            components = list.ToArray();
            return true;
        }

        public IEnumerable<T> GetAllComponents<T>() where T : class, IEcsComponent
        {
            IEcsComponentManager existing;
            if (ComponentManagers.TryGetValue(typeof(T), out existing))
            {
                var manager = (EcsComponentManagerOf<T>)existing;
                foreach (var item in manager.Components)
                    yield return (T)item;

            }

        }

        public bool HasAny(int entityId, params Type[] types)
        {
            foreach (var type in types)
            {
                if (ComponentManagers[type].ForEntity(entityId).Any())
                {
                    return true;
                }
            }
            return false;
        }

        public bool TryGetComponent<TComponent>(int entityId, out TComponent component) where TComponent : class, IEcsComponent
        {
            IEcsComponentManager existing;
            if (!ComponentManagers.TryGetValue(typeof(TComponent), out existing))
            {
                component = null;
                return false;
            }
            var manager = (IEcsComponentManagerOf<TComponent>)existing;
            var result = manager.Components.FirstOrDefault(p => p.EntityId == entityId);
            if (result != null)
            {
                component = result;
                return true;
            }
            component = null;
            return false;
        }
    }
}