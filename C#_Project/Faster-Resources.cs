using RimWorld;
using UnityEngine;
using Verse;

namespace FasterResourceAcquirement
{
    [StaticConstructorOnStartup]
    public static class SpeedPatch
    {
        static SpeedPatch()
        {
            LongEventHandler.QueueLongEvent(() =>
            {
                Settings settings = LoadedModManager.GetMod<FasterResources>().GetSettings<Settings>();

                StatDef stat_drill = DefDatabase<StatDef>.GetNamed("DeepDrillingSpeed");
                StatDef stat_mining_yield = DefDatabase<StatDef>.GetNamed("MiningYield");

                if (stat_drill != null && stat_mining_yield != null)
                {
                    stat_drill.defaultBaseValue = settings.drilling_speed;
                    stat_mining_yield.defaultBaseValue = settings.mining_yield;

                    Log.Message("Drill Speed has been changed to: " + stat_drill.defaultBaseValue);
                    Log.Message("Mining Yield has been changed to: " + stat_mining_yield.defaultBaseValue);
                }
                else
                {
                    Log.Error("FasterResourcesAcquirement: DeepDrillingSpeed or MiningYield StatDef not found!");
                }
            }, "FasterResourcesAcquirement.SpeedPatch.Init", false, null, true);
        }
    }

    public class Settings : ModSettings
    {
        public float drilling_speed = 1.0f;
        public float mining_yield = 1.0f;

        public override void ExposeData()
        {
            Scribe_Values.Look<float>(ref drilling_speed, "drilling_speed", 1.0f);
            Scribe_Values.Look<float>(ref mining_yield, "mining_yield", 1.0f);
            base.ExposeData();
        }
    }

    public class FasterResources : Mod
    {
        Settings settings;

        public FasterResources(ModContentPack content) : base(content)
        {
            this.settings = GetSettings<Settings>();
        }

        public override void DoSettingsWindowContents(Rect inRect)
        {
            Listing_Standard listing = new Listing_Standard();
            listing.Begin(inRect);

            listing.Label("ANY CHANGE REQUIRES GAME RESTART");
            listing.Label("Deep Drill Speed " + settings.drilling_speed + " (Vanilla: 1)");
            settings.drilling_speed = listing.Slider(settings.drilling_speed, 1.0f, 10.0f);
            settings.drilling_speed = ClampFloat(settings.drilling_speed);

            listing.Label("Mining Yield " + settings.mining_yield + " (Vanilla: 1)");
            settings.mining_yield = listing.Slider(settings.mining_yield, 1.0f, 10.0f);
            settings.mining_yield = ClampFloat(settings.mining_yield);

            listing.End();
            base.DoSettingsWindowContents(inRect);
        }

        public override string SettingsCategory()
        {
            return "Faster Resources".Translate();
        }

        public float ClampFloat(float value)
        {
            return Mathf.Round(value * 10f) / 10f;
        }
    }
}