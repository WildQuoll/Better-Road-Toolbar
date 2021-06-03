﻿using ColossalFramework;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BetterRoadToolbar
{
    class SortUtils
    {
        private static int ToComparisonInt(bool isLessThan)
        {
            return isLessThan ? -1 : 1;
        }

        private static int GetCategoryOrder(string cat)
        {
            switch (cat)
            {
                case "RoadsTiny": // NExt 2
                    return 0;
                case "RoadsSmall":
                    return 1;
                case "RoadsSmallHV": // NExt 2
                    return 2;
                case "RoadsMedium":
                    return 3;
                case "RoadsLarge":
                    return 4;
                case "RoadsHighway":
                    return 5;
                default:
                    return 6;
            };
        }

        public static int CompareRoadCategories(string first, string second)
        {
            return ToComparisonInt(GetCategoryOrder(first) < GetCategoryOrder(second));
        }

        private static int Compare(NetInfo first, NetInfo second)
        {
            if (Mathf.Abs(first.m_halfWidth - second.m_halfWidth) > 0.1f)
            {
                return ToComparisonInt(first.m_halfWidth < second.m_halfWidth);
            }

            float firstRoadwayWidth = RoadUtils.GetEffectiveRoadwayWidth(first);
            float secondRoadwayWidth = RoadUtils.GetEffectiveRoadwayWidth(second);

            if (Mathf.Abs(firstRoadwayWidth - secondRoadwayWidth) > 0.1f)
            {
                return ToComparisonInt(firstRoadwayWidth < secondRoadwayWidth); // narrower road before wider road
            }

            uint firstLaneCount = RoadUtils.GetLaneCount(first);
            uint secondLaneCount = RoadUtils.GetLaneCount(second);

            if (firstLaneCount != secondLaneCount)
            {
                return ToComparisonInt(firstLaneCount < secondLaneCount);
            }

            uint firstDirLaneCount = RoadUtils.GetHighestLaneCountPerDirection(first);
            uint secondDirLaneCount = RoadUtils.GetHighestLaneCountPerDirection(second);

            if (firstDirLaneCount != secondDirLaneCount)
            {
                return ToComparisonInt(firstDirLaneCount < secondDirLaneCount); // 2+2 before 1+3 before 4L1W
            }

            bool firstHasMonorail = RoadUtils.HasMonorail(first);
            bool secondHasMonorail = RoadUtils.HasMonorail(second);

            if (firstHasMonorail != secondHasMonorail)
            {
                return ToComparisonInt(secondHasMonorail); // no monorail before monorail
            }

            bool firstHasTramTracks = RoadUtils.HasTramTracks(first);
            bool secondHasTramTracks = RoadUtils.HasTramTracks(second);

            if (firstHasTramTracks != secondHasTramTracks)
            {
                return ToComparisonInt(secondHasTramTracks);
            }

            bool firstHasDedicatedTramLanes = RoadUtils.HasDedicatedTramLanes(first);
            bool secondHasDedicatedTramLanes = RoadUtils.HasDedicatedTramLanes(second);

            if (firstHasDedicatedTramLanes != secondHasDedicatedTramLanes)
            {
                return ToComparisonInt(secondHasDedicatedTramLanes);
            }

            bool firstHasTrolleybusWires = RoadUtils.HasTrolleybusWires(first);
            bool secondHasTrolleybusWires = RoadUtils.HasTrolleybusWires(second);

            if (firstHasTrolleybusWires != secondHasTrolleybusWires)
            {
                return ToComparisonInt(secondHasTrolleybusWires);
            }

            bool firstHasBusLanes = RoadUtils.HasBusLanes(first);
            bool secondHasBusLanes = RoadUtils.HasBusLanes(second);

            if (firstHasBusLanes != secondHasBusLanes)
            {
                return ToComparisonInt(secondHasBusLanes);
            }

            bool firstHasBikeLanes = RoadUtils.HasBikeLanes(first);
            bool secondHasBikeLanes = RoadUtils.HasBikeLanes(second);

            if (firstHasBikeLanes != secondHasBikeLanes)
            {
                return ToComparisonInt(secondHasBikeLanes);
            }

            float firstNoise = (first.m_netAI as RoadBaseAI).m_noiseAccumulation;
            float secondNoise = (second.m_netAI as RoadBaseAI).m_noiseAccumulation;

            if (firstNoise != secondNoise)
            {
                return ToComparisonInt(firstNoise > secondNoise); // pavement before grass before trees / no sound barriers before sound barriers
            }

            if (first.m_hasParkingSpaces != second.m_hasParkingSpaces)
            {
                return ToComparisonInt(second.m_hasParkingSpaces); // no parking before parking
            }

            uint firstAuxiliaryLaneCount = RoadUtils.GetAuxiliaryLaneCount(first);
            uint secondAuxiliaryLaneCount = RoadUtils.GetAuxiliaryLaneCount(second);

            if (firstAuxiliaryLaneCount != secondAuxiliaryLaneCount)
            {
                return ToComparisonInt(firstAuxiliaryLaneCount < secondAuxiliaryLaneCount); // fewer bike lanes/parking lanes before more
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
            if(Mod.CurrentConfig.UseStandardSortOrder)
            {
                // Pre-sort the roads by their default category (Small roads ALWAYS before Medium for example).
                int catResult = SortUtils.CompareRoadCategories(a.category, b.category);

                if (catResult == 0)
                {
                    return true; // fall back to default sorting
                }

                __result = catResult;
                return false;
            }

            int result = SortUtils.Sort(a, b);

            if (result == 0)
            {
                return true; // fall back to default sorting
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
            if (Mod.CurrentConfig.UseStandardSortOrder)
            {
                return true;
            }

            int result = SortUtils.Sort(a, b);

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