using ICities;
using CitiesHarmony.API;
using UnityEngine;

namespace BetterRoadToolbar
{
    public class Mod : IUserMod
    {
        public string Name => "Better Road Toolbar";
        public string Description => "Adds more tabs in the Roads toolbar and changes sort order to make finding the right road that little bit easier.";

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
