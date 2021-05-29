using HarmonyLib;
using System;

namespace BetterRoadToolbar
{
	[HarmonyPatch(typeof(GeneratedScrollPanel), "IsCategoryValid", new Type[] { typeof(NetInfo), typeof(bool) })]
	class IsCategoryValidPatch
	{
		[HarmonyPostfix]
		public static void Postfix(NetInfo info, bool ignore, ref bool __result, ref string ___m_Category)
		{
			if(ignore)
            {
				return;
            }

			var roadInfo = info.m_netAI as RoadBaseAI;

			if (!roadInfo)
            {
				return;
            }

			var roadType = RoadAnalyser.GetRoadCategory(info);
			var group = RoadAnalyser.CreateGroup(roadType);

			__result = (group.name == ___m_Category);
		}
	}
}