using UnityEngine;
using System.Collections;

public class MissionDescriptionUI : MonoBehaviour {
	public UILabel title;
	public UILabel paragraph;
	
	public void SetMissionDescription(string titleKey) {
		var description = MissionDescriptions.Instance.GetDescriptionForTitleKey(titleKey);
		title.text = description.title;
		paragraph.text = description.paragraph;//"fuck\n uuuuuuuuuuuuuuuuuuuuuuuuuuuuyeahhhhhhhhhhhhhhhhhhhhhhhh\n \n";//description.paragraph;
		paragraph.multiLine = true;
		paragraph.supportEncoding = true;
	}
}
