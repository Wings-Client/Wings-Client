using UnityEngine;
using System.Collections;
using MelonLoader;
using VRC;

namespace WingsClient.Modules
{
    public class Esp : BaseModule
    {
        public override void OnStateChange(bool state)
        {
            
            foreach (Player player in PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0)
            {
                Esp.HighlightPlayer(player, state);
            }
        }

        // Token: 0x0600006D RID: 109 RVA: 0x00005744 File Offset: 0x00003944
        public override void OnConfigLoaded()
        {
            foreach (Player player in PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0)
            {
                Esp.HighlightPlayer(player, this.state);
            }
        }

        // Token: 0x0600006E RID: 110 RVA: 0x0000577C File Offset: 0x0000397C
        public override void OnPlayerJoined(Player player)
        {
            MelonCoroutines.Start(this.Delay(player));
        }

        // Token: 0x0600006F RID: 111 RVA: 0x0000578B File Offset: 0x0000398B
        public IEnumerator Delay(Player player)
        {
            if (player == null)
            {
                yield break;
            }

            int timeout = 0;
            while (player.gameObject == null && timeout < 30)
            {
                yield return new WaitForSeconds(1f);
                int num = timeout;
                timeout = num + 1;
            }

            Esp.HighlightPlayer(player, this.state);
            yield break;
        }

        // Token: 0x06000070 RID: 112 RVA: 0x000057A1 File Offset: 0x000039A1
        public IEnumerator DelayRefresh()
        {
            yield return null;
            foreach (Player player in PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0)
            {
                Esp.HighlightPlayer(player, this.state);
            }

            yield break;
        }

        // Token: 0x06000071 RID: 113 RVA: 0x000057B0 File Offset: 0x000039B0
        public static void HighlightPlayer(Player player, bool state)
        {
            Renderer renderer;
            if (player == null)
            {
                renderer = null;
            }
            else
            {
                Transform transform = player.transform.Find("SelectRegion");
                renderer = ((transform != null) ? transform.GetComponent<Renderer>() : null);
            }

            Renderer renderer2 = renderer;
            if (renderer2)
            {
                HighlightsFX.prop_HighlightsFX_0.Method_Public_Void_Renderer_Boolean_0(renderer2, state);
                renderer2.sharedMaterial.color = Utils.Colors.primary;
            }
        }
    }
}