using HarmonyLib;
using Rimefeller;
using RimWorld;
using System.Linq;
using Verse;

namespace RCP_Core
{
    [StaticConstructorOnStartup]
    public class Initialize
    {
        private const string ModName = "RCP";

        static Initialize()
        {
            var harmony = new Harmony(ModName);
            harmony.PatchAll();

            if(AccessTools.Method("BioReactor.Building_BioReactor:MakeFuel") != null)
            {
                var original = AccessTools.Method("BioReactor.Building_BioReactor:MakeFuel");
                var prefix = typeof(BioReactor_MakeFuel).GetMethod(nameof(BioReactor_MakeFuel.Prefix));
                harmony.Patch(original, prefix: new HarmonyMethod(prefix));
            }

            AddBuildingsToRimefellerPipelineNet();
        }

        private static void AddBuildingsToRimefellerPipelineNet()
        {
            var thingsDefs = DefDatabase<ThingDef>
                .AllDefsListForReading
                .Where(def => (def.HasComp(typeof(CompRefuelable)) || def.HasComp(typeof(CompSpawner))) && !def.HasComp(typeof(CompPipe)));

            int countRefuel = 0;
            int countSpawn = 0;
            foreach (var def in thingsDefs)
            {
                //Log.Message(def.defName);
                //Log.Message("Comps: ");
                //foreach (CompProperties c in def.comps)
                //    Log.Message("- " + c.compClass.Name);

                var r = def.GetCompProperties<CompProperties_Refuelable>();
                if (r != null && r.fuelFilter.Allows(ThingDefOf.Chemfuel))
                {
                    def.comps.Add(new CompProperties_RefuelFromRimefellerPipeNet());
                    countRefuel++;
                    //Log.Message($"[{modName}]: Added Rimefeller Refuel Pipe Compatibility to {t.defName}");
                }

                var s = def.GetCompProperties<CompProperties_Spawner>();
                if(s != null && s.thingToSpawn == ThingDefOf.Chemfuel)
                {
                    def.comps.Add(new CompProperties_Pipe());
                    countSpawn++;
                    //Log.Message($"[{ModName}]: Added Rimefeller Spawner Pipe Compatibility to {def.defName}");
                }
            }

            Log.Message($"[{ModName}]: Finished adding Rimefeller Refuel Pipe Compatibility to {countRefuel} items");
            Log.Message($"[{ModName}]: Finished adding Rimefeller Spawner Pipe Compatibility to {countSpawn} items");
        }
    }
}