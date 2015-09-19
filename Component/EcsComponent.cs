using System;
using uFrame.Attributes;
using uFrame.Kernel;
using UniRx;
using UnityEngine;

namespace uFrame.ECS
{
    public class EcsComponent : uFrameComponent, IEcsComponent, IDisposableContainer
    {
        //[SerializeField]
        private int _entityId;

        private Transform _cachedTransform;
        [uFrameEventMapping("Source")]
        public virtual int EntityId
        {
            get
            {
                return _entityId;
            }
            set { 
                _entityId = value;
            }
        }

        public virtual int ComponentId
        {
            get
            {
                throw new Exception(string.Format("ComponentId is not implement on {0} component.  Make sure you override it and give it a unique integer.", this.GetType().Name));
            }
        }

        public Entity _Entity;
        private Subject<Unit> _changed;

        public void Reset()
        {
            var entityComponent = GetComponent<Entity>();
            if (entityComponent == null)
                entityComponent = gameObject.AddComponent<Entity>();
            Entity = entityComponent;
        }
        private void OnApplicationQuit()
        {
            IsQuiting = true;
        }

        public bool IsQuiting { get; set; }

        public override void KernelLoaded()
        {
            IsQuiting = false;
            base.KernelLoaded();
            if (EntityId != 0)
            {
                EcsComponentService.Instance.RegisterComponentInstance(this.GetType(),this);
                return;
            }
            if (Entity != null)
            {
                _entityId = Entity.EntityId;
            }
            else if (_entityId == 0)
            {
                _entityId = GetComponent<Entity>().EntityId;
            }
            EcsComponentService.Instance.RegisterComponentInstance(this.GetType(), this);
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            EcsComponentService.Instance.DestroyComponentInstance(this.GetType(), this);
 
        }

        /// <summary>
        /// A lazy loaded cached reference to the transform
        /// </summary>
        public Transform CachedTransform
        {
            get { return _cachedTransform ?? (_cachedTransform = transform); }
            set { _cachedTransform = value; }
        }

        public IObservable<Unit> Changed
        {
            get { return _changed ?? (_changed = new Subject<Unit>()); }
        }

        public Entity Entity
        {
            get { return _Entity ?? (_Entity = GetComponent<Entity>()); }
            set { _Entity = value; }
        }
    }
}