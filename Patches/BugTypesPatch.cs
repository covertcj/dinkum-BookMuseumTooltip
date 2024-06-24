using System;
using HarmonyLib;

namespace BookMuseumTooltip.Patches
{
    class BugTypesPatch
    {
        /// <summary>
        /// Closes all toggled books when the user decides to sleep, so that
        /// there's at least some burden to re-open the book, as opposed to
        /// making it permenant.
        /// </summary>
        /// <returns>True, always. So as to not overrite functionality.</returns>
        [HarmonyPatch(typeof(BugTypes), nameof(BugTypes.openBook))]
        [HarmonyPrefix]
        static bool openBook()
        {
            Plugin.Logger.LogInfo("Bug book opened");
            return true;
        }
    }
}