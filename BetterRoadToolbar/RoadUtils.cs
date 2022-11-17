using System;
using System.Collections.Generic;
using UnityEngine;

namespace BetterRoadToolbar
{
    enum RoadCategory
    {
        Pedestrian = 1,
        Urban_1U,
        Urban_2U_2LMax,
        Urban_2U_3LMin,
        Urban_3U,
        Urban_4U_4LMax,
        Urban_4U_5LMin,
        Urban_5UMin,
        Industrial,
        Rural,
        Highway,

        Bike,
        Bus,
        Tram,
        Trolleybus,
        Monorail
    }

    static class RoadUtils
    {
        public static bool IsDefaultRoadCategory(string cat)
        {
            switch (cat)
            {
                case "RoadsSmall":
                case "RoadsMedium":
                case "RoadsLarge":
                case "RoadsHighway":
                // NExt2
                case "RoadsTiny":
                case "RoadsSmallHV":
                    return true;
                case "RoadsBusways":
                    return Mod.CurrentConfig.CreateTabsForTransportModes; // treat it as default only if we're creating our own Bus tab
                default:
                    return false;
            };
        }

        public const string BRIDGES_DLC_CATEGORY = "RoadsModderPack";
        public const string PLAZAS_DLC_CATEGORY = "BeautificationPedestrianZoneEssentials";

        public static string GetToolbarTitle(RoadCategory cat)
        {
            switch (cat)
            {
                case RoadCategory.Pedestrian:
                    return "Ped";
                case RoadCategory.Urban_1U:
                    return "1U";
                case RoadCategory.Urban_2U_2LMax:
                    return "2U";
                case RoadCategory.Urban_2U_3LMin:
                    return "2U 3L+";
                case RoadCategory.Urban_3U:
                    return "3U";
                case RoadCategory.Urban_4U_4LMax:
                    return "4U";
                case RoadCategory.Urban_4U_5LMin:
                    return "4U 5L+";
                case RoadCategory.Urban_5UMin:
                    return "5U+";
                case RoadCategory.Industrial:
                    return "Ind";
                case RoadCategory.Rural:
                    return "Rural";
                case RoadCategory.Highway:
                    return "Hwy";
                case RoadCategory.Bike:
                    return "Bike";
                case RoadCategory.Bus:
                    return "Bus";
                case RoadCategory.Trolleybus:
                    return "Trolley";
                case RoadCategory.Tram:
                    return "Tram";
                case RoadCategory.Monorail:
                    return "Mono";
                default:
                    break;
            }
            return "";
        }

        public static string GetTooltip(RoadCategory cat)
        {
            switch (cat)
            {
                case RoadCategory.Pedestrian:
                    return "Pedestrianised and traffic-calmed streets";
                case RoadCategory.Urban_1U:
                    return "Urban, 1U wide";
                case RoadCategory.Urban_2U_2LMax:
                    return "Urban, 2U wide, ≤2 lanes";
                case RoadCategory.Urban_2U_3LMin:
                    return "Urban, 2U wide, ≥3 lanes";
                case RoadCategory.Urban_3U:
                    return "Urban, 3U wide";
                case RoadCategory.Urban_4U_4LMax:
                    return "Urban, 4U wide, ≤4 lanes";
                case RoadCategory.Urban_4U_5LMin:
                    return "Urban, 4U wide, ≥5 lanes";
                case RoadCategory.Urban_5UMin:
                    return "Urban, 5U wide or wider";
                case RoadCategory.Rural:
                    return "Rural";
                case RoadCategory.Industrial:
                    return "Industrial";
                case RoadCategory.Highway:
                    return "Highways";
                case RoadCategory.Bike:
                    return "Bike roads";
                case RoadCategory.Bus:
                    return "Bus roads";
                case RoadCategory.Trolleybus:
                    return "Trolleybus roads";
                case RoadCategory.Tram:
                    return "Tram roads";
                case RoadCategory.Monorail:
                    return "Monorail roads";
                default:
                    break;
            }
            return "";
        }

        private static bool HasDirection(NetInfo.Lane lane, NetInfo.Direction direction)
        {
            return (lane.m_direction & direction) != 0;
        }

        private static bool IsVehicleLane(NetInfo.Lane lane)
        {
            var laneTypes = (NetInfo.LaneType.Vehicle | NetInfo.LaneType.TransportVehicle);
            var vehicleTypes = (VehicleInfo.VehicleType.Car | VehicleInfo.VehicleType.Tram | VehicleInfo.VehicleType.Trolleybus);
            var vehicleCategories = (VehicleInfo.VehicleCategoryPart1.PassengerCar |
                VehicleInfo.VehicleCategoryPart1.Bus |
                VehicleInfo.VehicleCategoryPart1.Trolleybus |
                VehicleInfo.VehicleCategoryPart1.Tram | VehicleInfo.VehicleCategoryPart1.CargoTruck | VehicleInfo.VehicleCategoryPart1.Taxi);

            return (lane.m_laneType & laneTypes) != 0 &&
                   (lane.m_vehicleType & vehicleTypes) != 0 &&
                   (lane.m_vehicleCategoryPart1 & vehicleCategories) != 0;
        }

        private static bool IsCarLane(NetInfo.Lane lane)
        {
            var laneType = NetInfo.LaneType.Vehicle;
            var vehicleType = VehicleInfo.VehicleType.Car;
            var vehicleCategory = VehicleInfo.VehicleCategoryPart1.PassengerCar;

            return (lane.m_laneType & laneType) != 0 &&
                   (lane.m_vehicleType & vehicleType) != 0 &&
                   (lane.m_vehicleCategoryPart1 & vehicleCategory) != 0;
        }

        private static bool IsAuxiliaryLane(NetInfo.Lane lane)
        {
            if (lane.m_laneType == NetInfo.LaneType.Parking || lane.m_vehicleType == VehicleInfo.VehicleType.Bicycle)
            {
                return true;
            }

            return false;
        }

        public static bool AllowsTwoWayVehicleTraffic(NetInfo info)
        {
            return info.m_backwardVehicleLaneCount > 0 && info.m_forwardVehicleLaneCount > 0;
        }

        /// Counts car lanes. A car lane is a lane that allows "normal" cars on it.
        public static uint GetCarLaneCount(NetInfo info)
        {
            uint count = 0;

            foreach (var lane in info.m_lanes)
            {
                if (IsCarLane(lane))
                {
                    count += 1;
                }
            }

            return count;
        }

        ///
        /// Counts vehicle lanes. A vehicle lane is a lane that allows some or all "car-sized" (cars, buses, trams, etc.) vehicles on it.
        /// Bicycle lanes are not a vehicle lanes. Lanes restricted to emergency vehicles are also not counted.
        /// 
        public static uint GetVehicleLaneCount(NetInfo info)
        {
            uint count = 0;

            foreach (var lane in info.m_lanes)
            {
                if (IsVehicleLane(lane))
                {
                    count += 1;
                }
            }

            return count;
        }

        public static uint GetHighestLaneCountPerDirection(NetInfo info)
        {
            return Math.Max(GetVehicleLaneCount(info, NetInfo.Direction.Forward), GetVehicleLaneCount(info, NetInfo.Direction.Backward));
        }

        public static uint GetVehicleLaneCount(NetInfo info, NetInfo.Direction direction)
        {
            uint count = 0;

            foreach (var lane in info.m_lanes)
            {
                if (HasDirection(lane, direction) && IsVehicleLane(lane))
                {
                    count += 1;
                }
            }

            return count;
        }

        public static uint GetAuxiliaryLaneCount(NetInfo info)
        {
            uint count = 0;

            foreach (var lane in info.m_lanes)
            {
                if (IsAuxiliaryLane(lane))
                {
                    count += 1;
                }
            }

            return count;
        }

        public static uint GetCellWidth(NetInfo info)
        {
            return (uint)Mathf.Round(info.m_halfWidth / 4.0f);
        }

        private static bool IsSlowTraffic(NetInfo info)
        {
            return (info.m_hasPedestrianLanes && info.m_averageVehicleLaneSpeed <= 0.5f);
        }

        private static bool IsPedestrianised(NetInfo info)
        {
            if (!info.m_hasPedestrianLanes)
            {
                return false;
            }

            foreach (var lane in info.m_lanes)
            {
                if ((lane.m_vehicleCategoryPart1 & VehicleInfo.VehicleCategoryPart1.PassengerCar) != 0)
                {
                    return false;
                }
            }

            return true;
        }

        public static bool IsGravel(NetInfo info)
        {
            return (info.m_setVehicleFlags & Vehicle.Flags.OnGravel) != 0;
        }

        public static bool IsHighway(NetInfo info)
        {
            return (info.m_netAI as RoadBaseAI).m_highwayRules && !info.m_hasPedestrianLanes;
        }

        private static bool IsIndustrial(NetInfo info)
        {
            return (info.m_requiredExpansion & (SteamHelper.ExpansionBitMask.Industry | SteamHelper.ExpansionBitMask.Airport)) != 0;
        }

        public static bool ShouldKeepExistingCategory(string cat)
        {
            switch (cat)
            {
                case RoadUtils.BRIDGES_DLC_CATEGORY:
                    if (Mod.CurrentConfig.IgnoreBridgesDlcTab)
                    {
                        return true;
                    }
                    break;
                case RoadUtils.PLAZAS_DLC_CATEGORY:
                    if (Mod.CurrentConfig.IgnorePlazasDlcTab)
                    {
                        return true;
                    }
                    break;
                default:
                    if (Mod.CurrentConfig.IgnoreOtherCustomTabs && !RoadUtils.IsDefaultRoadCategory(cat))
                    {
                        return true;
                    }
                    break;
            }

            return false;
        }

        public static List<RoadCategory> GetRoadCategories(NetInfo info)
        {
            var cats = new List<RoadCategory>();

            if (Mod.CurrentConfig.CreateTabsForTransportModes)
            {
                if (HasBikeLanes(info))
                {
                    cats.Add(RoadCategory.Bike);
                }

                if (HasBusLanes(info))
                {
                    cats.Add(RoadCategory.Bus);
                }

                if (HasTramTracks(info))
                {
                    cats.Add(RoadCategory.Tram);
                }

                if (HasTrolleybusWires(info))
                {
                    cats.Add(RoadCategory.Trolleybus);
                }

                if (HasMonorail(info))
                {
                    cats.Add(RoadCategory.Monorail);
                }
            }

            // Roads categorised into any of the above categories should not be added
            // to "by width" categories.
            if (cats.Count > 0)
            {
                return cats;
            }

            if (IsPedestrianised(info)
                || (Mod.CurrentConfig.TreatSlowRoadsAsPedestrian && IsSlowTraffic(info)))
            {
                cats.Add(RoadCategory.Pedestrian);
                return cats;
            }

            if (Mod.CurrentConfig.CreateIndustrialTab && IsIndustrial(info))
            {
                cats.Add(RoadCategory.Industrial);
                return cats;
            }

            if (IsHighway(info))
            {
                cats.Add(RoadCategory.Highway);
                return cats;
            }

            if (!info.m_createPavement)
            {
                cats.Add(RoadCategory.Rural);
                return cats;
            }

            uint laneCount = GetVehicleLaneCount(info);
            uint cellWidth = GetCellWidth(info);

            switch (cellWidth)
            {
                case 0:
                case 1:
                    cats.Add(RoadCategory.Urban_1U);
                    break;
                case 2:
                    cats.Add(laneCount <= 2 ? RoadCategory.Urban_2U_2LMax : RoadCategory.Urban_2U_3LMin);
                    break;
                case 3:
                    cats.Add(RoadCategory.Urban_3U);
                    break;
                case 4:
                    cats.Add(laneCount <= 4 ? RoadCategory.Urban_4U_4LMax : RoadCategory.Urban_4U_5LMin);
                    break;

                default:
                    cats.Add(RoadCategory.Urban_5UMin);
                    break;

            }
            return cats;
        }

        public static GeneratedGroupPanel.GroupInfo CreateGroup(RoadCategory roadType)
        {
            return new GeneratedGroupPanel.GroupInfo(Mod.Identifier + ((int)roadType).ToString(), (int)roadType);
        }

        private class LaneExtent
        {
            public LaneExtent(float min, float max)
            {
                this.min = min;
                this.max = max;
            }

            public float min;
            public float max;
        }

        public static bool HasMedian(NetInfo info)
        {
            float min = float.MinValue;
            float max = float.MaxValue;

            foreach (var lane in info.m_lanes)
            {
                if (IsVehicleLane(lane))
                {
                    float halfWidth = lane.m_width / 2.0f;

                    if (lane.m_position < 0.0f)
                    {
                        min = Math.Max(min, lane.m_position + halfWidth);
                    }
                    else
                    {
                        max = Math.Min(max, lane.m_position - halfWidth);
                    }
                }
            }

            if (min >= 0.0f || max <= 0.0f)
            {
                return false;
            }

            float threshold = 1.5f;
            return max - min >= threshold;
        }

        /// <summary>
        /// Returns the "effective" roadway width. This is the distance between the centres of the two outermost lanes used by moving traffic.
        /// Parking lanes, bicycle lanes and "emergency" lanes are ignored.
        /// </summary>
        public static float GetEffectiveRoadwayWidth(NetInfo info)
        {
            float min = float.MaxValue;
            float max = float.MinValue;

            foreach (var lane in info.m_lanes)
            {
                if (IsVehicleLane(lane))
                {
                    min = Mathf.Min(lane.m_position, min);
                    max = Mathf.Max(lane.m_position, max);
                }
            }

            if (min > max)
            {
                return 0.0f;
            }

            return max - min;
        }

        public static bool HasBikeLanes(NetInfo info)
        {
            return (info.m_vehicleTypes & VehicleInfo.VehicleType.Bicycle) != 0;
        }

        public static bool HasBusLanes(NetInfo info)
        {
            // Bus lanes which also allow, but discourage, cars.
            if ((info.m_laneTypes & NetInfo.LaneType.TransportVehicle) != 0)
            {
                return true;
            }

            // Bus lanes which strictly prohibit cars
            foreach (var lane in info.m_lanes)
            {
                if ((lane.m_laneType & NetInfo.LaneType.Vehicle) != 0 &&
                    (lane.m_vehicleType & VehicleInfo.VehicleType.Car) != 0 &&
                    (lane.m_vehicleCategoryPart1 & VehicleInfo.VehicleCategoryPart1.Bus) != 0 &&
                    (lane.m_vehicleCategoryPart1 & VehicleInfo.VehicleCategoryPart1.PassengerCar) == 0)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasDedicatedTramLanes(NetInfo info)
        {
            foreach (var lane in info.m_lanes)
            {
                if ((lane.m_vehicleType & VehicleInfo.VehicleType.Tram) != 0 &&
                    (lane.m_laneType == NetInfo.LaneType.TransportVehicle || // cars allowed but discouraged
                    (lane.m_vehicleType & VehicleInfo.VehicleType.Car) == 0)) // cars not allowed
                {
                    return true;
                }
            }

            return false;
        }

        public static bool HasTramTracks(NetInfo info)
        {
            return (info.m_vehicleTypes & VehicleInfo.VehicleType.Tram) != 0;
        }

        public static bool HasTrolleybusWires(NetInfo info)
        {
            return (info.m_vehicleTypes & VehicleInfo.VehicleType.Trolleybus) != 0;
        }

        public static bool HasMonorail(NetInfo info)
        {
            return (info.m_vehicleTypes & VehicleInfo.VehicleType.Monorail) != 0;
        }
    }
}
