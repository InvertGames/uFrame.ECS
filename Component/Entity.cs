using uFrame.Kernel;
using UnityEngine;

namespace uFrame.ECS
{
    public partial class Entity : uFrameComponent, IEcsComponent
    {
        [SerializeField]
        private int _entityId;

        public int EntityId
        {
            get
            {
                return _entityId;
            }
            set { _entityId = value; }
        }

        public int ComponentId
        {
            get { return 0; }
        }

        public override void KernelLoading()
        {
            base.KernelLoading();
            EntityService.NewId();
            if (_entityId == 0)
            {
                _entityId = EntityService.NewId();
            }
            EntityService.RegisterEntityView(this);

        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            EntityService.UnRegisterEntityView(this);
        }
    }
}