using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using uFrame.Kernel;
using UniRx;
using UnityEngine;

namespace uFrame.ECS
{
    public class BlackBoardGroup : DescriptorGroup<IBlackBoardComponent>
    {
        
    }
    public class BlackBoardSystem : EcsSystem, IBlackBoardSystem
    {
        void Awake()
        {
            // Make sure all the blackboard components are registered first.
            StartingBlackBoardComponents = uFrameKernel.Instance.gameObject.GetComponentsInChildren<IBlackBoardComponent>();
        }

        public IBlackBoardComponent[] StartingBlackBoardComponents { get; set; }

        public override void Setup()
        {
            base.Setup();
            BlackBoards = this.ComponentSystem.RegisterGroup<BlackBoardGroup, IBlackBoardComponent>();

         
        }

        public BlackBoardGroup BlackBoards { get; set; }

        public TType Get<TType>() where TType : Component
        {
            return BlackBoards.Components.OfType<TType>().FirstOrDefault() ?? StartingBlackBoardComponents.OfType<TType>().FirstOrDefault();
        }

        public TType EnsureBlackboard<TType>() where TType : Component
        {
            var result = Get<TType>();
            if (result != null)
            {
                return result;
            }
            return this.gameObject.AddComponent<TType>();
        }
    }
    public interface IBlackBoardComponent : IEcsComponent
    {
        
    }
}