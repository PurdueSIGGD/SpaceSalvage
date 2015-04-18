using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {
	
	public Texture backgroundTexture;

	public float guiPlacementY2;
	void Start() {

		
	}
	void OnGUI() {
		GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height),backgroundTexture);
		GUI.Box(new Rect((Screen.width / 2) - 200, (Screen.height / 2) - 100, 400, 200), "GAME OVER");
		if (GUI.Button(new Rect(0, (Screen.height * .9f)  , Screen.width * .5f, Screen.height * .1f), "Main Menu")) {
			Application.LoadLevel("MainMenu");	
		}
		
		
		if (GUI.Button(new Rect(Screen.width * .5f, Screen.height * .9f, Screen.width * .5f, Screen.height * .1f), "Quit")) {
			Application.Quit();
		}
		
	}
}
