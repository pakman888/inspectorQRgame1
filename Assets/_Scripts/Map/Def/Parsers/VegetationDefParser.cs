using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class VegetationDefParser {
    public static List<string> Parse(string filename) {
        var modelRegex = new Regex("model\\d.*");
        var pathRegex = new Regex("/[^\"]*");
        var lines = File.ReadAllLines(filename);
        var modelLines = from line in lines
                     where modelRegex.IsMatch(line)
                     select line;
        var paths = new List<string>();
        foreach (var line in modelLines) {
            if (pathRegex.IsMatch(line)) {
                paths.Add("base" + pathRegex.Match(line).ToString().Replace(".pmd", "")); // The paths don't include the 'base' 
            }
        }
        return paths;
    }
}
