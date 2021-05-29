using ColossalFramework;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BetterRoadToolbar
{
    class ItemsTypeSort
    {
        private static int ToComparisonInt(bool isLessThan)
        {
            return isLessThan ? -1 : 1;
        }

        private static int Compare(NetInfo first, NetInfo second)
        {
            if (Mathf.Abs(first.m_halfWidth - second.m_halfWidth) > 0.1f)
            {
                return ToComparisonInt(first.m_halfWidth < second.m_halfWidth);
            }

            float firstRoadwayWidth = RoadAnalyser.GetEffectiveRoadwayWidth(first);
            float secondRoadwayWidth = RoadAnalyser.GetEffectiveRoadwayWidth(second);

            if (Mathf.Abs(firstRoadwayWidth - secondRoadwayWidth) > 0.1f)
            {
                return ToComparisonInt(firstRoadwayWidth < secondRoadwayWidth); // narrower road before wider road
            }

            uint firstLaneCount = RoadAnalyser.GetLaneCount(first);
            uint secondLaneCount = RoadAnalyser.GetLaneCount(second);

            if (firstLaneCount != secondLaneCount)
            {
                return ToComparisonInt(firstLaneCount < secondLaneCount);
            }

            uint firstAuxiliaryLaneCount = RoadAnalyser.GetAuxiliaryLaneCount(first);
            uint secondAuxiliaryLaneCount = RoadAnalyser.GetAuxiliaryLaneCount(second);

            if (firstAuxiliaryLaneCount != secondAuxiliaryLaneCount)
            {
                return ToComparisonInt(firstAuxiliaryLaneCount < secondAuxiliaryLaneCount);
            }

            bool firstTwoWay = RoadAnalyser.IsTwoWay(first);
            bool secondTwoWay = RoadAnalyser.IsTwoWay(second);

            if (firstTwoWay != secondTwoWay)
            {
                return ToComparisonInt(firstTwoWay); // two way before one way
            }

            bool firstHasMonorail = RoadAnalyser.HasMonorail(first);
            bool secondHasMonorail = RoadAnalyser.HasMonorail(second);

            if (firstHasMonorail != secondHasMonorail)
            {
                return ToComparisonInt(secondHasMonorail); // no monorail before monorail
            }

            bool firstHasTramTracks = RoadAnalyser.HasTramTracks(first);
            bool secondHasTramTracks = RoadAnalyser.HasTramTracks(second);

            if (firstHasTramTracks != secondHasTramTracks)
            {
                return ToComparisonInt(secondHasTramTracks);
            }

            bool firstHasDedicatedTramLanes = RoadAnalyser.HasDedicatedTramLanes(first);
            bool secondHasDedicatedTramLanes = RoadAnalyser.HasDedicatedTramLanes(second);

            if (firstHasDedicatedTramLanes != secondHasDedicatedTramLanes)
            {
                return ToComparisonInt(secondHasDedicatedTramLanes);
            }

            bool firstHasTrolleybusWires = RoadAnalyser.HasTrolleybusWires(first);
            bool secondHasTrolleybusWires = RoadAnalyser.HasTrolleybusWires(second);

            if (firstHasTrolleybusWires != secondHasTrolleybusWires)
            {
                return ToComparisonInt(secondHasTrolleybusWires);
            }

            bool firstHasBusLanes = RoadAnalyser.HasBusLanes(first);
            bool secondHasBusLanes = RoadAnalyser.HasBusLanes(second);

            if (firstHasBusLanes != secondHasBusLanes)
            {
                return ToComparisonInt(secondHasBusLanes);
            }

            bool firstHasBikeLanes = RoadAnalyser.HasBikeLanes(first);
            bool secondHasBikeLanes = RoadAnalyser.HasBikeLanes(second);

            if (firstHasBikeLanes != secondHasBikeLanes)
            {
                return ToComparisonInt(secondHasBikeLanes);
            }

            if (first.m_hasParkingSpaces != second.m_hasParkingSpaces)
            {
                return ToComparisonInt(second.m_hasParkingSpaces); // no parking before parking
            }

            if (first.m_UIPriority != second.m_UIPriority)
            {
                return ToComparisonInt(first.m_UIPriority < second.m_UIPriority);
            }

            return 0;
        }

        //! returns 0 if items can't be sorted
        public static int Sort(PrefabInfo a, PrefabInfo b)
        {
            var aNetInfo = a as NetInfo;
            if (!aNetInfo)
            {
                return 0;
            }

            var bNetInfo = b as NetInfo;
            if (!bNetInfo)
            {
                return 0;
            }

            if (aNetInfo.GetSubService() != ItemClass.SubService.None ||
                bNetInfo.GetSubService() != ItemClass.SubService.None ||
                aNetInfo.GetService() != ItemClass.Service.Road ||
                bNetInfo.GetService() != ItemClass.Service.Road)
            {
                return 0;
            }

            return Compare(aNetInfo, bNetInfo);
        }
    }

	[HarmonyPatch(typeof(GeneratedScrollPanel), "ItemsTypeSort")]
	class ItemsTypeSortPatch
	{
		[HarmonyPrefix]
        static bool Prefix(PrefabInfo a, PrefabInfo b, ref int __result)
        {
            int result = ItemsTypeSort.Sort(a, b);

            if (result == 0)
            {
                return true; // use default sort
            }
            else
            {
                __result = result;
                return false;
            }
        }
    }

    [HarmonyPatch(typeof(GeneratedScrollPanel), "ItemsTypeReverseSort")]
    class ItemsTypeReverseSortPatch
    {
        [HarmonyPrefix]
        static bool Prefix(PrefabInfo a, PrefabInfo b, ref int __result)
        {
            int result = ItemsTypeSort.Sort(a, b);

            if (result == 0)
            {
                return true; // use default sort
            }
            else
            {
                __result = result;
                return false;
            }
        }
    }
}