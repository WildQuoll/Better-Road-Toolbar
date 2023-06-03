using ICities;
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
            var extractTransportModesCheckbox = (UICheckBox)helper.AddCheckbox(Translations.GetString(Translations.SETTING_TRANSPORT_TABS),
                EditableConfig.CreateTabsForTransportModes,
                (isChecked) => EditableConfig.Update(createTabsForTransportModes: isChecked));

            var extractMultiModalCheckbox = (UICheckBox)helper.AddCheckbox(Translations.GetString(Translations.SETTING_MULTIMODAL_TAB),
                EditableConfig.CreateMultiModalTab,
                (isChecked) => EditableConfig.Update(createMultiModalTab: isChecked));

            helper.AddSpace(16);

            var createIndustrialTabCheckbox = (UICheckBox)helper.AddCheckbox(Translations.GetString(Translations.SETTING_INDUSTRIAL_TAB),
                EditableConfig.CreateIndustrialTab,
                (isChecked) => EditableConfig.Update(createIndustrialTab: isChecked));

            var treatSlowRoadsAsPedestrianCheckbox = (UICheckBox)helper.AddCheckbox(Translations.GetString(Translations.SETTING_TRAFFIC_CALMED_IS_PED_TAB),
                EditableConfig.TreatSlowRoadsAsPedestrian,
                (isChecked) => EditableConfig.Update(treatSlowRoadsAsPedestrian: isChecked));

            var useStandardSortingCheckbox = (UICheckBox)helper.AddCheckbox(Translations.GetString(Translations.SETTING_DEFAULT_SORT_ORDER),
                EditableConfig.UseDefaultSortOrder,
                (isChecked) => EditableConfig.Update(useDefaultSortOrder: isChecked));

            helper.AddSpace(16);

            var ignorePlazasDlcTabCheckbox = (UICheckBox)helper.AddCheckbox(Translations.GetString(Translations.SETTING_PP_TAB),
                EditableConfig.IgnorePlazasDlcTab,
                (isChecked) => EditableConfig.Update(ignorePlazasDlcTab: isChecked));

            var ignoreBridgesDlcTabCheckbox = (UICheckBox)helper.AddCheckbox(Translations.GetString(Translations.SETTING_BP_TAB),
                EditableConfig.IgnoreBridgesDlcTab,
                (isChecked) => EditableConfig.Update(ignoreBridgesDlcTab: isChecked));

            var ignoreOtherCustomTabsCheckbox = (UICheckBox)helper.AddCheckbox(Translations.GetString(Translations.SETTING_DLC_TABS),
                EditableConfig.IgnoreOtherCustomTabs,
                (isChecked) => EditableConfig.Update(ignoreOtherCustomTabs: isChecked));

            helper.AddSpace(16);

            var showAssetFiltersCheckbox = (UICheckBox)helper.AddCheckbox(Translations.GetString(Translations.SETTING_SHOW_FILTERS),
                EditableConfig.ShowAssetFilters,
                (isChecked) => EditableConfig.Update(showAssetFilters: isChecked));

            helper.AddSpace(16);
            helper.AddButton(Translations.GetString(Translations.SETTING_RESET),
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
                helper.AddGroup(Translations.GetString(Translations.SETTING_RESTART_REQUIRED));
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
