namespace WingsClient.Modules
{
    public class Speed : BaseModule
    {
        private static float originalWalkSpeed = 0f;


        private static float originalRunSpeed = 0f;


        private static float originalStrafeSpeed = 0f;


        public static float Modifier = 1f;


        public static bool SpeedModified = false;

        public override void OnStateChange(bool state)
        {
            SpeedModified = state;
        }

        public static void ChangeModifier(float modifier)
        {
            Speed.Modifier += modifier;
        }

        public override void OnUpdate()
        {
            if (!this.state) return;
            if (RoomManager.field_Internal_Static_ApiWorld_0 == null)
            {
                Speed.originalRunSpeed = 0f;
                Speed.originalWalkSpeed = 0f;
                Speed.originalStrafeSpeed = 0f;
            }


            if (VRCPlayer.field_Internal_Static_VRCPlayer_0 != null &&
                VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCPlayerApi_0 != null &&
                RoomManager.field_Internal_Static_ApiWorld_0 != null)
            {
                if (Speed.SpeedModified && (Speed.originalRunSpeed == 0f || Speed.originalWalkSpeed == 0f ||
                                            Speed.originalStrafeSpeed == 0f))
                {
                    Speed.originalWalkSpeed = VRCPlayer.field_Internal_Static_VRCPlayer_0
                        .prop_VRCPlayerApi_0.GetWalkSpeed();
                    Speed.originalRunSpeed = VRCPlayer.field_Internal_Static_VRCPlayer_0
                        .prop_VRCPlayerApi_0.GetRunSpeed();
                    Speed.originalStrafeSpeed = VRCPlayer.field_Internal_Static_VRCPlayer_0
                        .prop_VRCPlayerApi_0.GetStrafeSpeed();
                }

                if (!Speed.SpeedModified && Speed.originalRunSpeed != 0f && Speed.originalWalkSpeed != 0f &&
                    Speed.originalStrafeSpeed != 0f)
                {
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCPlayerApi_0
                        .SetWalkSpeed(Speed.originalWalkSpeed);
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCPlayerApi_0
                        .SetRunSpeed(Speed.originalRunSpeed);
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCPlayerApi_0
                        .SetStrafeSpeed(Speed.originalStrafeSpeed);
                    Speed.originalRunSpeed = 0f;
                    Speed.originalWalkSpeed = 0f;
                    Speed.originalStrafeSpeed = 0f;
                    Speed.Modifier = 1f;
                }

                if (Speed.SpeedModified && Speed.originalWalkSpeed != 0f && Speed.originalRunSpeed != 0f &&
                    Speed.originalStrafeSpeed != 0f)
                {
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCPlayerApi_0
                        .SetWalkSpeed(Speed.originalWalkSpeed * Speed.Modifier);
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCPlayerApi_0
                        .SetRunSpeed(Speed.originalRunSpeed * Speed.Modifier);
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCPlayerApi_0
                        .SetStrafeSpeed(Speed.originalStrafeSpeed * Speed.Modifier);
                }
            }
        }
    }
}