using Rimefeller;
using RimWorld;
using HarmonyLib;
using VFEAncients;
using Verse;

namespace RCP_VFE_Ancients
{
    public class CompJunctionToPipeNet : CompPipe
    {
        Traverse ticksTillRefill;
        ThingOwner innerContainer;
        bool hacked;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);

            var junction  =  Traverse.Create(parent);
            ticksTillRefill = junction.Field("ticksTillRefill");
            innerContainer = junction.Field("innerContainer").GetValue<ThingOwner>();
        }

        public override void CompTick()
        {
            base.CompTick();

            if (hacked
                && !Gen.IsHashIntervalTick(parent, 3)
                && parent is Building_PipelineJunction junction
                && pipeNet != null
                && pipeNet.PushFuel(junction.Count) == 0)
            {
                ticksTillRefill.SetValue(junction.RefillTime);
                hacked = false;
                innerContainer.ClearAndDestroyContents();
            }
        }

        public override void ReceiveCompSignal(string signal)
        {
            if (signal == CompHackable.HackedSignal)
                hacked = true;
        }
    }
}
