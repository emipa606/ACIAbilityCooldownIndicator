using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Reflection.Emit;
using HarmonyLib;
using RimWorld;
using UnityEngine;
using Verse;

namespace CooldownIndicator;

[StaticConstructorOnStartup]
public static class HarmonyPatches
{
    private static readonly List<CodeInstruction> inject;

    private static readonly List<CodeInstruction> injectLto;

    private static readonly List<CodeInstruction> injectMoodBar;

    static HarmonyPatches()
    {
        inject =
        [
            new CodeInstruction(OpCodes.Ldsfld,
                typeof(ColonistBarColonistDrawer).GetField("tmpIconsToDraw",
                    BindingFlags.Static | BindingFlags.NonPublic)),

            new CodeInstruction(OpCodes.Ldsfld, typeof(MainFunctionality).GetField(nameof(MainFunctionality.icon))),
            new CodeInstruction(OpCodes.Ldnull),
            new CodeInstruction(OpCodes.Ldarg_2),
            new CodeInstruction(OpCodes.Call,
                typeof(MainFunctionality).GetMethod(nameof(MainFunctionality.CurrentColor))),
            new CodeInstruction(OpCodes.Newobj, AccessTools.Constructor(typeof(Color?), [typeof(Color)])),
            new CodeInstruction(OpCodes.Newobj,
                typeof(ColonistBarColonistDrawer)
                    .GetNestedType("IconDrawCall", BindingFlags.Instance | BindingFlags.NonPublic)
                    .GetConstructors()[0]),

            new CodeInstruction(OpCodes.Callvirt,
                typeof(List<>)
                    .MakeGenericType(typeof(ColonistBarColonistDrawer).GetNestedType("IconDrawCall",
                        BindingFlags.Instance | BindingFlags.NonPublic)).GetMethod("Add"))
        ];
        var harmony = new Harmony("rimworld.cooldownindicator");
        if (ModLister.GetActiveModWithIdentifier("DerekBickley.LTOColonyGroupsFinal", true) != null)
        {
            Log.Message("Patching LTOColonyGroupsFinal");
            injectLto =
            [
                new CodeInstruction(OpCodes.Ldsfld,
                    AccessTools.TypeByName("TacticalGroups.ColonistBarColonistDrawer").GetField("tmpIconsToDraw",
                        BindingFlags.Static | BindingFlags.NonPublic)),

                new CodeInstruction(OpCodes.Ldsfld, typeof(MainFunctionality).GetField(nameof(MainFunctionality.icon))),
                new CodeInstruction(OpCodes.Ldnull),
                new CodeInstruction(OpCodes.Ldarg_2),
                new CodeInstruction(OpCodes.Call,
                    typeof(MainFunctionality).GetMethod(nameof(MainFunctionality.CurrentColor))),
                new CodeInstruction(OpCodes.Newobj,
                    AccessTools.Constructor(typeof(Color?), [typeof(Color)])),

                new CodeInstruction(OpCodes.Newobj,
                    AccessTools.TypeByName("TacticalGroups.ColonistBarColonistDrawer")
                        .GetNestedType("IconDrawCall", BindingFlags.Instance | BindingFlags.NonPublic)
                        .GetConstructors()[0]),

                new CodeInstruction(OpCodes.Callvirt,
                    typeof(List<>).MakeGenericType(AccessTools.TypeByName("TacticalGroups.ColonistBarColonistDrawer")
                            .GetNestedType("IconDrawCall", BindingFlags.Instance | BindingFlags.NonPublic))
                        .GetMethod("Add"))
            ];
            harmony.Patch(AccessTools.Method("TacticalGroups.ColonistBarColonistDrawer:DrawIcons"), null, null,
                new HarmonyMethod(typeof(HarmonyPatches), nameof(Transpiler)));
        }
        else if (ModLister.GetActiveModWithIdentifier("CrashM.ColorCodedMoodBar.11", true) != null)
        {
            const BindingFlags bindingAttr = BindingFlags.Instance | BindingFlags.Static | BindingFlags.Public |
                                             BindingFlags.NonPublic;
            Log.Message("Patching CrashM.ColorCodedMoodBar");
            injectMoodBar =
            [
                new CodeInstruction(OpCodes.Ldarg_0),
                new CodeInstruction(OpCodes.Ldfld,
                    AccessTools.TypeByName("MoodCache").GetField("IconsToDraw", bindingAttr)),

                new CodeInstruction(OpCodes.Ldsfld, typeof(MainFunctionality).GetField(nameof(MainFunctionality.icon))),
                new CodeInstruction(OpCodes.Ldnull),
                new CodeInstruction(OpCodes.Ldarg_1),
                new CodeInstruction(OpCodes.Call,
                    typeof(MainFunctionality).GetMethod(nameof(MainFunctionality.CurrentColor))),
                new CodeInstruction(OpCodes.Newobj,
                    AccessTools.Constructor(typeof(Color?), [typeof(Color)])),

                new CodeInstruction(OpCodes.Newobj,
                    AccessTools.TypeByName("ColoredMoodBar13.IconDrawCall").GetConstructors()[0]),

                new CodeInstruction(OpCodes.Callvirt,
                    typeof(List<>).MakeGenericType(AccessTools.TypeByName("ColoredMoodBar13.IconDrawCall"))
                        .GetMethod("Add"))
            ];
            harmony.Patch(AccessTools.Method("ColoredMoodBar13.MoodCache:DoIconVars"), null, null,
                new HarmonyMethod(typeof(HarmonyPatches), "Transpiler"));
        }
        else
        {
            harmony.Patch(AccessTools.Method(typeof(ColonistBarColonistDrawer), "DrawIcons"), null, null,
                new HarmonyMethod(typeof(HarmonyPatches), nameof(Transpiler)));
        }

        Log.Message("finished loading ACI");
    }

    private static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
    {
        var num = 0;
        var list = new List<CodeInstruction>(instructions);
        foreach (var item in list)
        {
            if (item.ToString().Contains("callvirt UnityEngine.Color RimWorld.Ideo::get_Color()"))
            {
                break;
            }

            num++;
        }

        num += 4;
        if (ModLister.GetActiveModWithIdentifier("DerekBickley.LTOColonyGroupsFinal", true) != null)
        {
            list.InsertRange(num, injectLto);
        }
        else if (ModLister.GetActiveModWithIdentifier("CrashM.ColorCodedMoodBar.11", true) != null)
        {
            list.InsertRange(num, injectMoodBar);
        }
        else
        {
            list.InsertRange(num, inject);
        }

        return list.AsEnumerable();
    }
}