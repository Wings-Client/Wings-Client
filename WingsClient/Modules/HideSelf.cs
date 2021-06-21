using MelonLoader;
using UnityEngine;

namespace WingsClient.Modules
{
    public class HideSelf : BaseModule //Credit: Kirai (Modified by Literal)
    {
        private Transform head;
        private VRC_AnimationController animController;
        private VRCVrIkController ikController;

        public override void OnStateChange(bool state)
        {
            if (VRCPlayer.field_Internal_Static_VRCPlayer_0?.transform is null) return;

            if (head is null)
                head = VRCVrCamera.field_Private_Static_VRCVrCamera_0.transform.parent;

            if (animController is null)
                animController = VRCPlayer.field_Internal_Static_VRCPlayer_0
                    .GetComponentInChildren<VRC_AnimationController>();

            if (ikController is null)
                ikController = VRCPlayer.field_Internal_Static_VRCPlayer_0.GetComponentInChildren<VRCVrIkController>();

            VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position += new Vector3(0, state ? -4 : 4, 0);

            animController.field_Private_Boolean_0 = !state;

            MelonCoroutines.Start(ToggleIKController());

            if (state)
            {
                head.localPosition += new Vector3(0, 4 / head.parent.transform.localScale.y, 0);
            }
            else
            {
                head.localPosition = Vector3.zero;
            }
        }


        public override void OnLevelWasLoaded()
        {
            animController = null;
            ikController = null;
        }

        private System.Collections.IEnumerator ToggleIKController()
        {
            if (state)
                yield return new WaitForSeconds(2);
            else yield return null;

            ikController.field_Private_Boolean_0 = !state;
        }
    }
}


