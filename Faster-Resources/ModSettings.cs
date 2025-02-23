using Verse;
using System.Collections.Generic;
using UnityEngine;

namespace FasterResource
{
    public class ResourceSettings : ModSettings
    {
        public float drilling_speed = 1.0f;

        public override void ExposeData()
        {
            Scribe_Values.Look(ref drilling_speed, "drilling_speed", 1.0f);
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
            listing_standard.Label("Drilling Speed: " + settings.drilling_speed);
            settings.drilling_speed = listing_standard.Slider(settings.drilling_speed, 0f, 10f);
        }

        public override string SettingsCategory()
        {
            return "Faster Resource".Translate();
        }
    }
}