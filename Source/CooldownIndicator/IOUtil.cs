using System.Collections.Generic;
using System.IO;
using HarmonyLib;
using Verse;

namespace CooldownIndicator;

internal class IOUtil
{
    public static void writeToFile(string name, List<CodeInstruction> instructions)
    {
        var array = instructions.ToArray();
        var array2 = new string[array.Length];
        var num = 0;
        foreach (var codeInstruction in array)
        {
            array2[num] = codeInstruction.ToString();
            num++;
        }

        Log.Message($"try to write file: {name}");
        try
        {
            File.WriteAllLines(name, array2);
        }
        catch
        {
            // ignored
        }
    }
}