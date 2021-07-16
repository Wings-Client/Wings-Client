using System;
using System.Linq;
using System.Net;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.IO;
using Il2CppSystem.Security.Cryptography;
using Il2CppSystem.Text;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;
using VRC;
using VRC.SDKBase;

namespace WingsClient
{
    public class Utils
    {
        private static Action<Player> requestedAction;
        private static VRC_SceneDescriptor descriptor;
        public static string[] cachedPrefabs;

        public static string SHA256(string unencrypted)
        {
            HashAlgorithm hashAlgorithm = new SHA256Managed();
            StringBuilder stringBuilder = new StringBuilder();
            foreach (byte b in hashAlgorithm.ComputeHash(Encoding.UTF8.GetBytes(unencrypted)))
            {
                stringBuilder.Append(b.ToString("x2"));
            }

            return stringBuilder.ToString();
        }

        public static void SelectPlayer(Player user)
        {
            QuickMenu.prop_QuickMenu_0.Method_Public_Void_Player_PDM_0(user);
        }

        public static void GetEachPlayer(Action<Player> act)
        {
            Utils.requestedAction = act;
            foreach (Player obj in PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0)
            {
                Utils.requestedAction(obj);
            }
        }

        public void SetImage(Image img, string path, Color color)
        {
            try
            {
                Texture2D texture2D = new Texture2D(2, 2);
                if (!Il2CppImageConversionManager.LoadImage(texture2D, File.ReadAllBytes(path))) return;
                Sprite sprite = Sprite.CreateSprite(texture2D,
                    new Rect(0f, 0f, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f),
                    100f, 0U, 0, default, false);
                img.sprite = sprite;
                img.color = color;
                img.m_Color = color;
            }
            catch (Exception ex)
            {
                MelonLogger.Error(ex);
            }
        }

        public void SaveImage(string fileName, string webLocation)
        {
            using (WebClient webClient = new WebClient())
            {
                Uri uri = new Uri(webLocation);
                try
                {
                    webClient.DownloadFileAsync(uri, fileName);
                }
                catch (Exception e)
                {
                    MelonLogger.Error(e.StackTrace);
                }
            }
        }

        public static void AlertV2(string title, string Content, string buttonname, Action action, string button2,
            Action action2)
        {
            VRCUiPopupManager.field_Private_Static_VRCUiPopupManager_0
                .Method_Public_Void_String_String_String_Action_String_Action_Action_1_VRCUiPopup_1(title, Content,
                    buttonname, action, button2, action2, null);
        }
        
        public static Player GetPlayer(string name)
        {
            List<Player> players = PlayerManager.field_Private_Static_PlayerManager_0.field_Private_List_1_Player_0;

            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].prop_APIUser_0.displayName == name)
                {
                    return players[i];
                }
            }

            return null;
        }

        public void SpawnDynamicPrefab()
        {
            if (descriptor is null)
            {
                descriptor = UnityEngine.Object.FindObjectOfType<VRC_SceneDescriptor>();
                cachedPrefabs = descriptor.DynamicPrefabs.ToArray().Where(p => p?.name != null).Select(p => p.name).ToArray();
            }

            if (descriptor is null)
                MelonLogger.Msg("Failed to find scene descriptor");
            else
            {
                VRC.Player target = Shared.TargetPlayer ?? VRC.Player.prop_Player_0;

                if (cachedPrefabs.Length > 0)
                    Networking.Instantiate(VRC_EventHandler.VrcBroadcastType.Always,
                        cachedPrefabs[0], 
                        Shared.annoy 
                            ? target.field_Private_VRCPlayerApi_0.GetBonePosition(HumanBodyBones.Head)
                            : target.transform.position,
                        target.transform.rotation);
                else MelonLogger.Msg("World has no dynamic prefabs.");
            }
        }

        public void ClearCache()
        {
            cachedPrefabs = null;
        }
        
        public static PlayerManager PlayerManager
        {
            get
            {
                return PlayerManager.field_Private_Static_PlayerManager_0;
            }
        }
        
        public static QuickMenu QuickMenu
        {
            get
            {
                return QuickMenu.prop_QuickMenu_0;
            }
        }
        
        public static class Colors
        {
            public static readonly Color primary = new Color(0.34f, 0f, 0.65f);

            public static readonly Color highlight = new Color(0.8f, 0.8f, 1f);

            public static readonly Color legendary = new Color(1f, 0.41f, 0.7f);

            public static readonly Color veteran = new Color(1f, 0.82f, 0f);

            public static readonly Color trusted = new Color(0.75f, 0.26f, 0.9f);

            public static readonly Color known = new Color(1f, 0.48f, 0.25f);

            public static readonly Color user = new Color(0.17f, 0.81f, 0.36f);

            public static readonly Color newuser = new Color(0.09f, 0.47f, 1f);

            public static readonly Color visitor = new Color(0.8f, 0.8f, 0.8f);

            public static readonly Color quest = new Color(0f, 0.87f, 0.25f);

            public static readonly Color black = new Color(0f, 0f, 0f);

            public static readonly Color red = new Color(1f, 0f, 0f);

            public static readonly Color green = new Color(0f, 1f, 0f);

            public static readonly Color aqua = new Color(0f, 1f, 1f);

            public static readonly Color orange = new Color(1f, 0.65f, 0f);

            public static readonly Color white = new Color(1f, 1f, 1f);
            
            
        }
    }
}