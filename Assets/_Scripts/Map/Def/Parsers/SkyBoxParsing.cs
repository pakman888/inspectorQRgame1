using UnityEngine;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

public class SkyBoxParsing{
    public static int skyBoxCount = 13;
    public static string skyBoxPrefix = "base/model/skybox/";
    public static List<SkyBoxItem> Parse(string filename) {
        var skyRegex = new Regex(@"sky\d.*");
        var stringRegex = new Regex("\".*\"");
        var lines = File.ReadAllLines(filename);
        var skyLines = from line in lines
                       where skyRegex.IsMatch(line)
                       select line; 
        var skyItems = new List<SkyBoxItem>();
        foreach (var line in skyLines) {
            var skyItem = new SkyBoxItem();
            
            skyItem.index = int.Parse(Regex.Match(line, "[0-9]+").ToString());
            var attributes = stringRegex.Match(line).ToString().Replace("\"", "");
            var tokens = attributes.Split('|');

            skyItem.name = tokens[0];
            skyItem.texture = tokens[1];
            skyItem.top = tokens[2];
            var color = tokens[3].Split(new []{' '},System.StringSplitOptions.RemoveEmptyEntries);

            skyItem.fogColor = new Vector3(
                float.Parse(color[0].Trim(' ')),
                float.Parse(color[1].Trim(' ')),
                float.Parse(color[2].Trim(' '))
                );
            skyItem.fogDensity = float.Parse(tokens[4]);
            skyItem.rain = tokens[5].Contains("true") ? true : false;
            skyItem.snow = tokens[6].Contains("true") ? true : false;
            skyItems.Add(skyItem);

        }
        if (skyItems.Count < skyBoxCount) {
            Debug.LogError("not enough skyboxes");
        }
        return skyItems.OrderBy(item => item.index).ToList();
    }
}
