using HarmonyLib;
using System;

namespace BetterRoadToolbar
{
	// This patch overrides the category(ies) that a road will be assigned to.
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

			if (Mod.CurrentConfig.IgnoreCustomTabs && !RoadUtils.IsDefaultRoadCategory(info.category))
			{
				return;
			}

			var cats = RoadUtils.GetRoadCategories(info);

			foreach (var cat in cats)
			{
				var group = RoadUtils.CreateGroup(cat);

				if (group.name == ___m_Category)
                {
					__result = true;
					return;
                }
			}

			__result = false;
		}
	}
}