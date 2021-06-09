using UnityEngine;
using UnityEngine.XR;
using VRC;
using VRC.Animation;
using VRC.SDKBase;

namespace WingsClient.Modules
{
    public class Flight : BaseModule
    {
        private VRCMotionState componentCache;
        public override void OnStateChange(bool state)
        {
            Player.prop_Player_0.GetComponent<CharacterController>().enabled = !state;

            if (VRCPlayer.field_Internal_Static_VRCPlayer_0 == null)
            {
                return;
            }

            VRCMotionState component = VRCPlayer.field_Internal_Static_VRCPlayer_0.GetComponent<VRCMotionState>();
            componentCache = component;
            if (component == null)
            {
                return;
            }

            if (state)
            {
                return;
            }

            component.Method_Public_Void_0();
        }

        public override void OnUpdate()
        {
            if (!this.state)
            {
                return;
            }

            if (Networking.LocalPlayer == null)
            {
                return;
            }

            float num;
            float num2;
            float num3;
            if (XRDevice.isPresent)
            {
                num = Input.GetAxis("Horizontal");
                num2 = Input.GetAxis("Oculus_CrossPlatform_SecondaryThumbstickVertical");
                num3 = Input.GetAxis("Vertical");
            }
            else
            {
                num2 = (num = (num3 = 0f));
                if (Input.GetKey(KeyCode.W))
                {
                    num3 += 1f;
                }

                if (Input.GetKey(KeyCode.S))
                {
                    num3 -= 1f;
                }

                if (Input.GetKey(KeyCode.D))
                {
                    num += 1f;
                }

                if (Input.GetKey(KeyCode.A))
                {
                    num -= 1f;
                }

                if (Input.GetKey(KeyCode.E))
                {
                    num2 += 1f;
                }

                if (Input.GetKey(KeyCode.Q))
                {
                    num2 -= 1f;
                }
            }

            if (this.camera == null)
            {
                this.camera = VRCVrCamera.field_Private_Static_VRCVrCamera_0.GetComponentInChildren<Camera>().transform;
            }

            VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position +=
                (this.directional ? this.camera.right : VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.right) *
                Shared.modules.flight.speed * Time.deltaTime * num * (float) (Input.GetKey(KeyCode.LeftShift) ? 8 : 1);
            VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position +=
                (this.directional
                    ? this.camera.forward
                    : VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.forward) * Shared.modules.flight.speed *
                Time.deltaTime * num3 * (float) (Input.GetKey(KeyCode.LeftShift) ? 8 : 1);
            VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.position +=
                (this.directional ? this.camera.up : VRCPlayer.field_Internal_Static_VRCPlayer_0.transform.up) *
                Shared.modules.flight.speed * Time.deltaTime * num2 * (float) (Input.GetKey(KeyCode.LeftShift) ? 8 : 1);
            Networking.LocalPlayer.SetVelocity(new Vector3(0f, 0f, 0f));
            componentCache.Reset();
        }

        private Transform camera;

        public Vector3 oGravity;

        public float speed = 8f;

        public bool directional;
    }
}