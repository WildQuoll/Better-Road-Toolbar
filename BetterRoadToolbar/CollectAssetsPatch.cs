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

			__result.Clear();

			var categoriesNeeded = new List<RoadCategory >();

			var toolManagerExists = Singleton<ToolManager>.exists;

			for (uint num = 0u; num < PrefabCollection<NetInfo>.LoadedCount(); num++)
			{
				NetInfo info = PrefabCollection<NetInfo>.GetLoaded(num);
				if (info != null &&
					info.GetSubService() == ItemClass.SubService.None &&
					info.GetService() == ItemClass.Service.Road &&
					(!toolManagerExists || info.m_availableIn.IsFlagSet(Singleton<ToolManager>.instance.m_properties.m_mode)) &&
					info.m_placementStyle == ItemClass.Placement.Manual)
				{
					RoadCategory roadType = RoadAnalyser.GetRoadCategory(info);

					if (!categoriesNeeded.Contains(roadType))
					{
						categoriesNeeded.Add(roadType);
					}
				}
			}

			categoriesNeeded.Sort();

			foreach(var cat in categoriesNeeded)
            {
				__result.Add(RoadAnalyser.CreateGroup(cat));
            }
		}
    }
}