using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RawImage))]
public class NeuralToGraph2D : MonoBehaviour , IPointerClickHandler{
	public Brain brain;

	public int steps = 40;

	void Calculate(){
		float stepSize = 1f / steps;
		Color[] colors = new Color[(steps+1) * (steps+1)];
		for (int x = 0; x <= steps; x++) {
			for (int y = 0; y <= steps; y++) {
				double[] res = brain.Execute (new double[]{x * stepSize, y * stepSize});
				colors [y * (steps +1) + x] = new Color (1 - (float)res[0], (float)res[0], 0);
			}
		}

		foreach (var ti in brain.trainingData.itens){
			var x = Mathf.FloorToInt((float)ti.input [0] * steps);
			var y = Mathf.FloorToInt ((float)ti.input [1] * steps);

			float res = (float)(brain.Execute (new double[]{x * stepSize, y * stepSize})[0]) * 0.5f;

			colors [y * (steps + 1) + x] = new Color (0.5f - res, res, 0);
		}

		Texture2D t2d = new Texture2D (steps+1, steps+1);
		t2d.SetPixels (colors);
		t2d.Apply ();
		GetComponent<RawImage> ().texture = t2d;
	}

	#region IPointerClickHandler implementation

	public void OnPointerClick (PointerEventData eventData)
	{
		Calculate ();
	}

	#endregion
	
	// Update is called once per frame
	void Update () {
		
	}
}
