using UnityEngine;

namespace WingsClient.Modules
{
    public class FPSUnlocker : BaseModule
    {
        public override void OnStateChange(bool state)
        {
            bool doesIntegerExist = int.TryParse(Shared.settings.GetSetting("FPSLimit", "200"),
                out int settingsTargetFramerate);
            if (doesIntegerExist)
            {
                Application.targetFrameRate = state ? settingsTargetFramerate : 60;
            }
            else
            {
                Application.targetFrameRate = 60;
            }
        }
    }
}