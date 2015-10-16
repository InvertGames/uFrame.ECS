using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace uFrame.ECS
{
    /// <summary>
    /// Reactive Group is the base class of all group type components in ECS.
    /// </summary>
    /// <typeparam name="TContextItem"></typeparam>
    public class ReactiveGroup<TContextItem> : EcsComponentManagerOf<TContextItem>, IReactiveGroup where TContextItem : GroupItem, new()
    {
        /// <summary>
        /// Does the given entity match this group filter, if so it will return the group item, otherwise, it will return null.
        /// </summary>
        /// <param name="entityId"></param>
        /// <returns></returns>
        public TContextItem MatchAndSelect(int entityId)
        {
            if (_components.ContainsKey(entityId))
            {
                return _components[entityId].FirstOrDefault();
            }
            return null;
        }

        /// <summary>
        /// This method is used to determine **when** to check that a entity still belongs to this group. It should also initially store any component managers needed for matching.
        /// Ex. If a HealthComponent belongs to a PlayerGroup then it should return ComponentSystem.RegisterComponent'HealthComponent'.CreatedObservable.Select(p=>p.EntityId)
        /// 
        /// This method is invoked from the EcsComponentService after all groups have been registered.
        /// </summary>
        /// <param name="ecsComponentService">The component system that is intalling this reactive-group.</param>
        /// <returns></returns>
        public virtual IEnumerable<IObservable<int>> Install(IComponentSystem ecsComponentService)
        {
            yield break;
        }

        /// <summary>
        /// Determine's wether or not an entity still belongs to this component or not and adjusts the list accordingly.
        /// </summary>
        /// <param name="entityId"></param>
        public void UpdateItem(int entityId)
        {
            if (Match(entityId))
            {
                var item = Select();
                AddItem(item);
            }
            else
            {
                if (_components.ContainsKey(entityId))
                    this.RemoveItem(_components[entityId].FirstOrDefault());
            }
        }

        /// <summary>
        /// Selects the last successfully matched item.
        /// </summary>
        /// <returns></returns>
        public virtual TContextItem Select()
        {
            return new TContextItem();
        }
    }
}