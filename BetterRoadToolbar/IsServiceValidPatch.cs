using ColossalFramework;
using HarmonyLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace BetterRoadToolbar
{
	[HarmonyPatch(typeof(RoadsGroupPanel), "IsServiceValid")]
	class IsServiceValidPatch
	{
		[HarmonyPostfix]
		public static void Postfix(PrefabInfo info, bool __result)
		{
			if (!ToolsModifierControl.isGame)
            {
				return;
            }

			// Unlike default, we don't filter out roads without car lanes
			__result = info.GetService() == ItemClass.Service.Road;
		}
	}
}