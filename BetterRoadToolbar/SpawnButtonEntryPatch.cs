using ColossalFramework.UI;
using HarmonyLib;
using System;
using UnityEngine;

namespace BetterRoadToolbar
{
	[HarmonyPatch(typeof(GeneratedGroupPanel), "SpawnButtonEntry",
		new Type[] { typeof(UITabstrip), typeof(string), typeof(string), typeof(bool), typeof(string), typeof(string), typeof(string), typeof(bool), typeof(bool) })]
	class SpawnButtonEntryPatch
	{
		[HarmonyPostfix]
		public static void Postfix(string localeID, ref UIButton __result)
		{
			string cat = "MAIN_CATEGORY";

			Debug.Log(localeID + "..." + __result.tooltip); // temp
			if (localeID == cat && __result.tooltip.StartsWith(cat))
            {
				__result.tooltip = __result.tooltip.Replace(cat + "[", "");
				__result.tooltip = __result.tooltip.Replace("]:0", "");
            }
		}
	}
}