using System;
using uFrame.Attributes;
using uFrame.Kernel;
using UniRx;
using UnityEngine;

namespace uFrame.ECS
{
    /// <summary>
    /// The base class for all ECS components, these components are nothing more than just data.  
    /// For the sake of Unity Compatability, it listens for a few Unity messages to make sure the ecs component system is always updated.
    /// </summary>
    [RequireComponent(typeof(Entity))]
    public class EcsComponent : uFrameComponent, IEcsComponent, IDisposableContainer
    {
        //[SerializeField]
        private int _entityId;

        private Transform _cachedTransform;

        /// <summary>
        /// The id for the entity that this component belongs to.  This id belongs to the IEcsComponent interface and is used
        /// for matching under the hood.
        /// </summary>
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

        /// <summary>
        /// Is this component enabled
        /// </summary>
        public bool Enabled
        {
            get
            {
                return this.enabled;
            }
            set { this.enabled = value; }
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

        /// <summary>
        /// A bool variable to determine if the application is quiting, useful in some situations.
        /// </summary>
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

        /// <summary>
        /// The actual entity component that this component belongs to.
        /// </summary>
        public Entity Entity
        {
            get { return _Entity ?? (_Entity = GetComponent<Entity>()); }
            set { _Entity = value; }
        }


        public void SetProperty<TValue>(ref TValue valueField, TValue value, ref PropertyChangedEvent<TValue> cachedEvent, Subject<PropertyChangedEvent<TValue>> observable = null)
        {
            var previousValue = valueField;
            valueField = value;
            if (observable != null)
            {
                if (cachedEvent == null)
                    cachedEvent = new PropertyChangedEvent<TValue>();

                cachedEvent.PreviousValue = previousValue;
                cachedEvent.CurrentValue = valueField;
                observable.OnNext(cachedEvent);
            }
        }
    }

    public class PropertyChangedEvent<TValue>
    {
        public TValue PreviousValue;
        public TValue CurrentValue;
    }

    public class TestComponent : EcsComponent
    {

        public IObservable<PropertyChangedEvent<Vector3>> OffsetObservable
        {
            get
            {
                if (_OffsetObservable == null)
                {
                    _OffsetObservable = new Subject<PropertyChangedEvent<Vector3>>();
                }
                return _OffsetObservable;
            }
        }

        public Vector3 Offset
        {
            get
            {
                return _Offset;
            }
            set
            {
                SetProperty(ref _Offset,value,ref _OffsetEvent,_OffsetObservable);
            }
        }

        private Subject<PropertyChangedEvent<Vector3>> _OffsetObservable;

        [UnityEngine.SerializeField()]
        private Vector3 _Offset;

        private PropertyChangedEvent<Vector3> _OffsetEvent;
    }

}