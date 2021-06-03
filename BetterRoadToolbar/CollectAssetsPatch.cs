﻿using ColossalFramework;
using HarmonyLib;
using System;
using System.Collections.Generic;
using static GeneratedGroupPanel;

namespace BetterRoadToolbar
{
	// This patch overrides the list of tabs that will be listed in the Roads toolbar (not their contents!).
	[HarmonyPatch(typeof(GeneratedGroupPanel), "CollectAssets")]
    class CollectAssetsPatch
    {
		[HarmonyPostfix]
        public static void Postfix(GroupFilter filter,
								   Comparison<GroupInfo > comparison,
								   ref PoolList<GroupInfo> __result,
								   GeneratedGroupPanel __instance)
        {
			if (!filter.IsFlagSet(GroupFilter.Net) ||
				__instance.service != ItemClass.Service.Road)
			{
				return;
			}

			// Intersections, toll booths, etc. and custom modded tabs
			var miscTabs = new List<GroupInfo>();
			foreach(var group in __result)
            {
				if (!RoadUtils.IsDefaultRoadCategory(group.name))
                {
					miscTabs.Add(group);
                }
            }
			__result.Clear();

			var roadCategoriesNeeded = new List<RoadCategory >();
			var miscCategoriesNeeded = new List<string>();

			var toolManagerExists = Singleton<ToolManager>.exists;

			for (uint i = 0u; i < PrefabCollection<NetInfo>.LoadedCount(); ++i)
			{
				NetInfo info = PrefabCollection<NetInfo>.GetLoaded(i);
				if (info != null &&
					info.GetSubService() == ItemClass.SubService.None &&
					info.GetService() == ItemClass.Service.Road &&
					info.category.StartsWith("Roads") && // filters out Snowfall DLC tram-only roads. alternatively, could patch GeneratedScrollPanel.CollectAssets to include them.
					(!toolManagerExists || info.m_availableIn.IsFlagSet(Singleton<ToolManager>.instance.m_properties.m_mode)) &&
					info.m_placementStyle == ItemClass.Placement.Manual)
				{
					if(Mod.CurrentConfig.IgnoreCustomTabs && !RoadUtils.IsDefaultRoadCategory(info.category))
                    {
						if (!miscCategoriesNeeded.Contains(info.category))
						{
							miscCategoriesNeeded.Add(info.category);
						}
						continue;
                    }

					var cats = RoadUtils.GetRoadCategories(info);

					foreach (var cat in cats)
					{
						if (!roadCategoriesNeeded.Contains(cat))
						{
							roadCategoriesNeeded.Add(cat);
						}
					}
				}
			}

			roadCategoriesNeeded.Sort();

			// Check which of the other tabs can be removed
			// Note: We only allow Buildings, in line with RoadsGroupPanel.groupFilter.
			for (uint i = 0u; i < PrefabCollection<BuildingInfo>.LoadedCount(); ++i)
			{
				BuildingInfo info = PrefabCollection<BuildingInfo>.GetLoaded(i);
				// We could check manual placement and all that, but it seems unnecessary
				if(info != null && info.GetService() == ItemClass.Service.Road)
                {
					if(!miscCategoriesNeeded.Contains(info.category))
                    {
						miscCategoriesNeeded.Add(info.category);
                    }
                }
			}

			// Re-create tabs
			foreach (var cat in roadCategoriesNeeded)
            {
				__result.Add(RoadUtils.CreateGroup(cat));
            }

			foreach(var miscTab in miscTabs)
            {
				if (miscCategoriesNeeded.Contains(miscTab.name))
				{
					__result.Add(miscTab);
				}
            }
		}
    }
}