using RubyButtonAPI;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VRC;
using WingsClient.Extensions;
using WingsClient.External;
using WingsClient.Modules;
using WingsClient.Wrappers;

namespace WingsClient.Cheats
{
    public class PlayerList : BaseModule
    {
        public static List<string> Players = new List<string>();

        public static MenuText PlayerCount;

        public static List<MenuText> Logs = new List<MenuText>();

        public static float posx = -1220;

        public static float posy = -400f;

        public static List<string> BlockList = new List<string>();
        public Transform shortcutMenu;

        public override void OnLevelWasLoaded()
        {
            Start();
        }


        public void Start()

        {
            var num = posy;
            var parent = Utils.QuickMenu.transform.Find("ShortcutMenu");
            PlayerCount = new MenuText(shortcutMenu, posx, -500f, "<b>==========Playerlist========== </b>");
            for (var i = 0; i <= 38; i++)
            {
                var item = new MenuText(shortcutMenu, posx, posy, "");
                posy += 70f;
            }

            posy = num;
        }

        public override void OnPlayerJoined(Player player)
        {
            AddPlayerToList();
        }

        public override void OnPlayerLeft(Player player)
        {
            AddPlayerToList();
        }

        public static void AddPlayerToList()
        {
            try
            {
                Players.Clear();
                foreach (var Player in Utils.PlayerManager.GetAllPlayers().ToArray())
                {
                    string Text;
                    Text = (Player.GetAPIUser().hasModerationPowers ? "<color=#850700>[MOD]</color> " : "") +
                           (Player.GetIsBot() ? "<color=#a33333>[BOT]</color> " : "") +
                           (Player.IsFriend() ? "<color=#ebc400>[F]</color> " : "") +
                           (Player.GetIsMaster() ? "<color=#3c1769>[M]</color> " : "") +
                           (Player.GetAPIUser().isSupporter ? "<color=#b66b25>[V+]</color> " : "") +
                           (Player.GetAPIUser().IsOnMobile ? "<color=#27b02d>[Q]</color> " : "") +
                           (Player.GetVRCPlayerApi().IsUserInVR()
                               ? "<color=#00d4f0>[VR]</color> "
                               : "<color=#00d4f0>[D]</color> ") + "<color=#00d4f0>" + Player.DisplayName() +
                           "</color>" + " [P] " + Player.GetPingColored() + " [F] " + Player.GetFramesColored();
                    Players.Insert(0, Text);
                }
            }
            catch
            {
            }
        }

        public static void UpdateText()
        {
            try
            {
                for (var i = 0; i <= Logs.Count; i++)
                    try
                    {
                        if (Players[i] != null) Logs[i].SetText(Players[i]);
                    }
                    catch
                    {
                        Logs[i].SetText("");
                    }
            }
            catch
            {
            }
        }
    }
}