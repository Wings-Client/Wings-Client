using System.Collections.Generic;
using VRC;

namespace WingsClient.Modules
{
    public class Modules
    {
        public List<BaseModule> modules = new List<BaseModule>();
        public Esp esp;
        public Flight flight;

        public Modules()
        {
            this.modules.Add(this.esp = new Esp());
            this.modules.Add(this.flight = new Flight());
        }

        public void StartCoroutines()
        {
            //MelonCoroutines.Start(Shared.modules.portal.AutoPortal());
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