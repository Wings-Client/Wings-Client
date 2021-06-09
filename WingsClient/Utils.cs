using System;
using System.Collections.Generic;
using Il2CppSystem.Collections.Generic;
using Il2CppSystem.IO;
using Il2CppSystem.Security.Cryptography;
using Il2CppSystem.Text;
using MelonLoader;
using UnityEngine;
using UnityEngine.UI;
using VRC;

namespace WingsClient
{
    public class Utils
    {
        private static Action<Player> requestedAction;

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

        public static void SetImage(Image img, string path)
        {
            try
            {
                Texture2D texture2D = new Texture2D(2, 2);
                if (Il2CppImageConversionManager.LoadImage(texture2D, File.ReadAllBytes(path)))
                {
                    Sprite sprite = Sprite.CreateSprite(texture2D,
                        new Rect(0f, 0f, (float) texture2D.width, (float) texture2D.height), new Vector2(0.5f, 0.5f),
                        100f, 0U, 0, default(Vector4), false);
                    img.sprite = sprite;
                }
            }
            catch (Exception ex)
            {
                MelonLogger.Msg(ex);
            }
        }
    }
}