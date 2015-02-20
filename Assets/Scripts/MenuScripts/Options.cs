using UnityEngine;
using System.Collections;

public class Options : MonoBehaviour {
	
	public Texture backgroundTexture;
	public float guiPlacementX1 = .5f;
	public float guiPlacementX2 = .5f;
	public float guiPlacementY1 = .5f;
	public float guiPlacementY2;

	private string label,backLabel;
	private bool clicked,backClicked;
	void Start() {
		label = "Delete Player Progress";
		backLabel = "Back";
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
		
		if (GUI.Button (new Rect ((Screen.width - Screen.width*.5f)/2.0f, Screen.height -Screen.height*.1f, Screen.width * .5f, Screen.height * .1f), backLabel)) {
		
			Application.LoadLevel("MainMenu");
			clicked = false;
		
		}
		
	}
}
