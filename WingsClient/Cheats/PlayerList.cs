using RubyButtonAPI;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using VRC;
using MelonLoader;
using VRC.Core;
using WingsClient.Wrappers;

//mad by four ;)

namespace WingsClient.Modules
{
    public class PlayerList : BaseModule
    {
        private List<QMLable> playerList = new List<QMLable>();

        public override void OnStateChange(bool state)
        {
			if(state){
				for (int i = 0; i < PlayerManager.prop_PlayerManager_0.field_Private_List_1_Player_0.ToArray().ToList().Count; i++)
            {
                try
                {
                    Player player = PlayerManager.prop_PlayerManager_0.field_Private_List_1_Player_0.ToArray().ToList()[i];
                    QMLable lable = new QMLable("", 6, playerList.Count, QuickMenu.prop_QuickMenu_0.transform.Find("ShortcutMenu"), new Action(() =>
                    {
                        QuickMenu.prop_QuickMenu_0.SelectPlayer(player);
                    }));
                    lable.qmLable.name = player.prop_APIUser_0.id;
                    playerList.Add(lable);
                }
                catch { }
            }
				MelonCoroutines.Start(LableUpdater());
			}else{
				for (int i = 0; i < PlayerManager.prop_PlayerManager_0.field_Private_List_1_Player_0.ToArray().ToList().Count; i++)
            {
                try
                {
                    Player player = PlayerManager.prop_PlayerManager_0.field_Private_List_1_Player_0.ToArray().ToList()[i];
                    QMLable lable = playerList.Find(x => x.qmLable.name == player.prop_APIUser_0.id);
                    GameObject.Destroy(lable.qmLable);
                    playerList.Remove(lable);
                }
                catch { }
            }
			}
        }
		
        public IEnumerator LableUpdater()
        {
            while (this.state)
            {
                yield return new WaitForSeconds(0.25f);
                for(int i = 0; i < PlayerManager.prop_PlayerManager_0.field_Private_List_1_Player_0.ToArray().ToList().Count; i++)
                {
                    try {
                        Player player = PlayerManager.prop_PlayerManager_0.field_Private_List_1_Player_0.ToArray().ToList()[i];
                        if (player.prop_APIUser_0 != APIUser.CurrentUser)
                        {
                            QMLable playerLable = playerList.Find(x => x.qmLable.name == player.prop_APIUser_0.id);
                            playerLable.text.text = "<color=#" + ColorUtility.ToHtmlStringRGB(VRCPlayer.Method_Public_Static_Color_APIUser_0(player.prop_APIUser_0)) + ">" + player.prop_APIUser_0.displayName + "</color> [<color=#ffa500ff>P</color>]: " + player.GetPing() + "ms [<color=#ffa500ff>F</color>] " + player.GetFrames() + (player.GetIsMod() ? "[<color=#a52a2aff>Mod</color>] " : "") + (player.GetIsBot() ? "[<color=#000000ff>Bot</color>] " : "");
                            playerLable.setLocation(6, i);
                        }
                    } catch { }
                }
            }
            yield break;
        }
        
        public override void OnPlayerJoined(Player player)
        {
			if(!this.state)
				return;
            QMLable lable = new QMLable("<color=#" + ColorUtility.ToHtmlStringRGB(VRCPlayer.Method_Public_Static_Color_APIUser_0(player.prop_APIUser_0)) + ">" + player.prop_APIUser_0.displayName + "</color>", 6, playerList.Count, QuickMenu.prop_QuickMenu_0.transform.Find("ShortcutMenu"), new Action(() =>
            {
                QuickMenu.prop_QuickMenu_0.SelectPlayer(player);
            }));
            lable.qmLable.name = player.prop_APIUser_0.id;
            playerList.Add(lable);
        }

        public override void OnPlayerLeft(Player player)
        {
			if(!this.state)
				return;
            QMLable lable = playerList.Find(x => x.qmLable.name == player.prop_APIUser_0.id);
            GameObject.Destroy(lable.qmLable);
            playerList.Remove(lable);
        }
    }
}
