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
		public static void Postfix(UITabstrip strip, string name, string category, bool isDefaultCategory, string localeID, string unlockText, string spriteBase, bool enabled, bool forceFillContainer, ref UIButton __result)
		{
			string id = "MAIN_CATEGORY";
			if (localeID == id && __result.tooltip.StartsWith(id))
            {
				string s = __result.tooltip.Replace(id + "[WQ.BRT/", "");
				s = s.Replace("]:0", "");

				RoadCategory cat = (RoadCategory)int.Parse(s);

				__result.tooltip = RoadAnalyser.GetTooltip(cat);
				__result.text = RoadAnalyser.GetToolbarTitle(cat);
            }
		}
	}
}