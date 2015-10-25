using UnityEngine;
using System.Collections;

public class PauseScript : MonoBehaviour {
	public Font thisFont;

	bool IsPaused;
	bool tutorial;
	bool playing;
	GameObject[] infos;
	public AudioClip clip;
	GUIStyle labelStyle, buttonStyle; 
	public Texture2D buttonTexture, hoverButtonTexture, labelTexture;
	GUIStyleState gbs, gs, labels;
	// Use this for initialization
	void Start(){
		labelStyle = new GUIStyle();
		buttonStyle = new GUIStyle();

		labelStyle.fontSize = (int)(Screen.height * .2f);
		buttonStyle.fontSize = (int)(Screen.height * .08f);
		labelStyle.font = buttonStyle.font = thisFont;
		gbs = new GUIStyleState();
		gs = new GUIStyleState();
		labels = new GUIStyleState();
		gs.background = this.buttonTexture;
		gbs.background = this.hoverButtonTexture;
		labels.background = labelTexture;

		labelStyle.alignment = buttonStyle.alignment = TextAnchor.MiddleCenter;
		labelStyle.normal = labels;
		buttonStyle.hover = gbs;
		buttonStyle.normal = gs;

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
		GUIContent labelContent = new GUIContent("Paused");
		GUIContent tutorialOnContent = new GUIContent("Turn Tutorial OFF");
		GUIContent tutorialOffContent = new GUIContent("Turn Tutorial ON");
		GUIContent returnContent = new GUIContent("Return");
		GUIContent mainContent = new GUIContent("Main Menu");
		GUIContent exitContent = new GUIContent("Exit Game");

		if (IsPaused){
			this.BroadcastMessage("PauseGame", true);
			Time.timeScale = 0;
			GUI.Box (new Rect ((Screen.width * .3f),Screen.height * .1f,Screen.width * .4f,Screen.height * .2f), labelContent, labelStyle);
			if (tutorial) {
				if (GUI.Button(new Rect (Screen.width * .4f,Screen.height * .5f,Screen.width * .2f,Screen.height * .1f), tutorialOnContent, buttonStyle)) {
					playing = true;
					tutorial = false;
					PlayerPrefs.SetInt("tutorial", 1);
					foreach (GameObject g in infos) {
						g.SetActive(false);
					}
				}
			} else {
				if (GUI.Button(new Rect (Screen.width * .4f,Screen.height * .5f,Screen.width * .2f,Screen.height * .1f), tutorialOffContent, buttonStyle)) {
					playing = true;
					tutorial = true;
					PlayerPrefs.SetInt("tutorial", 0);
					foreach (GameObject g in infos) {
						g.SetActive(true);
					}
				}
			}

			// Make the Quit button.
			if (GUI.Button (new Rect (Screen.width * .2f, (Screen.height * .9f) ,Screen.width * .2f,Screen.height * .1f), mainContent, buttonStyle)) {
				playing = true;
				Application.LoadLevel("MainMenu");
			}

			if(GUI.Button(new Rect(Screen.width * .4f,Screen.height * .7f,Screen.width * .2f,Screen.height * .1f), returnContent, buttonStyle)){
				playing = true;
				IsPaused = false;
			}
			if (GUI.Button (new Rect (Screen.width * .6f, (Screen.height * .9f),Screen.width * .2f ,Screen.height * .1f), exitContent, buttonStyle)) {
				playing = true;
				Application.Quit();
			}
			

		} else {
			this.BroadcastMessage("PauseGame", false);

			Time.timeScale = 1;
		}
		
	}
}
