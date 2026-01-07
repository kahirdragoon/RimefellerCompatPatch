//using ItemProcessor;
using PipeSystem;
using Rimefeller;
using RimWorld;
using Verse;

namespace RCP_VanillaExpanded
{
    public class CompFillStorageProcessorFromRimefellerPipeNet : CompPipe
    {
        private CompResourceStorage compResourceStorage;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);

            compResourceStorage = parent.GetComp<CompResourceStorage>();
        }

        public override void CompTick()
        {
            if (compResourceStorage is null
                || pipeNet is null
                || !Gen.IsHashIntervalTick(parent, 3)
                || compResourceStorage.AmountCanAccept < 1
                || !pipeNet.PullFuel(1))
                return;

            compResourceStorage.AddResource(1);
        }
    }
}
