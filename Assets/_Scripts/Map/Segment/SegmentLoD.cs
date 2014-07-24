using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;

public static class SegmentLoD {

	public const int 	Exclude = 0,
						Default = 2;

#if UNITY_EDITOR
	const string kRankingsFilePath = "Assets/SegmentLoD.txt";

	public static Dictionary<int, int> LoadRawData(){
		var result = new Dictionary<int, int>();
		if(File.Exists(kRankingsFilePath)){
			foreach(string line in File.ReadAllLines(kRankingsFilePath)){
				string[] parts = line.Split();
				result[int.Parse(parts[0])] = int.Parse(parts[1]);
			}
		}
		return result;
	}
	
	public static void SaveRawData(Dictionary<int, int> data){
		string[] linesToWrite = data.Select(kvp => kvp.Key + " " + kvp.Value).ToArray();
		File.WriteAllLines(kRankingsFilePath, linesToWrite);
	}
#endif
}
