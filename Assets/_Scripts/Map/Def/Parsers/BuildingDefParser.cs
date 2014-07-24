using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

[System.Serializable]
public class BuildingDefParser {
    public List<BuildingInfo> modelBuildings;
    public List<SchemeInfo> schemeInfo;
    public override string ToString() {
        string output = "ModelCount: " + modelBuildings.Count() + "\n";
        for (int i = 0; i < modelBuildings.Count(); i++) {
            output += i + ":   " + modelBuildings[i].path + "\n";
        }
        output += "SchemeCount: " + schemeInfo.Count() + "\n";
        for (int i = 0; i < schemeInfo.Count(); i++) {
            output += i + ":   " + schemeInfo[i].name + " | ";
            for (int j = 0; j < schemeInfo[i].modelIndexs.Count; j++) {
                output += " " + i;
            }
            output += "\n";
        }
        return output;
    }
    // because buildings have lots of data, it's probably a good Idea to create some sort of tuple
    // to help with the proper use of buildings and looks.
    public BuildingDefParser(string filename) {
        var modelRegex = new Regex(@"model\d.*"); // A regex to accept Strings that start with model
        var schemeRegex = new Regex(@"scheme\d.*"); // A regeex to accept lines in the def file that 
                                                    // are info about schemes
        var lines = File.ReadAllLines(filename);
        var modelLines = from line in lines
                         where modelRegex.IsMatch(line)
                         select line;
        var schemeLines = from line in lines
                          where schemeRegex.IsMatch(line)
                          select line;
        modelBuildings = ParseModelLines(modelLines.ToList());
        schemeInfo = ParseSchemeLines(schemeLines.ToList());
    }

    private List<SchemeInfo> ParseSchemeLines(List<string> schemeLine) {
        List<SchemeInfo> schemeLineInfo = new List<SchemeInfo>();
        foreach (string line in schemeLine) {
            if (line[0] == '#') {
                //do nothing
            } else {
                string[] idString = line.Split(':');
                // check to see if this is empty,
                //Debug.Log(idString[1]);
                //Debug.Log(idString[1].Length);
                if ( idString[1].Substring(0,2) == "\"\"") { // for some reason a single item is slipping through here.
                    //create empty scheme
                    schemeLineInfo.Add(new SchemeInfo( "empty", new List<int>()));
                }
                else {
                    List<int> schemeModelIndexs = new List<int>();
                    idString[1] = idString[1].Replace("\"", "");
                    string[] nameRange = idString[1].Split('|');
                    string name = nameRange[0];
                    string range = nameRange[1];
                    string[] rangebounds = range.Split('-');
                    if (rangebounds.Count() > 1 ) {
                        var start = (int) Convert.ToUInt32(rangebounds[0]);
                        var end = (int) Convert.ToUInt32( rangebounds[1] );
                        for ( int i = start; i <= end; i++ ){ 
                           schemeModelIndexs.Add(i); 
                        }
                        schemeLineInfo.Add(new SchemeInfo(name, schemeModelIndexs));
                    } else {
                        schemeModelIndexs.Add((int) Convert.ToUInt32(rangebounds[0]));
                        schemeLineInfo.Add(new SchemeInfo(name, schemeModelIndexs));
                    }
                }
            }
        }
        return schemeLineInfo;
    }

    private List<BuildingInfo> ParseModelLines(List<string> modelLines) {
        
        List<BuildingInfo> buildingInfoList = new List<BuildingInfo>();
        foreach (string line in modelLines ) {
            // if the line is a comment or empty do nothing.
            if( line[0] == '#' ) {
                //Do Nothing
            }
            else {
                string[] keyValue = line.Split(':');
                // Removing warning: id never used
				//string id = keyValue[0];
                string parameters = keyValue[1];
                string[] splitParameters = parameters.Split('|');
                string path = splitParameters[0].Replace("\"", ""); //strip the quotes out Of MeshLink the string
                // add additional look stuff here.
                if ( path != null )
                    if (path != "") {
                        buildingInfoList.Add(new BuildingInfo("base" + path.Replace(".pmd", "")));
                    } else {
                        buildingInfoList.Add(new BuildingInfo( path ));
                    }
            }
            
        }
        //foreach (BuildingInfo info in buildingInfoList) {
            //Debug.Log(info.path);
        //}
        return buildingInfoList;
    }
}
    /*
     * Since the building def has look information I feel it's better to create a struct that 
     * has room to scale with the extra data as we progress further. 
     */
[System.Serializable]
public class BuildingInfo
{
    public BuildingInfo(string path)
    {
        this.path = path;
    }
    public string path;
    public int look;
    public int width;
    public int depth;
}

[System.Serializable]
public class SchemeInfo {
    public SchemeInfo(string name, List<int> modelIndexs) {
        this.name = name;
        this.modelIndexs = modelIndexs;
    }
    public string name;
    public List<int> modelIndexs;
}