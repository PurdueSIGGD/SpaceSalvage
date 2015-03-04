using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {
	
	public Texture backgroundTexture;
	public float guiPlacementX1 = .5f;
	public float guiPlacementX2 = .5f;
	public float guiPlacementY1 = .5f;
	public float guiPlacementY2;
	void Start() {
		guiPlacementX2 = .5f;
		guiPlacementY2 = .5f;
		guiPlacementY1 = .5f;
		
	}
	void OnGUI() {
		GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height),backgroundTexture);
		
		if (GUI.Button(new Rect(Screen.width * guiPlacementX1, Screen.height * guiPlacementY1, Screen.width * .5f, Screen.height * .1f), "Main Menu")) {
			Application.LoadLevel("MainMenu");	
		}
		
		
		if (GUI.Button(new Rect(Screen.width * guiPlacementX2, Screen.height * guiPlacementY2, Screen.width * .5f, Screen.height * .1f), "Quit")) {
			Application.Quit();
		}
		
	}
}
