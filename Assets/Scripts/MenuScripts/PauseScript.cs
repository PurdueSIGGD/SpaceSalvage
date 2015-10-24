using UnityEngine;
using System.Collections;

public class PauseScript : MonoBehaviour {
	bool IsPaused;
	bool tutorial;
	bool playing;
	GameObject[] infos;
	public AudioClip clip;
	// Use this for initialization
	void Start(){
		IsPaused = false;
		if (PlayerPrefs.HasKey("tutorial")) {
			tutorial = PlayerPrefs.GetInt("tutorial")==0?true:false;
		} else {
			tutorial = false;
			PlayerPrefs.SetInt("tutorial", 0);
		}

		infos = GameObject.FindGameObjectsWithTag("Info");
		foreach (GameObject g in infos) {
			g.SetActive(tutorial);
		}

	}
	
	void Update () {
		//print(PlayerPrefs.GetInt("tutorial"));
		if (Input.GetKeyDown(KeyCode.Escape) ){
			IsPaused = !IsPaused;

		}
		if (playing) {
			this.transform.FindChild("SoundEffectController").FindChild("MenuButton").GetComponent<AudioSource>().Play();
			playing = false;
		}
	}
	
	void OnGUI(){

		if (IsPaused){
			this.BroadcastMessage("PauseGame", true);
			Time.timeScale = 0;
			GUI.Box (new Rect ((Screen.width * .45f),Screen.height * .1f,Screen.width * .1f,Screen.height * .1f), "Menu");
			if (tutorial) {
				if (GUI.Button(new Rect (Screen.width * .8f, (Screen.height * .2f) ,Screen.width * .15f,Screen.height * .08f), "Turn Tutorial OFF")) {
					playing = true;
					tutorial = false;
					PlayerPrefs.SetInt("tutorial", 1);
					foreach (GameObject g in infos) {
						g.SetActive(false);
					}
				}
			} else {
				if (GUI.Button(new Rect (Screen.width * .8f, (Screen.height * .2f) ,Screen.width * .15f,Screen.height * .08f), "Turn Tutorial ON")) {
					playing = true;
					tutorial = true;
					PlayerPrefs.SetInt("tutorial", 0);
					foreach (GameObject g in infos) {
						g.SetActive(true);
					}
				}
			}

			// Make the Quit button.
			if (GUI.Button (new Rect (Screen.width * .2f, (Screen.height * .9f) ,Screen.width * .2f,Screen.height * .1f), "Main Menu")) {
				playing = true;
				Application.LoadLevel("MainMenu");
			}

			if(GUI.Button(new Rect(Screen.width * .4f,Screen.height * .7f,Screen.width * .2f,Screen.height * .1f), "Back")){
				playing = true;
				IsPaused = false;
			}
			if (GUI.Button (new Rect (Screen.width * .6f, (Screen.height * .9f),Screen.width * .2f ,Screen.height * .1f), "Exit Game")) {
				playing = true;
				Application.Quit();
			}
			

		} else {
			this.BroadcastMessage("PauseGame", false);

			Time.timeScale = 1;
		}
		
	}
}
