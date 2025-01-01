using ModSettings;
using System.Reflection;

namespace SilentWalker
{
    public class Settings : JsonModSettings
    {

        internal static bool debug_log = true; // Set to true to enable debug logging

        internal static Settings Instance { get; } = new();
        internal static void OnLoad()
        {
            Instance.AddToModSettings(BuildInfo.GUIName);
            Instance.RefreshGUI();
        }
        protected override void OnChange(FieldInfo field, object? oldValue, object? newValue)
        {
            base.OnChange(field, oldValue, newValue);
            Instance.RefreshGUI();
        }
        //protected override void OnConfirm()
        //{
        //    base.OnConfirm();
        //}

        [Name("Silence Foot Steps")]
        [Description("Completely silence the sound of your own footsteps.\nTurning this on ignores the values below." +
            "\n(Applies instantly)")]
        public bool silenceFootSteps = false;

        [Name("Metal")]
        [Description("Adjust the sound level for metal items in your backback. Vanilla is 100%" +
            "\n(Change requires scene reload)")]
        [Slider(1, 100, NumberFormat = "{0:0}%")]
        public int inventoryWeightMetalVolume = 100;

        [Name("Wood")]
        [Description("Adjust the sound level for wood items in your backback. Vanilla is 100%" +
            "\n(Change requires scene reload)")]
        [Slider(1, 100, NumberFormat = "{0:0}%")]
        public int inventoryWeightWoodVolume = 100;

        [Name("Water")]
        [Description("Adjust the sound level for water in your backback. Vanilla is 100%" +
            "\n(Change requires scene reload)")]
        [Slider(1, 100, NumberFormat = "{0:0}%")]
        public int inventoryWeightWaterVolume = 100;

        [Name("Other")]
        [Description("Adjust the sound level for miscellaneous items in your backback. Vanilla is 100%" +
            "\n(Change requires scene reload)")]
        [Slider(1, 100, NumberFormat = "{0:0}%")]
        public int inventoryWeightGeneralVolume = 100;
    }
}
