using System;
using System.Collections;
using MelonLoader;

namespace WingsClient
{
    public abstract class CustomizedMelonMod:MelonMod

    {
    static CustomizedMelonMod()
    {
    }

    protected void DoAfterUiManagerInit(Action code)
    {
        MelonCoroutines.Start(OnUiManagerInitCoro(code));
    }

    private IEnumerator OnUiManagerInitCoro(Action code)
    {
        while (VRCUiManager.prop_VRCUiManager_0 == null)
            yield return null;

        code();
    }
    }
}