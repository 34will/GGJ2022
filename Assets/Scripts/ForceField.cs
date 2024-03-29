using UnityEngine;

namespace GGJ2020
{
    public enum State
    {
        None = 0,
        Solid = 1,
        Flash = 2,
        HyperFlash = 3
    }

    [RequireComponent(typeof(Flicker))]
    [RequireComponent(typeof(Collider))]
    public class ForceField : MonoBehaviour
    {
        private State state = State.None;
        private float timer = 0.0f;
        private float marker = 0.0f;
        private Flicker flicker;

        public float SolidDuration = 3.0f;
        public float FlashDuration = 2.0f;
        public float HyperFlashDuration = 2.0f;
        public float FlashSpeed = 0.1f;
        public float HyperFlashSpeed = 0.05f;
        public Controller Controller;

        public void Start()
        {
            flicker = GetComponent<Flicker>();
        }

        public void OnTriggerEnter(Collider collider)
        {
            if (state == State.None || collider.tag != "Obstacle")
                return;

            Obstacle obstacle = collider.GetComponent<Obstacle>();
            if (obstacle == null)
                return;

            obstacle.Collide();
        }

        public void Enable()
        {
            if (state != State.None)
                return;

            timer = 0.0f;
            marker = SolidDuration;
            flicker.enabled = false;
            flicker.MeshRenderer.enabled = true;
            state = State.Solid;
            Controller.IgnoringCollisions = true;
        }

        public void Disable()
        {
            flicker.enabled = false;
            flicker.MeshRenderer.enabled = false;
            state = State.None;
            Controller.IgnoringCollisions = false;
        }

        public void Update()
        {
            if (state == State.None)
                return;

            timer += Time.deltaTime;
            if (timer < marker)
                return;

            while (timer >= marker)
            {
                timer -= marker;

                if (state == State.Solid)
                {
                    marker = FlashDuration;
                    flicker.Duration = FlashSpeed;
                    flicker.enabled = true;
                    state = State.Flash;
                }
                else if (state == State.Flash)
                {
                    marker = HyperFlashDuration;
                    flicker.Duration = HyperFlashSpeed;
                    state = State.HyperFlash;
                }
                else if (state == State.HyperFlash)
                    Disable();
            }
        }
    }
}
