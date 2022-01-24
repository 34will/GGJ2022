using System.Collections.Generic;
using UnityEngine;

namespace GGJ2020
{
    public class Tubes : MonoBehaviour
    {
        private float tubeLength;
        private int previousLastTubeIndex = 0;
        private int lastTubeIndex = 0;
        private float cameraOffset;

        public GameManager GameManager;
        public Controller Player;
        public GameObject TubePrefab;
        public List<Tube> TubeObjects;
        public float VisibleLength;

        public void Start()
        {
            Tube tube = TubePrefab?.GetComponent<Tube>();
            if (tube == null)
                return;

            tubeLength = tube.GetLength();
            if (tubeLength <= 0)
                throw new System.Exception("TubeLength cannot be 0");

            Debug.Log(GameManager.Boons.Count);
            TubeObjects = new List<Tube>();
            float currentLength = 0.0f;
            int boonOffset = 0;
            while (currentLength < VisibleLength)
            {
                Tube instance = Instantiate(TubePrefab, new Vector3(0, 0, currentLength), Quaternion.identity, transform)
                    .GetComponent<Tube>();
                TubeObjects.Add(instance);
                currentLength += tubeLength;

                instance.Gate.SetBoon(GameManager.Boons[boonOffset]);
                boonOffset++;
            }
            previousLastTubeIndex = TubeObjects.Count - 1;

            cameraOffset = Mathf.Abs(Player.Camera.transform.localPosition.z) + 1;
        }

        public void Update()
        {
            Tube lastTube = TubeObjects[lastTubeIndex];
            if (Player.transform.position.z - cameraOffset < lastTube.transform.position.z + tubeLength)
                return;

            Tube finalTube = TubeObjects[previousLastTubeIndex];
            lastTube.transform.localPosition = new Vector3(0.0f, 0.0f, finalTube.transform.position.z + tubeLength);

            previousLastTubeIndex = lastTubeIndex;
            lastTubeIndex++;
            if (lastTubeIndex >= TubeObjects.Count)
                lastTubeIndex = 0;

            GameManager.GatePassed();
            Boon boon = GameManager.Boons.Count <= 0 ? Boon.None : GameManager.Boons[0];
            lastTube.Gate.SetBoon(boon);
        }
    }
}
