using UnityEngine;

namespace GGJ2020
{
    public static class Vector3Helper
    {
        public static Vector3 Change(this Vector3 vector3, float? x = null, float? y = null, float? z = null)
        {
            if (x == null && y == null && z == null)
                return vector3;

            float newX = x ?? vector3.x;
            float newY = y ?? vector3.y;
            float newZ = z ?? vector3.z;
            return new Vector3(newX, newY, newZ);
        }
    }
}
