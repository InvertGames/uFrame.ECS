using uFrame.Attributes;
using UnityEngine;

namespace uFrame.Actions
{
    [ActionLibrary, uFrameCategory("Random")]
    public static class CreateRandoms
    {
        [ActionTitle("Random Vector3")]
        public static Vector3 RandomVector3(int minX, int maxX, int minY, int maxY, int minZ, int maxZ)
        {
            return new Vector3(
                UnityEngine.Random.Range(minX, maxX),
                UnityEngine.Random.Range(minY, maxY),
                UnityEngine.Random.Range(minZ, maxZ)
                );
        }
        [ActionTitle("Random Vector2")]
        public static Vector2 RandomVector2(int minX, int maxX, int minY, int maxY)
        {
            return new Vector2(
                UnityEngine.Random.Range(minX, maxX),
                UnityEngine.Random.Range(minY, maxY)
                );
        }
        [ActionTitle("Random Float")]
        public static float RandomFloat(float min, float max)
        {
            return UnityEngine.Random.Range(min, max);
        }
        [ActionTitle("Random Int")]
        public static int RandomInt(int min, int max)
        {
            return UnityEngine.Random.Range(min, max);
        }
        [ActionTitle("Random Bool")]
        public static bool RandomBool()
        {
            return UnityEngine.Random.Range(0, 2) == 1;
        }
    }
}