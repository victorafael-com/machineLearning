using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "victorafael/Misc/New Style", order = 2, fileName = "new Style")]
public class GUIStyleData : ScriptableObject {
	public GUIStyle style;

	public void Label(string content){
		GUILayout.Label (content, style);
	}
	public void Label(string content, params object[] data){
		GUILayout.Label (string.Format (content, data), style);
	}
}
