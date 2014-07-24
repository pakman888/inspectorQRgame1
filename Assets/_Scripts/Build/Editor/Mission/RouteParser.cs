using UnityEngine;
using UnityEditor;

using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public static class RouteParser {
    public static void ParseAndWrite(string filename, ResourceServer server) {
        string name = "missing"; 
        var lines = File.ReadAllText(filename);
        var reader = new StringReader(lines);
        var routeItem = ScriptableObject.CreateInstance<RouteItem>();
        while (reader.Peek() != -1) {
            var line = reader.ReadLine();
            if (line.Length < 1) {
                continue;
            }
            line = CleanLeadingWhiteSpace(line);
            var tokens = line.Split(' ');
            if (tokens.Length < 2) {
                continue;
            }
            var label = tokens[0];
            var value = tokens[1];

            //mission for some reason has a space between the colon and the label.
            if ( label.Equals("mission") ){
                name = tokens[2];
                routeItem.routeName = name;
            } else if (label.Equals("controller_type:")) {
                routeItem.controllerType = value;
            } else if (label.Equals("short_desc:")) {
                routeItem.shortDescription = value;
            } else if (label.Equals("long_desc:")) {
                routeItem.longDescription = value;
            } else if (label.Equals("map_name:")) {
                routeItem.mapName = value;
            } else if (label.Equals("time_limit:")) {
                routeItem.timeLimit = Convert.ToInt32(value);
            } else if (label.Equals("bonus_time_limit:")) {
                routeItem.bonusTimeLimit = Convert.ToInt32(value);
            } else if (label.Equals("sun_idx:")) {
                routeItem.sunIndexes = DoSimpleIntegerListParsing(reader, line, tokens);
                routeItem.sunCount = routeItem.sunIndexes.Count;
            } else if (label.Equals("sky_idx:")) {
                routeItem.skyIndexes = DoSimpleIntegerListParsing(reader, line, tokens);
                routeItem.skyCount = routeItem.skyIndexes.Count;
            } else if (label.Equals("tier:")) {
                routeItem.tier = Convert.ToInt32(value);
            } else if (label.Equals("rank:")) {
                routeItem.rank = Convert.ToInt32(value);
            } else if (label.Equals("overlay_offset_x:")) {
                routeItem.overlayOffsetX = Convert.ToInt32(value);
            } else if (label.Equals("overlay_offset_y:")) {
                routeItem.overlayOffsetY = Convert.ToInt32(value);
			} else if (label.Equals("vehicle_data:")) {
				routeItem.vehicle = value;
				routeItem.vehiclePrefab = AssetDatabase.LoadMainAssetAtPath("Assets/_Prefabs/Bus/" + routeItem.vehicle + ".prefab") as GameObject;
            } else if (label.Equals("time_table_name:")) {
                routeItem.timeTableName = value;
                routeItem.orderedNodeIndexList =  ParseTimeTable(routeItem.timeTableName);
            } else if (label.Equals("bus_stop_cameras:")) {
                routeItem.busStopCameras = ParseBusStopCameras(reader, line, tokens);
				routeItem.busStopCamerasCount = routeItem.busStopCameras.Count;
            } else if (label.Equals("entering_passengers:")) {
                routeItem.enteringPassengersList = DoSimpleIntegerListParsing(reader, line, tokens);
                routeItem.enteringPassengersCount = routeItem.enteringPassengersList.Count;
            } else if (label.Equals("max_leaving_passengers:")) {
                routeItem.leavingPassengersList = DoSimpleIntegerListParsing(reader, line, tokens);
                routeItem.leavingPassengersCount = routeItem.leavingPassengersList.Count;
            } else if (label.Equals("driving_times:")) {
                routeItem.drivingTimesList = DoSimpleIntegerListParsing(reader, line, tokens);
                routeItem.drivingTimesCount = routeItem.drivingTimesList.Count;
            } else if (label.Equals("bus_stop_times:")) {
                routeItem.stopTimesList = DoSimpleIntegerListParsing(reader, line, tokens);
                routeItem.stopTimesCount = routeItem.stopTimesList.Count;
            } else if (label.Equals("icons:")) {
                routeItem.iconsList = DoSimpleStringListParsing(reader, line, tokens);
                routeItem.iconCount = routeItem.iconsList.Count;
            } else if (label.Equals("passengers:")) {
                routeItem.passengerList = DoSimpleStringListParsing(reader, line, tokens);
                routeItem.passengerCount = routeItem.passengerList.Count;
            } 
        }
        routeItem.passengerModels = AddPassengerPrefabsToMissionObject(routeItem);
        routeItem.startPos = server.nodes[routeItem.orderedNodeIndexList[0]].position;
        AssetDatabase.CreateAsset(routeItem,string.Format("Assets/GameDataPersistent/Missions/{0}.asset", name));
        AssetDatabase.SaveAssets();
   	}
	
    private static List<GameObject> AddPassengerPrefabsToMissionObject(RouteItem routeItem) {
        List<GameObject> list = new List<GameObject>();
        for (int i = 0; i < routeItem.passengerCount; i++) {
            var modelPath = routeItem.passengerList[i].Replace("/model", "Assets/_Prefabs");
            //this information will be in the mission.
            modelPath = modelPath.Replace("pmd", "dae");
            modelPath = modelPath.Replace("\"", String.Empty); 
            var model = (GameObject)AssetDatabase.LoadAssetAtPath(modelPath,typeof(GameObject));
            if ( model == null ) {
                if (modelPath.Length > 0) {
                    Debug.Log(modelPath);
                    Debug.LogError("Missing PassengerModel");
                }
                model = new GameObject();
            }
            model.name = "Passengers";
            list.Add(model);
        }
        return list;
    }
	
    private static List<PosRot> ParseBusStopCameras(StringReader reader, string line, string[] tokens ) {
		var result = new List<PosRot>();
		var steps = Convert.ToInt32(tokens[1]);
		Debug.Log (steps);
		Debug.Log(line);
		for (int i = 0; i < steps; i++) {
			line = reader.ReadLine();
			Debug.Log (line);
			if (line.Length < 1) {
				continue;	
			}
			line = CleanLeadingWhiteSpace(line);
			
			if (!line.Contains("(0, 0, 0) (1; 0, 0, 0)")) {
				string[] lineTokens = line.Split(' ');
				var x = ConvertRawElementToFloat(lineTokens[1]);
				var y = ConvertRawElementToFloat(lineTokens[2]);
				var z = ConvertRawElementToFloat(lineTokens[3]);
				
				var s = ConvertRawElementToFloat(lineTokens[4]);
				var a = ConvertRawElementToFloat(lineTokens[5]);
				var b = ConvertRawElementToFloat(lineTokens[6]);
				var c = ConvertRawElementToFloat(lineTokens[7]);
				
				result.Add(new PosRot(new Vector3(x, y, -z), new Quaternion(-a, -b, c, s)));
			}
			else {
				// Special identity case
				result.Add(new PosRot(Vector3.zero, Quaternion.identity));
			}
		}
		
		return result;
    }
	
	private static float ConvertRawElementToFloat(string element) {
		element = element.Replace("(", "");
		element = element.Replace(")", "");
		element = element.Replace("&", "");
		element = element.Replace(",", "");
		element = element.Replace(";", "");
		
		//Debug.Log (element);
		byte[] raw = new byte[element.Length / 2];
		//Debug.Log (raw.Length);
		for (int i = 0; i < raw.Length; i++)
		{
			if (!BitConverter.IsLittleEndian) {
		    	raw[i] = Convert.ToByte(element.Substring(i * 2, 2), 16);
			}
			else {
		    	raw[raw.Length - i - 1] = Convert.ToByte(element.Substring(i * 2, 2), 16);
			}
		}
		return BitConverter.ToSingle(raw, 0);
	}

    private static List<string> DoSimpleStringListParsing(StringReader reader, string line, string[] tokens) {
        var listToReturn = new List<string>();
        var steps = Convert.ToInt32(tokens[1]);
        for (int i = 0; i < steps; i++) {
            line = reader.ReadLine();
            if (line.Length < 1) {
                continue;
            }
            line = CleanLeadingWhiteSpace(line);
            tokens = line.Split(' ');
            var value = tokens[1];
            listToReturn.Add(value);
        }
        return listToReturn;
    }

    private static List<int> DoSimpleIntegerListParsing(StringReader reader, string line, string[] tokens) {
        var listToReturn = new List<int>();
        var steps = Convert.ToInt32(tokens[1]);
        for (int i = 0; i < steps; i++) {
            line = reader.ReadLine();
            if (line.Length < 1) {
                continue;
            }
            line = CleanLeadingWhiteSpace(line);
            tokens = line.Split(' ');
            var value = Convert.ToInt32(tokens[1]);
            listToReturn.Add(value);
        }
        return listToReturn;
    }

    private static string CleanLeadingWhiteSpace(string line){
        if (line.Length != 0) {
            if (char.IsWhiteSpace(line[0])) {
                return line.Substring(1);
            } else return line;
        } else return line;

    }
   
    private static List<int> ParseTimeTable(string filename) {
        filename = filename.Replace("\"", "");
        var fullFilename = Directory.GetCurrentDirectory() + Path.DirectorySeparatorChar + 
            "Assets" + Path.DirectorySeparatorChar + "base" + filename;
        fullFilename = fullFilename.Replace('/', Path.DirectorySeparatorChar);

        var lines = File.ReadAllText(fullFilename);
        var reader = new StringReader(lines);
        var nodes = new Dictionary<string,int>();
        var timeTable = new List<string>();
        int[] orderedNodes = new int[0]; 
        while (reader.Peek() != -1) {
            var line = reader.ReadLine();
            if (line.Length == 0) {
                continue;
            }
            line = CleanLeadingWhiteSpace(line);
            if (line.Equals("SiiNUnit")) {
                continue;
            }
            var tokens = line.Split(' ');
            if (tokens[0].Equals("bus_line_data_table")) {
                line = reader.ReadLine();
                line = CleanLeadingWhiteSpace(line);
                tokens = line.Split(' ');
                timeTable = FillTimeTable(reader, line, tokens);
            } else if (tokens[0].Equals("bus_line_data")) {
                var name = tokens[2];
                line = reader.ReadLine();
                line = CleanLeadingWhiteSpace(line);
                tokens = line.Split(' ');
                nodes.Add(name, Convert.ToInt32(tokens[1]));
            }
            // now order nodes, in the order of they appear in timetable. 
        }
        orderedNodes = new int[nodes.Count];
        for (int i = 0; i < timeTable.Count; i++) {
            orderedNodes[i] = nodes[timeTable[i]];
        }
        return orderedNodes.OfType<int>().ToList(); 
    }

    private static List<string> FillTimeTable( StringReader reader, string line, string[] tokens) {
        var returnlist = new List<string>();
        var steps = Convert.ToInt32(tokens[1]);
        for( int i = 0; i < steps; i++) {
            line = reader.ReadLine();
            tokens = CleanLeadingWhiteSpace(line).Split(' ');
            returnlist.Add(tokens[1]);
        }
        return returnlist;
    }
}
