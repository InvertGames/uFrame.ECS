using System.Collections;
using uFrame.Kernel;

namespace uFrame.ECS
{
    public interface IEcsSystem : ISystemService, IDisposableContainer
    {
        IComponentSystem ComponentSystem { get; }
   
    }
}