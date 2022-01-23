using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace GGJ2020
{



    public class UI : MonoBehaviour
    {
        public TextMeshProUGUI scoreText;
        public TextMeshProUGUI speedText;

        public TextMeshProUGUI gatesText;
        public TextMeshProUGUI livesText;
        public TextMeshProUGUI rotText;

        private int score;
        private float speed;
        private float gates;
        private float rot;
        private float lives;

        // Start is called before the first frame update
        void Start()
        {
            speed = 100;
            score = 0;
            lives = 0;
            gates = 0;
            rot = 0;
            UpdateScore(0);
            UpdateSpeed(0);
            UpdateGates(0);
            UpdateLives(0);
            UpdateRotation(0);
        }

        // Update is called once per frame
        void Update()
        {
            UpdateScore(1);
        }

        public void UpdateScore(int scoreToAdd)
        {
            score += scoreToAdd;
            scoreText.text = "Distance: " + score;

        }

        public void UpdateSpeed(float scoreToAdd)
        {
            speed += scoreToAdd;
            speedText.text = "Speed: " + speed;

        }

        public void UpdateLives(float scoreToAdd)
        {
            lives += scoreToAdd;
            livesText.text = "Lives: " + lives;

        }

        public void UpdateRotation(float scoreToAdd)
        {
            rot += scoreToAdd;
            rotText.text = "Rotation Speed: " + rot;

        }

        public void UpdateGates(float scoreToAdd)
        {
            gates += scoreToAdd;
            gatesText.text = "Gates Passed: " + gates;

        }
    }
}