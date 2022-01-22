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
        private Rigidbody rigidbodyComponent;
        private HorizontalDirection horizontalDirection = HorizontalDirection.None;
        private float currentHorizontalTiltTarget = 0.0f;
        private float currentHorizontalTiltDuration = 0.0f;
        private VerticalDirection verticalDirection = VerticalDirection.None;
        private float currentVerticalTiltTarget = 0.0f;
        private float currentVerticalTiltDuration = 0.0f;

        public float InitialForce = 10.0f;
        public float SideForce = 10.0f;
        public float TiltAngle = 10.0f;
        public float TiltAnimationDuration = 200.0f;

        private void Start()
        {
            rigidbodyComponent = GetComponent<Rigidbody>();
            rigidbodyComponent.AddForce(new Vector3(0, 0, InitialForce), ForceMode.Impulse);
        }

        private void HorizontalSteer()
        {
            HorizontalDirection newDirection = HorizontalDirection.None;
            bool left = Input.GetKey(KeyCode.LeftArrow) || Input.GetKey(KeyCode.A);
            bool right = Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.D);
            if (left && !right)
                newDirection = horizontalDirection == HorizontalDirection.Right ? HorizontalDirection.None : HorizontalDirection.Left;
            else if (!left && right)
                newDirection = horizontalDirection == HorizontalDirection.Left || newDirection == HorizontalDirection.Left ? HorizontalDirection.None : HorizontalDirection.Right;

            if (newDirection == horizontalDirection)
                return;

            switch (newDirection)
            {
                case HorizontalDirection.Left:
                    rigidbodyComponent.AddForce(new Vector3(-SideForce, 0, 0), ForceMode.Impulse);
                    currentHorizontalTiltTarget = TiltAngle;
                    break;
                case HorizontalDirection.Right:
                    rigidbodyComponent.AddForce(new Vector3(SideForce, 0, 0), ForceMode.Impulse);
                    currentHorizontalTiltTarget = -TiltAngle;
                    break;
                case HorizontalDirection.None:
                    switch (horizontalDirection)
                    {
                        case HorizontalDirection.Left:
                            rigidbodyComponent.AddForce(new Vector3(SideForce, 0, 0), ForceMode.Impulse);
                            break;
                        case HorizontalDirection.Right:
                            rigidbodyComponent.AddForce(new Vector3(-SideForce, 0, 0), ForceMode.Impulse);
                            break;
                    }
                    currentHorizontalTiltTarget = 0.0f;
                    break;
            }

            currentHorizontalTiltDuration = 0.0f;
            horizontalDirection = newDirection;
        }

        private void VerticalSteer()
        {
            VerticalDirection newDirection = VerticalDirection.None;
            bool up = Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.W);
            bool down = Input.GetKey(KeyCode.DownArrow) || Input.GetKey(KeyCode.S);
            if (up && !down)
                newDirection = verticalDirection == VerticalDirection.Down ? VerticalDirection.None : VerticalDirection.Up;
            else if (!up && down)
                newDirection = verticalDirection == VerticalDirection.Up || newDirection == VerticalDirection.Up ? VerticalDirection.None : VerticalDirection.Down;

            if (newDirection == verticalDirection)
                return;

            switch (newDirection)
            {
                case VerticalDirection.Up:
                    rigidbodyComponent.AddForce(new Vector3(0, SideForce, 0), ForceMode.Impulse);
                    currentVerticalTiltTarget = -TiltAngle;
                    break;
                case VerticalDirection.Down:
                    rigidbodyComponent.AddForce(new Vector3(0, -SideForce, 0), ForceMode.Impulse);
                    currentVerticalTiltTarget = TiltAngle;
                    break;
                case VerticalDirection.None:
                    switch (verticalDirection)
                    {
                        case VerticalDirection.Up:
                            rigidbodyComponent.AddForce(new Vector3(0, -SideForce, 0), ForceMode.Impulse);
                            break;
                        case VerticalDirection.Down:
                            rigidbodyComponent.AddForce(new Vector3(0, SideForce, 0), ForceMode.Impulse);
                            break;
                    }
                    currentVerticalTiltTarget = 0.0f;
                    break;
            }

            currentVerticalTiltDuration = 0.0f;
            verticalDirection = newDirection;
        }

        private float HorizontalTilt()
        {
            float currentTilt = transform.rotation.eulerAngles.z;
            if (Mathf.Approximately(currentHorizontalTiltTarget, currentTilt))
                return currentTilt;

            currentHorizontalTiltDuration += Time.deltaTime;
            if (currentHorizontalTiltDuration >= TiltAnimationDuration)
                currentHorizontalTiltDuration = TiltAnimationDuration;
            float interpolationPoint = currentHorizontalTiltDuration / TiltAnimationDuration;
            
            return Mathf.LerpAngle(transform.rotation.eulerAngles.z, currentHorizontalTiltTarget, interpolationPoint);
        }

        private float VerticalTilt()
        {
            float currentTilt = transform.rotation.eulerAngles.x;
            if (Mathf.Approximately(currentVerticalTiltTarget, currentTilt))
                return currentTilt;

            currentVerticalTiltDuration += Time.deltaTime;
            if (currentVerticalTiltDuration >= TiltAnimationDuration)
                currentVerticalTiltDuration = TiltAnimationDuration;
            float interpolationPoint = currentVerticalTiltDuration / TiltAnimationDuration;
            
            return Mathf.LerpAngle(transform.rotation.eulerAngles.x, currentVerticalTiltTarget, interpolationPoint);
        }

        private void Update()
        {
            HorizontalSteer();
            VerticalSteer();

            float zRot = HorizontalTilt();
            float xRot = VerticalTilt();
            transform.rotation = Quaternion.Euler(xRot, 0, zRot);
        }
    }
}
