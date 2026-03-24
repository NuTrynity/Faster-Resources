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

    #region Changes
    public class FasterResourceStats : StatPart
    {
        public override void TransformValue(StatRequest req, ref float val)
        {
            if (req.Thing is Pawn pawn && pawn.Faction != null && pawn.Faction.IsPlayer)
            {
                var settings = LoadedModManager.GetMod<FasterResourceMod>().mod_settings;

                if (parentStat.defName == "MininYield") val *= settings.mining_yield;
                if (parentStat.defName == "MiningSpeed") val *= settings.mining_speed;
                if (parentStat.defName == "PlantYield") val *= settings.plant_yield;
                if (parentStat.defName == "PlantSpeed") val *= settings.plant_speed;
            }
        }

        public override string? ExplanationPart(StatRequest req)
        {
            if (req.Thing is Pawn pawn && pawn.Faction?.IsPlayer == true)
                return "[FRA] Player Multiplier Active";
            
            return null;
        }
    }
    #endregion

    public class FasterResourceMod : Mod
    {
        public FasterResourceSettings mod_settings;

        public FasterResourceMod(ModContentPack content) : base(content)
        {
            mod_settings = GetSettings<FasterResourceSettings>();
            //LongEventHandler.QueueLongEvent(ApplyChanges, "LoadingDefs_FRA_ApplyStats", true, null);
        }

        #region Mod Settings
        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            listing.Label("Mining Yield " + mod_settings.mining_yield.ToString("F1"));
            mod_settings.mining_yield = listing.Slider(mod_settings.mining_yield, 1.0f, 10.0f);

            listing.Label("Mining Speed " + mod_settings.mining_speed.ToString("F1"));
            mod_settings.mining_speed = listing.Slider(mod_settings.mining_speed, 1.0f, 10.0f);

            listing.Label("Plant Yield " + mod_settings.plant_yield.ToString("F1"));
            mod_settings.plant_yield = listing.Slider(mod_settings.plant_yield, 1.0f, 10.0f);

            listing.Label("Plant Speed " + mod_settings.plant_speed.ToString("F1"));
            mod_settings.plant_speed = listing.Slider(mod_settings.plant_speed, 1.0f, 10.0f);

            listing.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override void WriteSettings()
        {
            base.WriteSettings();
        }

        public override string SettingsCategory()
        {
            return "[NuT] Faster Resource Acquirement";
        }
        #endregion
    }
}