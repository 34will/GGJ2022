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
        public TextMeshProUGUI particlesHitText;
        public TextMeshProUGUI forceFieldUsagesText;

        public void UpdateLives(int lives, int maxLives)
        {
            livesText.text = $"Lives: {lives}/{maxLives}";
        }

        public void UpdateObstaclesHit(int hits)
        {
            particlesHitText.text = $"Particles Hit: {hits}";
        }

        public void UpdateForceFieldUsages(int usages)
        {
            forceFieldUsagesText.text = $"Particle Mode Usages: {usages}";
        }

        public void UpdateDistance(float score)
        {
            string scoreFormatted = score.ToString("0");
            scoreText.text = $"Distance: {scoreFormatted}m";
        }

        public void UpdateSpeed(float speed)
        {
            string speedFormatted = speed.ToString("0");
            speedText.text = $"Speed: {speedFormatted}m/s";
        }

        public void UpdateRotation(float rotation)
        {
            string rotationFormatted = rotation.ToString("0");
            rotText.text = $"Rotation Speed: {rotationFormatted}m/s";
        }

        public void UpdateGates(int gates)
        {
            gatesText.text = $"Gates Passed: {gates}";
        }
    }
}