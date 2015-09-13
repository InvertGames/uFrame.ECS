using System;
using uFrame.Attributes;
using uFrame.ECS;
using UnityEngine;

namespace uFrame.Actions
{
    [ActionLibrary, uFrameCategory("Destroy", "Component", "Entity")]
    public static class DestroyLibrary
    {
        [ActionTitle("Destroy Component")]
        public static void DestroyComponent(MonoBehaviour behaviour)
        {
            UnityEngine.Object.Destroy(behaviour);
        }
        [ActionTitle("Destroy Entity")]
        public static void DestroyEntity(int entityId)
        {
            UnityEngine.Object.Destroy(EntityService.GetEntityView(entityId).gameObject);
        }

        [ActionTitle("Destroy Timer")]
        public static void DestroyTimer(IDisposable timer)
        {
            timer.Dispose();
        }
    }
}