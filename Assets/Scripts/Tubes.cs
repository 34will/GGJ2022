using System.Collections.Generic;
using UnityEngine;

namespace GGJ2020
{
    public class Tubes : MonoBehaviour
    {
        private float tubeLength;
        private int previousLastTubeIndex = 0;
        private int lastTubeIndex = 0;

        public GameManager GameManager;
        public Transform Player;
        public GameObject TubePrefab;
        public List<GameObject> TubeObjects;
        public float VisibleLength;

        public void Start()
        {
            Tube tube = TubePrefab?.GetComponent<Tube>();
            if (tube == null)
                return;

            tubeLength = tube.GetLength();
            if (tubeLength <= 0)
                throw new System.Exception("TubeLength cannot be 0");

            TubeObjects = new List<GameObject>();
            float currentLength = 0.0f;
            while (currentLength < VisibleLength)
            {
                TubeObjects.Add(Instantiate(TubePrefab, new Vector3(0, 0, currentLength), Quaternion.identity, transform));
                currentLength += tubeLength;
            }
            previousLastTubeIndex = TubeObjects.Count - 1;
        }

        public void Update()
        {
            GameObject lastTube = TubeObjects[lastTubeIndex];
            if (Player.transform.position.z - 15 < lastTube.transform.position.z + tubeLength)
                return;

            GameObject finalTube = TubeObjects[previousLastTubeIndex];
            lastTube.transform.localPosition = new Vector3(0.0f, 0.0f, finalTube.transform.position.z + tubeLength);

            previousLastTubeIndex = lastTubeIndex;
            lastTubeIndex++;
            if (lastTubeIndex >= TubeObjects.Count)
                lastTubeIndex = 0;

            GameManager.GatePassed();
        }
    }
}
