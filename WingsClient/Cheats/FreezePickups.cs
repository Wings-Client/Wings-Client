using VRC.SDKBase;

namespace WingsClient.Modules
{
    public class FreezePickups : BaseModule
    {
        private static VRC_Pickup[] pickups;
        private bool isActive = false;

        public override void OnStateChange(bool state)
        {
            isActive = state;
            if (state)
            {
                pickups = UnityEngine.Object.FindObjectsOfType<VRC_Pickup>();
            }
        }

        public override void OnUpdate()
        {
            if (isActive)
            {
                foreach (VRC_Pickup pickup in pickups)
                    if (Networking.GetOwner(pickup.gameObject) != Networking.LocalPlayer)
                        Networking.SetOwner(Networking.LocalPlayer, pickup.gameObject);
            }
        }

        public override void OnLevelWasLoaded()
        {
            if (state) pickups = UnityEngine.Object.FindObjectsOfType<VRC_Pickup>();
        }
    }
}