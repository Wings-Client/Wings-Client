using UnityEngine;
using VRC.SDKBase;

namespace WingsClient.Modules
{
    public class ItemOrbit: BaseModule //Created by Kirai, Modified by WingsClient Team
    {
        public float speed = 1;
        public float size = 1;

        VRC_Pickup[] cached;

        public override void OnLevelWasLoaded()
        {
            if (state) Recache();
        }

        public override void OnStateChange(bool state)
        {
            if (state) Recache();
        }

        public override void OnUpdate()
        {
            if (!state) return;

            if (VRCPlayer.field_Internal_Static_VRCPlayer_0 == null) return;

            if (cached == null) Recache();

            GameObject puppet = new GameObject();
            
            puppet.transform.position = VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position + new Vector3(0, 0.2f, 0);

            puppet.transform.Rotate(new Vector3(0, 360f * Time.time * speed, 0));

            foreach (VRC_Pickup pickup in cached)
            {
                if (Networking.GetOwner(pickup.gameObject) != Networking.LocalPlayer) Networking.SetOwner(Networking.LocalPlayer, pickup.gameObject);

                pickup.transform.position = puppet.transform.position + puppet.transform.forward * size;

                puppet.transform.Rotate(new Vector3(0, 360 / cached.Length, 0));
            }

            Object.Destroy(puppet);
        }

        public void Recache()
        {
            cached = Object.FindObjectsOfType<VRC_Pickup>();
        }
    }
}