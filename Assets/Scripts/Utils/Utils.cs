using System;
using UnityEngine;

namespace Mans
{
    public static partial class Utils
    {
        static public Vector3 Vectro3Zero = Vector3.zero;
        static public Vector2 Vectro2Zero = Vector2.zero;
        static public float SqrDistance(Vector3 v1, Vector3 v2)
        {
            return (v1 - v2).sqrMagnitude;
        }
        public static Vector3 Change(this Vector3 org, float? x = null, float? y = null, float? z = null)
        {
            return new Vector3(x == null ? org.x : (float)x, y == null ? org.y : (float)y, z == null ? org.z : (float)z);
        }

        public static Vector2 Change(this Vector2 org, object x = null, object y = null)
        {
            return new Vector2(x == null ? org.x : (float)x, y == null ? org.y : (float)y);
        }

        public static int ParseLayers(params string[] names)
        {
            int result = 0;

            foreach (string name in names)
            {
                var index = LayerMask.NameToLayer(name);
                if (index != -1)
                    result |= 1 << index;
                else
                    Debug.LogError($"Layer '{name}' does not found.");
            }

            return result;
        }
    }
}