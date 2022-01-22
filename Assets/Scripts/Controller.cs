using UnityEngine;

namespace GGJ2020
{
    public enum HorizontalDirection
    {
        None = 0,
        Left = 1,
        Right = 2
    }

    public enum VerticalDirection
    {
        None = 0,
        Up = 1,
        Down = 2
    }

    public class Controller : MonoBehaviour
    {
        private float currentSpeed = 0.0f;

        private Rigidbody rigidbodyComponent;
        private HorizontalDirection horizontalDirection = HorizontalDirection.None;
        private float currentHorizontalTiltTarget = 0.0f;
        private float currentHorizontalTiltDuration = 0.0f;
        private float currentParticleHorizontalTiltDuration = 0.0f;
        private VerticalDirection verticalDirection = VerticalDirection.None;
        private float currentVerticalTiltTarget = 0.0f;
        private float currentVerticalTiltDuration = 0.0f;
        private float currentParticleVerticalTiltDuration = 0.0f;

        public float Speed = 10.0f;
        public float SideSpeed = 10.0f;
        public float TiltAngle = 10.0f;
        public float TiltAnimationDuration = 200.0f;
        public GameObject Particles;
        public GameObject CameraRoot;

        public void Start()
        {
            currentSpeed = Speed;

            rigidbodyComponent = GetComponent<Rigidbody>();
            rigidbodyComponent.velocity = new Vector3(0, 0, currentSpeed);
        }

        private float HorizontalSteer()
        {
            HorizontalDirection newDirection = HorizontalDirection.None;
            bool left = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
            bool right = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
            if (left && !right)
                newDirection = horizontalDirection == HorizontalDirection.Right ? HorizontalDirection.None : HorizontalDirection.Left;
            else if (!left && right)
                newDirection = horizontalDirection == HorizontalDirection.Left || newDirection == HorizontalDirection.Left ? HorizontalDirection.None : HorizontalDirection.Right;

            if (newDirection == horizontalDirection)
                return rigidbodyComponent.velocity.x;

            float result = 0.0f;
            switch (newDirection)
            {
                case HorizontalDirection.Left:
                    result = -SideSpeed;
                    currentHorizontalTiltTarget = TiltAngle;
                    break;
                case HorizontalDirection.Right:
                    result = SideSpeed;
                    currentHorizontalTiltTarget = -TiltAngle;
                    break;
                case HorizontalDirection.None:
                    result = 0.0f;
                    currentHorizontalTiltTarget = 0.0f;
                    break;
            }

            currentHorizontalTiltDuration = 0.0f;
            currentParticleHorizontalTiltDuration = 0.0f;
            horizontalDirection = newDirection;
            return result;
        }

        private float VerticalSteer()
        {
            VerticalDirection newDirection = VerticalDirection.None;
            bool up = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
            bool down = Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);
            if (up && !down)
                newDirection = verticalDirection == VerticalDirection.Down ? VerticalDirection.None : VerticalDirection.Up;
            else if (!up && down)
                newDirection = verticalDirection == VerticalDirection.Up || newDirection == VerticalDirection.Up ? VerticalDirection.None : VerticalDirection.Down;

            if (newDirection == verticalDirection)
                return rigidbodyComponent.velocity.y;

            float result = 0.0f;
            switch (newDirection)
            {
                case VerticalDirection.Up:
                    result = SideSpeed;
                    currentVerticalTiltTarget = -TiltAngle;
                    break;
                case VerticalDirection.Down:
                    result = -SideSpeed;
                    currentVerticalTiltTarget = TiltAngle;
                    break;
                case VerticalDirection.None:
                    result = 0.0f;
                    currentVerticalTiltTarget = 0.0f;
                    break;
            }

            currentVerticalTiltDuration = 0.0f;
            currentParticleVerticalTiltDuration = 0.0f;
            verticalDirection = newDirection;
            return result;
        }

        private float Tilt(float start, float end, ref float duration)
        {
            if (start > 180.0f)
                start -= 360.0f;

            if (Mathf.Abs(end - start) < 0.5f)
                return end;

            duration += Time.deltaTime;
            if (duration > TiltAnimationDuration)
                duration = TiltAnimationDuration;
            float interpolationPoint = duration / TiltAnimationDuration;
            
            return Mathf.LerpAngle(start, end, interpolationPoint);
        }

        public void Update()
        {
            float xVelocity = HorizontalSteer();
            float yVelocity = VerticalSteer();
            rigidbodyComponent.velocity = new Vector3(xVelocity, yVelocity, currentSpeed);

            Vector3 eulerAngles = CameraRoot.transform.localRotation.eulerAngles;
            float zRot = Tilt(eulerAngles.z, currentHorizontalTiltTarget, ref currentHorizontalTiltDuration);
            float xRot = Tilt(eulerAngles.x, currentVerticalTiltTarget, ref currentVerticalTiltDuration);

            CameraRoot.transform.localRotation = Quaternion.Euler(xRot, 0, zRot);

            Vector3 particlesEulerAngles = Particles.transform.localRotation.eulerAngles;
            float xParticlesRot = Tilt(particlesEulerAngles.x, 5 * currentVerticalTiltTarget, ref currentParticleVerticalTiltDuration);
            float yParticlesRot = Tilt(particlesEulerAngles.y, -5 * currentHorizontalTiltTarget, ref currentParticleHorizontalTiltDuration);
            Particles.transform.localRotation = Quaternion.Euler(xParticlesRot, yParticlesRot, particlesEulerAngles.z);

            if (Input.GetKeyDown(KeyCode.Space))
                currentSpeed = currentSpeed == 0.0f ? Speed : 0.0f;
        }
    }
}
