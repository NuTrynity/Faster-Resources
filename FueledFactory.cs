using RimWorld;

namespace FasterResource
{
    public class FueledFactoryCompProperties : CompProperties_Spawner
    {
        public FueledFactoryCompProperties()
        {
            compClass = typeof(FueledFactoryComp);
        }
    }

    public class FueledFactoryComp : CompSpawner
    {
        private CompRefuelable? fuelComp;
        public FueledFactoryComp Props => Props;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            fuelComp = parent.GetComp<CompRefuelable>();
        }

        public override void CompTick()
        {
            if (fuelComp == null || !fuelComp.HasFuel) return;

            base.CompTick();
        }
    }
}