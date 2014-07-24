using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class MissionDescriptions : ScriptableSingleton<MissionDescriptions> {
	public List<MissionDescription> descriptions;
	
	public MissionDescription GetDescriptionForTitleKey(string titleKey) {
		titleKey = titleKey.Substring(3);
		titleKey = titleKey.Substring(0, titleKey.Length - 3);
		return (from description in descriptions 
		 		where description.titleKey.Equals(titleKey) 
		 		select description).FirstOrDefault();	
		
	}
}

[System.Serializable]
public class MissionDescription {
	public string titleKey;
	public string title;
	[Multiline] public string paragraph;
	public Texture2D icon;
	public Texture2D map;
}