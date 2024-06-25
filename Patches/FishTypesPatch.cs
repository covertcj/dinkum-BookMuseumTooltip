using HarmonyLib;

namespace BookMuseumTooltip.Patches
{
    class FishTypesPatch
    {
        const string ICON_GOLD = "<sprite=11>";
        const string ICON_MUSEUM = "<sprite=15>";

        /// <summary>
        /// Runs after the game's openBook call and replaces the tooltip if and
        /// only if the item is in the pedia but missing from the museum.
        /// </summary>
        [HarmonyPatch(typeof(FishType), nameof(FishType.openBook))]
        [HarmonyPostfix]
        static void openBook(NameTag ___myNameTag, int ___myFishType)
        {
            // the original openBook will activate the nametag if the tooltip
            // is shown
            if (!___myNameTag.isActiveAndEnabled) return;

            // only override the game's text if its displaying the full tooltip
            if (___myFishType < 0 || !PediaManager.manage.isInPedia(___myFishType)) return;

            InventoryItem item = Inventory.Instance.allItems[___myFishType];
            // only override the game's text if the item can be donated
            if (!MuseumManager.manage.checkIfDonationNeeded(item)) return;

            string value = item.value.ToString();
            string name = item.getInvItemName();

            ___myNameTag.turnOn($"{ICON_GOLD}{value}\n{ICON_MUSEUM}{name}");
        }
    }
}