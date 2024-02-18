using HarmonyLib;
using Rimefeller;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace RCP_Core
{
    [HarmonyPatch(typeof(CompSpawner), nameof(CompSpawner.TryDoSpawn))]
    internal static class Rimefeller_CompSpawner
    {
        private static bool Prefix(CompSpawner __instance)
        {
            if (!__instance.parent.Spawned)
                return false;

            if (__instance.PropsSpawner.thingToSpawn != ThingDefOf.Chemfuel)
                return true;

            var compPipe = __instance.parent.GetComp<CompPipe>();
            if (compPipe is null)
                return true;
            float remainingFuel = compPipe.pipeNet.PushFuel(__instance.PropsSpawner.spawnCount);

            if (remainingFuel > 0)
                return true;

            return false;
        }
    }
}
