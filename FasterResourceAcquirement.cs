using Verse;
using HarmonyLib;
using System.Reflection;
using System.Linq;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;

namespace FasterResource
{
    public class FasterResourceSettings : ModSettings
    {
        public float miningYieldMultiplier = 1.25f;
        public float miningSpeedMultiplier = 1.25f;
        public float plantHarvestYieldMultiplier = 1.25f;
        public float plantWorkSpeedMultiplier = 1.25f;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref miningYieldMultiplier, "miningYieldMultiplier", 1.25f);
            Scribe_Values.Look(ref miningSpeedMultiplier, "miningSpeedMultiplier", 1.25f);
            Scribe_Values.Look(ref plantHarvestYieldMultiplier, "plantHarvestYieldMultiplier", 1.25f);
            Scribe_Values.Look(ref plantWorkSpeedMultiplier, "plantWorkSpeedMultiplier", 1.25f);

            base.ExposeData();
        }
    }

    public class FasterResourceAcquirementMod : Mod
    {
        public FasterResourceSettings settings;

        public FasterResourceAcquirementMod(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<FasterResourceSettings>();

            LongEventHandler.QueueLongEvent(ApplyChanges, "LoadingDefs_FRA_ApplyStats", true, null);
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            listing.Label("GAME REQUIRES A GAME RESTART TO APPLY CHANGES");

            // === Mining Yield ===
            settings.miningYieldMultiplier = listing.SliderLabeled(
                "Mining Yield: " + settings.miningYieldMultiplier.ToString("P0"), // P0 formats as percentage with 0 decimal places
                settings.miningYieldMultiplier,
                0.5f,  // Min value (50%)
                3.0f   // Max value (300%)
            );
            listing.Gap(12f); // Add a small gap after each slider

            // === Mining Speed ===
            settings.miningSpeedMultiplier = listing.SliderLabeled(
                "Mining Speed: " + settings.miningSpeedMultiplier.ToString("P0"),
                settings.miningSpeedMultiplier,
                0.5f,
                3.0f
            );
            listing.Gap(12f);

            // === Plant Harvest Yield ===
            settings.plantHarvestYieldMultiplier = listing.SliderLabeled(
                "Plant Harvest Yield: " + settings.plantHarvestYieldMultiplier.ToString("P0"),
                settings.plantHarvestYieldMultiplier,
                0.5f,
                3.0f
            );
            listing.Gap(12f);

            // === Plant Work Speed ===
            settings.plantWorkSpeedMultiplier = listing.SliderLabeled(
                "Plant Work Speed: " + settings.plantWorkSpeedMultiplier.ToString("P0"),
                settings.plantWorkSpeedMultiplier,
                0.5f,
                3.0f
            );
            listing.Gap(12f);

            listing.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "Faster Resource Acquirement";
        }

        private void ApplyChanges()
        {
            // Get references to the StatDefs you want to modify
            // StatDefOf provides direct references to many common stats.
            if (StatDefOf.MiningYield != null)
            {
                StatDefOf.MiningYield.defaultBaseValue = 1.0f * settings.miningYieldMultiplier;
                Log.Message($"[FRA] MiningYield set to: {StatDefOf.MiningYield.defaultBaseValue}");
            }

            if (StatDefOf.MiningSpeed != null)
            {
                StatDefOf.MiningSpeed.defaultBaseValue = 1.0f * settings.miningSpeedMultiplier;
                Log.Message($"[FRA] MiningSpeed set to: {StatDefOf.MiningSpeed.defaultBaseValue}");
            }

            if (StatDefOf.PlantHarvestYield != null)
            {
                StatDefOf.PlantHarvestYield.defaultBaseValue = 1.0f * settings.plantHarvestYieldMultiplier;
                Log.Message($"[FRA] PlantHarvestYield set to: {StatDefOf.PlantHarvestYield.defaultBaseValue}");
            }

            if (StatDefOf.PlantWorkSpeed != null)
            {
                StatDefOf.PlantWorkSpeed.defaultBaseValue = 1.0f * settings.plantWorkSpeedMultiplier;
                Log.Message($"[FRA] PlantWorkSpeed set to: {StatDefOf.PlantWorkSpeed.defaultBaseValue}");
            }
        }
    }
}