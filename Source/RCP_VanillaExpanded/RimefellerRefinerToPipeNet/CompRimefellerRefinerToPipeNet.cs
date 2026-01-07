//using ItemProcessor;
using PipeSystem;
using Rimefeller;
using RimWorld;
using System;
using Verse;

namespace RCP_VanillaExpanded
{
    public class CompRimefellerRefinerToPipeNet : ThingComp
    {
        private CompRefinery compRefinery;
        private CompResource compResource;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);

            compRefinery = parent.GetComp<CompRefinery>();
            compResource = parent.GetComp<CompResource>();
        }

        public override void CompTick()
        {
            if (compResource is null
                || compResource.PipeNet is null
                || compRefinery is null
                || compRefinery.Buffer < compRefinery.Props.BufferSize
                || compResource.PipeNet.AvailableCapacity < compRefinery.Props.StackSize)
                return;

            compResource.PipeNet.DistributeAmongStorage(compRefinery.Props.StackSize, out var stored);
            if (stored == compRefinery.Props.StackSize)
                compRefinery.Buffer = 0;
        }
    }
}
