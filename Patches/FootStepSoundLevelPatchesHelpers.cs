using SilentWalker;

internal static class FootStepSoundLevelPatchesHelpers
{

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