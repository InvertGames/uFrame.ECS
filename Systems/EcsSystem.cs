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
    public abstract partial class EcsSystem : SystemServiceMonoBehavior, IEcsSystem
    {
        [Inject]
        public IComponentSystem ComponentSystem { get; set; }

        public override void Setup()
        {
            base.Setup();
            //ComponentSystem.RegisterGroup<PlayerGroup>();
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