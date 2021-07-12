using MelonLoader;
using UnityEngine;
using VRC;

namespace WingsClient.Modules
{
    public abstract class BaseModule
    {
        public void SetState(bool? n_state = null)
        {
            if ((n_state ?? (!this.state)) == this.state)
            {
                return;
            }

            this.state = (n_state ?? (!this.state));
            MelonLogger.Msg(base.GetType().Name + (this.state ? " On" : " Off"));
            this.OnStateChange(this.state);
        }

        public virtual void OnStateChange(bool state)
        {
        }

        public virtual void OnConfigLoaded()
        {
        }

        public virtual void OnUnload()
        {
        }

        public virtual void OnReload()
        {
        }

        public virtual void OnUpdate()
        {
        }

        public virtual void OnLevelWasLoaded()
        {
        }

        public virtual void OnPlayerJoined(Player player)
        {
        }

        public virtual void OnPlayerLeft(Player player)
        {
        }

        public virtual void OnAvatarInitialized(GameObject avatar, VRCAvatarManager manager)
        {
        }

        public bool state;
    }
}