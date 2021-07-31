using System;
using System.Reflection;
using System.Linq;
using RimWorld;
using Verse;
using UnityEngine;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;
using TacticalGroups;

namespace CooldownIndicator
{
    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {
        //IL code to inject
        private static List<CodeInstruction> inject = new List<CodeInstruction>()
        {
            new CodeInstruction(OpCodes.Ldsfld, typeof(RimWorld.ColonistBarColonistDrawer).GetField("tmpIconsToDraw", BindingFlags.NonPublic | BindingFlags.Static)),
            new CodeInstruction(OpCodes.Ldsfld, typeof(CooldownIndicator.MainFunctionality).GetField("icon")),
            new CodeInstruction(OpCodes.Ldnull),
            new CodeInstruction(OpCodes.Ldarg_2),
            new CodeInstruction(OpCodes.Call, typeof(CooldownIndicator.MainFunctionality).GetMethod("currentColor")),
            new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(System.Nullable<UnityEngine.Color>), new Type[] { typeof(Color)})),
            new CodeInstruction(OpCodes.Newobj, typeof(RimWorld.ColonistBarColonistDrawer).GetNestedType("IconDrawCall", BindingFlags.NonPublic | BindingFlags.Instance).GetConstructors()[0]),
            new CodeInstruction(OpCodes.Callvirt, typeof(List<>).MakeGenericType(new Type[] { typeof(RimWorld.ColonistBarColonistDrawer).GetNestedType("IconDrawCall", BindingFlags.NonPublic | BindingFlags.Instance) }).GetMethod("Add"))
        };

        //IL code to inject if LTO
        private static List<CodeInstruction> injectLTO;

        //IL code to inject if MoodBar
        private static List<CodeInstruction> injectMoodBar;

        static HarmonyPatches()
        {
            //harmony patch
            Harmony harmony = new Harmony(id: "rimworld.cooldownindicator");

            //check for colonygroups mod
            if (Verse.ModLister.GetActiveModWithIdentifier("DerekBickley.LTOColonyGroupsFinal") != null)
            {
                Log.Message("Patching LTOColonyGroupsFinal");
                injectLTO = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldsfld, AccessTools.TypeByName("TacticalGroups.ColonistBarColonistDrawer").GetField("tmpIconsToDraw", BindingFlags.NonPublic | BindingFlags.Static)),
                    new CodeInstruction(OpCodes.Ldsfld, typeof(CooldownIndicator.MainFunctionality).GetField("icon")),
                    new CodeInstruction(OpCodes.Ldnull),
                    new CodeInstruction(OpCodes.Ldarg_2),
                    new CodeInstruction(OpCodes.Call, typeof(CooldownIndicator.MainFunctionality).GetMethod("currentColor")),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(System.Nullable<UnityEngine.Color>), new Type[] { typeof(Color)})),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.TypeByName("TacticalGroups.ColonistBarColonistDrawer").GetNestedType("IconDrawCall", BindingFlags.NonPublic | BindingFlags.Instance).GetConstructors()[0]),
                    new CodeInstruction(OpCodes.Callvirt, typeof(List<>).MakeGenericType(new Type[] { AccessTools.TypeByName("TacticalGroups.ColonistBarColonistDrawer").GetNestedType("IconDrawCall", BindingFlags.NonPublic | BindingFlags.Instance) }).GetMethod("Add"))
                };
                harmony.Patch(AccessTools.Method("TacticalGroups.ColonistBarColonistDrawer:DrawIcons"), transpiler: new HarmonyMethod(typeof(HarmonyPatches), nameof(Transpiler)));
            }
            //check for ColorCodedMoodBar
            else if ((Verse.ModLister.GetActiveModWithIdentifier("CrashM.ColorCodedMoodBar.11") != null))
            {
                var flags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static;

                Log.Message("Patching CrashM.ColorCodedMoodBar");
                injectMoodBar = new List<CodeInstruction>()
                {
                    new CodeInstruction(OpCodes.Ldarg_0),
                    new CodeInstruction(OpCodes.Ldfld, AccessTools.TypeByName("MoodCache").GetField("IconsToDraw", flags)),
                    new CodeInstruction(OpCodes.Ldsfld, typeof(CooldownIndicator.MainFunctionality).GetField("icon")),
                    new CodeInstruction(OpCodes.Ldnull),
                    new CodeInstruction(OpCodes.Ldarg_1),
                    new CodeInstruction(OpCodes.Call, typeof(CooldownIndicator.MainFunctionality).GetMethod("currentColor")),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(System.Nullable<UnityEngine.Color>), new Type[] { typeof(Color)})),
                    new CodeInstruction(OpCodes.Newobj, AccessTools.TypeByName("ColoredMoodBar13.IconDrawCall").GetConstructors()[0]),
                    new CodeInstruction(OpCodes.Callvirt, typeof(List<>).MakeGenericType(new Type[] { AccessTools.TypeByName("ColoredMoodBar13.IconDrawCall") }).GetMethod("Add"))
                };
                harmony.Patch(AccessTools.Method("ColoredMoodBar13.MoodCache:DoIconVars"), transpiler: new HarmonyMethod(typeof(HarmonyPatches), nameof(Transpiler)));
            }
            else
            {
                //vanilla
                harmony.Patch(AccessTools.Method(typeof(RimWorld.ColonistBarColonistDrawer), name: "DrawIcons"), transpiler: new HarmonyMethod(typeof(HarmonyPatches), nameof(Transpiler)));
            }

            Log.Message("finished loading ACI");
        }

        //add IL code at index:53
        static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            int index = 0;
            var codes = new List<CodeInstruction>(instructions);

            foreach (CodeInstruction entry in codes)
            {
                if (entry.ToString().Contains("callvirt UnityEngine.Color RimWorld.Ideo::get_Color()"))
                {
                    //Log.Message("found at: " + index + " callvirt UnityEngine.Color RimWorld.Ideo::get_Color()");
                    break;
                }

                index++;
            }

            index += 4;
            //check for colonygroups mod
            if (Verse.ModLister.GetActiveModWithIdentifier("DerekBickley.LTOColonyGroupsFinal") != null)
            {
                
                //colonygroups mod LTO
                codes.InsertRange(index, injectLTO);

            }
            else if ((Verse.ModLister.GetActiveModWithIdentifier("CrashM.ColorCodedMoodBar.11") != null))
            {
                //colormood bar
                codes.InsertRange(index, injectMoodBar);
            }
            else
            {
                //vanilla
                codes.InsertRange(index, inject);
            }

            return codes.AsEnumerable();
        }

    }
}
