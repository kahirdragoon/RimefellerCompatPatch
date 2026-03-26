using HarmonyLib;
using Rimefeller;
using RimWorld;
using System.Linq;
using Verse;

namespace RCP_VanillaExpanded
{
    [StaticConstructorOnStartup]
    public class Initialize
    {
        private const string ModName = "RCP";

        static Initialize()
        {
            var harmony = new Harmony(ModName);
            harmony.PatchAll();
        }
    }
}