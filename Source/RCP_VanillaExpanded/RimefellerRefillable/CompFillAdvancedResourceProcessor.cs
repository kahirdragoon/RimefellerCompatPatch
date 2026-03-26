using HarmonyLib;
using PipeSystem;
using Rimefeller;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Verse;

namespace RCP_VanillaExpanded
{
    public class CompFillAdvancedResourceProcessor : CompPipe
    {
        private CompAdvancedResourceProcessor processor;

        private static readonly AccessTools.FieldRef<ThingAndResourceOwner, int> processCount = AccessTools.FieldRefAccess<ThingAndResourceOwner, int>("count");

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);
            processor = parent.GetComp<CompAdvancedResourceProcessor>();
        }

        public override void CompTickRare()
        {
            base.CompTickRare();

            if (pipeNet == null || processor?.Process == null || !processor.AllCompsOn)
                return;
            processor.Process.IngredientsOwners.Where(i => i.ThingDef == ThingDefOf.Chemfuel).ToList().ForEach(i =>
            {
                if (i.Required > 0)
                {
                    if (pipeNet.PullFuel(i.Required))
                        processCount(i) += i.Required;
                    else
                    {
                        pipeNet.PullFuel((int)pipeNet.TotalFuelNow);
                        processCount(i) = (int)pipeNet.TotalFuelNow;
                    }

                }
            });
        }
    }
}
