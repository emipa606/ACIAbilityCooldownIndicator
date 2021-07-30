using System;
using System.Reflection;
using System.Linq;
using RimWorld;
using Verse;
using UnityEngine;
using HarmonyLib;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CooldownIndicator
{
    [StaticConstructorOnStartup]
    public static class HarmonyPatches
    {
        //IL code to inject
        private static List<CodeInstruction> inject = new List<CodeInstruction>()
        {
            new CodeInstruction(OpCodes.Ldsfld, typeof(ColonistBarColonistDrawer).GetField("tmpIconsToDraw", BindingFlags.NonPublic | BindingFlags.Static)),
            new CodeInstruction(OpCodes.Ldsfld, typeof(CooldownIndicator.MainFunctionality).GetField("icon")),
            new CodeInstruction(OpCodes.Ldnull),
            new CodeInstruction(OpCodes.Ldarg_2),
            new CodeInstruction(OpCodes.Call, typeof(CooldownIndicator.MainFunctionality).GetMethod("currentColor")),
            new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(System.Nullable<UnityEngine.Color>), new Type[] { typeof(Color)})),
            new CodeInstruction(OpCodes.Newobj, typeof(ColonistBarColonistDrawer).GetNestedType("IconDrawCall", BindingFlags.NonPublic | BindingFlags.Instance).GetConstructors()[0]),
            new CodeInstruction(OpCodes.Callvirt, typeof(List<>).MakeGenericType(new Type[] { typeof(ColonistBarColonistDrawer).GetNestedType("IconDrawCall", BindingFlags.NonPublic | BindingFlags.Instance) }).GetMethod("Add")),
        };

        static HarmonyPatches()
        {
            //harmony patch
            Harmony harmony = new Harmony(id: "rimworld.cooldownindicator");
            harmony.Patch(AccessTools.Method(typeof(RimWorld.ColonistBarColonistDrawer), name: "DrawIcons"), transpiler: new HarmonyMethod(typeof(HarmonyPatches), nameof(Transpiler)));
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

            codes.InsertRange(index, inject);

            return codes.AsEnumerable();
        }

    }
}
