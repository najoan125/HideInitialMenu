using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideInitialMenu
{
    internal static class Patcher { 
        [PatchCondition("HideInitialMenu.scnCLSUpdate", "scnCLS", "Start")]
        public static class Patch
        {
            public static void Prefix(scnCLS __instance)
            {
                if (!Main.featured)
                {
                    __instance.WorkshopLevelsPortal();
                }
                else if (Main.featured)
                {
                    Main.featured = false;
                    __instance.FeaturedLevelsPortal();
                }
            }
        }

        [PatchCondition("HideInitialMenu.scnLevelSelectUpdate","scnLevelSelect","Update")]
        public static class UpdatePatch
        {
            public static void Prefix()
            {
                if (RDEditorUtils.CheckForKeyCombo(true,true,UnityEngine.KeyCode.C) || RDEditorUtils.CheckForKeyCombo(true, true, UnityEngine.KeyCode.E)){
                    return;
                }
                else if (RDEditorUtils.CheckForKeyCombo(true, true, UnityEngine.KeyCode.F))
                {
                    Main.featured = true;
                    GCS.sceneToLoad = "scnCLS";
                    scrUIController.instance.WipeToBlack(WipeDirection.StartsFromRight, null);
                }
            }
        }
    }
}
