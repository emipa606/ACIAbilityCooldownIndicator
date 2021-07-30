using RimWorld;
using Verse;
using UnityEngine;
using System.Collections.Generic;

namespace CooldownIndicator
{

    class MainFunctionality
    {
        public static readonly Texture2D icon = ContentFinder<Texture2D>.Get("cooldownIcon", true);
        static readonly UnityEngine.Color red = new UnityEngine.Color(1f, 0, 0);
        static readonly UnityEngine.Color yellow = new UnityEngine.Color(1f, 1f, 0);
        static readonly UnityEngine.Color green = new UnityEngine.Color(0, 1f, 0);

        static HashSet<Pawn> pawnsOnCooldown = new HashSet<Pawn>();
        public static void shouldDisplayCooldownMessage(Pawn pawn)
        {
            List<Ability> abilities = pawn.abilities.AllAbilitiesForReading;
            Ability selected = abilities[0];

            if (abilities[0].CooldownTicksTotal == 0)
            {
                selected = abilities[1];
            }

            if (selected.CooldownTicksRemaining != 0)
            {
                if (!pawnsOnCooldown.Contains(pawn))
                {
                    pawnsOnCooldown.Add(pawn);
                }
            }
            else
            {

                if (pawnsOnCooldown.Contains(pawn))
                {
                    pawnsOnCooldown.Remove(pawn);

                    Messages.Message(pawn + " has no more ability cooldown", MessageTypeDefOf.NeutralEvent);
                }
            }
        }

        public static Color currentColor(Pawn pawn)
        {

            List<Ability> abilities = pawn.abilities.AllAbilitiesForReading;

            Ability selected = abilities[0];

            float position = 0;
            Color lerpedColor = Color.white;


            if (abilities[0].CooldownTicksTotal == 0)
            {
                selected = abilities[1];
            }

            if (selected.CooldownTicksRemaining != 0)
            {
                position = 1f * selected.CooldownTicksRemaining / selected.CooldownTicksTotal;
            }
            else
            {
                position = 0;
            }

            position = Mathf.Clamp(position, 0, 1);

            if (position == 0)
            {
                lerpedColor = Color.cyan;
            }
            else
            {
                if (position < .5)
                {
                    lerpedColor = Color.Lerp(green, yellow, position * 2f);
                }
                else
                {
                    lerpedColor = Color.Lerp(yellow, Color.red, (position - .5f) * 2);
                }
            }

            //check if cooldown message should be shown
            shouldDisplayCooldownMessage(pawn);
            return lerpedColor;
        }
    }
}
