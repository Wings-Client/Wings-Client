using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using VRC;

namespace WingsClient.Modules
{
    public class TrustRankNameplate : BaseModule
    {
        public override void OnStateChange(bool state)
        {
            if (!state)
            {
                for (int i = 0; i < PlayerManager.prop_PlayerManager_0.prop_ArrayOf_Player_0.Count; i++)
                {
                    PlayerNameplate nameplate = PlayerManager.prop_PlayerManager_0.prop_ArrayOf_Player_0[i]
                        .prop_VRCPlayer_0.field_Public_PlayerNameplate_0;
                    Color color = Color.gray;

                    nameplate.gameObject.transform.Find("Contents/Icon/Background").GetComponent<Image>().color = color;
                    nameplate.gameObject.transform.Find("Contents/Main/Background").GetComponent<ImageThreeSlice>()
                        .color = color;
                }
            }
            else
            {
                for (int i = 0; i < PlayerManager.prop_PlayerManager_0.prop_ArrayOf_Player_0.Count; i++)
                {
                    PlayerNameplate nameplate = PlayerManager.prop_PlayerManager_0.prop_ArrayOf_Player_0[i]
                        .prop_VRCPlayer_0.field_Public_PlayerNameplate_0;
                    Color color =
                        VRCPlayer.Method_Public_Static_Color_APIUser_0(PlayerManager.prop_PlayerManager_0
                            .prop_ArrayOf_Player_0[i].prop_APIUser_0) + new Color(0.2f, 0.2f, 0.2f);

                    nameplate.gameObject.transform.Find("Contents/Icon/Background").GetComponent<Image>().color = color;
                    nameplate.gameObject.transform.Find("Contents/Main/Background").GetComponent<ImageThreeSlice>()
                        .color = color;
                }
            }
        }

        public override void OnPlayerJoined(Player player)
        {
            if (!state)
                return;

            PlayerNameplate nameplate = player.prop_VRCPlayer_0.field_Public_PlayerNameplate_0;
            Color color = VRCPlayer.Method_Public_Static_Color_APIUser_0(player.prop_APIUser_0) +
                          new Color(0.2f, 0.2f, 0.2f);

            nameplate.gameObject.transform.Find("Contents/Icon/Background").GetComponent<Image>().color = color;
            nameplate.gameObject.transform.Find("Contents/Main/Background").GetComponent<ImageThreeSlice>().color =
                color;
        }
    }
}