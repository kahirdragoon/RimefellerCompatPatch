# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

A RimWorld mod (Steam Workshop id in `About/PublishedFileId.txt`) that connects things using chemfuel to **Rimefeller** pipe networks (a hard dependency). Things that burn chemfuel draw it from the pipe net. Things that produce chemfuel push it into the net. The mod also adds compat for several Vanilla Expanded and other mods. It supports RimWorld 1.4–1.7. The repo root *is* the mod folder inside `RimWorld/Mods/`, so changes take effect on the next game launch.

## Build

There are no tests and no linter. To verify a change, launch RimWorld with the mod enabled and check the log (dev mode). `RCP_Core.csproj.user` starts `RimWorld.exe` when you debug.

The SDK-style projects target only the current game version (1.7) and write straight into the mod folder:

```
dotnet build Source/RCP_Core/RCP_Core.csproj                       # -> 1.7/Assemblies/RCP_Core.dll
dotnet build Source/RCP_VanillaExpanded/RCP_VanillaExpanded.csproj # -> Mods/VanillaExpanded/1.7/Assemblies/RCP_VanillaExpanded.dll
```

- Don't build the whole `ChemfuelCompatPatch.sln`. `RCP_VFE_Ancients` and `RCP_VFE_Mechanoids` are legacy net472 projects for 1.4/1.5 only, with stale output paths. Their DLLs are committed and no longer rebuilt.
- The DLLs for older game versions (`1.4`–`1.6/Assemblies`, `Mods/*/1.x/Assemblies`) are committed build outputs. To rebuild one, temporarily point the csproj's `OutputPath`, the `Krafs.Rimworld.Ref` version and the HintPaths at that version (git history shows the 1.6 values).
- Mod DLLs are referenced through relative HintPaths into the Steam workshop folder (`steamapps/workshop/content/294100/<id>/<ver>/Assemblies`). `1321849735` is Rimefeller and `2023507013` is the Vanilla Expanded Framework (`PipeSystem.dll`, `VEF.dll`). The HintPath version folder must match the target game version.
- SDK projects pick up every `*.cs` file automatically. The legacy projects need explicit `<Compile Include>` entries.

## Architecture

**Loading is per game version and per mod, via `LoadFolders.xml`.** Each `<v1.x>` block loads `/` (the root `Patches/`), the matching `1.x/` folder (`RCP_Core.dll`), and the `Mods/<Name>/1.x/` folders gated by `IfModActive`. Supporting a new mod or game version means adding the folder **and** its `LoadFolders.xml` entry. Also add the mod's packageId to `loadAfter` and list it in the description in `About/About.xml`.

**RCP_Core** (always loaded, references only Rimefeller):
- `Initialize` (`[StaticConstructorOnStartup]`) runs after all XML patches are applied. It scans every `ThingDef` and adds comps at runtime:
  - a `CompProperties_Refuelable` whose fuel filter allows Chemfuel gets `CompProperties_RefuelFromRimefellerPipeNet`
  - a `CompProperties_Spawner` that spawns Chemfuel gets `Rimefeller.CompProperties_Pipe`
  - defs that already have a `CompPipe`, or a subclass, are skipped. Mods with a custom refuelable `compClass` (e.g. the Gravship astrofuel synthesizer) aren't picked up by this scan and need an explicit XML patch instead.
- `CompRefuelFromRimefellerPipeNet` extends Rimefeller's `CompPipe`. Its `CompTick` is deliberately empty: the comp registers with the `RefuelableRimefellerPipeSystemCompatManager` MapComponent, which ticks it through `CompTick2`. That way the refuel logic doesn't run twice. RimWorld creates MapComponents by reflection, so no def is needed.
- Harmony prefixes: `CompSpawner.TryDoSpawn` pushes chemfuel into the pipe net instead of spawning items, and falls back to vanilla when the net is full. `BioReactor.Building_BioReactor:MakeFuel` is patched by name only when that type exists, so there's no hard dependency on Bioreactor.

**RCP_VanillaExpanded** (loaded when VFE Core is active, references `PipeSystem`/`VEF`) provides comps that bridge Rimefeller's pipe net and VEF's `PipeSystem`. XML patches under `Mods/*` attach them by class name:
- `CompProperties_FillStorageFromRimefellerPipeNet`: Rimefeller net → VEF `CompResourceStorage` (VE Chemfuel tanks)
- `CompProperties_FillAdvancedResourceProcessor`: Rimefeller net → chemfuel ingredients of a VEF `CompAdvancedResourceProcessor` (VFE Factory)
- `CompProperties_RimefellerRefinerToPipeNet`: Rimefeller `CompRefinery` buffer → a VEF pipe net (VRE Androids neutroamine)
- a Harmony prefix on `PipeSystem.Process.SpawnOrPushToNet` that pushes single-result chemfuel processes into the Rimefeller net

**XML-only compat**: most `Mods/<Name>/1.x/` folders contain only `Patches/` (or `Defs/`) that add `Rimefeller.CompProperties_Pipe` or one of the comps above to specific defNames. In a patch, refer to this mod's comps by namespace-qualified class name (`RCP_Core.…`, `RCP_VanillaExpanded.…`). A patch that references an `RCP_VanillaExpanded` class only works where that DLL is loaded for the same version.

## Publishing

`_PublisherPlus.xml` and `Source/.rimignore` both exclude `Source/`, `Images/` and `.git` from the Workshop upload. `Images/` and `Source/SteamDescription.txt` hold Workshop page assets and text.
