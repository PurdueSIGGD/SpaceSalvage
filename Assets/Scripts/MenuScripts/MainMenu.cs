using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour {

	public AudioClip menuSelect;
	public AudioClip menuBack;

	public bool pressing;

	public Texture backgroundTexture;
	public float guiPlacementX1 = .5f;
	public float guiPlacementX2 = .5f;
	public float guiPlacementY1 = .5f;
	public float guiPlacementY2;

	private bool KeyCodeUpp;
	private bool KeyCodeDownp;
	private bool KeyCodeLeftp;
	private bool KeyCodeRightp;

	private bool KeyCodeRetractp;
	private bool KeyCodeExtendp;
	private bool KeyCodeClawp;
	private bool KeyCodeEjectp;
	private bool KeyCodeUsep;


	private string KeyCodeUp;
	private string KeyCodeDown;
	private string KeyCodeLeft;
	private string KeyCodeRight;

	
	private string KeyCodeRetract;
	private string KeyCodeExtend;
	private string KeyCodeClaw;
	private string KeyCodeEject;
	private string KeyCodeUse;



	private string label,backLabel;
	private bool clicked,backClicked;

	private string menu = "Main";

	static private KeyCode[] validKeyCodes;
	private void Init() { //Initialize the key codes, unity must work like this 
		if(validKeyCodes!=null) return;
		validKeyCodes=(KeyCode[])System.Enum.GetValues(typeof(KeyCode));
	}
	void Start() {
		Init();
		label = "Delete Player Progress";
		backLabel = "Back";
		this.transform.position = new Vector3(20.5f, 19.3f, -10);
		guiPlacementX2 = .5f;
		guiPlacementY2 = .5f;
		guiPlacementY1 = .5f;


		if (PlayerPrefs.HasKey("Up")) {
			KeyCodeUp = PlayerPrefs.GetString("Up");
		} else {
			PlayerPrefs.SetString("Up","W");
		}
		if (PlayerPrefs.HasKey("Down")) {
			KeyCodeDown = PlayerPrefs.GetString("Down");
		} else {
			PlayerPrefs.SetString("Down","S");
		}
		if (PlayerPrefs.HasKey("Left")) {
			KeyCodeLeft = PlayerPrefs.GetString("Left");
		} else {
			PlayerPrefs.SetString("Left","D");
		}
		if (PlayerPrefs.HasKey("Right")) {
			KeyCodeRight = PlayerPrefs.GetString("Right");
		} else {
			PlayerPrefs.SetString("Right","A");
		}


		if (PlayerPrefs.HasKey("Retract")) {
			KeyCodeRetract = PlayerPrefs.GetString("Retract");
		} else {
			PlayerPrefs.SetString("Retract","LeftControl");
		}
		if (PlayerPrefs.HasKey("Extend")) {
			KeyCodeExtend = PlayerPrefs.GetString("Extend");
		} else {
			PlayerPrefs.SetString("Extend","LeftShift");
		}
		if (PlayerPrefs.HasKey("Eject")) {
			KeyCodeEject = PlayerPrefs.GetString("Eject");
		} else {
			PlayerPrefs.SetString("Eject","G");
		}

		if (PlayerPrefs.HasKey("Claw")) {
			KeyCodeClaw = PlayerPrefs.GetString("Claw");
		} else {
			PlayerPrefs.SetString("Claw","Mouse1");
		}
		if (PlayerPrefs.HasKey("Use")) {
			KeyCodeUse = PlayerPrefs.GetString("Use");
		} else {
			PlayerPrefs.SetString("Use","F");
		}

	}
	void OnGUI() {
		//GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height),backgroundTexture);
		if (menu == "Main") {
			if (GUI.Button(new Rect(Screen.width * guiPlacementX1, Screen.height * guiPlacementY1, Screen.width * .5f, Screen.height * .1f), "Play Game")) {
				this.GetComponent<AudioSource>().PlayOneShot(this.menuSelect);
				Application.LoadLevel("ProcGen");	
			}


			if (GUI.Button(new Rect(Screen.width * guiPlacementX2, Screen.height * guiPlacementY2, Screen.width * .5f, Screen.height * .1f), "Options")) {
				this.GetComponent<AudioSource>().PlayOneShot(this.menuSelect);

				menu = "Options";
			}


			if (GUI.Button (new Rect ((Screen.width - Screen.width*.5f)/2.0f, Screen.height -Screen.height*.1f, Screen.width * .5f, Screen.height * .1f), "Exit Game")) {
				this.GetComponent<AudioSource>().PlayOneShot(this.menuSelect);

				Application.Quit();
			}


		} 
		if (menu == "Options") {
			//GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height),backgroundTexture);
			if (GUI.Button(new Rect(Screen.width * guiPlacementX1, Screen.height * guiPlacementY1, Screen.width * .5f, Screen.height * .1f), label)) {
				this.GetComponent<AudioSource>().PlayOneShot(this.menuSelect);

				if (!clicked) {
					clicked = true;
					label = "Are you sure? Cannot be undone";
				} else {
					PlayerPrefs.DeleteAll();
					menu = "Main";
					
				}
			}
			
			if (GUI.Button(new Rect(Screen.width * guiPlacementX2, Screen.height * guiPlacementY2, Screen.width * .5f, Screen.height * .1f), "KeyBinding")) {
				////print("Just kidding, no keybindings yet");
				//Application.LoadLevel ("KeyBindings");
				this.GetComponent<AudioSource>().PlayOneShot(this.menuSelect);

				menu = "Keys";
			}
			
			if (GUI.Button (new Rect ((Screen.width - Screen.width*.5f)/2.0f, Screen.height -Screen.height*.1f, Screen.width * .5f, Screen.height * .1f), backLabel)) {
				this.GetComponent<AudioSource>().PlayOneShot(this.menuBack);

				menu =  "Main";
				clicked = false;
				
			}
		}
		if (menu == "Keys") {
			GUI.Label(new Rect (Screen.width * .37f, Screen.height*.1f, Screen.width * .5f, Screen.height * .1f), "Press your button with your desired key!");
			if (GUI.Button(new Rect(Screen.width * .1f, Screen.height * .2f, Screen.width * .2f, Screen.height * .1f), "Up: " + KeyCodeUp)) {
				this.GetComponent<AudioSource>().PlayOneShot(this.menuSelect);

				KeyCodeUpp = true;
				pressing = true;
				KeyCodeUp = "Press any key";
			}
			if (KeyCodeUpp) {
				foreach(KeyCode kcode in validKeyCodes)
				{
					if (Input.GetKey(kcode)) {
						pressing = false;
						KeyCodeUpp = false;
						KeyCodeUp = kcode.ToString();
						//print(kcode.ToString());
					}
						
				}
			}
			if (GUI.Button(new Rect(Screen.width * .1f, Screen.height * .4f, Screen.width * .2f, Screen.height * .1f), "Down: " + KeyCodeDown)) {
				this.GetComponent<AudioSource>().PlayOneShot(this.menuSelect);

				KeyCodeDownp = true;
				pressing = true;
				KeyCodeDown = "Press any key";
			}
			if (KeyCodeDownp) {
				foreach(KeyCode kcode in validKeyCodes)
				{
					if (Input.GetKey(kcode)) {
						KeyCodeDownp = false;
						KeyCodeDown = kcode.ToString();
						pressing = false;
						//print(kcode.ToString());
					}
					
				}
			}
			if (GUI.Button(new Rect(Screen.width * .1f, Screen.height * .6f, Screen.width * .2f, Screen.height * .1f), "Left: " + KeyCodeLeft)) {
				this.GetComponent<AudioSource>().PlayOneShot(this.menuSelect);

				KeyCodeLeftp = true;
				pressing = true;
				KeyCodeLeft = "Press any key";

			}
			if (KeyCodeLeftp) {
				foreach(KeyCode kcode in validKeyCodes)
				{
					if (Input.GetKey(kcode)) {
						pressing = false;
						KeyCodeLeftp = false;
						KeyCodeLeft = kcode.ToString();
						//print(kcode.ToString());
					}
					
				}
			}
			if (GUI.Button(new Rect(Screen.width * .1f, Screen.height * .8f, Screen.width * .2f, Screen.height * .1f), "Right: " + KeyCodeRight)) {
				this.GetComponent<AudioSource>().PlayOneShot(this.menuSelect);

				KeyCodeRightp = true;
				pressing = true;
				KeyCodeRight = "Press any key";

			}
			if (KeyCodeRightp) {
				foreach(KeyCode kcode in validKeyCodes)
				{
					if (Input.GetKey(kcode)) {
						KeyCodeRightp = false;
						pressing = false;
						KeyCodeRight = kcode.ToString();
						//print(kcode.ToString());
					}
					
				}
			}
			if (GUI.Button(new Rect(Screen.width * .4f, Screen.height * .2f, Screen.width * .2f, Screen.height * .1f), "Retract: " + KeyCodeRetract)) {
				this.GetComponent<AudioSource>().PlayOneShot(this.menuSelect);
				KeyCodeRetractp = true;
				pressing = true;
				KeyCodeRetract = "Press any key";
				
			}
			if (KeyCodeRetractp) {
				foreach(KeyCode kcode in validKeyCodes)
				{
					if (Input.GetKey(kcode)) {
						pressing = false;
						KeyCodeRetractp = false;
						KeyCodeRetract = kcode.ToString();
						//print(kcode.ToString());
					}
					
				}
			}
			if (GUI.Button(new Rect(Screen.width * .4f, Screen.height * .4f, Screen.width * .2f, Screen.height * .1f), "Extend: " + KeyCodeExtend)) {
				this.GetComponent<AudioSource>().PlayOneShot(this.menuSelect);
				KeyCodeExtendp = true;
				pressing = true;
				KeyCodeExtend = "Press any key";
				
			}
			if (KeyCodeExtendp) {
				foreach(KeyCode kcode in validKeyCodes)
				{
					if (Input.GetKey(kcode)) {
						pressing = false;
						KeyCodeExtendp = false;
						KeyCodeExtend = kcode.ToString();
						//print(kcode.ToString());
					}
					
				}
			}
			if (GUI.Button(new Rect(Screen.width * .4f, Screen.height * .6f, Screen.width * .2f, Screen.height * .1f), "Claw: " + KeyCodeClaw)) {
				this.GetComponent<AudioSource>().PlayOneShot(this.menuSelect);
				KeyCodeClawp = true;
				KeyCodeClaw = "Press any key";
				pressing = true;
				
			}
			if (KeyCodeClawp) {
				foreach(KeyCode kcode in validKeyCodes)
				{
					if (Input.GetKey(kcode)) {
						pressing = false;
						KeyCodeClawp = false;
						KeyCodeClaw = kcode.ToString();
						//print(kcode.ToString());
					}
					
				}
			}
			if (GUI.Button(new Rect(Screen.width * .4f, Screen.height * .8f, Screen.width * .2f, Screen.height * .1f), "Eject: " + KeyCodeEject)) {
				this.GetComponent<AudioSource>().PlayOneShot(this.menuSelect);
				KeyCodeEjectp = true;
				pressing = true;
				KeyCodeEject = "Press any key";
				
			}
			if (KeyCodeEjectp) {
				foreach(KeyCode kcode in validKeyCodes)
				{
					if (Input.GetKey(kcode)) {
						KeyCodeEjectp = false;
						pressing = false;
						KeyCodeEject = kcode.ToString();
						//print(kcode.ToString());
					}
					
				}
			}

			if (GUI.Button(new Rect(Screen.width * .7f, Screen.height * .2f, Screen.width * .2f, Screen.height * .1f), "Use: " + KeyCodeUse)) {
				this.GetComponent<AudioSource>().PlayOneShot(this.menuSelect);
				KeyCodeUsep = true;
				pressing = true;
				KeyCodeUse = "Press any key";
				
			}
			if (KeyCodeUsep) {
				foreach(KeyCode kcode in validKeyCodes)
				{
					if (Input.GetKey(kcode)) {
						pressing = false;
						KeyCodeUsep = false;
						KeyCodeUse = kcode.ToString();
						//print(kcode.ToString());
					}
					
				}
			
			}
			if (GUI.Button(new Rect(Screen.width * .7f, Screen.height * .4f, Screen.width * .2f, Screen.height * .1f), "[NOT USED]")) {
				this.GetComponent<AudioSource>().PlayOneShot(this.menuSelect);
				//print(Input.inputString);
			}
			if (GUI.Button(new Rect(Screen.width * .7f, Screen.height * .6f, Screen.width * .2f, Screen.height * .1f), "[NOT USED]")) {
				this.GetComponent<AudioSource>().PlayOneShot(this.menuSelect);
				//print(Input.inputString);
			}
			if (GUI.Button(new Rect(Screen.width * .7f, Screen.height * .8f, Screen.width * .2f, Screen.height * .1f), "[NOT USED]")) {
				this.GetComponent<AudioSource>().PlayOneShot(this.menuSelect);
				//print(Input.inputString);
			}

			if (!pressing && GUI.Button (new Rect ((Screen.width - Screen.width*.5f)/2.0f, Screen.height -Screen.height*.1f, Screen.width * .25f, Screen.height * .1f), "Save")) {
				this.GetComponent<AudioSource>().PlayOneShot(this.menuSelect);
				PlayerPrefs.SetString ("Up",this.KeyCodeUp);
				PlayerPrefs.SetString ("Down",this.KeyCodeDown);
				PlayerPrefs.SetString ("Left",this.KeyCodeLeft);
				PlayerPrefs.SetString ("Right",this.KeyCodeRight);
				PlayerPrefs.SetString ("Extend",this.KeyCodeExtend);
				PlayerPrefs.SetString ("Retract",this.KeyCodeRetract);
				PlayerPrefs.SetString ("Eject",this.KeyCodeEject);
				PlayerPrefs.SetString ("Claw",this.KeyCodeClaw);
				PlayerPrefs.SetString ("Use",this.KeyCodeUse);

				
			}
			if (!pressing && GUI.Button (new Rect (Screen.width/2, Screen.height -Screen.height*.1f, Screen.width * .25f, Screen.height * .1f), backLabel)) {
				
				this.GetComponent<AudioSource>().PlayOneShot(this.menuBack);
				menu =  "Options";
				clicked = false;
				
			}

		}

	}
}
