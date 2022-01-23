using System.Collections.Generic;
using UnityEngine;
using Exception = System.Exception;

namespace GGJ2020
{
    public class ObstaclesGenerator : MonoBehaviour
    {
        private List<Obstacle> instances;
        private float maxDistance;
        private float cameraOffset;

        public UI Ui;
        public List<GameObject> ObstaclePrefabs;
        public float MinScale = 1.0f;
        public float MaxScale = 1.0f;
        public float MaxZ;
        public float Radius;
        public int NumberOfObstacles = 100;
        public Controller Player;

        private (Vector3 position, Quaternion rotation, Vector3 scale) GenerateCubeTransform(float minZ)
        {
            float x = 0.0f;
            float y = 0.0f;
            bool inCircle = false;
            while (!inCircle)
            {
                x = Random.Range(-Radius, Radius);
                y = Random.Range(-Radius, Radius);
                inCircle = (x * x) + (y * y) < maxDistance;
            }

            float playerPosition = Player.transform.position.z + minZ;
            float z = Random.Range(playerPosition, playerPosition + MaxZ);
            float xRot = Random.Range(0, 360);
            float yRot = Random.Range(0, 360);
            float zRot = Random.Range(0, 360);
            float scale = Random.Range(MinScale, MaxScale);
            return (new Vector3(x, y, z), Quaternion.Euler(xRot, yRot, zRot), new Vector3(scale, scale, scale));
        }

        public void Start()
        {
            if (ObstaclePrefabs == null || ObstaclePrefabs.Count <= 0 || NumberOfObstacles <= 0 || Radius <= 0.0f)
                return;

            maxDistance = Radius * Radius;
            instances = new List<Obstacle>(NumberOfObstacles);
            for (int i = 0; i < NumberOfObstacles; i++)
            {
                int index = Random.Range(0, ObstaclePrefabs.Count);
                (Vector3 pos, Quaternion rot, Vector3 scale) = GenerateCubeTransform(20.0f);
                GameObject instance = Instantiate(ObstaclePrefabs[index], pos, rot, transform);
                Renderer objectRen = instance.GetComponent<Renderer>();
                objectRen.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                instance.transform.localScale = scale;
                Obstacle obstacleInstance = instance.GetComponent<Obstacle>();
                if (obstacleInstance == null)
                    throw new Exception("The obstacle prefab needs an Obstacle Component.");
                instances.Add(obstacleInstance);
            }

            cameraOffset = Mathf.Abs(Player.Camera.transform.localPosition.z) + 1;
        }

        public void Update()
        {
            float playerZ = Player.transform.position.z - cameraOffset;
            foreach (Obstacle instance in instances)
            {
                if (instance.transform.position.z > playerZ)
                    continue;

                instance.Reset();
                (Vector3 pos, Quaternion rot, Vector3 scale) = GenerateCubeTransform(MaxZ);
                instance.transform.position = pos;
                instance.transform.rotation = rot;
                instance.transform.localScale = scale;
            }
        }
    }
}
