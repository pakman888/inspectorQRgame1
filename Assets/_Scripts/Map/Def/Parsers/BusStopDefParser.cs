using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class BusStopDefParser {

    public static List<BusStopLook> Parse ( string filename ) {
        var modelRegex = new Regex(@"model\d.*");
        var stringRegex = new Regex("\".*\"");
        var lines = File.ReadAllLines(filename);
        var modelLines = from line in lines
                         where modelRegex.IsMatch(line)
                         select stringRegex.Match(line).ToString().Replace("\"", "");
        var busStopLooks = new List<BusStopLook>();
        foreach (var line in modelLines) {
            var look = new BusStopLook();
            var tokens = line.Split('|');
            
            //hack 0 = missing. 
            look.name = tokens[0].Trim();
            look.busStopModel = IfBlankReturnZero(CleanWhiteSpace(tokens[1]));
            look.busStopCollision = IfBlankReturnZero(CleanWhiteSpace(tokens[2]));
            look.modelOffset = Convert.ToSingle(IfBlankReturnZero(CleanWhiteSpace(tokens[3])));
            look.activePlate = IfBlankReturnZero(CleanWhiteSpace(tokens[4]));
            look.inactivePlate = IfBlankReturnZero(CleanWhiteSpace(tokens[5]));
            look.radius = Convert.ToSingle(IfBlankReturnZero(CleanWhiteSpace(tokens[6])));

            busStopLooks.Add(look);

        }
        return busStopLooks;
    }
 
    private static string CleanWhiteSpace( string input){
        StringBuilder output = new StringBuilder();
        for (int index = 0; index < input.Length; index++) {
            if (!char.IsWhiteSpace(input[index])){ 
                output.Append(input[index]);
            }
        }
        return output.ToString();
    }
    private static string IfBlankReturnZero(string str) {
        if (str == "") {
            return "0";
        } else return str;
    }

}
