using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HideInitialMenu
{
    internal static class Patcher {
        public static bool isUpdate = false;

        [HarmonyPatch(typeof(scnCLS),"Update")]
        public static class Patch
        {
            public static void Prefix(scnCLS __instance)
            {
                if (!Main.featured && !Daily && isUpdate)
                {
                    __instance.WorkshopLevelsPortal();
                }
                else if (Main.featured && isUpdate)
                {
                    Main.featured = false;
                    __instance.FeaturedLevelsPortal();
                }
                else if (Daily)
                {
                    Daily = false;
                }
                isUpdate = false;
            }
        }

        [HarmonyPatch(typeof(scnCLS),"Start")]
        public static class StartPatch
        {
            public static void Prefix()
            {
                isUpdate = true;
            }
        }

        [HarmonyPatch(typeof(RDEditorUtils), "IsNullOrEmpty")]
        public static class isNullPatch
        {
            public static void Postfix(ref bool __result)
            {
                if (isUpdate)
                {
                    __result = true;
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
                else if (RDEditorUtils.CheckForKeyCombo(true, false, UnityEngine.KeyCode.D))
                {
                    Daily = true;
                    GCS.sceneToLoad = "scnCLS";
                    scrUIController.instance.WipeToBlack(WipeDirection.StartsFromRight, null);
                }
            }
        }

        public static bool Daily = false;
    }
}
