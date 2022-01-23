using System.Collections.Generic;
using UnityEngine;

namespace GGJ2020
{
    public class GameManager : MonoBehaviour
    {
        private List<int> boons = null;
        private int lives;

        public UI Ui;
        public Controller Player;
        public Rotater TubesRotater;
        public Rotater ObstaclesRotater;
        public float InitialSpeed;
        public float SpeedIncrease;
        public float MaxSpeed;
        public float RotationSpeedIncrease;
        public float MaxRotationSpeed;
        public int StartLives = 3;

        public void Start()
        {
            Player.Speed = InitialSpeed;

            lives = StartLives;

            int speedChanges = (int)(MaxSpeed / SpeedIncrease);
            int rotationChanges = (int)(MaxRotationSpeed / RotationSpeedIncrease);
            boons = new List<int>(speedChanges + rotationChanges);
            for (int i = 0; i < speedChanges; i++)
                boons.Add(0);
            for (int i = 0; i < rotationChanges; i++)
                boons.Add(1);
        }

        public void GatePassed()
        {
            if (boons.Count <= 0)
                return;

            int boonIndex = Random.Range(0, boons.Count);
            int boon = boons[boonIndex];
            boons.RemoveAt(boonIndex);
            switch (boon)
            {
                case 0:
                    float newSpeed = Player.Speed + SpeedIncrease;
                    if (newSpeed > MaxSpeed)
                        newSpeed = MaxSpeed;
                    Player.SetSpeed(newSpeed);
                    Ui.UpdateSpeed(Player.Speed);
                    Ui.UpdateGates(1);

                    break;
                case 1:
                    float newRotateSpeed = ObstaclesRotater.RotateSpeed + RotationSpeedIncrease;
                    if (newRotateSpeed > MaxRotationSpeed)
                        newRotateSpeed = MaxRotationSpeed;
                    TubesRotater.RotateSpeed = -newRotateSpeed;
                    ObstaclesRotater.RotateSpeed = newRotateSpeed;
                    Ui.UpdateRotation(ObstaclesRotater.RotateSpeed);
                    Ui.UpdateGates(1);
                    break;
            }
        }
    }
}
