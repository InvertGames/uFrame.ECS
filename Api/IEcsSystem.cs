using System;
using System.Collections;
using uFrame.Kernel;
using UnityEngine;

namespace uFrame.ECS
{
    public interface IEcsSystem : ISystemService, IDisposableContainer
    {
        IComponentSystem ComponentSystem { get; }
        IBlackBoardSystem BlackBoardSystem { get; }
   
    }

    public interface IBlackBoardSystem
    {
        TType Get<TType>() where TType : class;
        TType EnsureBlackBoard<TType>() where TType : class, IEcsComponent;
    }
    
}