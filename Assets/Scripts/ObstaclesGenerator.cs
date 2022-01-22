using System.Collections.Generic;
using UnityEngine;

namespace GGJ2020
{
    public class ObstaclesGenerator : MonoBehaviour
    {
        private List<GameObject> instances;
        private float maxDistance;

        public List<GameObject> ObstaclePrefabs;
        public float MinScale = 1.0f;
        public float MaxScale = 1.0f;
        public float MaxZ;
        public float Radius;
        public int NumberOfObstacles = 100;
        public GameObject Player;

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
            instances = new List<GameObject>(NumberOfObstacles);
            for (int i = 0; i < NumberOfObstacles; i++)
            {
                int index = Random.Range(0, ObstaclePrefabs.Count);
                (Vector3 pos, Quaternion rot, Vector3 scale) = GenerateCubeTransform(20.0f);
                GameObject instance = Instantiate(ObstaclePrefabs[index], pos, rot, transform);
                Renderer objectRen;
                objectRen = instance.GetComponent<Renderer>();
                objectRen.material.color = Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
                instance.transform.localScale = scale;
                instances.Add(instance);
            }
        }

        public void Update()
        {
            float playerZ = Player.transform.position.z - 1;
            foreach (GameObject instance in instances)
            {
                if (instance.transform.position.z > playerZ)
                    continue;

                (Vector3 pos, Quaternion rot, Vector3 scale) = GenerateCubeTransform(MaxZ);
                instance.transform.position = pos;
                instance.transform.rotation = rot;
                instance.transform.localScale = scale;
            }
        }
    }
}
