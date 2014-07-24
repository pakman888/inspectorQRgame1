using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class MaterialGenerator {
    public static void Generate() {
    	var prismMaterials = ReadPrismMaterials(Directory.GetCurrentDirectory() + "/" + WorldConversion.sourceBaseFolder);
        WriteUnityMaterials(prismMaterials);
		PrismMaterial.LogShaderCache();
    }
	
    public static List<PrismMaterial> ReadPrismMaterials(string root) {
        List<PrismMaterial> mats = new List<PrismMaterial>();
        Regex filenameRegex = new Regex(@"\.mat$", RegexOptions.IgnoreCase);
		
        foreach (string d in Directory.GetDirectories(root)) {
            foreach (string f in Directory.GetFiles(d)) {
                if (filenameRegex.IsMatch(f)){
                    mats.Add(ReadPrismMaterial(f));
                }
            }
            mats.AddRange(ReadPrismMaterials(d));
        }
		
        return mats;
    }
	
    private static PrismMaterial ReadPrismMaterial(string path) {
        StreamReader reader = new StreamReader(path);
        PrismMaterial thisMat = new PrismMaterial();
        thisMat.pathName = path;
		try{
    	    while (reader.Peek() != -1 ) {
	            var line = reader.ReadLine().Trim();
	            var splitLine = line.Split(' ');
	            if (Regex.IsMatch(splitLine[0].Trim(), "material", RegexOptions.IgnoreCase)) {
	                thisMat.alias = splitLine[2].Trim('"');
	            }
	            if (Regex.IsMatch(splitLine[0].Trim(), @"^texture\[?[0-9]?\]?$")) {
	                thisMat.tobjPaths.Add(splitLine[2].Replace('"', ' ').Trim());
	            }
	            if (Regex.IsMatch(splitLine[0], @"^texture_name\[?[0-9]?\]?$")) {
	                thisMat.textureNames.Add(splitLine[2].Replace('"',' ').Trim());
	            }
			
	//            if (Regex.IsMatch(splitLine[0].Trim(), @"^ambient$")) {
	//                thisMat.ambient = GetColorFromMatFile(splitLine); 
	//            }
	//            if (Regex.IsMatch(splitLine[0].Trim(), @"^diffuse$")) {
	//                thisMat.diffuse = GetColorFromMatFile(splitLine);
	//            }
	//            if (Regex.IsMatch(splitLine[0].Trim(), @"^specular$")) {
	//                thisMat.specular = GetColorFromMatFile(splitLine);
	//            }
	//            if (Regex.IsMatch(splitLine[0].Trim(), @"^tint$")) {
	//                thisMat.tint = GetColorFromMatFile(splitLine);
	//            }
	//            if (Regex.IsMatch(splitLine[0].Trim(), @"^shininess$")) {
	//                thisMat.shininess = Convert.ToSingle(splitLine[2]);
	//            }	
	        }
		}
		catch(Exception e){
			Debug.LogError("Error reading material from path " + path + ":" + e.ToString());
		}
        
		thisMat.LoadTexturePathsFromTObjPaths();

        return thisMat;
	}
	
	public static string GetPathRelativeToBase(string materialPath, string filePath) {
		if (filePath[0] == Path.DirectorySeparatorChar) {
            filePath = filePath.Remove(0, 1);
		}
        else {
            string trimmedPath = materialPath.Substring(Directory.GetCurrentDirectory().Length + "Assets/base/".Length  );
            trimmedPath = trimmedPath.Remove(trimmedPath.LastIndexOf(Path.DirectorySeparatorChar) + 1);
            filePath = trimmedPath + filePath;
            if (filePath[0] == Path.DirectorySeparatorChar) {
                filePath = filePath.Remove(0, 1);
            }
        }
		return filePath;
	}
	
	private static void WriteUnityMaterials(List<PrismMaterial> listOfMats) {
		AssetDatabase.StartAssetEditing();
		try{
	        for(int i = 0; i < listOfMats.Count; i++) {
				if(i % 50 == 0){
					EditorUtility.DisplayProgressBar("Writing Materials", string.Empty, i / (float)listOfMats.Count);
				}
		 		PrismMaterial prismMaterial = listOfMats[i];
	            if (prismMaterial.texturePaths.Count != 0) {
			        Material newMat = prismMaterial.CreateUnityMaterial();       
			        string path = prismMaterial.pathName.Replace("base", "GameData/Materials");
			        string newPath = path;
			        int index = newPath.LastIndexOf(Path.DirectorySeparatorChar);
			        if ( index != -1 )
			            newPath = newPath.Remove(index + 1);
			        Directory.CreateDirectory(newPath);

			        //unity wants project relative root. 
			        string unityPath = path.Substring(Directory.GetCurrentDirectory().Length + 1);
			        unityPath = unityPath.Replace("\\", "/");
				
					if (unityPath.Contains("guide_a")) {
						newMat = FixArrowMaterial(newMat);
					}

	                   AssetDatabase.CreateAsset(newMat, unityPath);
					//EditorUtility.UnloadUnusedAssets();
	            }
	        }
		}
		finally{
			EditorUtility.ClearProgressBar();
		}
		AssetDatabase.StopAssetEditing();
    }
	
	static Material FixArrowMaterial(Material mat) {
		// Makes the arrow material look orange w/ a nice lil' gradient
		var texture = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/base/material/ui/icon_uninstall.bmp", typeof(Texture2D));
		mat.SetTexture("_MainTex", texture);
		return mat;
	}
	
    public static Color GetColorFromMatFile(string[] splitLine) {
        try {
            return new Color(Convert.ToInt32(splitLine[3]),
                               Convert.ToInt32(splitLine[5]),
                               Convert.ToInt32(splitLine[7]));
        } catch (FormatException e) {
            Debug.LogError(e.Message);
            return Color.grey;
        } 
    }
}

