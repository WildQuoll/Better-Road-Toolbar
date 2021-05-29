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

			var categoriesNeeded = new SortedDictionary<RoadType, SortedDictionary< uint /* cell width */, SortedDictionary< uint /* lane count */, uint> > >();

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
					RoadType roadType = RoadAnalyser.GetRoadType(info);
					uint cellWidth = RoadAnalyser.GetCellWidth(info);
					uint laneCount = RoadAnalyser.GetLaneCount(info);

					if (!categoriesNeeded.ContainsKey(roadType))
                    {
						categoriesNeeded.Add(roadType, new SortedDictionary<uint, SortedDictionary< uint, uint > >());
                    }

					if (!categoriesNeeded[roadType].ContainsKey(cellWidth))
                    {
						categoriesNeeded[roadType].Add(cellWidth, new SortedDictionary<uint, uint>());
					}

					if (!categoriesNeeded[roadType][cellWidth].ContainsKey(laneCount))
                    {
						categoriesNeeded[roadType][cellWidth].Add(laneCount, 1);
                    }
					else
                    {
						categoriesNeeded[roadType][cellWidth][laneCount] += 1;
                    }
				}
			}

			foreach(var r in categoriesNeeded)
            {
				var roadType = r.Key;
				foreach(var s in r.Value)
                {
					var cellWidth = s.Key;
					foreach (var t in s.Value)
					{
						var laneCount = t.Key;
						__result.Add(RoadAnalyser.CreateGroup(roadType, cellWidth, laneCount));
					}
                }
            }
		}
    }
}