using Harmony;
using Il2CppSystem.IO;
using MelonLoader;
using RubyButtonAPI;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using VRC;

namespace WingsClient
{
    public class WingsClient : MelonMod
    {
        private QMNestedButton _menuButton;

        private QMSingleButton _forceQuitButton;

        private QMSingleButton _teleport;

        private QMNestedButton _movement;
        private QMToggleButton _flightButton;

        private QMNestedButton _render;
        private QMToggleButton _espButton;

        private QMNestedButton _world;
        private QMSingleButton _rejoinButton;

        private QMNestedButton _exploit;

        private QMNestedButton _settings;
        private QMToggleButton _trustRankNameplateButton;
        private QMToggleButton _askForPortal;

        public override void OnApplicationStart()
        {
            Shared.modules = new Modules.Modules();
            if (!Directory.Exists("WingsClient/Texture"))
            {
                Directory.CreateDirectory("WingsClient/Texture");
            }

            new Thread(() => { Patches.Init(HarmonyInstance.Create("Wings.Patches")); }).Start();
        }

        public override void VRChat_OnUiManagerInit()
        {
            InitButtons();
            RemoveAds();
            //trustNameplates();
        }

        public override void OnUpdate()
        {
            Shared.modules.OnUpdate();
        }


        private void InitButtons()
        {
            _menuButton = new QMNestedButton("ShortcutMenu", 0, 0, "", "Wings Client Menu");
            Utils.SetImage(_menuButton.getMainButton().getGameObject().GetComponent<Image>(),
                "WingsClient/textures/icon.png", Color.red);
            //"https://avatars.githubusercontent.com/u/85594022"
            _forceQuitButton = new QMSingleButton("Force Quit", 0, 1, "Force Quit", delegate { ForceQuit(); },
                "Force quit the game immediately");

            _movement = new QMNestedButton(_menuButton, 1, 0, "Movement", "Movement Menu");
            _render = new QMNestedButton(_menuButton, 2, 0, "Renderer", "Render Menu");
            _world = new QMNestedButton(_menuButton, 3, 0, "World", "World Menu");
            _exploit = new QMNestedButton(_menuButton, 4, 0, "Exploits", "Exploits Menu");
            _settings = new QMNestedButton(_menuButton, 1, 1, "Settings", "Settings Menu");

            Utils.SetImage(_menuButton.getMainButton().getGameObject().GetComponent<Image>(),
                "WingsClient/textures/icon.png", Color.red);
            //"https://avatars.githubusercontent.com/u/85594022"
            _espButton = new QMToggleButton(_movement, 1, 0, "ESP\nOn",
                delegate() { Shared.modules.esp.SetState(true); },
                "ESP\nOff", delegate() { Shared.modules.esp.SetState(false); }, "ESP");

            _flightButton = new QMToggleButton(_render, 1, 0, "Flight\nOn",
                delegate() { Shared.modules.flight.SetState(true); }, "Flight\nOff",
                delegate() { Shared.modules.flight.SetState(false); }, "Flight");

            _trustRankNameplateButton = new QMToggleButton(_settings, 1, 0, "TrustRankNameplate\nOn",
                delegate() { Shared.modules.trustRankNameplate.SetState(true); }, "TrustRankNameplate\nOff",
                delegate() { Shared.modules.trustRankNameplate.SetState(false); }, "TrustRankNameplate");

            _askForPortal = new QMToggleButton(_settings, 1, 0, "AskForPortal\nOn",
                delegate() { Shared.modules.askForPortal = true; }, "AskForPortal\nOff",
                delegate() { Shared.modules.askForPortal = false; }, "AskForPortal");

            _rejoinButton = new QMSingleButton(_world, 1, 0, "Rejoin",
                delegate()
                {
                    VRCFlowManager.prop_VRCFlowManager_0
                        .Method_Public_Void_String_String_WorldTransitionInfo_Action_1_String_Boolean_0(
                            RoomManager.field_Internal_Static_ApiWorld_0.id,
                            RoomManager.field_Internal_Static_ApiWorldInstance_0.idWithTags);
                }, "Rejoin World");

            _teleport = new QMSingleButton("UserInteractMenu", 1, 3, "Teleport",
                delegate()
                {
                    Player.prop_Player_0.transform.position =
                        QuickMenu.prop_QuickMenu_0.field_Private_Player_0.transform.position;
                }, "Teleport");
        }

        private static void RemoveAds()
        {
            GameObject.Destroy(QuickMenu.prop_QuickMenu_0.transform.Find("ShortcutMenu/HeaderContainer/VRCPlusBanner")
                .gameObject);
            GameObject.Destroy(QuickMenu.prop_QuickMenu_0.transform.Find("ShortcutMenu/VRCPlusMiniBanner").gameObject);
            GameObject.Destroy(GameObject.Find("MenuContent/Backdrop/Header/Tabs/ViewPort/Content/VRC+PageTab"));
        }

        private void trustNameplates()
        {
            //VRCPlayer.Method_Public_Static_Color_APIUser_0(Player.prop_Player_0.prop_APIUser_0); //this is literally not the way to do it so I'm just dumb.
        }

        private static void ForceQuit()
        {
            System.Diagnostics.Process.GetCurrentProcess().Kill();
        }
    }
}