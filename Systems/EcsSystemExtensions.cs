using System;
using System.Collections.Generic;
using System.Linq;
using uFrame.Kernel;
using UniRx;

namespace uFrame.ECS
{
    public static class EcsSystemExtensions
    {
        public static IEnumerable<IEcsComponent> MergeByEntity(this EcsSystem system, params IEcsComponentManager[] managers)
        {
            var list = new HashSet<int>();
            foreach (var manager in managers)
            {
                foreach (var item in manager.All)
                {
                    if (list.Contains(item.EntityId)) continue;
                    yield return item;
                    list.Add(item.EntityId);
                }
            }
        }
        public static void FilterWithDispatcher<TDispatcher>(this EcsSystem system, Func<TDispatcher, int> getMatchId, Action<int> handler, params Type[] forTypes)
            where TDispatcher : EcsDispatcher
        {
            system.OnEvent<ComponentCreatedEvent>().Where(p => forTypes.Contains(p.Component.GetType()))
                .Subscribe(_ =>
                {
                    var component = _.Component as EcsComponent;
                    if (component == null) return;

                    var d = component.gameObject.GetComponent<TDispatcher>();
                    if (d != null) return;
                    var entityId = component.EntityId;
                    
                    component.gameObject
                        .AddComponent<TDispatcher>()
                        .EntityId = entityId
                        ;

                    system.OnEvent<TDispatcher>()
                        .Where(p =>getMatchId(p) == component.EntityId)
                        .Subscribe(x => handler(x.EntityId))
                        .DisposeWith(system);
                })
                .DisposeWith(system);
            ;
        }
        public static IObservable<TComponentType> OnComponentCreated<TComponentType>(this IEcsSystem system) where TComponentType : class, IEcsComponent
        {
            return system.ComponentSystem.RegisterComponent<TComponentType>().CreatedObservable;
        }
        public static IObservable<TComponentType> OnComponentDestroyed<TComponentType>(this IEcsSystem system) where TComponentType : class, IEcsComponent
        {
            return system.ComponentSystem.RegisterComponent<TComponentType>().RemovedObservable;
        }
        public static IObservable<TComponentType> OnComponentCreated<TComponentType>(this IComponentSystem system) where TComponentType : class, IEcsComponent
        {
            return system.RegisterComponent<TComponentType>().CreatedObservable;
        }
        public static IObservable<TComponentType> OnComponentDestroyed<TComponentType>(this IComponentSystem system) where TComponentType : class, IEcsComponent
        {
            return system.RegisterComponent<TComponentType>().RemovedObservable;
        }

        public static void PropertyChanged<TComponentType, TPropertyType>(this IEcsSystem system, 
            Func<TComponentType, 
            IObservable<TPropertyType>> select, 
            Action<TComponentType, 
            TPropertyType> handler, Func<TComponentType,TPropertyType> getImmediateValue = null ) where TComponentType : class, IEcsComponent
        {
            
            system.OnComponentCreated<TComponentType>().Subscribe(_ =>
            {
                select(_).Subscribe(v=>handler(_,v)).DisposeWith(_).DisposeWith(system);
                if (getImmediateValue != null)
                {
                    handler(_, getImmediateValue(_));

                }
                
            }).DisposeWith(system);
        }
    }
}