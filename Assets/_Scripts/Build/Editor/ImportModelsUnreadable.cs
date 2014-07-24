using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class ImportModelsUnreadable : AssetPostprocessor {
	
	public void OnPreprocessModel() {		
		ModelImporter importer = assetImporter as ModelImporter;
		importer.isReadable = false;
		importer.generateAnimations = ModelImporterGenerateAnimations.None;
		importer.animationType = ModelImporterAnimationType.None;
		importer.importAnimation = false;
		if(assetPath.StartsWith(WorldConversion.meshesFolder)){
			importer.importMaterials = false;
		}
		importer.tangentImportMode = ModelImporterTangentSpaceMode.None;
		
	}
	
}