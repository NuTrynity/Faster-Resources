using System.Collections.Generic;
using Verse;
using RimWorld;
using UnityEngine;

namespace FasterResource
{
    public class FasterResourceSettings : ModSettings
    {
        public float mining_yield = 2.0f;
        public float mining_speed = 2.0f;
        public float plant_yield = 2.0f;
        public float plant_speed = 2.0f;
        public float drill_speed = 2.0f;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref mining_yield, "mining_yield", 2.0f);
            Scribe_Values.Look(ref mining_speed, "mining_speed", 2.0f);
            Scribe_Values.Look(ref plant_yield, "plant_yield", 2.0f);
            Scribe_Values.Look(ref plant_speed, "plant_speed", 2.0f);
            Scribe_Values.Look(ref drill_speed, "drill_speed", 2.0f);

            base.ExposeData();
        }
    }

    public class FasterResourceMod : Mod
    {
        public FasterResourceSettings mod_settings;

        public FasterResourceMod(ModContentPack content) : base(content)
        {
            mod_settings = GetSettings<FasterResourceSettings>();
            
            LongEventHandler.QueueLongEvent(InjectStatParts, "Initializing_FRA_Stats", false, null);
        }

        private void InjectStatParts()
        {
            // Define the exact internal StatDefs we want to target
            var targetStats = new List<StatDef>
            {
                StatDefOf.MiningYield,
                StatDefOf.MiningSpeed,
                StatDefOf.PlantHarvestYield,
                StatDefOf.PlantWorkSpeed,
                StatDefOf.DeepDrillingSpeed
            };

            // Create an instance of our custom modifier
            FasterResourceStats customPart = new FasterResourceStats();

            // Inject it seamlessly into each targeted stat track
            foreach (StatDef stat in targetStats)
            {
                if (stat.parts == null) stat.parts = new List<StatPart>();
                stat.parts.Add(customPart);
            }
        }

        #region Mod Settings UI
        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            listing.Label($"Mining Yield: {mod_settings.mining_yield:F1}x");
            mod_settings.mining_yield = listing.Slider(mod_settings.mining_yield, 1.0f, 10.0f);

            listing.Label($"Mining Speed: {mod_settings.mining_speed:F1}x");
            mod_settings.mining_speed = listing.Slider(mod_settings.mining_speed, 1.0f, 10.0f);

            listing.Label($"Plant Yield: {mod_settings.plant_yield:F1}x");
            mod_settings.plant_yield = listing.Slider(mod_settings.plant_yield, 1.0f, 10.0f);

            listing.Label($"Plant Speed: {mod_settings.plant_speed:F1}x");
            mod_settings.plant_speed = listing.Slider(mod_settings.plant_speed, 1.0f, 10.0f);

            listing.Label($"Deep Drilling Speed: {mod_settings.drill_speed:F1}x");
            mod_settings.drill_speed = listing.Slider(mod_settings.drill_speed, 1.0f, 10.0f);

            listing.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory() => "[NuT] Faster Resource Acquirement";
        #endregion
    }

    #region The Dynamic Stat Modifier
    public class FasterResourceStats : StatPart
    {
        public override void TransformValue(StatRequest req, ref float val)
        {
            // Only apply adjustments to player-owned pawns
            if (req.Thing is Pawn pawn && pawn.Faction?.IsPlayer == true)
            {
                var settings = LoadedModManager.GetMod<FasterResourceMod>().mod_settings;

                if (parentStat == StatDefOf.MiningYield) val *= settings.mining_yield;
                if (parentStat == StatDefOf.MiningSpeed) val *= settings.mining_speed;
                if (parentStat == StatDefOf.PlantHarvestYield) val *= settings.plant_yield;
                if (parentStat == StatDefOf.PlantWorkSpeed) val *= settings.plant_speed;
                if (parentStat == StatDefOf.DeepDrillingSpeed) val *= settings.drill_speed;
            }
        }

        public override string ExplanationPart(StatRequest req)
        {
            if (req.Thing is Pawn pawn && pawn.Faction?.IsPlayer == true)
                return "[FRA] Player Multiplier Active";
            
            return null;
        }
    }
    #endregion
}