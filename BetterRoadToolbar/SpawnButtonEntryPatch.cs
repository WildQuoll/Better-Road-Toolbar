using ColossalFramework.UI;
using HarmonyLib;
using System;

namespace BetterRoadToolbar
{
	[HarmonyPatch(typeof(GeneratedGroupPanel), "SpawnButtonEntry",
		new Type[] { typeof(UITabstrip), typeof(string), typeof(string), typeof(bool), typeof(string), typeof(string), typeof(string), typeof(bool), typeof(bool) })]
	class SpawnButtonEntryPatch
	{
		[HarmonyPostfix]
		public static void Postfix(UITabstrip strip, string name, string category, bool isDefaultCategory, string localeID, string unlockText, string spriteBase, bool enabled, bool forceFillContainer, ref UIButton __result)
		{
			string cat = "MAIN_CATEGORY";
			if (localeID == cat && __result.tooltip.StartsWith(cat))
            {
				__result.tooltip = __result.tooltip.Replace(cat + "[", "");
				__result.tooltip = __result.tooltip.Replace("]:0", "");
            }
		}
	}
}