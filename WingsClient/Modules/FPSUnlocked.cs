namespace WingsClient.Modules
{
    public class FPSUnlocked
    {
        public static void Initialize()
        {
            if (Configuration.JSONConfig.UnlimitedFPSEnabled)
            {
                Application.targetFrameRate = Shared.settings.GetSetting("FPSLimit", "60");
            }
        }
    }
}
    
