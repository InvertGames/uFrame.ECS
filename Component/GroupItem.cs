using System;
using UniRx;

namespace uFrame.ECS
{
    public class GroupItem : IEcsComponent
    {
        private Entity _entityView;
        private CompositeDisposable _disposer;

        public int EntityId { get; set; }

        public int ComponentId
        {
            get { throw new NotImplementedException(); }
            set { throw new NotImplementedException(); }
        }

        public Entity Entity
        {
            get { return _entityView ?? (_entityView = EntityService.GetEntityView(EntityId)); }
        }

        public CompositeDisposable Disposer
        {
            get { return _disposer ?? (_disposer = new CompositeDisposable()); }
            set { _disposer = value; }
        }
    }
}