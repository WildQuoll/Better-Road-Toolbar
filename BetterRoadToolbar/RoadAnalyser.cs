using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace BetterRoadToolbar
{
	enum RoadType
	{
		Urban = 0,
		Rural = 1,
		Highway = 2
	}

	class RoadAnalyser
    {
		public static uint GetLaneCount(NetInfo info)
		{
			uint count = 0;

			var laneTypes = (NetInfo.LaneType.Vehicle | NetInfo.LaneType.TransportVehicle);
			var vehicleTypes = (VehicleInfo.VehicleType.Car | VehicleInfo.VehicleType.Tram | VehicleInfo.VehicleType.Trolleybus);

			foreach (var lane in info.m_lanes)
			{
				if ((lane.m_laneType & laneTypes) != 0)
				{
					if ((lane.m_vehicleType & vehicleTypes) != 0)
					{
						count += 1;
					}
				}
			}

			return count;
		}

		public static uint GetCellWidth(NetInfo info)
		{
			return (uint)Mathf.Floor(info.m_halfWidth / 4.0f);
		}

		public static RoadType GetRoadType(NetInfo info)
		{
			var roadAi = info.m_netAI as RoadAI;

			if (roadAi.m_highwayRules && !info.m_hasPedestrianLanes)
			{
				return RoadType.Highway;
			}

			if (!info.m_createPavement)
			{
				return RoadType.Rural;
			}

			return RoadType.Urban;
		}

		public static GeneratedGroupPanel.GroupInfo CreateGroup(RoadType roadType, uint cellWidth, uint laneCount)
		{
			return new GeneratedGroupPanel.GroupInfo(
				roadType.ToString() + "/" + cellWidth.ToString() + "U" + laneCount.ToString() + "L",
				(int)(laneCount + cellWidth * 100 + (int)roadType * 10000));
		}
	}
}
