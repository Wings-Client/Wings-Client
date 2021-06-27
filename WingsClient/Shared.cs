using VRC;

namespace WingsClient
{
    public class Shared
    {
        public static Modules.Modules modules;
        public static Settings settings;
        public static Utils utils;

        public static string[] meshBlacklist;
        public static string[] avatarBlacklist;
        public static string[] shaderBlacklist;
        
        public static bool annoy;
        
        private static Player targetPlayer;

        public static class Events
        {
            public static Action OnUpdate = new Action(() => { });
        }
        
        public static Player TargetPlayer { 
            get => targetPlayer; 
            set { 
                targetPlayer = value;
            } 
        }
    }
}