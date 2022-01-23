using UnityEngine;

namespace GGJ2020
{
    public class Rotater : MonoBehaviour
    {
        private float rotation = 0.0f;

        public float RotateSpeed = 0.0f;

        // Update is called once per frame
        public void Update()
        {
            if (RotateSpeed == 0.0f)
                return;

            rotation += RotateSpeed * Time.deltaTime;
            transform.rotation = Quaternion.Euler(0.0f, 0.0f, rotation);
        }
    }
}
