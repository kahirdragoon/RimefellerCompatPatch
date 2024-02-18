using ItemProcessor;
using PipeSystem;
using Rimefeller;
using RimWorld;
using Verse;

namespace RCP_VanillaExpanded
{
    public class CompRefillFromRimefellerPipeNet : CompPipe
    {
        private RefillableRimefellerPipeSystemCompatManager manager;
        private Building_ItemProcessor itemProcessor;
        private CompRefillWithPipes compRefill;

        public override void PostSpawnSetup(bool respawningAfterLoad)
        {
            base.PostSpawnSetup(respawningAfterLoad);

            compRefill = parent.GetComp<CompRefillWithPipes>();

            if (parent is Building_ItemProcessor processor)
            {
                itemProcessor = processor;
                manager = parent.Map.GetComponent<RefillableRimefellerPipeSystemCompatManager>();
                manager.RefillComps.Add(this);
            }
        }

        public override void PostDeSpawn(Map map)
        {
            base.PostDeSpawn(map);

            manager?.RefillComps.Remove(this);
        }

        public override void CompTick()
        {
            // Prevents executing CompPipe.CompTick so Refill logic is not executed twice
        }

        public void CompTick2()
        {
            if (!Gen.IsHashIntervalTick(parent, 3))
                return;

            // Copied and modified from PipeSystem.CompRefillWithPipes.RefillProcessor from Vanilla Expanded Framework
            if (pipeNet != null 
                && itemProcessor != null 
                && itemProcessor.thisRecipe != null 
                && DefDatabase<CombinationDef>.GetNamed(itemProcessor.thisRecipe, false) is CombinationDef combinationDef)
            {
                if(combinationDef.items != null && combinationDef.items.Contains(ThingDefOf.Chemfuel.defName))
                {
                    int thingNeed = itemProcessor.ExpectedAmountFirstIngredient - itemProcessor.CurrentAmountFirstIngredient;
                    if (itemProcessor.processorStage != ProcessorStage.Working && thingNeed > 0)
                    {
                        int resourceNeeded = compRefill != null ? compRefill.Props.ratio : 1;
                        
                        if (!pipeNet.PullFuel(resourceNeeded))
                            return;
                        
                        Thing thing = ThingMaker.MakeThing(ThingDefOf.Chemfuel); 
                        thing.stackCount = 1;
                        
                        if (itemProcessor.compItemProcessor.Props.transfersIngredientLists)
                        {
                            if (thing.TryGetComp<CompIngredients>() is CompIngredients ingredientComp)
                            {
                                itemProcessor.ingredients.AddRange(ingredientComp.ingredients);
                            }
                        }
                        itemProcessor.CurrentAmountFirstIngredient += thing.stackCount;
                        if (itemProcessor.ExpectedAmountFirstIngredient != 0)
                        {
                            if (itemProcessor.CurrentAmountFirstIngredient >= itemProcessor.ExpectedAmountFirstIngredient)
                            {
                                itemProcessor.firstIngredientComplete = true;
                            }
                        }
                        itemProcessor.TryAcceptFirst(thing);
                        itemProcessor.Notify_StartProcessing();
                    }
                }
                if (combinationDef.secondItems != null && combinationDef.secondItems.Contains(ThingDefOf.Chemfuel.defName))
                {
                    int thingNeed = itemProcessor.ExpectedAmountSecondIngredient- itemProcessor.CurrentAmountSecondIngredient;
                    if (itemProcessor.processorStage != ProcessorStage.Working && thingNeed > 0)
                    {
                        int resourceNeeded = compRefill != null ? compRefill.Props.ratio : 1;
                        
                        if (!pipeNet.PullFuel(resourceNeeded))
                            return;
                        
                        Thing thing = ThingMaker.MakeThing(ThingDefOf.Chemfuel);
                        thing.stackCount = 1;
                        
                        if (itemProcessor.compItemProcessor.Props.transfersIngredientLists)
                        {
                            if (thing.TryGetComp<CompIngredients>() is CompIngredients ingredientComp)
                            {
                                itemProcessor.ingredients.AddRange(ingredientComp.ingredients);
                            }
                        }
                        
                        itemProcessor.CurrentAmountSecondIngredient += thing.stackCount;
                        if (itemProcessor.ExpectedAmountSecondIngredient!= 0)
                        {
                            if (itemProcessor.CurrentAmountSecondIngredient>= itemProcessor.ExpectedAmountSecondIngredient)
                            {
                                itemProcessor.secondIngredientComplete = true;
                            }
                        }

                        itemProcessor.TryAcceptSecond(thing);
                        itemProcessor.Notify_StartProcessing();
                    }
                }
                if (combinationDef.thirdItems != null && combinationDef.thirdItems.Contains(ThingDefOf.Chemfuel.defName))
                {
                    int thingNeed = itemProcessor.ExpectedAmountThirdIngredient - itemProcessor.CurrentAmountThirdIngredient;
                    if (itemProcessor.processorStage != ProcessorStage.Working && thingNeed > 0)
                    {
                        int resourceNeeded = compRefill != null ? compRefill.Props.ratio : 1;

                        if (!pipeNet.PullFuel(resourceNeeded))
                            return;

                        Thing thing = ThingMaker.MakeThing(ThingDefOf.Chemfuel);
                        thing.stackCount = 1;

                        if (itemProcessor.compItemProcessor.Props.transfersIngredientLists)
                        {
                            if (thing.TryGetComp<CompIngredients>() is CompIngredients ingredientComp)
                            {
                                itemProcessor.ingredients.AddRange(ingredientComp.ingredients);
                            }
                        }
                        itemProcessor.CurrentAmountThirdIngredient += thing.stackCount;
                        if (itemProcessor.ExpectedAmountThirdIngredient != 0)
                        {
                            if (itemProcessor.CurrentAmountThirdIngredient >= itemProcessor.ExpectedAmountThirdIngredient)
                            {
                                itemProcessor.thirdIngredientComplete = true;
                            }
                        }
                        itemProcessor.TryAcceptThird(thing);
                        itemProcessor.Notify_StartProcessing();
                    }
                }
                if (combinationDef.fourthItems != null && combinationDef.fourthItems.Contains(ThingDefOf.Chemfuel.defName))
                {
                    int thingNeed = itemProcessor.ExpectedAmountFourthIngredient - itemProcessor.CurrentAmountFourthIngredient;
                    if (itemProcessor.processorStage != ProcessorStage.Working && thingNeed > 0)
                    {
                        int resourceNeeded = compRefill != null ? compRefill.Props.ratio : 1;

                        if (!pipeNet.PullFuel(resourceNeeded))
                            return;

                        Thing thing = ThingMaker.MakeThing(ThingDefOf.Chemfuel);
                        thing.stackCount = 1;

                        if (itemProcessor.compItemProcessor.Props.transfersIngredientLists)
                        {
                            if (thing.TryGetComp<CompIngredients>() is CompIngredients ingredientComp)
                            {
                                itemProcessor.ingredients.AddRange(ingredientComp.ingredients);
                            }
                        }
                        itemProcessor.CurrentAmountFourthIngredient += thing.stackCount;
                        if (itemProcessor.ExpectedAmountFourthIngredient != 0)
                        {
                            if (itemProcessor.CurrentAmountFourthIngredient >= itemProcessor.ExpectedAmountFourthIngredient)
                            {
                                itemProcessor.fourthIngredientComplete = true;
                            }
                        }
                        itemProcessor.TryAcceptFourth(thing);
                        itemProcessor.Notify_StartProcessing();
                    }
                }
            }
        }
    }
}
