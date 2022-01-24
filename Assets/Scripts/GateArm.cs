using UnityEngine;

namespace GGJ2020
{
    public enum Boon
    {
        None = 0,
        Speed = 1,
        Spin = 2
    }

    public class GateArm : MonoBehaviour
    {
        public Renderer Renderer;
        public Material SpeedBoonTexture;
        public Material SpinBoonTexture;
        
        public void SetBoon(Boon boon)
        {
            Renderer.enabled = true;
            switch (boon)
            {
                case Boon.Speed:
                    Renderer.material = SpeedBoonTexture;
                    break;
                case Boon.Spin:
                    Renderer.material = SpinBoonTexture;
                    break;
                default:
                    Renderer.enabled = false;
                    break;
            }
        }
    }
}
