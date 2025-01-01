using HarmonyLib;
using Il2Cpp;
using MelonLoader;
using UnityEngine;

namespace SilentWalker;

internal class Patches : MelonMod
{
    [HarmonyPatch(typeof(GameAudioManager))]
    public static class FootStepSoundLevelPatches
    {
        private static uint lastLoggedID = 0; // Tracks the last ID logged
        private static float lastLoggedPercentage = 0; // Tracks the last percentage logged

        [HarmonyPatch(nameof(GameAudioManager.SetRTPCValue), typeof(uint), typeof(float), typeof(GameObject))]

        [HarmonyPrefix]
        public static void AdjustFootStepVolume(uint rtpcID, ref float rtpcValue, GameObject go)
        {
            int percentage = 100; // Default percentage
            float originalRtpcValue = rtpcValue; // Store the original value

            switch (rtpcID)
            {
                case 2064316281: // INVENTORYWEIGHTGENERAL
                    percentage = Settings.Instance.inventoryWeightGeneralVolume;
                    break;
                case 135115684: // INVENTORYWEIGHTMETAL
                    percentage = Settings.Instance.inventoryWeightMetalVolume;
                    break;
                case 1721946080: // INVENTORYWEIGHTWATER
                    percentage = Settings.Instance.inventoryWeightWaterVolume;
                    break;
                case 330491720: // INVENTORYWEIGHTWOOD
                    percentage = Settings.Instance.inventoryWeightWoodVolume;
                    break;
                default:
                    return; // Skip the rest of the method if the ID is not recognized
            }

            // Apply the percentage to the RTPC value
            rtpcValue *= percentage / 100f;

            if (Settings.debug_log)
            {
                Dictionary<uint, string> rtpcNames = new Dictionary<uint, string>
                {
                    { 2064316281, "INVENTORYWEIGHTGENERAL" },
                    { 135115684, "INVENTORYWEIGHTMETAL" },
                    { 1721946080, "INVENTORYWEIGHTWATER" },
                    { 330491720, "INVENTORYWEIGHTWOOD" },
                };

                // Log only if both the ID and percentage have changed
                if (lastLoggedID != rtpcID || lastLoggedPercentage != percentage)
                {
                    lastLoggedID = rtpcID;
                    lastLoggedPercentage = percentage;

                    if (rtpcNames.TryGetValue(rtpcID, out var rtpcName))
                    {
                        Main.Logger.Msg($"{rtpcName}: {originalRtpcValue} -> {rtpcValue} ({percentage}%)");
                    }
                }
            }
        }
    }

    [HarmonyPatch(typeof(FootStepSounds))]
    public static class FootStepSoundDisablePatches
    {
        [HarmonyPatch(nameof(FootStepSounds.PlayFootStepSound), typeof(Vector3), typeof(string), typeof(FootStepSounds.State))]
        [HarmonyPrefix]
        public static bool SuppressFootsteps()
        {
            // Check the boolean setting to determine whether to suppress footsteps
            if (Settings.Instance.silenceFootSteps)
            {
                return false; // Skip the original method
            }
            return true; // Allow footsteps if suppression is disabled
        }
    }
}