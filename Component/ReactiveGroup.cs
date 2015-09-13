using System.Collections.Generic;
using System.Linq;
using UniRx;

namespace uFrame.ECS
{
    public class ReactiveGroup<TContextItem> : EcsComponentManagerOf<TContextItem>, IReactiveGroup where TContextItem : GroupItem, new()
    {
        //readonly Dictionary<int, TContextItem> _components = new Dictionary<int, TContextItem>(); 

        public TContextItem MatchAndSelect(int entityId)
        {
            if (_components.ContainsKey(entityId))
            {
                return _components[entityId].FirstOrDefault();
            }
            return null;
        }

        public IEnumerable<TContextItem> Items
        {
            get { return _components.Values.Select(p=>p.FirstOrDefault()); }
        }

        /// <summary>
        /// This method is used to determine when to check that a group item is still valid. It should also initially store any component managers needed for matching.
        /// Ex. If a HealthComponent belongs to a PlayerGroup then it should return ComponentSystem.RegisterComponent'HealthComponent'.CreatedObservable.Select(p=>p.EntityId)
        /// </summary>
        /// <param name="ecsComponentService"></param>
        /// <returns></returns>
        public virtual IEnumerable<IObservable<int>> Install(IComponentSystem ecsComponentService)
        {
            yield break;
        }

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

        public virtual TContextItem Select()
        {
            return new TContextItem();
        }
    }
}