using UnityEngine;

namespace GGJ2020
{
    public class Tube : MonoBehaviour
    {
        public Renderer Renderer;

        public float GetLength()
        {
            if (Renderer == null)
                return 0.0f;

            Vector3 bounds = Renderer.bounds.size;
            return Mathf.Max(Mathf.Max(bounds.x, bounds.y), bounds.z);
        }
    }
}
