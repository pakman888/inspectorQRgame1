using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class PrefabDefParser : MonoBehaviour {
	 public static List<PrefabDef> Parse(string filename) {
		// Whitespace at the beginning of the sentence followed by a #, then anything
		var commentRegex = new Regex(@"^ *#.*");
		// Matches prefab0:, prefab1, prefab209:, etc...
        var prefabRegex = new Regex(@"prefab\d*:");
		// Matches just the path without the double quotes, or an empty path
        var pathRegex = new Regex(@"/[^""\s]*|""""");
		
        var lines = File.ReadAllLines(filename);
        var prefabLines = from line in lines
                     where prefabRegex.IsMatch(line)
                     select line;

        var defs = new List<PrefabDef>();                
        foreach (var line in prefabLines) {
            if (pathRegex.IsMatch(line) && !commentRegex.IsMatch(line)) {
                // Note: even if the path is empty, we have to add the def to the list, in order to
                // preserve the indices
                var def = new PrefabDef();
                var path = pathRegex.Match(line).ToString().Replace(".pmd", "");
                // Trixy: The empty path is not totally empty, it's "" (within a "" because its a string.) 
                if (!path.Equals("\"\"")) {
                    // * REFACTOR: could get rid of the PrefabDef Path string entirely if the model also gets parsed somehow
                    def.path = "base" + path;
                    // * 
                    def.prefabDesc = PrefabDescParser.Parse("Assets/" + def.path + ".pdd");
                }

                defs.Add(def);
            }
        }
        return defs;
    }
}
