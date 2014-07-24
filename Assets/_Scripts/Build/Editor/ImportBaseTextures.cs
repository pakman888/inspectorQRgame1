using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ImportBaseTextures : AssetPostprocessor {
	
	public string GetMipmapTexturePath(string baseTexturePath){
		return baseTexturePath.Replace("/base/", "/Textures/MipmapOverrides/");
	}
	
	public void OnPreprocessTexture() {
		if(assetPath.StartsWith(WorldConversion.sourceBaseFolder)){
			TextureImporter importer = assetImporter as TextureImporter;
			importer.textureType = TextureImporterType.Advanced; //Anything else is just annoying
			importer.filterMode = FilterMode.Bilinear;
			importer.mipMapBias = 0;
			
			//Setup mobile import settings
			importer.SetPlatformTextureSettings(
				"iPhone",
				importer.maxTextureSize,
				importer.DoesSourceTextureHaveAlpha() ? 
					  TextureImporterFormat.PVRTC_RGBA4
					: TextureImporterFormat.PVRTC_RGB4
			);
			importer.SetPlatformTextureSettings(
				"Android",
				importer.maxTextureSize,
				importer.DoesSourceTextureHaveAlpha() ? 
					  TextureImporterFormat.RGBA16 //ATC_RBGA8 not guaranteed to be supported
					: TextureImporterFormat.ETC_RGB4
			);
		}
	}
	
	/*
	public void OnPostprocessTexture(Texture2D texture){
		if(System.Array.Exists(AssetDatabase.GetLabels(AssetDatabase.LoadMainAssetAtPath(assetPath)), x => x == "Mipmaps")){
			string overrideAssetPath = GetMipmapTexturePath(assetPath);
			Texture2D mipmapTexture = AssetDatabase.LoadAssetAtPath(overrideAssetPath, typeof(Texture2D)) as Texture2D;
			if(!mipmapTexture){
				Debug.LogWarning("Texture " + assetPath + " is flagged for mipmap overrides but no texture exists at " + overrideAssetPath);
				return;
			}
			List<Color[]> levels = new List<Color[]>();			
			int offset = 0;			
			int height = mipmapTexture.height;
			int width = mipmapTexture.height;
			{
				int w = width;
				int h = height;
				for(;;){
					Debug.Log("Loading mipmap " + w + "x" + h + "@ x = " + offset);
					levels.Add(mipmapTexture.GetPixels(offset, 0, w, h));
					if(w == 1 && h == 1){
						break;
					}					
					offset += w;
					w = Mathf.Max(w/2, 1);
					h = Mathf.Max(h/2, 1);
				}
			}
			texture.Resize(texture.height, texture.height, texture.format, true);
			for(int i = 0; i < levels.Count; i++){
				texture.SetPixels(levels[i], i);
			}
			texture.Apply(false, true);
		}
	}
	*/
}