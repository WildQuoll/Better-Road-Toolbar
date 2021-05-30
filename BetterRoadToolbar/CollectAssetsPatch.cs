using ColossalFramework;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BetterRoadToolbar
{
	[HarmonyPatch(typeof(GeneratedGroupPanel), "CollectAssets")]
    class CollectAssetsPatch
    {
		[HarmonyPostfix]
        public static void Postfix(GeneratedGroupPanel.GroupFilter filter,
								   Comparison<GeneratedGroupPanel.GroupInfo > comparison,
								   ref PoolList<GeneratedGroupPanel.GroupInfo> __result,
								   GeneratedGroupPanel __instance)
        {
			if (!filter.IsFlagSet(GeneratedGroupPanel.GroupFilter.Net) ||
				__instance.service != ItemClass.Service.Road)
			{
				return;
			}

			var defaultRoadTabs = new string[]
			{
				"RoadsSmall",
				"RoadsMedium",
				"RoadsLarge",
				"RoadsHighway",

				// NExt2
				"RoadsTiny",
				"RoadsSmallHV",
				"RoadsBusways",
				"RoadsPedestrians"
			};

			// Keep intersections, toll booths, and similar tabs
			var toKeep = new List<GeneratedGroupPanel.GroupInfo>();
			foreach(var group in __result)
            {
				if (!defaultRoadTabs.Contains(group.name))
                {
					toKeep.Add(group);
                }
            }
			__result.Clear();

			var categoriesNeeded = new List<RoadCategory >();

			var toolManagerExists = Singleton<ToolManager>.exists;

			for (uint num = 0u; num < PrefabCollection<NetInfo>.LoadedCount(); num++)
			{
				NetInfo info = PrefabCollection<NetInfo>.GetLoaded(num);
				if (info != null &&
					info.GetSubService() == ItemClass.SubService.None &&
					info.GetService() == ItemClass.Service.Road &&
					info.category.StartsWith("Roads") && // filters out Snowfall DLC tram-only roads. alternatively, could patch GeneratedScrollPanel.CollectAssets to include them.
					(!toolManagerExists || info.m_availableIn.IsFlagSet(Singleton<ToolManager>.instance.m_properties.m_mode)) &&
					info.m_placementStyle == ItemClass.Placement.Manual)
				{
					var cats = RoadAnalyser.GetRoadCategories(info);

					foreach (var cat in cats)
					{
						if (!categoriesNeeded.Contains(cat))
						{
							categoriesNeeded.Add(cat);
						}
					}
				}
			}

			categoriesNeeded.Sort();

			foreach(var cat in categoriesNeeded)
            {
				__result.Add(RoadAnalyser.CreateGroup(cat));
            }

			foreach(var group in toKeep)
            {
				__result.Add(group);
            }
		}
    }
}