using Verse;
using RimWorld;
using UnityEngine;

namespace FasterResource
{
    public class FasterResourceSettings : ModSettings
    {
        public float miningYieldMultiplier = 2.0f;
        public float miningSpeedMultiplier = 2.0f;
        public float plantYieldMultiplier = 2.0f;
        public float plantSpeedMultiplier = 2.0f;
        public float drillSpeedMultiplier = 2.0f;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref miningYieldMultiplier, "miningYieldMultiplier", 2.0f);
            Scribe_Values.Look(ref miningSpeedMultiplier, "miningSpeedMultiplier", 2.0f);
            Scribe_Values.Look(ref plantYieldMultiplier, "plantYieldMultiplier", 2.0f);
            Scribe_Values.Look(ref plantSpeedMultiplier, "plantSpeedMultiplier", 2.0f);
            Scribe_Values.Look(ref drillSpeedMultiplier, "drillSpeedMultiplier", 2.0f);

            base.ExposeData();
        }
    }

    public class FasterResourceMod : Mod
    {
        public FasterResourceSettings modSettings;

        public float baseMiningYield;
        public float baseMiningSpeed;
        public float basePlantYield;
        public float basePlantSpeed;
        public float baseDrillSpeed;

        public FasterResourceMod(ModContentPack content) : base(content)
        {
            modSettings = GetSettings<FasterResourceSettings>();
            LongEventHandler.QueueLongEvent(ApplyStats, "Initializing_FRA_Stats", false, null);
        }

        #region Mod Settings UI
        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            listing.Label($"Mining Yield: {modSettings.miningYieldMultiplier:F1}x");
            modSettings.miningYieldMultiplier = listing.Slider(modSettings.miningYieldMultiplier, 1.0f, 10.0f);

            listing.Label($"Mining Speed: {modSettings.miningSpeedMultiplier:F1}x");
            modSettings.miningSpeedMultiplier = listing.Slider(modSettings.miningSpeedMultiplier, 1.0f, 10.0f);

            listing.Label($"Plant Yield: {modSettings.plantYieldMultiplier:F1}x");
            modSettings.plantYieldMultiplier = listing.Slider(modSettings.plantYieldMultiplier, 1.0f, 10.0f);

            listing.Label($"Plant Speed: {modSettings.plantSpeedMultiplier:F1}x");
            modSettings.plantSpeedMultiplier = listing.Slider(modSettings.plantSpeedMultiplier, 1.0f, 10.0f);

            listing.Label($"Deep Drilling Speed: {modSettings.drillSpeedMultiplier:F1}x");
            modSettings.drillSpeedMultiplier = listing.Slider(modSettings.drillSpeedMultiplier, 1.0f, 10.0f);

            listing.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory() => "[NuT] Faster Resource Acquirement";

        public override void WriteSettings()
        {
            base.WriteSettings();
            UpdateStats();
        }
        #endregion

        #region Stats
        public void ApplyStats()
        {
            SetBaseStats();
            UpdateStats();
        }

        private void SetBaseStats()
        {
            baseMiningSpeed = StatDefOf.MiningSpeed.defaultBaseValue;
            baseMiningYield = StatDefOf.MiningYield.defaultBaseValue;

            basePlantSpeed = StatDefOf.PlantWorkSpeed.defaultBaseValue;
            basePlantYield = StatDefOf.PlantHarvestYield.defaultBaseValue;

            baseDrillSpeed = StatDefOf.DeepDrillingSpeed.defaultBaseValue;
        }

        private void UpdateStats()
        {
            StatDefOf.MiningSpeed.defaultBaseValue = baseMiningSpeed * modSettings.miningSpeedMultiplier;
            StatDefOf.MiningYield.defaultBaseValue = baseMiningYield * modSettings.miningYieldMultiplier;

            StatDefOf.PlantWorkSpeed.defaultBaseValue = basePlantSpeed * modSettings.plantSpeedMultiplier;
            StatDefOf.PlantHarvestYield.defaultBaseValue = basePlantYield * modSettings.plantYieldMultiplier;

            StatDefOf.DeepDrillingSpeed.defaultBaseValue = baseDrillSpeed * modSettings.drillSpeedMultiplier;
        }
        #endregion
    }
}