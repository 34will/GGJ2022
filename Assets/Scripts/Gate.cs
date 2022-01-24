using UnityEngine;

namespace GGJ2020
{
    public class Gate : MonoBehaviour
    {
        public GateArm[] GateArms;

        public void SetBoon(Boon boon)
        {
            foreach (GateArm gateArm in GateArms)
                gateArm.SetBoon(boon);
        }
    }
}
