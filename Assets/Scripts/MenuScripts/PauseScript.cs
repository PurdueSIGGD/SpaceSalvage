using UnityEngine;
using System.Collections;

public class PauseScript : MonoBehaviour {
	bool IsPaused;
	// Use this for initialization
	void Start(){
		IsPaused = false;
		
	}
	
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape) ){
			IsPaused = !IsPaused;

		}
	}
	
	void OnGUI(){
		if (IsPaused){
			this.BroadcastMessage("PauseGame", true);
			Time.timeScale = 0;
			GUI.Box (new Rect ((Screen.width * .45f),Screen.height * .1f,Screen.width * .1f,Screen.height * .1f), "Menu");
			
			// Make the Quit button.
			if (GUI.Button (new Rect (Screen.width * .2f, (Screen.height * .9f) ,Screen.width * .2f,Screen.height * .1f), "Main Menu")) {
				Application.LoadLevel("MainMenu");
			}

			if(GUI.Button(new Rect(Screen.width * .4f,Screen.height * .7f,Screen.width * .2f,Screen.height * .1f), "Back")){
				IsPaused = false;
			}
			if (GUI.Button (new Rect (Screen.width * .6f, (Screen.height * .9f),Screen.width * .2f ,Screen.height * .1f), "Exit Game")) {
				Application.Quit();
			}
			

		} else {
			this.BroadcastMessage("PauseGame", false);

			Time.timeScale = 1;
		}
		
	}
}
