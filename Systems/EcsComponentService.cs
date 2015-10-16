using System;
using System.Collections.Generic;
using System.Linq;
using Invert.IOC;
using uFrame.Kernel;
using UniRx;
using UnityEngine;

namespace uFrame.ECS
{
    /// <summary>
    /// The main component service used to register and manage all components and groups.
    /// </summary>
    /// <code>
    /// // If you are inside of a system you can access this via 
    /// this.ComponentSystem
    /// // If you are inside of a code handler, you can access it via 
    /// System.ComponentSystem
    /// //If you are anywhere else you can access via 
    /// EcsComponentService.Instance
    /// </code>

    public class EcsComponentService : EcsSystem, IComponentSystem
    {
        /// <summary>
        /// The singleton instance property, this can be accessed from anywhere.
        /// </summary>
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

        /// <summary>
        /// Registers a component type with the component system.
        /// </summary>
        /// <param name="componentType">The type of component to register</param>
        /// <returns></returns>
        /// <code>
        /// var componentManager = System.ComponentSystem.RegisterComponent(typeof(PlayerComponent));
        /// foreach (var item in componentManager) {
        ///     Debug.Log(item.name);
        /// }
        /// </code>
        public IEcsComponentManager RegisterComponent(Type componentType)
        {
            IEcsComponentManager existing;
            if (!ComponentManagers.TryGetValue(componentType, out existing))
            {
                throw new Exception(string.Format("Component {0} not registered correctly.", componentType.Name));

            }
            return existing;
        }

        /// <summary>
        /// This method should be used to add any entity to the ecs component system
        /// 
        /// > You can use this if you want to register components that aren't derived from EcsComponent which requires MonoBehaviour, but you won't be able to see it in the unity inspector.
        /// </summary>
        /// <param name="componentType">The type of component to register.</param>
        /// <param name="instance">The actual instance that is being registered</param>
        /// <code>
        /// System.ComponentSystem.RegisterComponent(typeof(CustomComponent), new CustomComponent());
        /// </code>
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


        /// <summary>
        /// Registers a a reactive group with the list of managers.  If the group already exists it will return it, if not it will create a new one and return that.
        /// </summary>
        /// <typeparam name="TGroupType">The group type class. Usually derives from ReactiveGroup </typeparam>
        /// <typeparam name="TComponent"></typeparam>
        /// <returns>The instance of the group manager.</returns>
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