using Il2CppSystem.IO;
using MelonLoader;
using RubyButtonAPI;
using UnityEngine;
using UnityEngine.UI;
using VRC;
using WingsClient.Modules;

namespace WingsClient

{
    public class WingsClient : MelonMod

    {
        private QMNestedButton _menuButton;
        private QMToggleButton _espButton;
        private QMToggleButton _flightButton;

        public override void OnApplicationStart()
        {
            Shared.modules = new Modules.Modules();
            if (!Directory.Exists("WingsClient/Texture"))
            {
                Directory.CreateDirectory("WingsClient/Texture");
            }
        }

        public override void VRChat_OnUiManagerInit()
        {
            InitButtons();
            removeAds();
            trustNameplates();
        }

        public override void OnUpdate()
        {
            Shared.modules.OnUpdate();
        }

        private void InitButtons()
        {
            _menuButton = new QMNestedButton("ShortcutMenu", 5, -1, "", "Wings Client Menu");
            Utils.SetImage(_menuButton.getMainButton().getGameObject().GetComponent<Image>(),
                "WingsClient/textures/icon.png");

            _espButton = new QMToggleButton(_menuButton, 1, 0, "ESP\nOn",
                delegate() { Shared.modules.esp.SetState(true); },
                "ESP\nOff", delegate() { Shared.modules.esp.SetState(false); }, "ESP");

            _flightButton = new QMToggleButton(_menuButton, 2, 0, "Flight\nOn",
                delegate() { Shared.modules.flight.SetState(true); }, "Flight\nOff",
                delegate() { Shared.modules.flight.SetState(false); }, "Flight");
        }

        private void removeAds()
        {
            GameObject.Destroy(QuickMenu.prop_QuickMenu_0.transform.Find("ShortcutMenu/HeaderContainer/VRCPlusBanner").gameObject);
            GameObject.Destroy(QuickMenu.prop_QuickMenu_0.transform.Find("ShortcutMenu/VRCPlusMiniBanner").gameObject);
            GameObject.Destroy(GameObject.Find("MenuContent/Backdrop/Header/Tabs/ViewPort/Content/VRC+PageTab"));
        }

        private void trustNameplates()
        {
            VRCPlayer.Method_Public_Static_Color_APIUser_0(Player.prop_Player_0.prop_APIUser_0);
        }
    }
}