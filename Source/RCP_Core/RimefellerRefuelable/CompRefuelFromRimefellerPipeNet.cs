using Rimefeller;
using RimWorld;
using UnityEngine;
using Verse;

namespace RCP_Core
{
    public class CompRefuelFromRimefellerPipeNet : CompPipe
    {
        private RefuelableRimefellerPipeSystemCompatManager manager;
        private CompRefuelable compRefuelable;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);

            compRefuelable = parent.GetComp<CompRefuelable>();
            
            manager = parent.Map.GetComponent<RefuelableRimefellerPipeSystemCompatManager>();
            manager.RefuelComps.Add(this);
        }

        public override void PostDeSpawn(Map map)
        {
            base.PostDeSpawn(map);

            manager.RefuelComps.Remove(this);
        }

        public override void CompTick()
        {
            // Prevents executing CompPipe.CompTick so Refuel logic is not executed twice
        }

        public void CompTick2()
        {
            if (!Gen.IsHashIntervalTick(parent, 3) || compRefuelable is null || pipeNet is null)
                return;

            float missingChemfuel = (compRefuelable.TargetFuelLevel - compRefuelable.Fuel) / compRefuelable.Props.FuelMultiplierCurrentDifficulty;
            
            if (missingChemfuel >= compRefuelable.Props.FuelMultiplierCurrentDifficulty)
            {
                if (pipeNet.PullFuel(1))
                    compRefuelable.Refuel(1);
            }
        }
    }
}
