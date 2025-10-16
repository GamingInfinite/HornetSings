using System.Collections.Generic;
using HarmonyLib;
using TeamCherry.Localization;

namespace HornetShermaSong.Patches
{

    [HarmonyPatch(typeof(Language), nameof(Language.SwitchLanguage), typeof(LanguageCode))]
    public static class Localization
    {
        [HarmonyPostfix]
        private static void AddNewSheet()
        {
            Dictionary<string, Dictionary<string, string>> fullStore = Language._currentEntrySheets;

            if (!fullStore.ContainsKey("Sherma's BellTOOL"))
            {
                fullStore.Add("Sherma's BellTOOL", new()
                {
                    { "Sherma's BellTOOLNAME", "Sherma's Bell" },
                    { "Sherma's BellTOOLDESC", "The bell that Sherma plays while singing" }
                });
            }

            Language._currentEntrySheets = fullStore;
        }
    }
}
