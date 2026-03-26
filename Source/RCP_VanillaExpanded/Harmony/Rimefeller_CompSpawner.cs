using HarmonyLib;
using PipeSystem;
using Rimefeller;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace RCP_VanillaExpanded
{
    [HarmonyPatch(typeof(Process), nameof(Process.SpawnOrPushToNet))]
    internal static class Rimefeller_CompSpawner
    {
        private static bool Prefix(Process __instance)
        {
            var compPipe = __instance.advancedProcessor?.parent.GetComp<CompPipe>();
            if(compPipe is null || __instance.Def.results.Count != 1)
                return true;

            var result = __instance.Def.results[0];
            if (result.thing != ThingDefOf.Chemfuel)
                return true;

            var count = result.GetCount(__instance);
            float remainingFuel = compPipe.pipeNet.PushFuel(count);

            if (remainingFuel >= 1)
            {
                compPipe.pipeNet.PullFuel(count - remainingFuel);
                return true;
            }
            
            __instance.advancedProcessor.parent.Map.resourceCounter.UpdateResourceCounts();
            __instance.ResetProcess();
            __instance.Notify_Ended();

            return false;
        }
    }
}
