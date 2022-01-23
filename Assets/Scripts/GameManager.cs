using UnityEngine;

namespace GGJ2020
{
    public class GameManager : MonoBehaviour
    {
        public Controller Player;
        public Rotater TubesRotater;
        public Rotater ObstaclesRotater;
        public float InitialSpeed;
        public float SpeedIncrease;
        public float MaxSpeed;
        public float RotationSpeedIncrease;
        public float MaxRotationSpeed;

        public void Start()
        {
            Player.Speed = InitialSpeed;
        }

        public void GatePassed()
        {
            int boon = Random.Range(0, 2);
            switch (boon)
            {
                case 0:
                    float newSpeed = Player.Speed + SpeedIncrease;
                    if (newSpeed > MaxSpeed)
                        newSpeed = MaxSpeed;
                    Player.SetSpeed(newSpeed);
                    break;
                case 1:
                    float newRotateSpeed = ObstaclesRotater.RotateSpeed + RotationSpeedIncrease;
                    if (newRotateSpeed > MaxRotationSpeed)
                        newRotateSpeed = MaxRotationSpeed;
                    TubesRotater.RotateSpeed = -newRotateSpeed;
                    ObstaclesRotater.RotateSpeed = newRotateSpeed;
                    break;
            }
        }
    }
}
