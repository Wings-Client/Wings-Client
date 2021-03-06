using System;
using MelonLoader;
using RubyButtonAPI;
using System.Threading;
using WingsClient.Modules;
using Directory = System.IO.Directory;
using File = System.IO.File;
using Player = VRC.Player;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Il2CppSystem.Diagnostics;

namespace WingsClient
{
    public class WingsClient : MelonMod
    {
        private QMNestedButton _menuButton;
        private QMSingleButton _forceQuitButton;
        private QMSingleButton _forceRestartButton;

        private QMSingleButton _teleport;

        private QMNestedButton _movement;
        private QMToggleButton _flightButton;
        private QMToggleButton _speed;
        private QMSingleButton _speedUp;
        private QMSingleButton _speedDown;
        private QMSingleButton _speedReset;

        private QMNestedButton _render;
        private QMToggleButton _espButton;
        private QMToggleButton _itemESPButton;
        private QMToggleButton _playerList;


        private QMNestedButton _world;
        private QMSingleButton _rejoinButton;
        private QMSingleButton _copyWorldID;
        private QMSingleButton _joinWorldID;

        private QMNestedButton _exploit;
        private QMSingleButton _downloadVRCA;
        private QMToggleButton _itemOrbit;
        private QMSingleButton _downloadVRCW;
        private QMSingleButton _spawnPrefab;

        private QMToggleButton _annoyUser;

        //private QMToggleButton _amongUsExploit;
        private QMToggleButton _freezePickups;

        private QMNestedButton _settings;
        private QMToggleButton _trustRankNameplateButton;
        private QMToggleButton _askForPortal;

        private QMToggleButton _fpsUnlocker;
        //private QMToggleButton _hideSelf;

        private QMSingleButton _targetPlayer;

        public override void OnApplicationStart()
        {
            Shared.settings = new Settings();
            Shared.utils = new Utils();
            InitFolders();
            Shared.modules = new Modules.Modules();

            new Thread(() => { WingsClientPatches.Init(new HarmonyLib.Harmony("Wings.Patches")); }).Start();
            MelonCoroutines.Start(VRChat_OnUiManagerInit());
        }


        public IEnumerator VRChat_OnUiManagerInit()
        {
            while(VRCUiManager.field_Private_Static_VRCUiManager_0 == null)
                yield return null;

            InitButtons();
            RemoveAds();
            //GameObject.Destroy(GameObject.Find("UserInterface/QuickMenu/ShortcutMenu/SitButton").gameObject);

            Shared.modules.StartCoroutines();
        }

        public override void OnUpdate()
        {
            Shared.modules.OnUpdate();
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            Shared.modules.OnLevelLoad();
            Shared.utils.ClearCache();
    }

        private void InitButtons()
        {
            QuickMenu.prop_QuickMenu_0.GetComponent<BoxCollider>().size += new Vector3(
                QuickMenu.prop_QuickMenu_0.GetComponent<BoxCollider>().size.x / 2.75f,
                QuickMenu.prop_QuickMenu_0.GetComponent<BoxCollider>().size.y / 2.25f);
            _menuButton = new QMNestedButton("ShortcutMenu", 5, 3, "", "Wings Client Menu", Color.white, Color.white);
            Shared.utils.SetImage(_menuButton.getMainButton().getGameObject().GetComponent<Image>(),
                "WingsClient/textures/icon.png", Color.white);
            bool useWingsClientBackground;
            bool.TryParse(Shared.settings.GetSetting("useWingsClientBackground", "true"), out useWingsClientBackground);
            if (useWingsClientBackground)
            {
                Shared.utils.SetImage(
                    QuickMenu.prop_QuickMenu_0.transform.Find("QuickMenu_NewElements/_Background/Panel")
                        .GetComponent<Image>(), "WingsClient/textures/background.png",
                    new Color(0.75f, 0.75f, 0.75f, 0.75f));
            }

            //"https://avatars.githubusercontent.com/u/85594022"
            //_menuButton.getMainButton().setIntractable();

            _movement = new QMNestedButton(_menuButton, 1, 0, "Movement", "Movement Menu");
            _render = new QMNestedButton(_menuButton, 2, 0, "Renderer", "Render Menu");
            _world = new QMNestedButton(_menuButton, 3, 0, "World", "World Menu");
            _exploit = new QMNestedButton(_menuButton, 4, 0, "Exploits", "Exploits Menu");
            _settings = new QMNestedButton(_menuButton, 1, 1, "Settings", "Settings Menu");

            _espButton = new QMToggleButton(_render, 1, 0, "ESP\nOn",
                delegate() { Shared.modules.esp.SetState(true); },
                "ESP\nOff", delegate() { Shared.modules.esp.SetState(false); }, "ESP");

            _itemESPButton = new QMToggleButton(_render, 2, 0, "Item ESP\nOn",
                delegate() { Shared.modules.itemEsp.SetState(true); },
                "Item ESP\nOff", delegate() { Shared.modules.itemEsp.SetState(false); }, "Item ESP");

            _playerList = new QMToggleButton(_settings, 3, 0, "PlayerList\nOn",
                delegate() { Shared.modules.playerList.SetState(true); }, "PLayerList\nOff",
                delegate() { Shared.modules.playerList.SetState(false); }, "Player List");

            _flightButton = new QMToggleButton(_movement, 2, 0, "Flight\nOn",
                delegate() { Shared.modules.flight.SetState(true); }, "Flight\nOff",
                delegate() { Shared.modules.flight.SetState(false); }, "Flight");

            _speed = new QMToggleButton(_movement, 2, 1, "Speed\nOn",
                delegate() { Shared.modules.speed.SetState(true); }, "Speed\nOff",
                delegate() { Shared.modules.speed.SetState(false); }, "Speed");

            _speedUp = new QMSingleButton(_movement, 1, 0, "Speed\n+",
                delegate { Speed.ChangeModifier(0.1f); }, "Speed Up");

            _speedDown = new QMSingleButton(_movement, 1, 2, "Speed\n-",
                delegate { Speed.ChangeModifier(-0.1f); }, "Speed Down");
//This isn't working atm still looking into it
            _speedReset = new QMSingleButton(_movement, 1, 1, "Speed\nReset",
                delegate
                {
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCPlayerApi_0
                        .SetWalkSpeed(2f);
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCPlayerApi_0
                        .SetRunSpeed(4f);
                    VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_VRCPlayerApi_0
                        .SetStrafeSpeed(2f);
                    ;
                }, "SpeedReset");
            _trustRankNameplateButton = new QMToggleButton(_settings, 1, 0, "TrustRankNameplate\nOn",
                delegate()
                {
                    Shared.modules.trustRankNameplate.SetState(true);
                    Shared.settings.SetSetting("trustRankNameplate", "true");
                }, "TrustRankNameplate\nOff",
                delegate()
                {
                    Shared.modules.trustRankNameplate.SetState(false);
                    Shared.settings.SetSetting("trustRankNameplate", "false");
                }, "TrustRankNameplate");
            
            _playerList = new QMToggleButton(_settings, 3, 0, "PlayerList\nOn",
                delegate() { Shared.modules.playerList.SetState(true); }, "PLayerList\nOff",
                delegate() { Shared.modules.playerList.SetState(false); }, "Player List");

            _askForPortal = new QMToggleButton(_settings, 2, 0, "AskForPortal\nOn",
                delegate()
                {
                    Shared.modules.askForPortal = true;
                    Shared.settings.SetSetting("askForPortal", "true");
                }, "AskForPortal\nOff",
                delegate()
                {
                    Shared.modules.askForPortal = false;
                    Shared.settings.SetSetting("askForPortal", "false");
                }, "AskForPortal");

            _fpsUnlocker = new QMToggleButton(_settings, 0, 3, "FPS Unlocked",
                delegate { Shared.modules.fpsUnlocker.SetState(true); }, "FPS Locked",
                delegate { Shared.modules.fpsUnlocker.SetState(false); },
                "Unlock the FPS (set in settings)");

            //_hideSelf = new QMToggleButton(_settings, 2, 1,"HidesSelf\nOn",
            //    delegate() { Shared.modules.hideSelf.SetState(true); }, "HideSelf\nOff",
            //    delegate() { Shared.modules.hideSelf.SetState(false); }, "Hides your self");

            _rejoinButton = new QMSingleButton(_world, 1, 0, "Rejoin",
                delegate()
                {
                    VRCFlowManager.prop_VRCFlowManager_0
                        .Method_Public_Void_String_String_WorldTransitionInfo_Action_1_String_Boolean_0(
                            RoomManager.field_Internal_Static_ApiWorld_0.id,
                            RoomManager.field_Internal_Static_ApiWorldInstance_0.instanceId);
                }, "Rejoin World");
            _copyWorldID = new QMSingleButton(_world, 2, 0, "Copy World ID", delegate
            {
                string id =
                    $"{RoomManager.field_Internal_Static_ApiWorld_0.id}:{RoomManager.field_Internal_Static_ApiWorldInstance_0.instanceId}";
                System.Windows.Forms.Clipboard.SetText(id);
                MelonLogger.Msg("World ID: " + id + "\nCopied to clipboard.");
            }, "Copy the world's ID");


            _joinWorldID = new QMSingleButton(_world, 3, 0, "Join World From Clipboard", delegate
            {
                string id = System.Windows.Forms.Clipboard.GetText().Trim();
                string[] split = id.Split(':');

                if (split.Length != 2)
                {
                    MelonLogger.Error("Invalid World ID / Cannot Join");
                    return;
                }

                new PortalInternal().Method_Private_Void_String_String_PDM_0(split[0], split[1]);
            }, "Join the provided world's ID");


            _teleport = new QMSingleButton("UserInteractMenu", 1, 3, "Teleport",
                delegate()
                {
                    Player.prop_Player_0.transform.position =
                        QuickMenu.prop_QuickMenu_0.field_Private_Player_0.transform.position;
                }, "Teleport");


            _downloadVRCA = new QMSingleButton("UserInteractMenu", 3, 3, "Download VRCA",
                delegate
                {
                    Application.OpenURL(QuickMenu.prop_QuickMenu_0.field_Private_Player_0.prop_VRCPlayer_0
                        .prop_ApiAvatar_1.assetUrl);
                }, "Download VRCA of avatar");


            _downloadVRCA = new QMSingleButton(_exploit, 1, 0, "Download VRCA",
                delegate
                {
                    Application.OpenURL(VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_ApiAvatar_1.assetUrl);
                }, "download VRCA");

            _downloadVRCW = new QMSingleButton(_exploit, 1, 1, "Download VRCW",
                delegate { Application.OpenURL(RoomManager.field_Internal_Static_ApiWorldInstance_0.world.assetUrl); },
                "Download VRCW");

            _spawnPrefab = new QMSingleButton(_exploit, 1, 2, "Spawn Dynamic Prefab",
                delegate { Shared.utils.SpawnDynamicPrefab(); },
                "Spawns a dynamic prefab on either you or your target. Checks if annoy is enabled.");

            _itemOrbit = new QMToggleButton(_exploit, 2, 0, "Item Orbit\nOn",
                delegate { Shared.modules.itemOrbit.SetState(true); }, "Item Orbit\nOff",
                delegate { Shared.modules.itemOrbit.SetState(false); },
                "Items rotate around your feet");
            _annoyUser = new QMToggleButton(_exploit, 3, 0, "Annoy User\nOn",
                delegate { Shared.annoy = true; }, "Annoy User\nOff",
                delegate { Shared.annoy = false; },
                "Annoys the user you're running commands on");
            _freezePickups = new QMToggleButton(_exploit, 4, 0, "Freeze Pickups\nOn",
                delegate { Shared.modules.freezePickups.SetState(true); }, "Freeze Pickups\nOff",
                delegate { Shared.modules.freezePickups.SetState(false); }, "Stops others from picking up objects");

            //_amongUsExploit =
            //    new QMToggleButton(_exploit, 1, 1, "Among Us\nOn", delegate { }, "Among Us\nOff", delegate { },
            //        "WIP, Please be patient fuck face");
//lowkey want to die

            _forceQuitButton = new QMSingleButton(_settings, 2, 1, "Force Quit", delegate { ForceQuit(); },
                "Force quit the game immediately");

            _forceRestartButton = new QMSingleButton(_settings, 1, 1, "Force Restart",
                delegate { ForceRestart(); },
                "Force restart the game");
            //Shared.TargetPlayer = Utils.GetPlayer(QuickMenu.prop_QuickMenu_0.field_Private_APIUser_0.displayName);
            _targetPlayer = new QMSingleButton("UserInteractMenu", 2, 3, "Target User",
                delegate
                {
                    Shared.TargetPlayer =
                        Utils.GetPlayer(QuickMenu.prop_QuickMenu_0.field_Private_APIUser_0.displayName);
                }, "Targets the player for certain actions");


            bool trustNamePlate;
            bool askForPortal;
            bool.TryParse(Shared.settings.GetSetting("trustRankNameplate", "false"), out trustNamePlate);
            bool.TryParse(Shared.settings.GetSetting("askForPortal", "false"), out askForPortal);

            _trustRankNameplateButton.setToggleState(trustNamePlate);
            _askForPortal.setToggleState(askForPortal);
        }

        private static void InitFolders()
        {
            if (!Directory.Exists("WingsClient"))
            {
                Directory.CreateDirectory("WingsClient");
                MelonLogger.Msg("Created Directory 'WingsClient'");
            }

            if (!Directory.Exists("WingsClient/textures"))
            {
                Directory.CreateDirectory("WingsClient/textures");
                MelonLogger.Msg("Created Directory 'WingsClient/textures'");
            }

            if (!File.Exists("WingsClient/textures/icon.png"))
            {
                Shared.utils.SaveImage("WingsClient/textures/icon.png",
                    "https://i.imgur.com/ru5Dshr.png");
                MelonLogger.Msg("Created File 'WingsClient/textures/icon.png'");
            }

            if (!File.Exists("WingsClient/textures/background.png"))
            {
                Shared.utils.SaveImage("WingsClient/textures/background.png", "https://i.imgur.com/E5jQqTx.png");
                MelonLogger.Msg("Created File 'WingsClient/textures/background.png'");
            }

            if (!File.Exists(Settings.SettingsPath))
            {
                File.Create(Settings.SettingsPath).Close();
                MelonLogger.Msg("Created empty settings file.");
            }
        }

        private static void RemoveAds()
        {
            GameObject.Destroy(QuickMenu.prop_QuickMenu_0.transform.Find("ShortcutMenu/HeaderContainer/VRCPlusBanner")
                .gameObject);
            GameObject.Destroy(QuickMenu.prop_QuickMenu_0.transform.Find("ShortcutMenu/VRCPlusMiniBanner").gameObject);
            GameObject.Destroy(GameObject.Find("MenuContent/Backdrop/Header/Tabs/ViewPort/Content/VRC+PageTab"));
        }

        private static void ForceQuit()
        {
             Process.GetCurrentProcess().Kill();
        }
        private static void ForceRestart()
        {
            try
            {
                System.Diagnostics.Process.Start(Environment.CurrentDirectory + "\\VRChat.exe", Environment.CommandLine);
            }
            catch (Exception e)
            {
                MelonLogger.Error(e.Message);
            }

            Process.GetCurrentProcess().Kill();
        }
    }