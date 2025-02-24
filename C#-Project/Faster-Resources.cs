using Verse;
using RimWorld;
using UnityEngine;

namespace FasterResourceAcquirement
{
    public class ResourceSettings : ModSettings
    {
        public StatDef stat_drill;
        public StatDef stat_mining_yield;

        // Default values
        public float drilling_speed = 1.0f; 
        public float mining_yield = 1.0f;

        public ResourceSettings()
        {
            LongEventHandler.QueueLongEvent(() =>
            {
                stat_drill = DefDatabase<StatDef>.GetNamed("DeepDrillingSpeed");
                stat_mining_yield = DefDatabase<StatDef>.GetNamed("MiningYield");

                stat_drill.defaultBaseValue = drilling_speed;
                stat_mining_yield.defaultBaseValue = mining_yield;

            }, "FasterResourceAcquirement.Init", false, null);
        }

        public override void ExposeData()
        {
            Scribe_Values.Look(ref drilling_speed, "drilling_speed", 1.0f);
            Scribe_Values.Look(ref mining_yield, "mining_yield", 1.0f);
            base.ExposeData();
        }
    }

    public class FasterResource : Mod
    {
        ResourceSettings settings;

        public FasterResource(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<ResourceSettings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listing_standard = new Listing_Standard();
            listing_standard.Begin(inRect);
            listing_standard.Label("ANY CHANGE REQUIRES GAME RESTART");

            listing_standard.Label("Drilling Speed: " + settings.drilling_speed + " (Vanilla: 1)");
            settings.drilling_speed = listing_standard.Slider(settings.drilling_speed, 1.0f, 100f);
            settings.drilling_speed = ClampStep(settings.drilling_speed);

            listing_standard.Label("Mining Yield: " + settings.mining_yield + " (Vanilla: 1)");
            settings.mining_yield = listing_standard.Slider(settings.mining_yield, 1.0f, 10.0f);
            settings.mining_yield = ClampStep(settings.mining_yield);

            listing_standard.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "Faster Resource".Translate();
        }

        public static float ClampStep(float value, float step_size = 0.1f)
        {
            return Mathf.Round(value / step_size) * step_size;
        }
    }
}