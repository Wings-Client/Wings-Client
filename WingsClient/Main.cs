using System;
using System.Collections.Generic;
using Il2CppSystem.Collections;
using Il2CppSystem.IO;
using MelonLoader;
using RubyButtonAPI;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

namespace WingsClient

{
    public class WingsClient : MelonMod

    {
        private QMNestedButton menuButton;

        public override void OnApplicationStart()
        {
            if (!Directory.Exists("WingsClient/Texture"))
            {
                Directory.CreateDirectory("WingsClient/Texture");
            }
        }

        public override void VRChat_OnUiManagerInit()
        {
            InitButtons();
        }

        private void InitButtons()
        {
            menuButton = new QMNestedButton("ShortcutMenu", 5, -1, "", "Wings Client Menu");
            Utils.SetImage(menuButton.getMainButton().getGameObject().GetComponent<Image>(),
                "WingsClient/textures/icon.png");
        }
    }
}