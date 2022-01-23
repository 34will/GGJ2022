using UnityEngine;

namespace GGJ2020
{
    public enum Direction
    {
        None = 0,
        Positive = 1,
        Negative = 2
    }

    public class Controller : MonoBehaviour
    {
        private float currentSpeed = 0.0f;

        private Rigidbody rigidbodyComponent;
        private Direction horizontalDirection = Direction.None;
        private float currentHorizontalTiltTarget = 0.0f;
        private float currentHorizontalTiltDuration = 0.0f;
        private float currentParticleHorizontalTiltDuration = 0.0f;
        private Direction verticalDirection = Direction.None;
        private float currentVerticalTiltTarget = 0.0f;
        private float currentVerticalTiltDuration = 0.0f;
        private float currentParticleVerticalTiltDuration = 0.0f;

        public float Speed = 10.0f;
        public float SideSpeed = 10.0f;
        public float TiltAngle = 10.0f;
        public float TiltAnimationDuration = 200.0f;
        public GameObject Particles;
        public GameObject CameraRoot;
        public Camera Camera;
        public ForceField ForceField;

        public void Start()
        {
            currentSpeed = Speed;

            rigidbodyComponent = GetComponent<Rigidbody>();
            rigidbodyComponent.velocity = new Vector3(0, 0, currentSpeed);
        }

        private float Steer(
            KeyCode positive1,
            KeyCode positive2,
            KeyCode negative1,
            KeyCode negative2,
            float value,
            ref Direction direction,
            ref float tiltTarget,
            ref float tiltDuration,
            ref float particleTiltDuration
        )
        {
            Direction newDirection = Direction.None;
            bool positive = Input.GetKey(positive1) || Input.GetKey(positive2);
            bool negative = Input.GetKey(negative1) || Input.GetKey(negative2);
            if (positive && !negative)
                newDirection = direction == Direction.Negative ? Direction.None : Direction.Positive;
            else if (!positive && negative)
                newDirection = direction == Direction.Positive || newDirection == Direction.Positive ? Direction.None : Direction.Negative;

            if (newDirection == direction)
                return value;

            float result = 0.0f;
            switch (newDirection)
            {
                case Direction.Positive:
                    result = SideSpeed;
                    tiltTarget = -TiltAngle;
                    break;
                case Direction.Negative:
                    result = -SideSpeed;
                    tiltTarget = TiltAngle;
                    break;
                case Direction.None:
                    result = 0.0f;
                    tiltTarget = 0.0f;
                    break;
            }

            tiltDuration = 0.0f;
            particleTiltDuration = 0.0f;
            direction = newDirection;
            return result;
        }

        private float HorizontalSteer()
        {
            return Steer(
                KeyCode.RightArrow,
                KeyCode.D,
                KeyCode.LeftArrow,
                KeyCode.A,
                rigidbodyComponent.velocity.x,
                ref horizontalDirection,
                ref currentHorizontalTiltTarget,
                ref currentHorizontalTiltDuration,
                ref currentParticleHorizontalTiltDuration
            );
        }

        private float VerticalSteer()
        {
            return Steer(
                KeyCode.UpArrow,
                KeyCode.W,
                KeyCode.DownArrow,
                KeyCode.S,
                rigidbodyComponent.velocity.y,
                ref verticalDirection,
                ref currentVerticalTiltTarget,
                ref currentVerticalTiltDuration,
                ref currentParticleVerticalTiltDuration
            );
        }

        private float Tilt(float start, float end, ref float duration)
        {
            if (start > 180.0f)
                start -= 360.0f;

            if (Mathf.Abs(end - start) < 0.1f)
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
                ForceField.Enable();
                // currentSpeed = currentSpeed == 0.0f ? Speed : 0.0f;
        }

        public void SetSpeed(float speed)
        {
            Speed = speed;

            if (currentSpeed > 0.0f)
                currentSpeed = Speed;
        }

        // public void ActivateParticleMode(bool fromCollision)
        // {
        //     ForceField.SetActive(true);
        // }
    }
}
