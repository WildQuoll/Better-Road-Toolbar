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
			string mainCategoryId = "MAIN_CATEGORY";

			if (localeID == mainCategoryId && __result.tooltip.Contains(Mod.Identifier))
            {
				string s = __result.tooltip.Replace(mainCategoryId + "[" + Mod.Identifier, "");
				s = s.Replace("]:0", "");

				int val;
				bool result = int.TryParse(s, out val);

				if (!result)
				{
					Debug.Log(Mod.Identifier + "Unable to parse string: '" + __result.tooltip + "'");
					return;
				}

				RoadCategory cat = (RoadCategory)val;

				if(!Enum.IsDefined(typeof(RoadCategory), cat))
                {
					Debug.Log(Mod.Identifier + "Unexpected RoadCategory value: '" + val + "'");
					return;
				}

				__result.tooltip = RoadAnalyser.GetTooltip(cat);
				__result.text = RoadAnalyser.GetToolbarTitle(cat);
            }
		}
	}
}