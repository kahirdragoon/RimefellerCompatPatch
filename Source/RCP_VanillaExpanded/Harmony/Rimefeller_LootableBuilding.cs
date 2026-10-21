using HarmonyLib;
using Rimefeller;
using RimWorld;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using VEF.Buildings;
using Verse;

namespace RCP_VanillaExpanded
{
    [HarmonyPatch(typeof(LootableBuilding_Custom), nameof(LootableBuilding_Custom.Open))]
    internal static class Rimefeller_LootableBuilding
    {
        private static readonly MethodInfo TryPlaceThing = AccessTools.Method(typeof(GenPlace), nameof(GenPlace.TryPlaceThing),
            new[] { typeof(Thing), typeof(IntVec3), typeof(Map), typeof(ThingPlaceMode), typeof(Action<Thing, int>), typeof(Predicate<IntVec3>), typeof(Rot4?), typeof(int) });

        // Routes every loot placement in Open through PlaceOrPushToNet, with the building appended as an extra argument.
        private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            foreach (var instruction in instructions)
            {
                if (instruction.Calls(TryPlaceThing))
                {
                    yield return new CodeInstruction(OpCodes.Ldarg_0).MoveLabelsFrom(instruction);
                    yield return CodeInstruction.Call(typeof(Rimefeller_LootableBuilding), nameof(PlaceOrPushToNet));
                }
                else
                    yield return instruction;
            }
        }

        private static bool PlaceOrPushToNet(Thing thing, IntVec3 center, Map map, ThingPlaceMode mode, Action<Thing, int> placedAction,
            Predicate<IntVec3> nearPlaceValidator, Rot4? rot, int squareRadius, LootableBuilding_Custom building)
        {
            var compPipe = building.GetComp<CompPipe>();
            if (thing.def == ThingDefOf.Chemfuel && compPipe?.pipeNet != null && compPipe.pipeNet.PushFuel(thing.stackCount) < 1)
            {
                thing.Destroy();
                return true;
            }

            return GenPlace.TryPlaceThing(thing, center, map, mode, placedAction, nearPlaceValidator, rot, squareRadius);
        }
    }
}
