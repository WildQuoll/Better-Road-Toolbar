using System;
using UnityEngine;

namespace BetterRoadToolbar
{
    public class Config
    {
        public Config()
        {
            Load(CONFIG_PATH);
        }

        public Config(Config other)
        {
            UseStandardSortOrder = other.UseStandardSortOrder;
            IgnoreCustomTabs = other.IgnoreCustomTabs;
            CreateTabsForTransportModes = other.CreateTabsForTransportModes;
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
                        case "UseStandardSortOrder":
                            UseStandardSortOrder = FromString(splitLine[1]);
                            break;
                        case "IgnoreCustomTabs":
                            IgnoreCustomTabs = FromString(splitLine[1]);
                            break;
                        case "CreateTabsForTransportModes":
                            CreateTabsForTransportModes = FromString(splitLine[1]);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch
            {}
        }

        private void Save(string path)
        {
            string text = "UseStandardSortOrder=" + (UseStandardSortOrder ? 1 : 0) + "\n"
                        + "IgnoreCustomTabs=" + (IgnoreCustomTabs ? 1 : 0) + "\n"
                        + "CreateTabsForTransportModes=" + (CreateTabsForTransportModes ? 1 : 0);

            try
            {
                System.IO.File.WriteAllText(path, text);
            }
            catch (Exception e)
            {
                Debug.Log(Mod.Identifier + "Failed to save config: " + e.Message);
            }
        }

        public void Update(bool? useStandardSortOrder = null, bool? ignoreCustomTabs = null, bool? createTabsForTransportModes = null)
        {
            if (useStandardSortOrder.HasValue && useStandardSortOrder.Value != UseStandardSortOrder)
            {
                UseStandardSortOrder = useStandardSortOrder.Value;
            }

            if (ignoreCustomTabs.HasValue && ignoreCustomTabs.Value != IgnoreCustomTabs)
            {
                IgnoreCustomTabs = ignoreCustomTabs.Value;
            }

            if (createTabsForTransportModes.HasValue && createTabsForTransportModes.Value != CreateTabsForTransportModes)
            {
                CreateTabsForTransportModes = createTabsForTransportModes.Value;
            }

            Save(CONFIG_PATH);
        }

        private const string CONFIG_PATH = "BetterRoadToolbarConfig.txt";

        public bool UseStandardSortOrder = false;
        public bool IgnoreCustomTabs = false;
        public bool CreateTabsForTransportModes = true;
    }
}
