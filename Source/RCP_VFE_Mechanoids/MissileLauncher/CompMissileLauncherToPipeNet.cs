using Rimefeller;
using RimWorld;
using HarmonyLib;
using Verse;
using VFEMech;

namespace RCP_VFE_Mechanoids
{
    public class CompMissileLauncherToPipeNet : CompPipe
    {

        public override void CompTick()
        {
            base.CompTick();

            if (!Gen.IsHashIntervalTick(parent, 3))
                return;

            if (parent is MissileSilo silo
                && silo.TargetAcquired
                && !silo.Satisfied
                && silo.CostMissing(ThingDefOf.Chemfuel) >= 1
                && pipeNet != null
                && pipeNet.PullFuel(1))
            {
                var chemfuel = ThingMaker.MakeThing(ThingDefOf.Chemfuel);
                chemfuel.stackCount = 1;
                silo.AddCost(chemfuel);
            }
        }
    }
}
