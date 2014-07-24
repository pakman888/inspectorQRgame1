using UnityEngine;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class PrefabDescParser : MonoBehaviour {
 	public static PrefabDesc Parse(string filename) {
        var prefabDesc = new PrefabDesc();
		
		// Refactor: These should be managed by the resource server
		var f = File.Open(filename, FileMode.Open);
        var reader = new BinaryReader(f);
		reader.ReadBytes(4); // Prefab file version, don't care about it 
		prefabDesc.nodeCount = BitConverter.ToInt32(reader.ReadBytes(4), 0);
		
		f.Close();
		return prefabDesc;
    }
}