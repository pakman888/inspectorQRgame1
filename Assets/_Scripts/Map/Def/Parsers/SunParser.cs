using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

public class SunParser {
    public static Vector3 tunnelLight = new Vector3(229f, 158f, 28f);
    public static int sunCount = 34;

    public static List<SunItem> Parse(string filename) {
       
        var sunRegex = new Regex(@"sun\d.*");
        var stringRegex = new Regex("\".*\"");
        var lines = File.ReadAllLines(filename);
        var sunLines = from line in lines
                       where sunRegex.IsMatch(line)
                       select line;
        var sunItems = new List<SunItem>();
        foreach (var sunLine in sunLines) {
            var sunItem = new SunItem();

            sunItem.index = int.Parse(Regex.Match(sunLine, "[0-9]+").ToString());
            var attributes = stringRegex.Match(sunLine).ToString().Replace("\"", "");
            var tokens = attributes.Split('|');
            sunItem.name = tokens[0];
            sunItem.ambient = SplitFloatsVector(tokens[1]);
            sunItem.diffuse = SplitFloatsVector(tokens[2]);
            sunItem.specular = SplitFloatsVector(tokens[3]);
            sunItem.enviroment = SplitFloatsVector(tokens[4]);
            sunItem.skyboxColor = SplitFloatsVector(tokens[5]);
            sunItem.sunDirection = SplitFloatsVector(tokens[6]);
            sunItem.lampsOn = tokens[7].Contains("true") ? true : false;
            sunItem.night = tokens[8].Contains("true") ? true : false;
            sunItem.sunModel = tokens.Length == 10 ? tokens[9] : string.Empty;
            sunItems.Add(sunItem);
        }
        if (sunItems.Count < sunCount) {
            Debug.LogError("missing sun items");
        }
        Debug.Log("sun done");
        return sunItems.OrderBy(n => n.index).ToList();
    }

    private static Vector3 SplitFloatsVector(string input) {
        var tokens = input.Split(new []{' '},System.StringSplitOptions.RemoveEmptyEntries);

        var outPut = new Vector3(
            float.Parse(tokens[0].Trim(' ')),
            float.Parse(tokens[1].Trim(' ')),
            float.Parse(tokens[2].Trim(' '))
            );
        return outPut;
    }

}
