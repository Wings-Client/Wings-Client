using Il2CppSystem.IO;
using MelonLoader;
using RubyButtonAPI;
using UnityEngine.UI;
using WingsClient.Modules;

namespace WingsClient

{
    public class WingsClient : MelonMod

    {
        private QMNestedButton menuButton;
        private QMToggleButton espButton;
        private QMToggleButton flightButton;

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
        }

        public override void OnUpdate()
        {
            Shared.modules.OnUpdate();
        }

        private void InitButtons()
        {
            menuButton = new QMNestedButton("ShortcutMenu", 5, -1, "", "Wings Client Menu");
            Utils.SetImage(menuButton.getMainButton().getGameObject().GetComponent<Image>(),
                "WingsClient/textures/icon.png");

            espButton = new QMToggleButton(menuButton, 1, 0, "ESP\nOn",
                delegate() { Shared.modules.esp.SetState(true); },
                "ESP\nOff", delegate() { Shared.modules.esp.SetState(false); }, "ESP");

            flightButton = new QMToggleButton(menuButton, 2, 0, "Flight\nOn",
                delegate() { Shared.modules.flight.SetState(true); }, "Flight\nOff",
                delegate() { Shared.modules.flight.SetState(false); }, "Flight");
        }
    }
}