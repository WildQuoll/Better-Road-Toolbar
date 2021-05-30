using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BetterRoadToolbar
{
	enum RoadCategory
	{
		Urban_1U = 1,
		Urban_2U_2LMax,
		Urban_2U_3LMin,
		Urban_3U,
		Urban_4U_4LMax,
		Urban_4U_5LMin,
		Urban_5UMin,
		Rural,
		Highway,

		Bike,
		Bus,
		Tram,
		Trolleybus,
		Monorail
	}

	class RoadAnalyser
    {
		public static string GetToolbarTitle(RoadCategory cat)
		{
			switch (cat)
			{
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

		private static bool IsVehicleLane(NetInfo.Lane lane)
        {
			var laneTypes = (NetInfo.LaneType.Vehicle | NetInfo.LaneType.TransportVehicle);
			var vehicleTypes = (VehicleInfo.VehicleType.Car | VehicleInfo.VehicleType.Tram | VehicleInfo.VehicleType.Trolleybus);

			return (lane.m_laneType & laneTypes) != 0 &&
				   (lane.m_vehicleType & vehicleTypes) != 0;
		}

		private static bool IsAuxiliaryLane(NetInfo.Lane lane)
		{
			if (lane.m_laneType == NetInfo.LaneType.Parking || lane.m_vehicleType == VehicleInfo.VehicleType.Bicycle)
            {
				return true;
            }

			return false;
		}

		public static uint GetLaneCount(NetInfo info)
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

		private static uint GetCellWidth(NetInfo info)
		{
			return (uint)Mathf.Floor(info.m_halfWidth / 4.0f);
		}

		public static List< RoadCategory > GetRoadCategories(NetInfo info)
		{
			var cats = new List<RoadCategory>();

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

			if (cats.Count > 0)
			{ 
				return cats;
            }

			if ((info.m_netAI as RoadBaseAI).m_highwayRules && !info.m_hasPedestrianLanes)
			{
				cats.Add( RoadCategory.Highway);
				return cats;
			}

			if (!info.m_createPavement)
			{
				cats.Add(RoadCategory.Rural);
				return cats;
			}

			uint laneCount = GetLaneCount(info);
			uint cellWidth = GetCellWidth(info);

			switch(cellWidth)
            {
				case 0:
				case 1:
					cats.Add( RoadCategory.Urban_1U);
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
			return new GeneratedGroupPanel.GroupInfo("WQ.BRT/" + ((int)roadType).ToString(), (int)roadType);
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

		/*
		//! returns 0 if extents touch or overlap
		private static float DistanceBetweenExtents(LaneExtent first, LaneExtent second)
        {
			if (first.min <= second.max && second.min <= first.max)
            {
				return 0.0f;
            }

			return Mathf.Min(Mathf.Abs(first.max - second.min), Mathf.Abs(first.min - second.max));
        }*/

		public static bool IsTwoWay(NetInfo info)
		{
			NetInfo.Direction dir = 0;

			foreach (var lane in info.m_lanes)
			{
				if (IsVehicleLane(lane))
                {
					dir |= lane.m_direction;
                }
			}

			return (dir & NetInfo.Direction.Both) == NetInfo.Direction.Both;
		}

		/*
		public static bool HasMedian(NetInfo info)
        {
			var laneExtents = new List<LaneExtent>();

			foreach(var lane in info.m_lanes)
            {
				if(IsPartOfRoadway(lane))
                {
					laneExtents.Add(new LaneExtent(lane.m_position - lane.m_width / 2.0f, lane.m_position + lane.m_width / 2.0f));
                }
            }

			if (laneExtents.Count < 2)
            {
				return false; // no roadway (?) or a single lane, so no medians
            }

			float medianThreshold = 0.2f; // a gap larger than 20cm is a median 

			var roadwayExtent = laneExtents[0];
			laneExtents.RemoveAt(0);

			while(laneExtents.Count > 0)
            {
				bool foundNeighbouringLane = false;
				foreach(var laneExtent in laneExtents)
                {
					if (DistanceBetweenExtents(laneExtent, roadwayExtent) < medianThreshold)
                    {
						roadwayExtent.min = Mathf.Min(roadwayExtent.min, laneExtent.min);
						roadwayExtent.max = Mathf.Max(roadwayExtent.max, laneExtent.max);
						laneExtents.Remove(laneExtent);
						foundNeighbouringLane = true;
						break;
                    }
                }

				if (!foundNeighbouringLane)
				{
					Debug.Log(info.name + " has a median");
					return true; // found a median
				}
            }

			Debug.Log(info.name + " does NOT have a median");
			return false;
        }
		*/

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
			return (info.m_laneTypes & NetInfo.LaneType.TransportVehicle) != 0;
		}

		public static bool HasDedicatedTramLanes(NetInfo info)
		{
			foreach (var lane in info.m_lanes)
			{
				if( (lane.m_vehicleType & VehicleInfo.VehicleType.Tram) != 0 && 
						(lane.m_laneType == NetInfo.LaneType.TransportVehicle || // cars allowed but discouraged
						(lane.m_vehicleType & VehicleInfo.VehicleType.Car) == 0) ) // cars not allowed
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
