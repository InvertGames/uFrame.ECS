using uFrame.Attributes;

namespace uFrame.ECS
{
    [SystemUFrameEvent("Update", "SystemUpdate")]
    public interface ISystemUpdate
    {
        void SystemUpdate();
    }
    [SystemUFrameEvent("Fixed Update", "SystemFixedUpdate")]
    public interface ISystemFixedUpdate
    {
        void SystemFixedUpdate();
    }
}