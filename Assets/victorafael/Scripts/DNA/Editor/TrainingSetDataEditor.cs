using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TrainingSetData))]
public class TrainingSetDataEditor : Editor {
	private const float itemSize = 16;
	private const float nameWidth = 100;
	private const float minItemWidth = 25;

	private float[] widths;
	private float[] offsets;

	public override void OnInspectorGUI ()
	{
		float panelWidth = EditorGUIUtility.currentViewWidth - 20;

		var arrayProp = serializedObject.FindProperty ("itens");

		var inputNamesProperty = serializedObject.FindProperty ("inputNames");
		var outputNamesProperty = serializedObject.FindProperty ("outputNames");

		int inputCount = 0;
		int outputCount = 0;

		bool hasItem = false;
		if (arrayProp.arraySize > 0) {
			var item = arrayProp.GetArrayElementAtIndex (0);
			inputCount = item.FindPropertyRelative ("input").arraySize;
			outputCount = item.FindPropertyRelative ("outputs").arraySize;
			hasItem = true;
		}

		//name | n inputs | addinput | space | n outputs | addOutput | remove
		int totalColumnCount = 1 + inputCount + 2 + outputCount + 2; 

		widths = new float[totalColumnCount];
		offsets = new float[totalColumnCount];
		widths[0] = nameWidth;

		float itemWidth =  (panelWidth - nameWidth) / (totalColumnCount - 1);

		offsets [0] = 0;
		float offset = widths[0];
		for (int i = 1; i < widths.Length; i++) {
			widths [i] = itemWidth;
			offsets [i] = offset;
			offset += minItemWidth;
		}
		EditorGUI.BeginChangeCheck ();

		#region Header
		GUILayout.BeginHorizontal ();
		GUILayout.Label("name", Wid(0));

		if(DrawHeaderGroup(1, inputCount, inputNamesProperty, hasItem, "input", "In", arrayProp)) return;
		GUILayout.Space(widths[2 + inputCount]);
		if(DrawHeaderGroup(3 + inputCount, outputCount, outputNamesProperty, hasItem, "outputs", "Out", arrayProp)) return;

		//GUILayout.Label("=", Wid(2+columnCount));
		GUILayout.EndHorizontal ();
		#endregion
		#region Itens
		for (int i = 0; i < arrayProp.arraySize; i++) {
			GUILayout.BeginHorizontal ();
			var el = arrayProp.GetArrayElementAtIndex (i);
			EditorGUILayout.PropertyField (el.FindPropertyRelative ("name"),GUIContent.none, Wid (0));

			for(int c = 0; c < inputCount; c++){
				EditorGUILayout.PropertyField (el.FindPropertyRelative ("input").GetArrayElementAtIndex (c), GUIContent.none, Wid (1 + c));
			}

			GUILayout.Space (widths [1 + inputCount] + widths [2 + inputCount]);

			int colStart = 3 + inputCount;
			for(int c = 0; c < outputCount; c++){
				EditorGUILayout.PropertyField (el.FindPropertyRelative ("outputs").GetArrayElementAtIndex (c), GUIContent.none, Wid (colStart + c));
			}

			GUILayout.Space(widths[totalColumnCount - 2]);

			if(GUILayout.Button("x", Wid(totalColumnCount - 1))){
				arrayProp.DeleteArrayElementAtIndex(i);
				serializedObject.ApplyModifiedProperties();
				GUILayout.EndHorizontal();
				return;
			}

			GUILayout.EndHorizontal ();
		}
		#endregion
		#region BottomLine
		GUILayout.BeginHorizontal ();
		if(GUILayout.Button("+", Wid(1))){
			arrayProp.InsertArrayElementAtIndex(arrayProp.arraySize);
			serializedObject.ApplyModifiedProperties();
			GUILayout.EndHorizontal ();
			return;
		}

		GUILayout.Space (widths [0] - widths[1]);

		if(DrawRemoveColumn(1, inputCount, "input", arrayProp)) return;
		GUILayout.Space (widths [1 + inputCount] + widths [2 + inputCount]);
		if(DrawRemoveColumn(3 + inputCount, outputCount, "outputs", arrayProp)) return;

		GUILayout.EndHorizontal ();
		#endregion

		EditorGUILayout.PropertyField (inputNamesProperty, new GUIContent( "Edit input names"), true);
		EditorGUILayout.PropertyField (outputNamesProperty, new GUIContent( "Edit output names"), true);
		EditorGUILayout.PropertyField (serializedObject.FindProperty ("epochCount"));

		if (EditorGUI.EndChangeCheck ()) {
			serializedObject.ApplyModifiedProperties ();
		}
	}

	private bool DrawHeaderGroup(int startIndex, int count, SerializedProperty namesProperty, bool hasItem, string targetPropertyName, string defaultPrefix, SerializedProperty itemsArrayProperty){
		for (int i = 0; i < count; i++) {
			string txt = defaultPrefix + i;

			if(namesProperty.arraySize > i){
				string customName = namesProperty.GetArrayElementAtIndex(i).stringValue;
				if(!string.IsNullOrEmpty(customName))
					txt = customName;
			}

			GUILayout.Label(txt, Wid(startIndex + i));
		}
		GUI.enabled = hasItem;
		if (GUILayout.Button ("+", Wid(startIndex+count))) {
			for(int n = 0; n < itemsArrayProperty.arraySize; n++){
				itemsArrayProperty.GetArrayElementAtIndex (n).FindPropertyRelative (targetPropertyName).InsertArrayElementAtIndex (
					count
				);
			}
			serializedObject.ApplyModifiedProperties();
			GUILayout.EndHorizontal();
			return true;
		}
		GUI.enabled = true;
		return false;
	}

	private bool DrawRemoveColumn(int startIndex, int count, string targetPropertyName, SerializedProperty itemsArrayProperty){
		for (int i = 0; i < count; i++) {
			GUI.enabled = i > 0;
			if (GUILayout.Button ("x", Wid (i + 1))) {
				for(int n = 0; n < itemsArrayProperty.arraySize; n++){
					itemsArrayProperty.GetArrayElementAtIndex (n).FindPropertyRelative (targetPropertyName).DeleteArrayElementAtIndex (i);
				}
				serializedObject.ApplyModifiedProperties();
				GUILayout.EndHorizontal ();
				return true;
			}
		}
		GUI.enabled = true;
		return false;
	}

	private GUILayoutOption Wid(int index){
		return GUILayout.Width (widths [index]);
	}
	private Rect GetRect(int col, int row){
		return new Rect (offsets [col], row * itemSize, widths [col], itemSize);
	}
}
