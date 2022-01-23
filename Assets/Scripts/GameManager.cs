using System.Collections.Generic;
using UnityEngine;

namespace GGJ2020
{
    public class GameManager : MonoBehaviour
    {
        private List<int> boons = null;
        private float startZ = 0.0f;
        private int gates = 0;

        public UI Ui;
        public Controller Player;
        public Rotater TubesRotater;
        public Rotater ObstaclesRotater;
        public float InitialSpeed;
        public float SpeedIncrease;
        public float MaxSpeed;
        public float RotationSpeedIncrease;
        public float MaxRotationSpeed;
        public int ForceFieldUses = 0;
        public int StartForceFieldUses = 3;
        public int ObstaclesHit = 0;
        public int Lives = 0;
        public int StartLives = 3;

        public void Start()
        {
            Player.Speed = InitialSpeed;

            Lives = StartLives;
            ForceFieldUses = StartForceFieldUses;

            int speedChanges = (int)(MaxSpeed / SpeedIncrease);
            int rotationChanges = (int)(MaxRotationSpeed / RotationSpeedIncrease);
            boons = new List<int>(speedChanges + rotationChanges);
            for (int i = 0; i < speedChanges; i++)
                boons.Add(0);
            for (int i = 0; i < rotationChanges; i++)
                boons.Add(1);

            Ui.UpdateLives(Lives, StartLives);
            Ui.UpdateObstaclesHit(ObstaclesHit);
            Ui.UpdateForceFieldUsages(ForceFieldUses);

            startZ = Player.transform.position.z;
            UpdateDistance();
            Ui.UpdateGates(gates);
            Ui.UpdateSpeed(Player.Speed);
            Ui.UpdateRotation(ObstaclesRotater.RotateSpeed);
        }

        public void Update()
        {
            UpdateDistance();
        }

        private void UpdateDistance()
        {
            Ui.UpdateDistance(Player.transform.position.z - startZ);
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

                    break;
                case 1:
                    float newRotateSpeed = ObstaclesRotater.RotateSpeed + RotationSpeedIncrease;
                    if (newRotateSpeed > MaxRotationSpeed)
                        newRotateSpeed = MaxRotationSpeed;
                    TubesRotater.RotateSpeed = -newRotateSpeed;
                    ObstaclesRotater.RotateSpeed = newRotateSpeed;
                    Ui.UpdateRotation(ObstaclesRotater.RotateSpeed);
                    break;
            }

            gates++;
            Ui.UpdateGates(gates);
            GiveForceField(1);
        }

        public void LifeLost()
        {
            Lives--;
            Ui.UpdateLives(Lives, StartLives);
        }

        public void ObstacleHit()
        {
            ObstaclesHit++;
            Ui.UpdateObstaclesHit(ObstaclesHit);
        }

        public bool UseForceField()
        {
            if (ForceFieldUses <= 0)
            {
                ForceFieldUses = 0;
                return false;
            }

            ForceFieldUses--;
            Ui.UpdateForceFieldUsages(ForceFieldUses);
            return true;
        }

        public void GiveForceField(int amount = 1)
        {
            ForceFieldUses += amount;
            Ui.UpdateForceFieldUsages(ForceFieldUses);
        }
    }
}
