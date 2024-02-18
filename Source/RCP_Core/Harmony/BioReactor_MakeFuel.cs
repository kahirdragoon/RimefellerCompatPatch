using HarmonyLib;
using Rimefeller;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Verse;

namespace RCP_Core
{
    internal static class BioReactor_MakeFuel
    {
        [HarmonyPatch]
        public static bool Prefix(Building __instance)
        {
            var pipe = __instance.GetComp<CompPipe>();
            if (pipe == null)
                return true;

            if(pipe.pipeNet.PushFuel(35) != 0)
                return true;
            return false;
        }
    }
}
