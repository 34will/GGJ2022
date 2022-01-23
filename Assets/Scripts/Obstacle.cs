using UnityEngine;

namespace GGJ2020
{
    [RequireComponent(typeof(Collider))]
    public class Obstacle : MonoBehaviour
    {
        private Collider Collider;

        public ParticleSystem ParticleSystem;
        public MeshRenderer MeshRenderer;
        public GameManager GameManager;

        public void Start()
        {
            Collider = GetComponent<Collider>();
        }

        public void OnCollisionEnter(Collision collision)
        {
            if (collision.transform.tag != "Player")
                return;

            Collide();
            GameManager.LifeLost();
        }

        public void Collide()
        {
            if (!Collider.enabled)
                return;

            MeshRenderer.enabled = false;
            Collider.enabled = false;
            ParticleSystem.Play();
            GameManager.ObstacleHit();
        }

        public void Reset()
        {
            MeshRenderer.enabled = true;
            Collider.enabled = true;
            ParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }
}
