using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace BetterRoadToolbar
{
    [HarmonyPatch]
    public static class UIFilterPatches
    {
        // private type -> has to be accessed via reflection
        private static Type UIFilterType = typeof(GeneratedScrollPanel).GetNestedType("UIFilterType", BindingFlags.NonPublic);
        // public type nested inside private type -> has to be accessed via reflection
        private static Type FilterGroupType = UIFilterType.GetNestedType("FilterGroup");
        // Could be accessed as "ref object ___m_FilterGroup" in the patch, but writing to it wouldn't work then (probably due to "boxing").
        private static FieldInfo FilterGroupField = UIFilterType.GetField("m_filterGroup", BindingFlags.Public | BindingFlags.Instance);

        private static string[] ALL_ROAD_PANELS = Enum.GetValues(typeof(RoadCategory))
            .Cast<RoadCategory>()
            .Select(cat => Mod.Identifier + ((int)cat).ToString() + "Panel")
            .ToArray();

        private static string[] PARKING_FILTER_EXCLUSIONS = new[] { RoadCategory.Highway, RoadCategory.Industrial, RoadCategory.Rural, RoadCategory.Pedestrian }
            .Select(cat => Mod.Identifier + ((int)cat).ToString() + "Panel")
            .ToArray();

        // The filter is primarily intended for when the "Generate public transport tabs" setting is off
        private static string[] PUBLIC_TRANSPORT_FILTER_EXCLUSIONS =
            new[] { RoadCategory.Bus, RoadCategory.Monorail, RoadCategory.Tram, RoadCategory.Trolleybus, RoadCategory.MultiModal }
            .Select(cat => Mod.Identifier + ((int)cat).ToString() + "Panel")
            .ToArray();

        // GeneratedScrollPanel.UIFilterType is a private nested type, so the usual patching approach can't be used.
        public static MethodBase TargetMethod()
        {
            var c = AccessTools.GetDeclaredConstructors(UIFilterType);
            return c[0];
        }

        [HarmonyPostfix]
        public static void ConstructorPostfix(ref object __instance, 
                                              ref string ___m_Name,
                                              ref object ___m_FilterTypeMethod,
                                              ref string[] ___m_whiteListedPanels,
                                              ref string[] ___m_blackListedPanels)
        {
            if (!Mod.IsInGame() || !Mod.CurrentConfig.ShowAssetFilters)
            {
                return;
            }

            switch (___m_Name)
            {
                case "RoadsOneWay":
                case "RoadsTwoWay":
                case "RoadsNotDecorated":
                case "RoadsDecorated":
                    ___m_whiteListedPanels = ALL_ROAD_PANELS;
                    break;
                case "RoadsOneLane": // repurposed to "without parking"
                case "RoadsTwoLane": // repurposed to "with parking"
                    ___m_whiteListedPanels = ALL_ROAD_PANELS;
                    ___m_blackListedPanels = PARKING_FILTER_EXCLUSIONS;

                    // Use group different to that used by not-decorated/decorated ("Buildings").
                    FilterGroupField.SetValue(__instance, Enum.Parse(FilterGroupType, "AirportStyle"));
                    break;
                case "RoadsPublicTransport":
                    ___m_whiteListedPanels = ALL_ROAD_PANELS;
                    ___m_blackListedPanels = PUBLIC_TRANSPORT_FILTER_EXCLUSIONS;

                    // Use group different to that used by not-decorated/decorated ("Buildings").
                    FilterGroupField.SetValue(__instance, Enum.Parse(FilterGroupType, "Unique"));
                    break;
                default:
                    return;
            }

            if (___m_Name == "RoadsOneWay")
            {
                ___m_FilterTypeMethod = new Func<object, bool>(obj =>
                {
                    var netInfo = obj as NetInfo;
                    if (netInfo == null)
                    {
                        return false;
                    }

                    // Similar to vanilla, but doesn't consider one-way roads with bicycle contralows as two-way roads
                    return !RoadUtils.IsTwoWayRoad(netInfo);
                });
            }
            else if (___m_Name == "RoadsTwoWay")
            {
                ___m_FilterTypeMethod = new Func<object, bool>(obj =>
                {
                    var netInfo = obj as NetInfo;
                    if (netInfo == null)
                    {
                        return false;
                    }

                    // Similar to vanilla, but doesn't consider roads with bicycle counterlows as two-way roads
                    return RoadUtils.IsTwoWayRoad(netInfo);
                });
            }
            else if (___m_Name == "RoadsPublicTransport")
            {
                ___m_FilterTypeMethod = new Func<object, bool>(obj =>
                {
                    var netInfo = obj as NetInfo;
                    if (netInfo == null)
                    {
                        return false;
                    }

                    // Similar to vanilla, but doesn't consider roads with bicycle counterlows as two-way roads
                    return RoadUtils.ContainsAnyPublicTransportInfrastructure(netInfo);
                });
            }
            else if (___m_Name == "RoadsNotDecorated")
            {
                ___m_FilterTypeMethod = new Func<object, bool>(obj =>
                {
                    var netInfo = obj as NetInfo;
                    if (netInfo == null)
                    {
                        return false;
                    }

                    return !RoadUtils.IsDecorated(netInfo);
                });
            }
            else if (___m_Name == "RoadsDecorated")
            {
                ___m_FilterTypeMethod = new Func<object, bool>(obj =>
                {
                    var netInfo = obj as NetInfo;
                    if (netInfo == null)
                    {
                        return false;
                    }

                    return RoadUtils.IsDecorated(netInfo);
                });
            }
            else if (___m_Name == "RoadsOneLane")
            {
                ___m_FilterTypeMethod = new Func<object, bool>(obj =>
                {
                    var netInfo = obj as NetInfo;
                    if (netInfo == null)
                    {
                        return false;
                    }

                    return !netInfo.m_hasParkingSpaces;
                });
            }
            else if (___m_Name == "RoadsTwoLane")
            {
                ___m_FilterTypeMethod = new Func<object, bool>(obj =>
                {
                    var netInfo = obj as NetInfo;
                    if (netInfo == null)
                    {
                        return false;
                    }

                    return netInfo.m_hasParkingSpaces;
                });
            }
        }
    }
}
