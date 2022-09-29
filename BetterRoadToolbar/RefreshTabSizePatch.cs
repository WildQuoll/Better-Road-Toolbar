using ColossalFramework.UI;
using HarmonyLib;

namespace BetterRoadToolbar
{
	// This patch suppresses shrinking of toolbar tabs, which makes the labels not fit,
	// and is hardly useful unless playing on a 1024x768 screen... (there was no shrinking before the Plazas and Promenades patch).
	[HarmonyPatch(typeof(GeneratedGroupPanel), "RefreshTabSize")]
	class RefreshTabSizePatch
	{
		[HarmonyPrefix]
		public static bool Prefix(ref UITabstrip ___m_Strip)
		{
			// Same as default:
			if (___m_Strip.childCount > 13)
			{
				___m_Strip.padding.right = 0;
			}

			// Skip default implementation
			return false;
		}
	}
}