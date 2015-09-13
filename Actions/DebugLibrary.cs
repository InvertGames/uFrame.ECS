using uFrame.Attributes;

namespace uFrame.Actions
{
    [ActionLibrary, uFrameCategory("Condition", "Floats")]
    public static class DebugLibrary
    {
        [ActionTitle("Log Message")]
        public static void LogMessage(object message)
        {
            UnityEngine.Debug.Log(message);
        }
    }
}