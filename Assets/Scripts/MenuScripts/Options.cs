using UnityEngine;
using System.Collections;

public class Options : MonoBehaviour {
	
	public Texture backgroundTexture;
	public float guiPlacementX1 = .5f;
	public float guiPlacementX2 = .5f;
	public float guiPlacementY1 = .5f;
	public float guiPlacementY2;
	private string label;
	private bool clicked;
	void Start() {
		label = "Delete Player Progress";
		guiPlacementX2 = .5f;
		guiPlacementY2 = .5f;
		guiPlacementY1 = .5f;
		
	}
	void OnGUI() {
		GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height),backgroundTexture);
		if (GUI.Button(new Rect(Screen.width * guiPlacementX1, Screen.height * guiPlacementY1, Screen.width * .5f, Screen.height * .1f), label)) {
			if (!clicked) {
				clicked = true;
				label = "Are you sure? Cannot be undone";
			} else {
				PlayerPrefs.DeleteAll();
				Application.LoadLevel("MainMenu");

			}
		}
		
		
		if (GUI.Button(new Rect(Screen.width * guiPlacementX2, Screen.height * guiPlacementY2, Screen.width * .5f, Screen.height * .1f), "Options")) {
			Application.LoadLevel ("KeyBindings");
		}
		
	}
}
