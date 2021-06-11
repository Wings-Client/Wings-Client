using UnityEngine;
using VRC.SDKBase;

namespace WingsClient.Modules
{
    public class ItemESP : BaseModule
    {
        private VRC_Pickup[] _pickupStored;

        public override void OnLevelWasLoaded()
        {
            this._pickupStored = Object.FindObjectsOfType<VRC_Pickup>();
        }

        public override void OnStateChange(bool state)
        {
            VRC_Pickup[] pickupStored = this._pickupStored;
            for (int i = 0; i < pickupStored.Length; i++)
            {
                Renderer component = pickupStored[i].GetComponent<Renderer>();
                if (component != null)
                {
                    HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(component, state);
                }
            }
        }
    }
}