using System;
using System.Collections.Generic;
using UniRx;

namespace uFrame.ECS
{
    public abstract class EcsComponentManager : IEcsComponentManager
    {
  
        public abstract Type For { get; }
        

        public abstract IEnumerable<IEcsComponent> All { get; }

        public void RegisterComponent(IEcsComponent item)
        {
            AddItem(item);
        }

        public void UnRegisterComponent(IEcsComponent item)
        {
            RemoveItem(item);
        }

        public abstract IEnumerable<IEcsComponent> ForEntity(int entityId);


        protected abstract void AddItem(IEcsComponent component);
        protected abstract void RemoveItem(IEcsComponent component);

        public abstract bool Match(int entityId);
    }
}