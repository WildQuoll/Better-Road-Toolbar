﻿using ICities;
using CitiesHarmony.API;
using ColossalFramework.UI;
using UnityEngine.SceneManagement;

namespace BetterRoadToolbar
{
    public class Mod : LoadingExtensionBase, IUserMod
    {
        public string Name => "Better Road Toolbar";
        public string Description => "Adds more tabs in the Roads toolbar and changes sort order to make finding the right road that little bit easier.";

        public void OnEnabled()
        {
            HarmonyHelper.DoOnHarmonyReady(() => Patcher.PatchAll());
        }

        public void OnDisabled()
        {
            if (HarmonyHelper.IsHarmonyInstalled) Patcher.UnpatchAll();
        }

        public override void OnLevelLoaded(LoadMode mode)
        {
            if (EditableConfig == CurrentConfig)
            {
                EditableConfig = new Config(); //any changes made from now on will require a restart
            }
        }

        public void OnSettingsUI(UIHelperBase helper)
        {
            helper.AddSpace(16);
            var extractTransportModesCheckbox = (UICheckBox)helper.AddCheckbox("Create separate tabs for bus, tram, bike, trolleybus and monorail roads",
                EditableConfig.CreateTabsForTransportModes,
                (isChecked) => EditableConfig.Update(createTabsForTransportModes: isChecked));

            var extractMultiModalCheckbox = (UICheckBox)helper.AddCheckbox("Create an additional tab for roads with 2 or more modes of transport",
                EditableConfig.CreateMultiModalTab,
                (isChecked) => EditableConfig.Update(createMultiModalTab: isChecked));

            helper.AddSpace(16);

            var createIndustrialTabCheckbox = (UICheckBox)helper.AddCheckbox("Create a separate tab for industrial roads",
                EditableConfig.CreateIndustrialTab,
                (isChecked) => EditableConfig.Update(createIndustrialTab: isChecked));

            var treatSlowRoadsAsPedestrianCheckbox = (UICheckBox)helper.AddCheckbox("Treat traffic-calmed streets (25 kph/15 mph or less) as pedestrianised",
                EditableConfig.TreatSlowRoadsAsPedestrian,
                (isChecked) => EditableConfig.Update(treatSlowRoadsAsPedestrian: isChecked));

            var useStandardSortingCheckbox = (UICheckBox)helper.AddCheckbox("Use default game sort order for roads",
                EditableConfig.UseDefaultSortOrder,
                (isChecked) => EditableConfig.Update(useDefaultSortOrder: isChecked));

            helper.AddSpace(16);

            var ignorePlazasDlcTabCheckbox = (UICheckBox)helper.AddCheckbox("Keep Plazas and Promenades DLC roads in their own tab",
                EditableConfig.IgnorePlazasDlcTab,
                (isChecked) => EditableConfig.Update(ignorePlazasDlcTab: isChecked));

            var ignoreBridgesDlcTabCheckbox = (UICheckBox)helper.AddCheckbox("Keep Bridges and Piers DLC roads in their own tab",
                EditableConfig.IgnoreBridgesDlcTab,
                (isChecked) => EditableConfig.Update(ignoreBridgesDlcTab: isChecked));

            var ignoreOtherCustomTabsCheckbox = (UICheckBox)helper.AddCheckbox("Keep custom road tabs generated by other mods or DLCs",
                EditableConfig.IgnoreOtherCustomTabs,
                (isChecked) => EditableConfig.Update(ignoreOtherCustomTabs: isChecked));

            helper.AddSpace(16);

            var showAssetFiltersCheckbox = (UICheckBox)helper.AddCheckbox("Show road filters (e.g. 1-way/2-way) in each tab",
                EditableConfig.ShowAssetFilters,
                (isChecked) => EditableConfig.Update(showAssetFilters: isChecked));

            helper.AddSpace(16);
            helper.AddButton("Reset to default",
                () =>
                {
                    // This not only updates the config, but also updates the checkboxes.
                    extractTransportModesCheckbox.isChecked = true;
                    extractMultiModalCheckbox.isChecked = false;
                    useStandardSortingCheckbox.isChecked = false;
                    ignorePlazasDlcTabCheckbox.isChecked = false;
                    ignoreBridgesDlcTabCheckbox.isChecked = true;
                    ignoreOtherCustomTabsCheckbox.isChecked = true;
                    createIndustrialTabCheckbox.isChecked = true;
                    treatSlowRoadsAsPedestrianCheckbox.isChecked = true;
                    showAssetFiltersCheckbox.isChecked = true;
                });

            helper.AddSpace(16);

            if (CurrentConfig != EditableConfig || !IsMainMenu())
            {
                helper.AddGroup("Note: Settings changes will take effect after restarting the game.");
            }
        }

        public static bool IsMainMenu()
        {
            return SceneManager.GetActiveScene().name == "MainMenu";
        }

        public static bool IsInGame()
        {
            return SceneManager.GetActiveScene().name == "Game";
        }

        public static string Identifier = "WQ.BRT/";

        public static Config CurrentConfig = new Config();
        public static Config EditableConfig = CurrentConfig;
    }
}
