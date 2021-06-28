using VRC.SDKBase;

namespace WingsClient.Modules
{
    public static class FreezePickups
    {
        public static bool active;
        public static bool Active {
            get => active;
            set => OnStateChange(active = value);
        }

        private static VRC_Pickup[] pickups;

        public static void OnStateChange(bool state)
        {
            if (state)
            {
                pickups = UnityEngine.Object.FindObjectsOfType<VRC_Pickup>();
                Shared.Events.OnUpdate += Freeze;
            }
            else Shared.Events.OnUpdate -= Freeze;
        }

        private static void Freeze()
        {
            foreach (VRC_Pickup pickup in pickups)
                if (Networking.GetOwner(pickup.gameObject) != Networking.LocalPlayer)
                    Networking.SetOwner(Networking.LocalPlayer, pickup.gameObject);
        }
    }
}