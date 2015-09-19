using System;
using System.Collections;
using uFrame.Actions.Attributes;
using uFrame.Attributes;
using uFrame.ECS;
using UnityEngine;

namespace uFrame.Actions
{
    [ActionLibrary, uFrameCategory("Condition")]
    public static class Comparisons
    {
        [ActionTitle("Is True")]
        public static void IsTrue(bool value, Action yes, Action no)
        {
            if (value)
            {
                if (yes != null) yes();

            }
                
                else
                {
                    if (no != null) no();
                }
        }
        [ActionTitle("Compare Floats")]
        public static bool CompareFloats(float a, float b)
        {
            return a == b;
        }
    
        [ActionTitle("Less Than")]
        public static bool LessThan(float a, float b, Action yes, Action no)
        {
            if (a < b)
            {
                if (yes != null) yes();
                return true;
            }
            else
            {
                if (no != null) no();
            }
          
            return false;
        }

        [ActionTitle("Less Than Or Equal")]
        public static bool LessThanOrEqual(float a, float b, Action yes, Action no)
        {
            if (a <= b)
            {
                if (yes != null) yes();
                return true;
            }
            else
            {
                if (no != null) no();
            }
           
            return false;
        }

        [ActionTitle("Greater Than")]
        public static bool GreaterThan(float a, float b, Action yes, Action no)
        {
            if (a > b)
            {
                if (yes != null) yes();
                return true;
            }
            else
            {
                if (no != null) no();
            }
            
            return false;
        }

        [ActionTitle("Greater Than Or Equal")]
        public static bool GreaterThanOrEqual(float a, float b, Action yes, Action no)
        {
            if (a >= b)
            {
                if (yes != null) yes();
                return true;
            }
            else
            {
                if (no != null) no();
            }
        
            return false;
        }

        [ActionTitle("Equal")]
        public static bool AreEqual(object a, object b, Action yes, Action no)
        {
            var result = false;
            if ((a == null || b == null))
            {
                result = a == b;
                if (yes != null) yes();
            }
            else
            {
                if (a.Equals(b))
                {
                    if (yes != null) yes();
                }
                else
                {
                    if (no != null)
                    {
                        no();
                    }
                }
            }

            return result;
        }

    }

    [ActionLibrary, uFrameCategory("Components")]
    public static class UnityLibrary
    {
  
        [ActionTitle("Get Unity Component")]
        public static Type GetUnityComponent<Type>(GameObject go, MonoBehaviour component)
        {
            if (component == null)
                return go.GetComponent<Type>();
            return component.GetComponent<Type>();
        }
        [ActionTitle("Get Rigidbody")]
        public static Rigidbody GetRigidbody(GameObject go, MonoBehaviour component)
        {
            return GetUnityComponent<Rigidbody>(go, component);
        }
        [ActionTitle("Get Rigidbody2D")]
        public static Rigidbody2D GetRigidbody2D(GameObject go, MonoBehaviour component)
        {
            return GetUnityComponent<Rigidbody2D>(go, component);
        }
        [ActionTitle("Get Collider 2D")]
        public static Collider2D GetCollider2D(GameObject go, MonoBehaviour component)
        {
            return GetUnityComponent<Collider2D>(go, component);
        }
        [ActionTitle("Get Collider")]
        public static Collider GetCollider(GameObject go, MonoBehaviour component)
        {
            return GetUnityComponent<Collider>(go, component);
        }
        [ActionTitle("Get Camera")]
        public static Camera GetCamera(GameObject go, MonoBehaviour component)
        {
            return GetUnityComponent<Camera>(go, component);
        }
        [ActionTitle("Get Main Camera")]
        public static Camera GetMainCamera()
        {
            return Camera.main;
        }
    }
    [ActionLibrary, uFrameCategory("Loops")]
    public static class LoopsLibrary
    {
        [ActionTitle("Loop Collection")]
        public static void LoopCollection(IList list, out object item, Action next)
        {
            item = null;
            for (var i = 0; i < list.Count; i++)
            {
                item = list[i];
                next();
            } 
        }
    }

    [ActionLibrary, uFrameCategory("Input")]
    public static class InputLibrary
    {
        [ActionTitle("Is Key Down")]
        public static bool IsKeyDown(KeyCode key, Action yes, Action no)
        {
            var result = Input.GetKeyDown(key);
            if (result)
            {
                if (yes != null) yes();
            }
            else
            {
                if (no != null)
                    no();
            }
            return result;
        }
        [ActionTitle("Is Key")]
        public static bool IsKey(KeyCode key, Action yes, Action no)
        {
            var result = Input.GetKey(key);
            if (result)
            {
                if (yes != null) yes();
                
            }
               
                else
                {
                    if (no != null)
                        no();
                }
            return Input.GetKey(key);
        }
        [ActionTitle("Is Key Up")]
        public static bool IsKeyUp(KeyCode key, Action yes, Action no)
        {
            var result = Input.GetKeyUp(key);
            if (result)
            {
                if (yes != null) yes();
            }
                
                else
                {
                    if (no != null)
                        no();
                }
            return result;
        }
    }

    [ActionLibrary, uFrameCategory("Rigidbody")]
    public static class RigidbodyLibrary
    {
        [ActionTitle("Set Velocity")]
        public static void SetVelocity(Rigidbody rigidBody, float x, float y, float z)
        {
            rigidBody.velocity = new Vector3(x, y, z);
        }
        [ActionTitle("Set Velocity With Speed")]
        public static void SetVelocityWithSpeed(Rigidbody rigidBody, float x, float y, float z, float speed)
        {
            rigidBody.velocity = new Vector3(x, y, z) * speed;
        }
        [ActionTitle("Set Rigidbody Position")]
        public static void SetRigidbodyPosition(Rigidbody rigidBody, float x, float y, float z)
        {
            rigidBody.position = new Vector3(x, y, z);
        }
        [ActionTitle("Set Rigidbody Rotation")]
        public static void SetRigidbodyRotation(Rigidbody rigidBody, float x, float y, float z)
        {
            rigidBody.rotation = Quaternion.Euler(x, y, z);
        }
    }

    [ActionLibrary, uFrameCategory("Time")]
    public static class TimeLibrary
    {
        [ActionTitle("Get Time")]
        public static float GetTime()
        {
            return Time.time;
        }
        [ActionTitle("Get Delta Time")]
        public static float GetDeltaTime()
        {
            return Time.deltaTime;
        }
        [ActionTitle("Get Fixed Time")]
        public static float GetFixedTime()
        {
            return Time.fixedTime;
        }
        [ActionTitle("Get Fixed Delta Time")]
        public static float GetFixedDeltaTime()
        {
            return Time.fixedDeltaTime;
        }
    }
    


}



namespace uFrame.Actions.Attributes
{



}