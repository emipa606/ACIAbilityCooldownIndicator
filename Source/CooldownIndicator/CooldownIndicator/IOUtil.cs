using System.IO;
using Verse;
using System.Collections.Generic;
using HarmonyLib;

namespace CooldownIndicator
{
    class IOUtil
    {

        //Util to debug created IL code from HarmonyPatches, txt should appear in RimWorld root
        static void writeToFile(string name, List<CodeInstruction> instructions)
        {
            CodeInstruction[] arrayIn = instructions.ToArray();
            string[] arrayOut = new string[arrayIn.Length];

            int i = 0;
            foreach (CodeInstruction entry in arrayIn)
            {
                arrayOut[i] = entry.ToString();
                i++;
            }

            Log.Message("try to write file: " + name);
            try
            {
                File.WriteAllLines(name, arrayOut);
            }
            catch
            {

            }

        }
    }
}
