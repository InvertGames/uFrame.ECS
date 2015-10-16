using System;
using System.Collections.Generic;
using UniRx;

namespace uFrame.ECS
{
    /// <summary>
    /// Manages components of a specific type
    /// </summary>
    public interface IEcsComponentManager : IGroup
    {
        Type For { get; }
        IEnumerable<IEcsComponent> All { get; }
        void RegisterComponent(IEcsComponent item);
        void UnRegisterComponent(IEcsComponent item);
        IEnumerable<IEcsComponent> ForEntity(int entityId);

    }
}