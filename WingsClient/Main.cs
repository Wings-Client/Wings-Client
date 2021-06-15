﻿using System;
using System.Diagnostics;
using Harmony;
using Il2CppSystem.IO;
using MelonLoader;
using RubyButtonAPI;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;
using VRC;
using WingsClient.Modules;

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

        private QMNestedButton _render;
        private QMToggleButton _espButton;
        private QMToggleButton _itemESPButton;

        private QMNestedButton _world;
        private QMSingleButton _rejoinButton;

        private QMNestedButton _exploit;
        private QMSingleButton _downloadVRCA;
        private QMToggleButton _amongUsExploit;

        private QMNestedButton _settings;
        private QMToggleButton _trustRankNameplateButton;
        private QMToggleButton _askForPortal;

        public override void OnApplicationStart()
        {
            InitFolders();

            Shared.modules = new Modules.Modules();
            Shared.settings = new Settings();
            Shared.utils = new Utils();

            new Thread(() => { Patches.Init(HarmonyInstance.Create("Wings.Patches")); }).Start();
        }

        public override void VRChat_OnUiManagerInit()
        {
            InitButtons();
            RemoveAds();
        }

        public override void OnUpdate()
        {
            Shared.modules.OnUpdate();
        }

        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            Shared.modules.OnLevelLoad();
        }


        private void InitButtons()
        {
            QuickMenu.prop_QuickMenu_0.GetComponent<BoxCollider>().size += new Vector3(
                QuickMenu.prop_QuickMenu_0.GetComponent<BoxCollider>().size.x / 2.75f,
                QuickMenu.prop_QuickMenu_0.GetComponent<BoxCollider>().size.y / 2.25f);
            _menuButton = new QMNestedButton("ShortcutMenu", 0, 0, "", "Wings Client Menu", Color.white, Color.white);
            Shared.utils.SetImage(_menuButton.getMainButton().getGameObject().GetComponent<Image>(),
                "WingsClient/textures/icon.png", Color.white);
            Shared.utils.SetImage(
                QuickMenu.prop_QuickMenu_0.transform.Find("QuickMenu_NewElements/_Background/Panel")
                    .GetComponent<Image>(), "WingsClient/textures/background.png",
                new Color(0.75f, 0.75f, 0.75f, 0.75f));
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

            _flightButton = new QMToggleButton(_movement, 1, 0, "Flight\nOn",
                delegate() { Shared.modules.flight.SetState(true); }, "Flight\nOff",
                delegate() { Shared.modules.flight.SetState(false); }, "Flight");

            _speed = new QMToggleButton(_movement, 2, 0, "Speed\nOn",
                delegate() { Shared.modules.speed.SetState(true); }, "Speed\nOff",
                delegate() { Shared.modules.speed.SetState(false); }, "Speed");

            _speedUp = new QMSingleButton(_movement, 0, 0, "Speed\n+",
                delegate { Speed.ChangeModifier (0.1f); }, "Speed Up");
            
            _speedDown = new QMSingleButton(_movement, 0, 1, "Speed\n-",
                delegate { Speed.ChangeModifier (-0.1f); }, "Speed Down");

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

            _downloadVRCA = new QMSingleButton(_exploit, 1, 0, "Download VRCA",
                delegate
                {
                    Application.OpenURL(VRCPlayer.field_Internal_Static_VRCPlayer_0.prop_ApiAvatar_0.assetUrl);
                }, "Download VRCA of avatar");
            //Application.OpenURL(VRCPlayer.field_Internal_Static_VRCPlayer_0.Method_Public_get_ApiAvatar_1().assetUrl);

            _amongUsExploit =
                new QMToggleButton(_exploit, 2, 0, "Among Us/nOn", delegate { }, "Among Us/nOff", delegate { },
                    "WIP, Please be patient fuck face");


            _forceQuitButton = new QMSingleButton("ShortcutMenu", 0, 3, "Force Quit", delegate { ForceQuit(); },
                "Force quit the game immediately");

            _forceRestartButton = new QMSingleButton("ShortcutMenu", 5, 3, "Force Restart",
                delegate { ForceRestart(); },
                "Force restart the game");


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
            }

            if (!Directory.Exists("WingsClient/textures"))
            {
                Directory.CreateDirectory("WingsClient/textures");
            }

            if (!File.Exists("WingsClient/textures/icon.png"))
            {
                Shared.utils.SaveImage("WingsClient/textures/icon.png",
                    "https://avatars.githubusercontent.com/u/85594022");
            }

            if (!File.Exists("WingsClient/textures/background.png"))
            {
                Shared.utils.SaveImage("WingsClient/textures/background.png", "https://i.imgur.com/E5jQqTx.png");
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
                Process.Start(Environment.CurrentDirectory + "\\VRChat.exe", Environment.CommandLine);
            }
            catch (Exception e)
            {
                MelonLogger.Error(e.Message);
            }

            Process.GetCurrentProcess().Kill();
        }
    }
}