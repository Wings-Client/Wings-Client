using System;
using System.Windows.Forms.VisualStyles;
using UnityEngine;
using UnityEngine.Playables;
using VRC;
using VRC.SDKBase;
using Object = UnityEngine.Object;

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
            
            if (Shared.annoy)
                puppet.transform.position = (Shared.TargetPlayer?.field_Private_VRCPlayerApi_0 ?? Networking.LocalPlayer).GetTrackingData(VRCPlayerApi.TrackingDataType.Head).position;
            else
                puppet.transform.position = (Shared.TargetPlayer?.transform.position ?? VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position) + new Vector3(0, 0.2f, 0);

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
        
        public override void OnPlayerLeft(Player player)
        {
            if (Shared.TargetPlayer == player)
                Shared.TargetPlayer = null;
        }
    }
}