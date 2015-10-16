using System;
using System.Collections;
using System.Linq;
using System.Runtime.CompilerServices;
using Invert.IOC;
using uFrame.Kernel;
using UniRx;
using UniRx.Triggers;
using UnityEngine;

namespace uFrame.ECS
{
    /// <summary>
    /// This is the base class for all systems.  It derives from SystemServiceMonoBehaviour which is part of the uFrame Kernel
    /// </summary>
    public abstract partial class EcsSystem : SystemServiceMonoBehavior, IEcsSystem
    {
        /// <summary>
        /// Access to the component system.  Use this to get/register components or groups.
        /// </summary>
        [Inject]
        public IComponentSystem ComponentSystem { get; set; }

        /// <summary>
        /// The setup method is used to register groups/components, and setup event listeners using EventAggregator
        /// </summary>
        /// <code>
        ///public override void Setup() {
        ///        base.Setup();
        ///        EnemyAIManager = ComponentSystem.RegisterComponent-EnemyAI-();
        ///        RandomRotationManager = ComponentSystem.RegisterComponent-RandomRotation-();
        ///        ProjectileManager = ComponentSystem.RegisterComponent-Projectile-();
        ///        SpawnWithRandomXManager = ComponentSystem.RegisterComponent-SpawnWithRandomX-();
        ///        DestroyOnCollisionManager = ComponentSystem.RegisterComponen-DestroyOnCollision-();
        ///        this.OnEvent-uFrame.ECS.OnTriggerEnterDispatcher-().Subscribe(_=>{ HandleDestroyOnCollisionFilter(_); }).DisposeWith(this);
        ///        RandomRotationManager.CreatedObservable.Subscribe(BeginRandomRotationComponentCreatedFilter).DisposeWith(this);
        ///        ProjectileManager.CreatedObservable.Subscribe(ProjectileCreatedComponentCreatedFilter).DisposeWith(this);
        ///        SpawnWithRandomXManager.CreatedObservable.Subscribe(SetRandomPositionComponentCreatedFilter).DisposeWith(this);
        ///        this.OnEvent-uFrame.ECS.OnCollisionEnterDispatcher-().Subscribe(_=>{ HazardSystemOnCollisionEnterDispatcherFilter(_); }).DisposeWith(this);
        ///}
        /// </code>
        public override void Setup()
        {
            base.Setup();
            //ComponentSystem.RegisterGroup<PlayerGroup>();
        }
        /// <summary>
        /// Invoked when all EcsSystem setup methods have been invoked
        /// </summary>
        public override void Loaded()
        {
            base.Loaded();
        }

        public void EnsureDispatcherOnComponents<TDispatcher>(params Type[] forTypes) where TDispatcher : EcsDispatcher
        {
            this.OnEvent<ComponentCreatedEvent>().Where(p => forTypes.Contains(p.Component.GetType()))
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
                })
                .DisposeWith(this);
        }

        public void ExecuteRoutine(IEnumerator playerLoopActionContinue)
        {
            throw new NotImplementedException();
        }
    }
}