﻿using ColossalFramework.UI;
using HarmonyLib;
using System;
using UnityEngine;
using static GeneratedGroupPanel;

namespace BetterRoadToolbar
{
	// Alternatively could patch GeneratedGroupPanel.SpawnButtonEntry, but NExt2 redirects that method, so that wouldn't always work.
	[HarmonyPatch(typeof(GeneratedGroupPanel), "PopulateGroups", new Type[] { typeof(GroupFilter), typeof(Comparison<GroupInfo>) })]
	class SpawnButtonEntryPatch
	{
		[HarmonyPostfix]
		public static void Postfix(GroupFilter filter, Comparison<GroupInfo> comparison, GeneratedGroupPanel __instance, UITabstrip ___m_Strip)
		{
			if (!(__instance is RoadsGroupPanel))
            {
				// We only want the "Roads" main tab
				return;
            }

			string mainCategoryId = "MAIN_CATEGORY";

			foreach (var tab in ___m_Strip.tabs)
            {
				var button = tab as UIButton;

				if(!button)
                {
					// shouldn't happen?
					continue;
                }

				if(button.tooltip.Contains(Mod.Identifier))
                {
					string s = button.tooltip.Replace(mainCategoryId + "[" + Mod.Identifier, "");
					s = s.Replace("]:0", "");

					int val;
					bool result = int.TryParse(s, out val);

					if (!result)
					{
						Debug.Log(Mod.Identifier + "Unable to parse string: '" + button.tooltip + "'");
						return;
					}

					RoadCategory cat = (RoadCategory)val;

					if (!Enum.IsDefined(typeof(RoadCategory), cat))
					{
						Debug.Log(Mod.Identifier + "Unexpected RoadCategory value: '" + val + "'");
						return;
					}

					button.tooltip = RoadAnalyser.GetTooltip(cat);
					button.text = RoadAnalyser.GetToolbarTitle(cat);
				}
            }
		}
	}
}