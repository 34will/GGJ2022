using UnityEngine;

namespace GGJ2020
{
    [RequireComponent(typeof(MeshRenderer))]
    public class Flicker : MonoBehaviour
    {
        private float timer;

        public MeshRenderer MeshRenderer;
        public float Duration = 0.1f;
        public bool DefaultEnabled = true;

        public void Start()
        {
            MeshRenderer = GetComponent<MeshRenderer>();
            MeshRenderer.enabled = DefaultEnabled;
        }

        public void Update()
        {
            timer += Time.deltaTime;
            if (timer < Duration)
                return;

            while (timer >= Duration)
            {
                timer -= Duration;
                MeshRenderer.enabled = !MeshRenderer.enabled;
            }
        }

        public void Enable()
        {
            MeshRenderer.enabled = DefaultEnabled;
            enabled = true;
        }

        public void Disable()
        {
            MeshRenderer.enabled = DefaultEnabled;
            enabled = false;
        }
    }
}
