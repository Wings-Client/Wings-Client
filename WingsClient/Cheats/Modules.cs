using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MelonLoader;
using UnityEngine.Playables;
using VRC;
using WingsClient.Modules;
using UnityEngine;

namespace WingsClient.Modules
{
    public class Modules
    {
        public List<BaseModule> modules = new List<BaseModule>();
        public Esp esp;
        public Flight flight;
        public Speed speed;
        public TrustRankNameplate trustRankNameplate;
        public ItemESP itemEsp;
        public FPSUnlocker fpsUnlocker;
        public ItemOrbit itemOrbit;
        public FreezePickups freezePickups;
        public bool askForPortal;

        public PlayerList playerList;
        //public HideSelf hideSelf;

        public Modules()
        {
            this.modules.Add(this.esp = new Esp());
            this.modules.Add(this.flight = new Flight());
            this.modules.Add(this.trustRankNameplate = new TrustRankNameplate());
            this.modules.Add(this.itemEsp = new ItemESP());
            this.modules.Add(this.speed = new Speed());
            this.modules.Add(this.fpsUnlocker = new FPSUnlocker());
            this.modules.Add(this.itemOrbit = new ItemOrbit());
            this.modules.Add(this.freezePickups = new FreezePickups());
            this.modules.Add(this.playerList = new PlayerList());
            //this.modules.Add(this.hideSelf = new HideSelf());
        }

        public void StartCoroutines()
        {
            MelonCoroutines.Start(Initialize());
            //MelonCoroutines.Start(Shared.modules.portal.AutoPortal());
        }

        private IEnumerator Initialize()
        {
            while (true)
            {
                try
                {
                    NetworkManager.field_Internal_Static_NetworkManager_0.field_Internal_VRCEventDelegate_1_Player_0.field_Private_HashSet_1_UnityAction_1_T_0.Add(new Action<Player>(player => OnPlayerJoined(player)));
                    //LogWithPadding("OnPlayerJoined", true);
                }
                catch
                {
                }

                try
                {
                    NetworkManager.field_Internal_Static_NetworkManager_0.field_Internal_VRCEventDelegate_1_Player_1.field_Private_HashSet_1_UnityAction_1_T_0.Add(new Action<Player>(player => OnPlayerLeft(player)));
                    //  LogWithPadding("OnPlayerLeft", true);
                }
                catch
                {
                }
                yield return new WaitForEndOfFrame();
            }
        }


        public void OnPlayerJoined(Player player)
        {
            for (int i = 0; i < this.modules.Count; i++)
            {
                this.modules[i].OnPlayerJoined(player);
            }
        }

        public void OnPlayerLeft(Player player)
        {
            for (int i = 0; i < this.modules.Count; i++)
            {
                this.modules[i].OnPlayerLeft(player);
            }
        }

        public void OnLevelLoad()
        {
            for (int i = 0; i < this.modules.Count; i++)
            {
                this.modules[i].OnLevelWasLoaded();
            }
        }

        public void OnUnload()
        {
            for (int i = 0; i < this.modules.Count; i++)
            {
                this.modules[i].OnUnload();
            }
        }

        public void OnUpdate()
        {
            for (int i = 0; i < this.modules.Count; i++)
            {
                this.modules[i].OnUpdate();
            }
        }
    }
}