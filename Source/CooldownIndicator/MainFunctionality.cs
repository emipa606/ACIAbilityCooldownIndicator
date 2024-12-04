using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse;

namespace CooldownIndicator;

[StaticConstructorOnStartup]
internal class MainFunctionality
{
    public static readonly Texture2D icon = ContentFinder<Texture2D>.Get("cooldownIcon");

    private static readonly Color red = new Color(1f, 0f, 0f);

    private static readonly Color yellow = new Color(1f, 1f, 0f);

    private static readonly Color green = new Color(0f, 1f, 0f);

    private static readonly HashSet<Pawn> pawnsOnCooldown = [];

    public static void shouldDisplayCooldownMessage(Pawn pawn)
    {
        var allAbilitiesForReading = pawn.abilities.AllAbilitiesForReading;
        var ability = allAbilitiesForReading[0];
        if (ability.CooldownTicksRemaining != 0)
        {
            pawnsOnCooldown.Add(pawn);
        }
        else if (pawnsOnCooldown.Contains(pawn))
        {
            pawnsOnCooldown.Remove(pawn);
            Messages.Message($"{pawn} has no more ability cooldown", MessageTypeDefOf.NeutralEvent);
        }
    }

    public static Color currentColor(Pawn pawn)
    {
        var allAbilitiesForReading = pawn.abilities.AllAbilitiesForReading;
        var ability = allAbilitiesForReading[0];
        var num = ability.CooldownTicksRemaining == 0
            ? 0f
            : 1f * ability.CooldownTicksRemaining / ability.CooldownTicksTotal;
        num = Mathf.Clamp(num, 0f, 1f);
        var white = num == 0f ? Color.cyan :
            !(num < 0.5) ? Color.Lerp(yellow, red, (num - 0.5f) * 2f) : Color.Lerp(green, yellow, num * 2f);
        shouldDisplayCooldownMessage(pawn);
        return white;
    }
}