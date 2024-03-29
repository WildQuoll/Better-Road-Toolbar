﻿using HarmonyLib;
using System;

namespace BetterRoadToolbar
{
    
    // This patch overrides the category(ies) that a road will be assigned to.
    [HarmonyPatch(typeof(GeneratedScrollPanel), "IsCategoryValid", new Type[] { typeof(NetInfo), typeof(bool) })]
    class IsCategoryValidPatch
    {
        [HarmonyPostfix]
        public static void Postfix(NetInfo info, bool ignore, GeneratedScrollPanel __instance, ref bool __result, ref string ___m_Category)
        {
            if (ignore || !(__instance is RoadsPanel) || !Mod.IsInGame())
            {
                return;
            }

            var roadInfo = info.m_netAI as RoadBaseAI;

            if (!roadInfo)
            {
                return;
            }

            if (RoadUtils.ShouldKeepExistingCategory(info.category))
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