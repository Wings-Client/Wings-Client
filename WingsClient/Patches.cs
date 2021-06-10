using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AmplitudeSDKWrapper;
using BestHTTP;
using ExitGames.Client.Photon;
using Harmony;
using Il2CppSystem.Collections;
using MelonLoader;
using Photon.Realtime;
using RubyButtonAPI;
using UnhollowerBaseLib;
using UnityEngine;
using UnityEngine.UI;
using VRC;
using VRC.Core;
using VRC.Management;
using VRC.SDKBase;
using VRC.Udon;
using VRC.Udon.Common.Interfaces;
using VRC.UserCamera;
using VRCSDK2.Validation.Performance;
using static VRC.SDKBase.VRC_EventHandler;

namespace WingsClient
{
    class Patches
    {
        private static string newHWID = "";

        public static void Init(Harmony.HarmonyInstance harmony)
        {
            try
            {
                harmony.Patch(typeof(SystemInfo).GetProperty("deviceUniqueIdentifier").GetGetMethod(), new HarmonyMethod(AccessTools.Method(typeof(Patches), nameof(FakeHWID))));
                harmony.Patch(typeof(AmplitudeWrapper).GetMethod("PostEvents"), new HarmonyMethod(AccessTools.Method(typeof(Patches), nameof(VoidPatch))));
                harmony.Patch(typeof(AmplitudeWrapper).GetMethods().First((MethodInfo x) => x.Name == "LogEvent" && x.GetParameters().Length == 4), new HarmonyMethod(AccessTools.Method(typeof(Patches), nameof(VoidPatch))));
                MelonLogger.Msg("[Patch] Analystics");
            }
            catch
            {
                MelonLogger.Msg(ConsoleColor.Red, "[Patch] Analystics Error while Patching");
            }

            try
            {
                harmony.Patch(typeof(VRCAvatarManager).GetMethod("Method_Private_IEnumerator_ApiAvatar_GameObject_Boolean_Boolean_Boolean_0"), new HarmonyMethod(AccessTools.Method(typeof(Patches), nameof(AvatarSafety))));
                MelonLogger.Msg(ConsoleColor.Red, "[Patch] Portal");
            }
            catch
            {
                MelonLogger.Msg(ConsoleColor.Red, "[Patch] Portal Error while Patching");
            }

            try
            {
                harmony.Patch(typeof(PortalTrigger).GetMethod("OnTriggerEnter"), new HarmonyMethod(AccessTools.Method(typeof(Patches), nameof(EnterPortal))));
                MelonLogger.Msg(ConsoleColor.Red, "[Patch] Safety");
            }
            catch
            {
                MelonLogger.Msg(ConsoleColor.Red, "[Patch] Safety Error while Patching");
            }

            while (NetworkManager.field_Internal_Static_NetworkManager_0 == null)
            {
                Thread.Sleep(5);
            }

            NetworkManager.field_Internal_Static_NetworkManager_0.field_Internal_VRCEventDelegate_1_Player_0.field_Private_HashSet_1_UnityAction_1_T_0.Add(new System.Action<VRC.Player>(Shared.modules.OnPlayerJoined));
            NetworkManager.field_Internal_Static_NetworkManager_0.field_Internal_VRCEventDelegate_1_Player_1.field_Private_HashSet_1_UnityAction_1_T_0.Add(new System.Action<VRC.Player>(Shared.modules.OnPlayerLeft));
        }

        private static bool EnterPortal(PortalTrigger __instance)
        {
            if (!Shared.modules.askForPortal)
                return true;

            Utils.AlertV2("Portal", "Do you want to enter?", "Enter", new Action(() =>
            {
                VRCFlowManager.prop_VRCFlowManager_0.Method_Public_Void_String_String_WorldTransitionInfo_Action_1_String_Boolean_0(__instance.field_Private_PortalInternal_0.field_Private_ApiWorld_0.id, __instance.field_Private_PortalInternal_0.field_Private_String_1);
            }), "Delete", new Action(() =>
            {
                GameObject.Destroy(__instance.gameObject);
            }));
            return false;
        }

        private static bool AvatarSafety(VRCAvatarManager __instance, ApiAvatar __0, GameObject __1, bool __2, bool __3, bool __4)
        {
            for (int i = 0; i < Shared.avatarBlacklist.Length; i++)
            {
                if (__0.id == Shared.avatarBlacklist[i])
                    return false;
            }

            Renderer[] renderers = __1.GetComponentsInChildren<Renderer>(true);

           /* for (int i = 0; i < renderers.Length; i++)
            {

                Renderer renderer = renderers[i];

                for (int j = 0; j < Shared.meshBlacklist.Length; j++)
                {
                    if (renderer.gameObject.name.ToLower().Contains(Shared.meshBlacklist[j]))
                    {
                        GameObject.Destroy(renderer.gameObject);
                    }
                }
                if (renderer.materials.Count >= 100)
                {
                    GameObject.Destroy(renderer.gameObject);
                }
                else
                {
                    for (int j = 0; j < renderer.materials.Count; j++)
                    {
                        for (int k = 0; k < Shared.shaderBlacklist.Length; k++)
                        {
                            if (renderer.materials[j].shader.name.ToLower().Contains(Shared.shaderBlacklist[k]))
                            {
                                renderer.materials[j].shader = Shader.Find("VRChat/PC/Toon Lit Cutout");
                            }
                        }
                    }
                }
            }*/

            AudioSource[] audios = __1.GetComponentsInChildren<AudioSource>(true);
            if (audios.Length >= 100)
            {
                for (int i = 0; i < audios.Length; i++)
                {
                    GameObject.Destroy(audios[i].gameObject);
                }
            }

            Light[] lights = __1.GetComponentsInChildren<Light>(true);
            if (lights.Length >= 25)
            {
                for (int i = 0; i < lights.Length; i++)
                {
                    GameObject.Destroy(lights[i].gameObject);
                }
            }

            /*Collider[] colliders = __1.GetComponentsInChildren<Collider>(true);
            if (colliders.Length >= 25)
            {
                for (int i = 0; i < colliders.Length; i++)
                {
                    GameObject.Destroy(colliders[i].gameObject);
                }
            }

            Cloth[] cloths = __1.GetComponentsInChildren<Cloth>(true);
            if (cloths.Length >= 25)
            {
                for (int i = 0; i < cloths.Length; i++)
                {
                    GameObject.Destroy(cloths[i].gameObject);
                }
            }*/

            SkinnedMeshRenderer[] skinnedMeshRenderer = __1.GetComponentsInChildren<SkinnedMeshRenderer>(true);
            MeshFilter[] meshFilters = __1.GetComponentsInChildren<MeshFilter>(true);

            for (int i = 0; i < skinnedMeshRenderer.Length; i++)
            {
                SkinnedMeshRenderer mesh = skinnedMeshRenderer[i];
                if (!mesh.sharedMesh.isReadable)
                {
                    GameObject.Destroy(mesh.gameObject);
                    goto nonReadableSkinnedMesh;
                }
                int polyCount = 0;
                for (int j = 0; j < mesh.sharedMesh.subMeshCount; j++)
                {
                    polyCount += mesh.sharedMesh.GetTriangles(i).Length / 3;
                }
                if (polyCount > 2000000)
                {
                    GameObject.Destroy(mesh.gameObject);
                }
            nonReadableSkinnedMesh:;
            }

            for (int i = 0; i < meshFilters.Length; i++)
            {
                MeshFilter mesh = meshFilters[i];
                if (!mesh.sharedMesh.isReadable)
                {
                    GameObject.Destroy(mesh.gameObject);
                    goto nonReadableMeshFilter;
                }
                int polyCount = 0;
                for (int j = 0; j < mesh.sharedMesh.subMeshCount; j++)
                {
                    polyCount += mesh.sharedMesh.GetTriangles(i).Length / 3;
                }
                if (polyCount > 2000000)
                {
                    GameObject.Destroy(mesh.gameObject);
                }
            nonReadableMeshFilter:;
            }

            DynamicBone[] dynamicBone = __1.GetComponentsInChildren<DynamicBone>(true);
            DynamicBoneCollider[] dynamicBoneCollider = __1.GetComponentsInChildren<DynamicBoneCollider>(true);
            return true;
        }

        private static bool FakeHWID(ref string __result)
        {
            if (Patches.newHWID == "")
            {
                Patches.newHWID = KeyedHashAlgorithm.Create().ComputeHash(Encoding.UTF8.GetBytes(string.Format("{0}A-{1}{2}-{3}{4}-{5}{6}-3C-1F", new object[]
                {
                    new System.Random().Next(0, 9),
                    new System.Random().Next(0, 9),
                    new System.Random().Next(0, 9),
                    new System.Random().Next(0, 9),
                    new System.Random().Next(0, 9),
                    new System.Random().Next(0, 9),
                    new System.Random().Next(0, 9)
                }))).Select(delegate (byte x)
                {
                    byte b = x;
                    return b.ToString("x2");
                }).Aggregate((string x, string y) => x + y);
                MelonLogger.Msg("[HWID] new " + Patches.newHWID);
            }
            __result = Patches.newHWID;
            return false;
        }

        private static bool VoidPatch()
        {
            return false;
        }

        private static bool VoidPatchTrue(bool __result)
        {
            __result = true;
            return false;
        }

        private static bool VoidPatchFalse(bool __result)
        {
            __result = false;
            return false;
        }
    }
}
