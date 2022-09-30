using HarmonyLib;
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
            int firstOrder = GetCategoryOrder(first);
            int secondOrder = GetCategoryOrder(second);

            if (firstOrder == secondOrder)
            {
                return 0;
            }

            return ToComparisonInt(GetCategoryOrder(first) < GetCategoryOrder(second));
        }

        /// <summary>
        /// Rounds 'value' to the nearest multiple of 'step'.
        /// </summary>
        private static float RoundToNearestMultiple(float value, float step)
        {
            return Mathf.Round(value / step) * step;
        }

        private static int Compare(NetInfo first, NetInfo second)
        {
            bool firstIsHighway = RoadUtils.IsHighway(first);
            bool secondIsHighway = RoadUtils.IsHighway(second);

            if (firstIsHighway != secondIsHighway)
            {
                return ToComparisonInt(!firstIsHighway);
            }

            bool firstIsGravel = RoadUtils.IsGravel(first);
            bool secondIsGravel = RoadUtils.IsGravel(second);

            if (firstIsGravel != secondIsGravel)
            {
                return ToComparisonInt(firstIsGravel);
            }

            if (firstIsHighway)
            {
                // When sorting highways, one-way roads always before two-way
                bool firstIsTwoWay = RoadUtils.AllowsTwoWayVehicleTraffic(first);
                bool secondIsTwoWay = RoadUtils.AllowsTwoWayVehicleTraffic(second);

                if (firstIsTwoWay != secondIsTwoWay)
                {
                    return ToComparisonInt(secondIsTwoWay);
                }
            }

            float halfWidthPrecision = 0.25f;
            if (RoundToNearestMultiple(first.m_halfWidth, halfWidthPrecision) != RoundToNearestMultiple(second.m_halfWidth, halfWidthPrecision))
            {
                return ToComparisonInt(first.m_halfWidth < second.m_halfWidth);
            }

            float firstRoadwayWidth = RoadUtils.GetEffectiveRoadwayWidth(first);
            float secondRoadwayWidth = RoadUtils.GetEffectiveRoadwayWidth(second);

            float roadwayWidthPrecision = 0.5f;
            if (RoundToNearestMultiple(firstRoadwayWidth, roadwayWidthPrecision) != RoundToNearestMultiple(secondRoadwayWidth, roadwayWidthPrecision))
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

            if (Mod.CurrentConfig.UseStandardSortOrder)
            {
                int result = a.m_isCustomContent.CompareTo(b.m_isCustomContent);

                if (result != 0)
                {
                    return result;
                }

                // Use default sorting, but pre-sort by default category (Small roads ALWAYS before Medium for example).
                return SortUtils.CompareRoadCategories(a.category, b.category);
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
            if (!Mod.IsInGame())
            {
                return true;
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
            if (!Mod.IsInGame())
            {
                return true;
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
}