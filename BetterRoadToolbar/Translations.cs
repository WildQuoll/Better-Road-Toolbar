using ColossalFramework.Globalization;
using ColossalFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BetterRoadToolbar
{
    internal class Translations
    {
        public static string FILTER_WITH_PARKING = "Filter_NP";
        public static string FILTER_WITHOUT_PARKING = "Filter_P";

        public static string SETTING_TRANSPORT_TABS = "Settings_TransportTabs";
        public static string SETTING_MULTIMODAL_TAB = "Settings_MultiModal";
        public static string SETTING_INDUSTRIAL_TAB = "Settings_Industrial";
        public static string SETTING_TRAFFIC_CALMED_IS_PED_TAB = "Settings_TrafficCalmed";
        public static string SETTING_DEFAULT_SORT_ORDER = "Settings_DefaultSortOrder";
        public static string SETTING_PP_TAB = "Settings_PPTab";
        public static string SETTING_BP_TAB = "Settings_BPTab";
        public static string SETTING_DLC_TABS = "Settings_DLCTabs";
        public static string SETTING_SHOW_FILTERS = "Settings_ShowFilters";
        public static string SETTING_RESET = "Settings_Reset";
        public static string SETTING_RESTART_REQUIRED = "Settings_RestartRequired";

        public static string GetString(string id)
        {
            string language = SingletonLite<LocaleManager>.instance.language;
            if (TranslationStrings[id].ContainsKey(language))
            {
                return TranslationStrings[id][language];
            }
            else
            {
                return TranslationStrings[id]["en"];
            }
        }

        public static string GetTabTitle(RoadCategory cat)
        {
            string language = SingletonLite<LocaleManager>.instance.language;
            if (TabTitles[cat].ContainsKey(language))
            {
                return TabTitles[cat][language];
            }
            else
            {
                return TabTitles[cat]["en"];
            }
        }

        public static string GetTabTooltip(RoadCategory cat)
        {
            string language = SingletonLite<LocaleManager>.instance.language;
            if (TabTooltips[cat].ContainsKey(language))
            {
                return TabTooltips[cat][language];
            }
            else
            {
                return TabTooltips[cat]["en"];
            }
        }

        private static Dictionary<string, Dictionary<string, string>> TranslationStrings = new Dictionary<string, Dictionary<string, string>>
        {
            {
                FILTER_WITHOUT_PARKING, new Dictionary<string, string>
                {
                    {"en", "Roads without parking"},
                    {"de", "Straßen ohne Parkplätze"},
                    {"es", "Calles sin estacionamiento"},
                    {"fr", "Routes sans stationnement"},
                    {"ja", "駐車枠無しの道路"},
                    {"ko", "주차장 없는 도로"},
                    {"pl", "Drogi bez miejsc postojowych"},
                    {"pt", "Ruas sem estacionamento"},
                    {"ru", "Дороги без парковки"},
                    {"zh", "无停车位的道路"}
                }
            },
            {
                FILTER_WITH_PARKING, new Dictionary<string, string>
                {
                    {"en", "Roads with parking"},
                    {"de", "Straßen mit Parkplätzen"},
                    {"es", "Calles con estacionamiento"},
                    {"fr", "Routes avec stationnement"},
                    {"ja", "駐車枠有りの道路"},
                    {"ko", "주차장 있는 도로"},
                    {"pl", "Drogi z miejscami postojowymi"},
                    {"pt", "Ruas com estacionamento"},
                    {"ru", "Дороги с парковкой"},
                    {"zh", "有停车位的道路"}
                }
            },
            {
                SETTING_TRANSPORT_TABS, new Dictionary<string, string>
                {
                    {"en", "Create separate tabs for bus, tram, bike, trolleybus and monorail roads"},
                    {"de", "Erstelle Registerkarten für Bus-, Straßenbahn-, Fahrrad-, O-Bus- und Monorailstraßen"},
                    {"es", "Crear pestañas para carreteras de autobús, tranvía, bicicleta, trolebús y monorraíl"},
                    {"fr", "Créer des onglets séparés pour les routes de bus, de tramway, de vélo, de trolleybus et de monorail"},
                    {"ja", "バス・路面電車・自転車・トロリーバス・モノレールの各道路は、個々のタブで分割"},
                    {"ko", "버스, 트램, 자전거, 트롤리버스, 모노레일 도로를 위한 별도의 탭을 생성합니다"},
                    {"pl", "Utwórz zakładki dla dróg autobusowych, tramwajowych, rowerowych, trolejbusowych i z koleją jednoszynową"},
                    {"pt", "Criar abas separadas para estradas de ônibus, bonde, bicicleta, trólebus e monotrilho"},
                    {"ru", "Создание отдельных вкладок для автобусных, трамвайных, велосипедных, троллейбусных и монорельсовых дорог"},
                    {"zh", "为公共汽车、电车、自行车、无轨电车和单轨道路创建单独的标签页"}
                }
            },
            {
                SETTING_MULTIMODAL_TAB, new Dictionary<string, string>
                {
                    {"en", "Create an additional tab for roads with 2 or more modes of transport"},
                    {"de", "Erstelle eine zusätzliche Registerkarte für Straßen mit 2 oder mehr Verkehrsmitteln"},
                    {"es", "Crear una pestaña adicional para carreteras con 2 o más modos de transporte"},
                    {"fr", "Créer un onglet supplémentaire pour les routes avec 2 modes de transport ou plus"},
                    {"ja", "道路に2タイプ以上の交通手段がある場合は、タブを追加で作成"},
                    {"ko", "2개 이상의 교통 수단을 갖는 도로를 위한 추가 탭을 생성합니다"},
                    {"pl", "Utwórz dodatkową zakładkę dla dróg z 2 lub więcej trybami transportu"},
                    {"pt", "Criar uma aba adicional para estradas com 2 ou mais modos de transporte"},
                    {"ru", "Создание дополнительной вкладки для дорог с 2 или более видами транспорта"},
                    {"zh", "为具有2种或更多种交通方式的道路创建额外的标签页"}
                }
            },
            {
                SETTING_INDUSTRIAL_TAB, new Dictionary<string, string>
                {
                    {"en", "Create a separate tab for industrial roads"},
                    {"de", "Erstelle eine separate Registerkarte für Industriestraßen"},
                    {"es", "Crear una pestaña separada para carreteras industriales"},
                    {"fr", "Créer un onglet séparé pour les routes industrielles"},
                    {"ja", "産業道路扱いの道路は、タブを分割して作成"},
                    {"ko", "산업 도로를 위한 별도의 탭을 생성합니다"},
                    {"pl", "Utwórz osobną zakładkę dla dróg przemysłowych"},
                    {"pt", "Criar uma aba separada para estradas industriais"},
                    {"ru", "Создание отдельной вкладки для промышленных дорог"},
                    {"zh", "为工业道路创建单独的标签页"}
                }
            },
            {
                SETTING_TRAFFIC_CALMED_IS_PED_TAB, new Dictionary<string, string>
                {
                    {"en", "Treat traffic-calmed streets (25 kph/15 mph or less) as pedestrianised"},
                    {"de", "Verkehrsberuhigte Straßen (25 km/h oder weniger) als Fußgängerzonen behandeln"},
                    {"es", "Considerar las calles con tráfico calmado (25 km/h o menos) como peatonales"},
                    {"fr", "Considérer les rues à circulation apaisée (25 km/h ou moins) comme piétonnes"},
                    {"ja", "速度規制された道路(最高速度 25km/15マイル以下)は、歩行者専用道路として扱う"},
                    {"ko", "교통이 완화된 도로(25 km/h 이하)를 보행자 도로로 취급"},
                    {"pl", "Traktuj ulice o spokojnym ruchu (25 km/h lub mniej) jako przeznaczone dla pieszych"},
                    {"pt", "Tratar ruas com tráfego calmado (25 km/h ou menos) como pedonais"},
                    {"ru", "Рассматривать улицы с замедленным движением (25 км/ч или меньше) как пешеходные"},
                    {"zh", "将交通平静的街道（25 km/h或以下）视为步行街"}
                }
            },
            {
                SETTING_DEFAULT_SORT_ORDER, new Dictionary<string, string>
                {
                    {"en", "Use default game sort order for roads"},
                    {"de", "Die standardmäßige Sortierreihenfolge des Spiels für Straßen verwenden"},
                    {"es", "Usar el orden de clasificación predeterminado del juego para las calles"},
                    {"fr", "Utiliser l'ordre de tri du jeu par défaut pour les routes"},
                    {"ja", "道路ソート時にゲームデフォルトの順序を適用"},
                    {"ko", "게임의 기본 도로 정렬 순서 사용"},
                    {"pl", "Używaj domyślnej kolejności sortowania dróg w grze"},
                    {"pt", "Usar a ordem de classificação padrão do jogo para as estradas"},
                    {"ru", "Использовать стандартный порядок сортировки дорог в игре"},
                    {"zh", "使用游戏默认的道路排序顺序"}
                }
            },
            {
                SETTING_PP_TAB, new Dictionary<string, string>
                {
                    {"en", "Keep Plazas and Promenades roads in their own tab"},
                    {"de", "Straßen von Plazas und Promenades im eigenen Tab belassen"},
                    {"es", "Mantener las carreteras de Plazas and Promenades en su propia pestaña"},
                    {"fr", "Garder les routes de Plazas and Promenades dans leur propre onglet"},
                    {"ja", "DLC Plazas ＆ Promenadesの道路は、専用のタブで分けて管理"},
                    {"ko", "Plazas and Promenades 도로를 별도의 탭에 유지"},
                    {"pl", "Zachowaj osobną zakładkę dla dróg z dodatku Plazas and Promenades"},
                    {"pt", "Manter as estradas de Plazas and Promenades em sua própria aba"},
                    {"ru", "Оставлять дороги Plazas and Promenades в отдельной вкладке"},
                    {"zh", "将Plazas and Promenades的道路保留在自己的标签页中"}
                }
            },
            {
                SETTING_BP_TAB, new Dictionary<string, string>
                {
                    {"en", "Keep Bridges and Piers roads in their own tab"},
                    {"de", "Straßen von Bridges und Piers im eigenen Tab behalten"},
                    {"es", "Mantener las carreteras de Bridges and Piers en su propia pestaña"},
                    {"fr", "Garder les routes de Bridges and Piers dans leur propre onglet"},
                    {"ja", "CCP Bridges ＆ Piersの道路は、専用のタブで分けて管理"},
                    {"ko", "Bridges and Piers 도로를 별도의 탭에 유지"},
                    {"pl", "Zachowaj osobną zakładkę dla dróg z dodatku Bridges and Piers"},
                    {"pt", "Manter as estradas de Bridges and Piers em sua própria aba"},
                    {"ru", "Оставлять дороги Bridges and Piers в отдельной вкладке"},
                    {"zh", "将Bridges and Piers的道路保留在自己的标签页中"}
                }
            },
            {
                SETTING_DLC_TABS, new Dictionary<string, string>
                {
                    {"en", "Keep custom road tabs generated by other mods or DLCs"},
                    {"de", "Benutzerdefinierte Straßen-Registerkarten von anderen Mods oder DLCs beibehalten"},
                    {"es", "Mantener las pestañas de carreteras personalizadas generadas por otros mods o DLCs"},
                    {"fr", "Conserver les onglets de routes personnalisées générés par d'autres mods ou DLCs"},
                    {"ja", "他のMODやDLCで生成した独自の道路タブは、変更せずにタブを保持"},
                    {"ko", "다른 모드 또는 DLC에서 생성된 사용자 정의 도로 탭 유지"},
                    {"pl", "Zachowaj zakładki dróg generowane przez inne mody lub DLC"},
                    {"pt", "Manter abas de estradas personalizadas geradas por outros mods ou DLCs"},
                    {"ru", "Сохранять пользовательские вкладки дорог, созданные другими модами или DLC"},
                    {"zh", "保留由其他模组或DLC生成的自定义道路标签页"}
                }
            },
            {
                SETTING_SHOW_FILTERS, new Dictionary<string, string>
                {
                    {"en", "Show road filters (e.g. 1-way/2-way) in each tab"},
                    {"de", "Straßenfilter (z.B. Einweg/Zweiweg) in jedem Tab anzeigen"},
                    {"es", "Mostrar filtros de carreteras (por ejemplo, de un solo sentido/dos sentidos) en cada pestaña"},
                    {"fr", "Afficher les filtres de route (par exemple, à sens unique/à double sens) dans chaque onglet"},
                    {"ja", "各タブに道路フィルター (例:一方通行/双方向)を表示"},
                    {"ko", "각 탭에 도로 필터(예: 일방통행/양방향) 표시"},
                    {"pl", "Pokazuj filtry dróg (np. jednokierunkowe/dwukierunkowe) w każdej zakładce"},
                    {"pt", "Mostrar filtros de estradas (por exemplo, sentido único/duplo sentido) em cada aba"},
                    {"ru", "Показывать фильтры дорог (например, односторонние/двусторонние) на каждой вкладке"},
                    {"zh", "在每个标签页显示道路过滤器（例如，单向/双向）"}
                }
            },
            {
                SETTING_RESET, new Dictionary<string, string>
                {
                    {"en", "Reset to default settings"},
                    {"de", "Einstellungen auf Standard zurücksetzen"},
                    {"es", "Restablecer las configuraciones a los ajustes predeterminados"},
                    {"fr", "Réinitialiser les paramètres par défaut"},
                    {"ja", "デフォルト設定にリセット"},
                    {"ko", "설정을 기본값으로 재설정"},
                    {"pl", "Przywróć ustawienia domyślne"},
                    {"pt", "Redefinir as configurações para o padrão"},
                    {"ru", "Сбросить настройки до значений по умолчанию"},
                    {"zh", "将设置重置为默认"}
                }
            },
            {
                SETTING_RESTART_REQUIRED, new Dictionary<string, string>
                {
                    {"en", "Note: Settings changes will take effect after restarting the game."},
                    {"de", "Hinweis: Änderungen an den Einstellungen werden erst nach dem Neustart des Spiels wirksam."},
                    {"es", "Nota: Los cambios en la configuración se aplicarán después de reiniciar el juego."},
                    {"fr", "Note: Les modifications des paramètres prendront effet après le redémarrage du jeu."},
                    {"ja", "注意: 設定の変更はゲームを再起動した後に反映されます。"},
                    {"ko", "참고: 설정 변경은 게임을 재시작한 후에 적용됩니다."},
                    {"pl", "Uwaga: Zmiany w ustawieniach zostaną wprowadzone po ponownym uruchomieniu gry."},
                    {"pt", "Nota: As alterações nas configurações terão efeito após reiniciar o jogo."},
                    {"ru", "Примечание: Изменения настроек вступят в силу после перезапуска игры."},
                    {"zh", "注意：设置更改将在重启游戏后生效。"}
                }
            }
        };

        private static Dictionary<RoadCategory, Dictionary<string, string>> TabTitles = new Dictionary<RoadCategory, Dictionary<string, string>>
        {
            {
                RoadCategory.Pedestrian, new Dictionary<string, string>
                {
                    {"en", "Ped"},
                    {"de", "Fuß"},
                    {"es", "Ped"},
                    {"fr", "Piet"},
                    {"ja", "歩道"},
                    {"ko", "보행"},
                    {"pl", "Piesz"},
                    {"pt", "Ped"},
                    {"ru", "Пеш"},
                    {"zh", "行人"},
                }
            },
            {
                RoadCategory.Urban_1U, new Dictionary<string, string>
                {
                    {"en", "1U"},
                    {"de", "1U"},
                    {"es", "1U"},
                    {"fr", "1U"},
                    {"ja", "1U"},
                    {"ko", "1U"},
                    {"pl", "1U"},
                    {"pt", "1U"},
                    {"ru", "1U"},
                    {"zh", "1U"},
                }
            },
            {
                RoadCategory.Urban_2U_2LMax, new Dictionary<string, string>
                {
                    {"en", "2U"},
                    {"de", "2U"},
                    {"es", "2U"},
                    {"fr", "2U"},
                    {"ja", "2U"},
                    {"ko", "2U"},
                    {"pl", "2U"},
                    {"pt", "2U"},
                    {"ru", "2U"},
                    {"zh", "2U"},
                }
            },
            {
                RoadCategory.Urban_2U_3LMin, new Dictionary<string, string>
                {
                    {"en", "2U 3L+"},
                    {"de", "2U 3S+"}, // Spur
                    {"es", "2U 3C+"}, // Carril
                    {"fr", "2U 3V+"}, // Voie
                    {"ja", "2U 3車線+"},
                    {"ko", "2U 3차로+"},
                    {"pl", "2U 3P+"}, // Pas
                    {"pt", "2U 3F+"}, // Faixa
                    {"ru", "2U 3П+"}, // Полоса
                    {"zh", "2U 3车道+"},
                }
            },
            {
                RoadCategory.Urban_3U, new Dictionary<string, string>
                {
                    {"en", "3U"},
                    {"de", "3U"},
                    {"es", "3U"},
                    {"fr", "3U"},
                    {"ja", "3U"},
                    {"ko", "3U"},
                    {"pl", "3U"},
                    {"pt", "3U"},
                    {"ru", "3U"},
                    {"zh", "3U"},
                }
            },
            {
                RoadCategory.Urban_4U_4LMax, new Dictionary<string, string>
                {
                    {"en", "4U"},
                    {"de", "4U"},
                    {"es", "4U"},
                    {"fr", "4U"},
                    {"ja", "4U"},
                    {"ko", "4U"},
                    {"pl", "4U"},
                    {"pt", "4U"},
                    {"ru", "4U"},
                    {"zh", "4U"},
                }
            },
            {
                RoadCategory.Urban_4U_5LMin, new Dictionary<string, string>
                {
                    {"en", "4U 5L+"},
                    {"de", "4U 5S+"},
                    {"es", "4U 5C+"},
                    {"fr", "4U 5V+"},
                    {"ja", "4U 5車線+"},
                    {"ko", "4U 5차로+"},
                    {"pl", "4U 5P+"},
                    {"pt", "4U 5F+"},
                    {"ru", "4U 5П+"},
                    {"zh", "4U 5车道+"},
                }
            },
            {
                RoadCategory.Urban_5UMin, new Dictionary<string, string>
                {
                    {"en", "5U+"},
                    {"de", "5U+"},
                    {"es", "5U+"},
                    {"fr", "5U+"},
                    {"ja", "5U+"},
                    {"ko", "5U+"},
                    {"pl", "5U+"},
                    {"pt", "5U+"},
                    {"ru", "5U+"},
                    {"zh", "5U+"},
                }
            },
            {
                RoadCategory.Industrial, new Dictionary<string, string>
                {
                    {"en", "Ind"},
                    {"de", "Ind"},
                    {"es", "Ind"},
                    {"fr", "Ind"},
                    {"ja", "産業"},
                    {"ko", "산업"},
                    {"pl", "Przem"},
                    {"pt", "Ind"},
                    {"ru", "Пром"},
                    {"zh", "工业"},
                }
            },
            {
                RoadCategory.Rural, new Dictionary<string, string>
                {
                    {"en", "Rural"},
                    {"de", "Ländl"},
                    {"es", "Rural"},
                    {"fr", "Rural"},
                    {"ja", "郊外"},
                    {"ko", "시골"},
                    {"pl", "Wiejs"},
                    {"pt", "Rurais"},
                    {"ru", "Сельск"},
                    {"zh", "农村"},
                }
            },
            {
                RoadCategory.Highway, new Dictionary<string, string>
                {
                    {"en", "Hwy"},
                    {"de", "Autob"},
                    {"es", "Autop"},
                    {"fr", "Autor"},
                    {"ja", "高速道路"},
                    {"ko", "고속도로"},
                    {"pl", "Autost"},
                    {"pt", "Rodov"},
                    {"ru", "Автодор"},
                    {"zh", "高速公路"},
                }
            },
            {
                RoadCategory.Bike, new Dictionary<string, string>
                {
                    {"en", "Bike"},
                    {"de", "Rad"},
                    {"es", "Bici"},
                    {"fr", "Vélo"},
                    {"ja", "自転車"},
                    {"ko", "자전거"},
                    {"pl", "Rower"},
                    {"pt", "Bici"},
                    {"ru", "Вело"},
                    {"zh", "自行"},
                }
            },
            {
                RoadCategory.Bus, new Dictionary<string, string>
                {
                    {"en", "Bus"},
                    {"de", "Bus"},
                    {"es", "Bus"},
                    {"fr", "Bus"},
                    {"ja", "バス"},
                    {"ko", "버스"},
                    {"pl", "Bus"},
                    {"pt", "Bus"},
                    {"ru", "Автоб"},
                    {"zh", "公车"},
                }
            },
            {
                RoadCategory.Tram, new Dictionary<string, string>
                {
                    {"en", "Tram"},
                    {"de", "Strab"},
                    {"es", "Tranv"},
                    {"fr", "Tram"},
                    {"ja", "路面電車"},
                    {"ko", "노면전차"},
                    {"pl", "Tramw"},
                    {"pt", "Elétr"},
                    {"ru", "Трамв"},
                    {"zh", "有轨电车"},
                }
            },
            {
                RoadCategory.Trolleybus, new Dictionary<string, string>
                {
                    {"en", "Trolley"},
                    {"de", "O-Bus"},
                    {"es", "Troleb"},
                    {"fr", "Trolley"},
                    {"ja", "トロリー"},
                    {"ko", "트롤리버스"},
                    {"pl", "Trolej"},
                    {"pt", "Trolei"},
                    {"ru", "Трол"},
                    {"zh", "无轨电车"},
                }
            },
            {
                RoadCategory.Monorail, new Dictionary<string, string>
                {
                    {"en", "Mono"},
                    {"de", "Mono"},
                    {"es", "Mono"},
                    {"fr", "Mono"},
                    {"ja", "モノレール"},
                    {"ko", "모노레일"},
                    {"pl", "Mono"},
                    {"pt", "Mono"},
                    {"ru", "Моно"},
                    {"zh", "单轨铁路"},
                }
            },
            {
                RoadCategory.MultiModal, new Dictionary<string, string>
                {
                    {"en", "Mix"},
                    {"de", "Multi"},
                    {"es", "Multi"},
                    {"fr", "Multi"},
                    {"ja", "多用途"},
                    {"ko", "다중 모드"},
                    {"pl", "Multi"},
                    {"pt", "Multi"},
                    {"ru", "Мульти"},
                    {"zh", "多模式"},
                }
            }
        };

        private static Dictionary<RoadCategory, Dictionary<string, string>> TabTooltips = new Dictionary<RoadCategory, Dictionary<string, string>>
        {
            {
                RoadCategory.Pedestrian, new Dictionary<string, string>
                {
                    {"en", "Pedestrianised and traffic-calmed streets"},
                    {"de", "Fußgängerzone und verkehrsberuhigte Straßen"},
                    {"es", "Calles peatonales y de tráfico calmado"},
                    {"fr", "Rues piétonnes et apaisées"},
                    {"ja", "歩行者専用道路と速度規制のある道路"},
                    {"ko", "보행자 우선 도로 및 교통 진정"},
                    {"pl", "Ulice dla pieszych i o spowolnionym ruchu"},
                    {"pt", "Ruas para pedestres e tráfego tranquilo"},
                    {"ru", "Пешеходные и улицы с ограниченным движением"},
                    {"zh", "步行街和交通缓行街"}
                }
            },
            {
                RoadCategory.Urban_1U, new Dictionary<string, string>
                {
                    {"en", "Urban, 1U wide"},
                    {"de", "Straßen, 1U breit"},
                    {"es", "Calles, 1U de ancho"},
                    {"fr", "Rues, 1U de large"},
                    {"ja", "一般道 - 1U幅"},
                    {"ko", "도로, 1U 폭"},
                    {"pl", "Ulice, 1U szerokości"},
                    {"pt", "Ruas, 1U de largura"},
                    {"ru", "Улицы, ширина 1U"},
                    {"zh", "街道，1U 宽"}
                }
            },
            {
                RoadCategory.Urban_2U_2LMax, new Dictionary<string, string>
                {
                    {"en", "Urban, 2U wide, ≤2 lanes"},
                    {"de", "Straßen, 2U breit, ≤2 Fahrstreifen"},
                    {"es", "Calles, 2U de ancho, ≤2 carriles"},
                    {"fr", "Rues, 2U de large, ≤2 voies"},
                    {"ja", "一般道 - 2U幅2車線以下"},
                    {"ko", "도로, 2U 폭, ≤2 차선"},
                    {"pl", "Ulice, 2U szerokości, ≤2 pasy"},
                    {"pt", "Ruas, 2U de largura, ≤2 faixas"},
                    {"ru", "Улицы, ширина 2U, ≤2 полосы"},
                    {"zh", "街道，2U 宽，≤2 车道"}
                }
            },
            {
                RoadCategory.Urban_2U_3LMin, new Dictionary<string, string>
                {
                    {"en", "Urban, 2U wide, ≥3 lanes"},
                    {"de", "Straßen, 2U breit, ≥3 Fahrstreifen"},
                    {"es", "Calles, 2U de ancho, ≥3 carriles"},
                    {"fr", "Rues, 2U de large, ≥3 voies"},
                    {"ja", "一般道 - 2U幅3車線以上"},
                    {"ko", "도로, 2U 폭, ≥3 차선"},
                    {"pl", "Ulice, 2U szerokości, ≥3 pasy"},
                    {"pt", "Ruas, 2U de largura, ≥3 faixas"},
                    {"ru", "Улицы, ширина 2U, ≥3 полосы"},
                    {"zh", "街道，2U 宽，≥3 车道"}
                }
            },
            {
                RoadCategory.Urban_3U, new Dictionary<string, string>
                {
                    {"en", "Urban, 3U wide"},
                    {"de", "Straßen, 3U breit"},
                    {"es", "Calles, 3U de ancho"},
                    {"fr", "Rues, 3U de large"},
                    {"ja", "一般道 - 3U幅"},
                    {"ko", "도로, 3U 폭"},
                    {"pl", "Ulice, 3U szerokości"},
                    {"pt", "Ruas, 3U de largura"},
                    {"ru", "Улицы, ширина 3U"},
                    {"zh", "街道，3U 宽"}
                }
            },
            {
                RoadCategory.Urban_4U_4LMax, new Dictionary<string, string>
                {
                    {"en", "Urban, 4U wide, ≤4 lanes"},
                    {"de", "Straßen, 4U breit, ≤4 Fahrstreifen"},
                    {"es", "Calles, 4U de ancho, ≤4 carriles"},
                    {"fr", "Rues, 4U de large, ≤4 voies"},
                    {"ja", "一般道 ｰ 4U幅4車線以下"},
                    {"ko", "도로, 4U 폭, ≤4 차선"},
                    {"pl", "Ulice, 4U szerokości, ≤4 pasy"},
                    {"pt", "Ruas, 4U de largura, ≤4 faixas"},
                    {"ru", "Улицы, ширина 4U, ≤4 полосы"},
                    {"zh", "街道，4U 宽，≤4 车道"}
                }
            },
            {
                RoadCategory.Urban_4U_5LMin, new Dictionary<string, string>
                {
                    {"en", "Urban, 4U wide, ≥5 lanes"},
                    {"de", "Straßen, 4U breit, ≥5 Fahrstreifen"},
                    {"es", "Calles, 4U de ancho, ≥5 carriles"},
                    {"fr", "Rues, 4U de large, ≥5 voies"},
                    {"ja", "一般道 ｰ 4U幅5車線以上"},
                    {"ko", "도로, 4U 폭, ≥5 차선"},
                    {"pl", "Ulice, 4U szerokości, ≥5 pasów"},
                    {"pt", "Ruas, 4U de largura, ≥5 faixas"},
                    {"ru", "Улицы, ширина 4U, ≥5 полос"},
                    {"zh", "街道，4U 宽，≥5 车道"}
                }
            },
            {
                RoadCategory.Urban_5UMin, new Dictionary<string, string>
                {
                    {"en", "Urban, 5U wide or wider"},
                    {"de", "Straßen, 5U breit oder breiter"},
                    {"es", "Calles, 5U de ancho o más"},
                    {"fr", "Rues, 5U de large ou plus"},
                    {"ja", "一般道 ｰ 5U幅以上"},
                    {"ko", "도로, 5U 폭 이상"},
                    {"pl", "Ulice, 5U szerokości lub szersze"},
                    {"pt", "Ruas, 5U de largura ou mais"},
                    {"ru", "Улицы, ширина 5U или больше"},
                    {"zh", "街道，5U 宽或更宽"}
                }
            },
            {
                RoadCategory.Rural, new Dictionary<string, string>
                {
                    {"en", "Rural roads"},
                    {"de", "Ländliche Straßen"},
                    {"es", "Caminos rurales"},
                    {"fr", "Routes rurales"},
                    {"ja", "郊外 ｰ 砂利道など"},
                    {"ko", "시골 도로"},
                    {"pl", "Drogi wiejskie"},
                    {"pt", "Estradas rurais"},
                    {"ru", "Сельские дороги"},
                    {"zh", "农村道路"}
                }
            },
            {
                RoadCategory.Industrial, new Dictionary<string, string>
                {
                    {"en", "Industrial roads"},
                    {"de", "Industriestraßen"},
                    {"es", "Caminos industriales"},
                    {"fr", "Routes industrielles"},
                    {"ja", "産業道路"},
                    {"ko", "산업 도로"},
                    {"pl", "Drogi przemysłowe"},
                    {"pt", "Estradas industriais"},
                    {"ru", "Промышленные дороги"},
                    {"zh", "工业道路"}
                }
            },
            {
                RoadCategory.Highway, new Dictionary<string, string>
                {
                    {"en", "Highways"},
                    {"de", "Autobahnen"},
                    {"es", "Autopistas"},
                    {"fr", "Autoroutes"},
                    {"ja", "高速道路"},
                    {"ko", "고속도로"},
                    {"pl", "Autostrady"},
                    {"pt", "Rodovias"},
                    {"ru", "Автодороги"},
                    {"zh", "高速公路"}
                }
            },
            {
                RoadCategory.Bike, new Dictionary<string, string>
                {
                    {"en", "Bike roads"},
                    {"de", "Straßen mit Radinfrastruktur"},
                    {"es", "Vías con infraestructura ciclista"},
                    {"fr", "Routes avec aménagements cyclables"},
                    {"ja", "自転車レーン付き道路"},
                    {"ko", "자전거 인프라가 구축된 도로"},
                    {"pl", "Drogi z infrastrukturą dla rowerzystów"},
                    {"pt", "Vias com infraestrutura para bicicletas"},
                    {"ru", "Дороги с инфраструктурой для велосипедистов"},
                    {"zh", "设有自行车交通设施的道路"}
                }
            },
            {
                RoadCategory.Bus, new Dictionary<string, string>
                {
                    {"en", "Bus roads"},
                    {"de", "Straßen mit Busfahrstreifen"},
                    {"es", "Vías con carriles exclusivos para autobuses"},
                    {"fr", "Voies avec des voies réservées aux bus"},
                    {"ja", "バスレーン付き道路"},
                    {"ko", "버스 전용 차로가 있는 도로"},
                    {"pl", "Drogi z buspasami"},
                    {"pt", "Vias com faixas exclusivas para ônibus"},
                    {"ru", "Дороги с полосами для автобусов"},
                    {"zh", "设有公交车道的道路"}
                }
            },
            {
                RoadCategory.Trolleybus, new Dictionary<string, string>
                {
                    {"en", "Trolleybus roads"},
                    {"de", "Straßen mit Oberleitungsbus-Infrastruktur"},
                    {"es", "Vías con infraestructura para trolebuses"},
                    {"fr", "Voies avec infrastructure pour trolleybus"},
                    {"ja", "トローリーバスの架線付き道路"},
                    {"ko", "트롤리버스 인프라가 갖춰진 도로"},
                    {"pl", "Drogi z infrastrukturą dla trolejbusów"},
                    {"pt", "Vias com infraestrutura para trolebus"},
                    {"ru", "Дороги с троллейбусной инфраструктурой"},
                    {"zh", "设有无轨电车基础设施的道路"}
                }
            },
            {
                RoadCategory.Tram, new Dictionary<string, string>
                {
                    {"en", "Tram roads"},
                    {"de", "Straßen mit Straßenbahnschienen"},
                    {"es", "Vías con infraestructura para tranvía"},
                    {"fr", "Voies avec infrastructure de tramway"},
                    {"ja", "路面電車の軌道付き道路"},
                    {"ko", "전차 트랙이 있는 도로"},
                    {"pl", "Drogi z torami tramwajowymi"},
                    {"pt", "Vias com trilhos de bonde"},
                    {"ru", "Дороги с трамвайными путями"},
                    {"zh", "设有电车轨道的道路"}
                }
            },
            {
                RoadCategory.Monorail, new Dictionary<string, string>
                {
                    {"en", "Monorail roads"},
                    {"de", "Straßen mit Einschienenbahnen"},
                    {"es", "Vías con vías de monorriel"},
                    {"fr", "Voies avec des voies de monorail"},
                    {"ja", "モノレールの軌道付き道路"},
                    {"ko", "도로 위에 단일 레일이 있는 도로"},
                    {"pl", "Drogi z torami kolei jednoszynowej"},
                    {"pt", "Vias com trilhos de monotrilho"},
                    {"ru", "Дороги с монорельсовыми путями"},
                    {"zh", "设有单轨铁路轨道的道路"}
                }
            },
            {
                RoadCategory.MultiModal, new Dictionary<string, string>
                {
                    {"en", "Multi-modal roads"},
                    {"de", "Multimodale Straßen"},
                    {"es", "Carriles multi-modales"},
                    {"fr", "Voies multi-modales"},
                    {"ja", "多用途な道路"},
                    {"ko", "다중 모드 도로"},
                    {"pl", "Drogi multimodalne"},
                    {"pt", "Faixas multimodais"},
                    {"ru", "Мультимодальные дороги"},
                    {"zh", "多模式道路"}
                }
            }
        };
    }
}
