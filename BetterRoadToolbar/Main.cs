using ICities;
using CitiesHarmony.API;
using UnityEngine;

namespace BetterRoadToolbar
{
    public class Mod : IUserMod
    {
        public string Name => "Better Road Toolbar";
        public string Description => "TODO Write me.";

        public void OnEnabled()
        {
            HarmonyHelper.DoOnHarmonyReady(() => Patcher.PatchAll());
        }

        public void OnDisabled()
        {
            if (HarmonyHelper.IsHarmonyInstalled) Patcher.UnpatchAll();
        }
    }
}
