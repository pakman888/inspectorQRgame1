using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

public class PrismMaterial {	
    public static string TEXTURE_NOT_FOUND = "TEXTURE_NOT_FOUND";
	
	private static Dictionary<string, Shader> effectShaderCache = new Dictionary<string, Shader>();

    public string alias;
    public string pathName;
    public List<string> tobjPaths;
    public List<string> textureNames;
    public List<string> texturePaths;
	public List<string> normalmapTobjPaths;
	public List<string> normalmapTexturePaths;
	public string specularTobjPath;
	public string specularTexturePath;
    public Color diffuse;
    public Color specular;
    public Color ambient;
    public Color tint;
	public Color emission;
    public float shininess;
	public float tintOpacity;
	
	public static string GetTexturePathFromTObjFile(string tObjFilepath) {
		if(!File.Exists(tObjFilepath)){
			return null;
		}
		var tobj = File.ReadAllBytes(tObjFilepath);
        byte[] pathbytes = new byte[tobj.Length - 48];
        for (int i = 48, j = 0; i < tobj.Length; ++i, ++j) {
            pathbytes[j] = tobj[i];
        }
		// TRIXY: All of the tobj files have .dds extensions, as that was the runtime format SCS
		// used. As it turns out, Unity doesn't fully support dds, so we switched them out with
		// TGA versions. Instead of modifying the .tobj files, we're performing the replacement
		// here.
		//Debug.Log (System.Text.Encoding.Default.GetString(pathbytes).Replace(".dds", ".tga"));
		return System.Text.Encoding.Default.GetString(pathbytes).Replace(".dds", ".tga");
	}
	
	public static Shader GetShaderForEffect(string effect){
		if(effect == null){
			Debug.LogError("Null material effect");
			return null;
		}
		if(effectShaderCache.ContainsKey(effect)){
			return effectShaderCache[effect];
		}
		Shader result;
		Regex transparencyPattern = new Regex(@"\.a\.");
		Regex additivePattern = new Regex(@"\.add\.");
		Regex environmentPattern = new Regex(@"\.add\.env\.");
		Regex specularPattern = new Regex(@"\.dif_spec\.");
		if(transparencyPattern.IsMatch(effect)){
			if(specularPattern.IsMatch(effect)){
				result = Shader.Find("Transparent/Specular");
			}
			else{
				result = Shader.Find("Transparent/Diffuse");
			}
		}
		else if(additivePattern.IsMatch(effect) && !environmentPattern.IsMatch(effect)){
			result = Shader.Find("Mobile/Particles/Additive");
		}
		else if(specularPattern.IsMatch(effect)){
			result = Shader.Find("Specular");
		}		
		//Fall-through
		else{
			result = Shader.Find("Mobile/Diffuse");
		}
		
		effectShaderCache[effect] = result;
		return result;
	}

	//Debug
	public static void LogShaderCache(){
		File.AppendAllText("Assets" + Path.DirectorySeparatorChar + "Logs" + Path.DirectorySeparatorChar + "Shaders.log", "PrismMaterial Shader Cache Contents\n");
		foreach(string line in effectShaderCache.Select(kvp => kvp.Key + "\t->\t" + kvp.Value.name + "\n")){
			File.AppendAllText("Assets" + Path.DirectorySeparatorChar + "Logs" + Path.DirectorySeparatorChar + "Shaders.log", line);
		}
	}

    public PrismMaterial() {
        tobjPaths = new List<string>();
        textureNames = new List<string>();
        texturePaths = new List<string>();
		normalmapTexturePaths = new List<string>();
		normalmapTobjPaths = new List<string>();
    }

	public void LoadTexturePathsFromTObjPaths(){
		foreach (string tobjFilename in tobjPaths) {
			texturePaths.Add(GetTexturePathFromTObjPath(tobjFilename));
        }
		foreach (string tobjFilename in normalmapTobjPaths){
			normalmapTexturePaths.Add(GetTexturePathFromTObjPath(tobjFilename));
		}
		if(!String.IsNullOrEmpty(specularTobjPath)){
			specularTexturePath = GetTexturePathFromTObjPath(specularTobjPath);
		}
	}
	
	private string GetTexturePathFromTObjPath(string tobjPath){
		tobjPath = tobjPath.Replace('/', Path.DirectorySeparatorChar);		
		tobjPath = MaterialGenerator.GetPathRelativeToBase(pathName, tobjPath);
        tobjPath = Directory.GetCurrentDirectory() + "/" + WorldConversion.sourceBaseFolder + tobjPath;

        try {
			string texturePath = GetTexturePathFromTObjFile(tobjPath);
			if(texturePath == null){
				LogMissingTexture(tobjPath, null);
				return TEXTURE_NOT_FOUND;
			}
            return texturePath;
        } catch (Exception e) {
			LogMissingTexture(tobjPath, e);
			return TEXTURE_NOT_FOUND;
        }
	}
	
	public Material CreateUnityMaterial(){
		Material result = new Material(GetShaderForEffect(alias));
		Texture2D texture = null;
		if(texturePaths.Count > 0){
			texture = (Texture2D)AssetDatabase.LoadAssetAtPath("Assets/base" + texturePaths[0].Replace('\\', '/'), typeof(Texture2D));
			result.SetTexture("_MainTex", texture);
		}
		//result.SetFloat("_Shininess", prismMaterial.shininess);
        //result.SetColor("_Specular", prismMaterial.specular);        
		return result;
	}
	
	private void LogMissingTexture(string filepath, Exception exception) {
        File.AppendAllText("Assets" + Path.DirectorySeparatorChar + "Logs" + Path.DirectorySeparatorChar + "MissingTexture.log",
            filepath + '\n');
		if(exception != null){
        	Debug.LogWarning(exception);
		}
		else{
			Debug.LogWarning("Missing texture: " + filepath);
		}
	}
	
	public override bool Equals(object other){
		PrismMaterial otherMaterial = (PrismMaterial)other;
		if(otherMaterial == null){
			return false;
		}		
		return texturePaths.SequenceEqual(otherMaterial.texturePaths)
			&& normalmapTexturePaths.SequenceEqual(otherMaterial.normalmapTexturePaths)
			&& String.Equals(specularTexturePath, otherMaterial.specularTexturePath)
			&& diffuse.Equals(otherMaterial.diffuse)
			&& ambient.Equals(otherMaterial.ambient)
			&& tint.Equals(otherMaterial.tint)
			&& emission.Equals(otherMaterial.emission)
			&& shininess.Equals(otherMaterial.shininess)
			&& tintOpacity.Equals(otherMaterial.tintOpacity);
	}
	
	public override int GetHashCode(){
		int hash = 0;
		foreach(string s in texturePaths){
			hash ^= s.GetHashCode();
		}
		foreach(string s in normalmapTexturePaths){
			hash ^= s.GetHashCode();
		}
		hash ^= (specularTexturePath == null ? 0 : specularTexturePath.GetHashCode());
		hash ^= diffuse.GetHashCode();
		hash ^= ambient.GetHashCode();
		hash ^= tint.GetHashCode();
		hash ^= emission.GetHashCode();
		hash ^= shininess.GetHashCode();
		hash ^= tintOpacity.GetHashCode();
		return hash;
	}
	
	public override string ToString(){
		System.Text.StringBuilder sb = new System.Text.StringBuilder();
		sb.Append("Textures{");
		foreach(string s in texturePaths){
			sb.Append(s + ",");
		}
		sb.AppendLine("}");
		sb.Append("Normals{");
		foreach(string s in normalmapTexturePaths){
			sb.Append(s + ",");
		}
		sb.AppendLine("}");
		sb.AppendLine("Specular{"+specularTexturePath+"}");
		sb.AppendLine("Diffuse " + diffuse);
		sb.AppendLine("Ambient " + ambient);
		sb.AppendLine("Tint " + tint);
		sb.AppendLine("Emission " + emission);
		sb.AppendLine("Shininess " + shininess);
		sb.AppendLine("TintOpacity " + tintOpacity);
		sb.AppendLine(GetHashCode().ToString());
		return sb.ToString();
	}
}