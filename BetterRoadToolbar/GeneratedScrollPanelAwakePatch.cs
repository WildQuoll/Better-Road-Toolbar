using ColossalFramework.UI;
using HarmonyLib;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace BetterRoadToolbar
{
    [HarmonyPatch(typeof(GeneratedScrollPanel), "Awake")]
    class GeneratedScrollPanelAwakePatch
    {
        [HarmonyPostfix]
        public static void Postfix(GeneratedScrollPanel __instance, ref UIPanel ___m_UIFilterPanel)
        {
            if (!(__instance is RoadsPanel) || !Mod.IsInGame() || !Mod.CurrentConfig.ShowAssetFilters || ___m_UIFilterPanel.components == null)
            {
                return;
            }

            foreach (var component in ___m_UIFilterPanel.components)
            {
                var button = component as UIMultiStateButton;
                if (!button)
                {
                    continue;
                }

                if (button.normalFgSprite == "UIFilterRoadsOneLane")
                {
                    button.tooltip = "Roads without parking";
                }
                else if (button.normalFgSprite == "UIFilterRoadsTwoLane")
                {
                    button.tooltip = "Roads with parking";

                    string replacementIcon = "UIFilterRoadsNotDecorated";
                    button.foregroundSprites[0].normal = replacementIcon;
                    button.foregroundSprites[0].hovered = replacementIcon + "Hovered";
                    button.foregroundSprites[0].pressed = replacementIcon + "Pressed";
                    button.foregroundSprites[1].normal = replacementIcon + "Focused";
                    button.foregroundSprites[1].hovered = replacementIcon + "FocusedHovered";
                    button.foregroundSprites[1].pressed = replacementIcon + "FocusedPressed";
                }
            }
        }
    }
}