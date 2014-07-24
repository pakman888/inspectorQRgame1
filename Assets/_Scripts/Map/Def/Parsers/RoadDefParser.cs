using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class RoadDefParser {
    public static List<RoadLook> Parse(string filename) {
        var roadRegex = new Regex(@"road_look_data\d.*");
		var stringRegex = new Regex("\".*\"");
        var lines = File.ReadAllLines(filename);
        var roadLines = from line in lines
                     where roadRegex.IsMatch(line)
                     select stringRegex.Match(line).ToString().Replace("\"", "");
        var roadLooks = new List<RoadLook>();
        foreach (var line in roadLines) {
			var fields = line.Split(';');
			var look = new RoadLook();
	
			look.size = Convert.ToSingle(fields[0]);
			look.offset = Convert.ToSingle(fields[1]);
			look.textureLeft = Convert.ToSingle(fields[2]);
			look.textureRight = Convert.ToSingle(fields[3]);
			look.doubleLine = Convert.ToBoolean(fields[4]);
			look.whiteStyle = Convert.ToInt32(fields[5]);
			look.centerLine = Convert.ToInt32(fields[6]);
			look.useWhite = Convert.ToBoolean(fields[7]);
			look.laneCount0 = Convert.ToInt32(fields[8]);
			look.laneCount1 = Convert.ToInt32(fields[9]);
			look.oneWay = Convert.ToBoolean(fields[10]);
			look.name = fields[11];
			look.id = roadLooks.Count;
			roadLooks.Add(look);
        }
        return roadLooks;
    }
}