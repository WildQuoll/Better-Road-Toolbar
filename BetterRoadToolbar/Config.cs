using System;
using UnityEngine;

namespace BetterRoadToolbar
{
    public class Config
    {
        public const string IGNORE_PLAZAS_DLC_TAB_STRING = "IgnorePlazasDlcTab";
        public const string IGNORE_BRIDGES_DLC_TAB_STRING = "IgnoreBridgesDlcTab";
        public const string IGNORE_OTHER_CUSTOM_TABS_STRING = "IgnoreCustomTabs";
        public const string USE_STANDARD_SORT_ORDER_STRING = "UseStandardSortOrder";
        public const string TREAT_SLOW_ROADS_AS_PEDESTRIAN_STRING = "CreatePedestrianTab"; // old string kept for backward compatibility
        public const string CREATE_INDUSTRIAL_TAB_STRING = "CreateIndustrialTab";
        public const string CREATE_TABS_BY_TRANSPORT_MODE_STRING = "CreateTabsForTransportModes";
        public const string CREATE_MULTI_MODAL_TAB_STRING = "CreateMultiModalTab";

        public Config()
        {
            Load(CONFIG_PATH);
        }

        private bool FromString(string s)
        {
            return (s == "1");
        }

        private void Load(string path)
        {
            try
            {
                string text = System.IO.File.ReadAllText(path);

                var splitText = text.Split('\n');

                foreach (var line in splitText)
                {
                    if (!line.Contains("="))
                    {
                        continue;
                    }

                    var splitLine = line.Split('=');
                    if (splitLine.Length != 2)
                    {
                        continue;
                    }
                    switch (splitLine[0])
                    {
                        case USE_STANDARD_SORT_ORDER_STRING:
                            UseStandardSortOrder = FromString(splitLine[1]);
                            break;
                        case IGNORE_PLAZAS_DLC_TAB_STRING:
                            IgnorePlazasDlcTab = FromString(splitLine[1]);
                            break;
                        case IGNORE_BRIDGES_DLC_TAB_STRING:
                            IgnoreBridgesDlcTab = FromString(splitLine[1]);
                            break;
                        case IGNORE_OTHER_CUSTOM_TABS_STRING:
                            IgnoreOtherCustomTabs = FromString(splitLine[1]);
                            break;
                        case CREATE_TABS_BY_TRANSPORT_MODE_STRING:
                            CreateTabsForTransportModes = FromString(splitLine[1]);
                            break;
                        case CREATE_MULTI_MODAL_TAB_STRING:
                            CreateMultiModalTab = FromString(splitLine[1]);
                            break;
                        case TREAT_SLOW_ROADS_AS_PEDESTRIAN_STRING:
                            TreatSlowRoadsAsPedestrian = FromString(splitLine[1]);
                            break;
                        case CREATE_INDUSTRIAL_TAB_STRING:
                            CreateIndustrialTab = FromString(splitLine[1]);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch
            { }
        }

        private void Save(string path)
        {
            string text = USE_STANDARD_SORT_ORDER_STRING + "=" + (UseStandardSortOrder ? 1 : 0) + "\n"
                        + IGNORE_PLAZAS_DLC_TAB_STRING + "=" + (IgnorePlazasDlcTab ? 1 : 0) + "\n"
                        + IGNORE_BRIDGES_DLC_TAB_STRING + "=" + (IgnoreBridgesDlcTab ? 1 : 0) + "\n"
                        + IGNORE_OTHER_CUSTOM_TABS_STRING + "=" + (IgnoreOtherCustomTabs ? 1 : 0) + "\n"
                        + CREATE_TABS_BY_TRANSPORT_MODE_STRING + "=" + (CreateTabsForTransportModes ? 1 : 0) + "\n"
                        + CREATE_MULTI_MODAL_TAB_STRING + "=" + (CreateMultiModalTab ? 1 : 0) + "\n"
                        + TREAT_SLOW_ROADS_AS_PEDESTRIAN_STRING + "=" + (TreatSlowRoadsAsPedestrian ? 1 : 0) + "\n"
                        + CREATE_INDUSTRIAL_TAB_STRING + "=" + (CreateIndustrialTab ? 1 : 0);

            try
            {
                System.IO.File.WriteAllText(path, text);
            }
            catch (Exception e)
            {
                Debug.Log(Mod.Identifier + "Failed to save config: " + e.Message);
            }
        }

        public void Update(bool? useStandardSortOrder = null,
                           bool? ignorePlazasDlcTab = null,
                           bool? ignoreBridgesDlcTab = null,
                           bool? ignoreOtherCustomTabs = null,
                           bool? createTabsForTransportModes = null,
                           bool? createMultiModalTab = null,
                           bool? treatSlowRoadsAsPedestrian = null,
                           bool? createIndustrialTab = null)
        {
            if (useStandardSortOrder.HasValue && useStandardSortOrder.Value != UseStandardSortOrder)
            {
                UseStandardSortOrder = useStandardSortOrder.Value;
            }

            if (ignorePlazasDlcTab.HasValue && ignorePlazasDlcTab.Value != IgnorePlazasDlcTab)
            {
                IgnorePlazasDlcTab = ignorePlazasDlcTab.Value;
            }

            if (ignoreBridgesDlcTab.HasValue && ignoreBridgesDlcTab.Value != IgnoreBridgesDlcTab)
            {
                IgnoreBridgesDlcTab = ignoreBridgesDlcTab.Value;
            }

            if (ignoreOtherCustomTabs.HasValue && ignoreOtherCustomTabs.Value != IgnoreOtherCustomTabs)
            {
                IgnoreOtherCustomTabs = ignoreOtherCustomTabs.Value;
            }

            if (createTabsForTransportModes.HasValue && createTabsForTransportModes.Value != CreateTabsForTransportModes)
            {
                CreateTabsForTransportModes = createTabsForTransportModes.Value;
            }

            if (createMultiModalTab.HasValue && createMultiModalTab.Value != CreateMultiModalTab)
            {
                CreateMultiModalTab = createMultiModalTab.Value;
            }

            if (treatSlowRoadsAsPedestrian.HasValue && treatSlowRoadsAsPedestrian.Value != TreatSlowRoadsAsPedestrian)
            {
                TreatSlowRoadsAsPedestrian = treatSlowRoadsAsPedestrian.Value;
            }

            if (createIndustrialTab.HasValue && createIndustrialTab.Value != CreateIndustrialTab)
            {
                CreateIndustrialTab = createIndustrialTab.Value;
            }

            Save(CONFIG_PATH);
        }

        private const string CONFIG_PATH = "BetterRoadToolbarConfig.txt";

        public bool UseStandardSortOrder = false;
        public bool IgnorePlazasDlcTab = false;
        public bool IgnoreBridgesDlcTab = true;
        public bool IgnoreOtherCustomTabs = true;
        public bool CreateTabsForTransportModes = true;
        public bool CreateMultiModalTab = false;
        public bool TreatSlowRoadsAsPedestrian = true;
        public bool CreateIndustrialTab = true;
    }
}
