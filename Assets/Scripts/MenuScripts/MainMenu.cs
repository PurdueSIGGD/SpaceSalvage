using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class MainMenu : MonoBehaviour {

	public AudioClip menuSelect;
	public AudioClip menuBack;
	public Font thisFont;
	public Texture2D pointer;

	public bool pressing, loading;
	private float time;
	public Texture backgroundTexture, mainLogo;
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

	private GUIStyle traverseButton, mainButton, keybindingButton, labelStyle, sliderStyle;
	private GUIStyleState gs, gbs;

	private string label,backLabel;
	private bool clicked,backClicked;
	public Texture2D buttonTexture, hoverButtonTexture, labelTexture, smallLabel;

	private string menu = "Main";

	static private KeyCode[] validKeyCodes;
	private void Init() { //Initialize the key codes, unity must work like this 
		if(validKeyCodes!=null) return;
		validKeyCodes=(KeyCode[])System.Enum.GetValues(typeof(KeyCode));
	}
	void Start() {
		//Cursor.SetCursor(pointer, Vector2.zero,CursorMode.Auto);
		Time.timeScale = 1;
		Init();
		label = "Reset\nProgress";
		backLabel = "Back";
		//this.transform.position = new Vector3(20.5f, 19.3f, -10);
		guiPlacementX2 = .5f;
		guiPlacementY2 = .5f;
		guiPlacementY1 = .5f;
		traverseButton = new GUIStyle();		
		mainButton = new GUIStyle();
		keybindingButton = new GUIStyle();
		labelStyle = new GUIStyle();
		sliderStyle = new GUIStyle();
		sliderStyle.font = labelStyle.font = traverseButton.font = mainButton.font = keybindingButton.font = labelStyle.font = thisFont;
		labelStyle.fontSize = (int)(Screen.height * .05f);
		traverseButton.fontSize = (int)(Screen.height * .07f);
		mainButton.fontSize = (int)(Screen.height * .07f);
		keybindingButton.fontSize = (int)(Screen.height * .05f);
		sliderStyle.fontSize = mainButton.fontSize;
		sliderStyle.alignment = TextAnchor.UpperCenter;
		traverseButton.alignment = TextAnchor.MiddleCenter;
		mainButton.alignment = TextAnchor.MiddleCenter;
		keybindingButton.alignment = TextAnchor.MiddleCenter;
		labelStyle.alignment = TextAnchor.MiddleCenter;

		gbs = new GUIStyleState();
		gs = new GUIStyleState();
		GUIStyleState labely = new GUIStyleState();
		gbs.textColor = Color.grey;
		gs.textColor = Color.grey;
		labely.background = this.labelTexture;
		gs.background = this.buttonTexture;
		gbs.background = this.hoverButtonTexture;
		sliderStyle.normal = labelStyle.normal = labely;
		keybindingButton.normal = traverseButton.normal = mainButton.normal = keybindingButton.normal = gs;
		keybindingButton.hover = traverseButton.hover = mainButton.hover = keybindingButton.hover = gbs;


		float currentAudioVal = 1;
		if (PlayerPrefs.HasKey("audioVol")) {
			currentAudioVal = PlayerPrefs.GetFloat("audioVol");			
			print(currentAudioVal);
			
		} else {
			PlayerPrefs.SetFloat("audioVol", currentAudioVal);
		}
		GameObject.Find("MenuMusic").GetComponent<AudioSource>().volume = currentAudioVal;
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
	void Update() {
		this.transform.rotation = Quaternion.Euler(0,0,0);
		if (loading) {

			time+= Time.deltaTime;
			if (time > 0) {
				Application.LoadLevel("ProcGen");
			}
		}

	}
	void OnGUI() {
		//GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height),backgroundTexture);
		if (loading) {
			GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height),backgroundTexture);
		}
		if (menu == "Main" && !loading) {
			GUIContent playContent = new GUIContent("Play Game");
			GUIContent optionsContent = new GUIContent("Options");
			GUIContent gameContent = new GUIContent("Exit Game");

		

			if (GUI.Button(new Rect(Screen.width * .1f, Screen.height * .65f, Screen.width * .3f, Screen.height * .2f), playContent, mainButton)) {
				this.GetComponent<AudioSource>().PlayOneShot(this.menuSelect);

				//this.transform.FindChild("Background").GetComponent<GUITexture>().enabled = true;
				loading = true;
			}


			if (GUI.Button(new Rect(Screen.width * .6f, Screen.height * .65f, Screen.width * .3f, Screen.height * .2f), optionsContent, mainButton)) {
				this.GetComponent<AudioSource>().PlayOneShot(this.menuSelect);

				menu = "Options";
			}


			if (GUI.Button (new Rect ((Screen.width - Screen.width*.5f)/2.0f, Screen.height -Screen.height*.1f, Screen.width * .5f, Screen.height * .1f), gameContent, traverseButton)) {
				this.GetComponent<AudioSource>().PlayOneShot(this.menuSelect);

				Application.Quit();
			}


		} 
		if (menu == "Options") {
			GUIContent aboutContent = new GUIContent("About");
			if (GUI.Button(new Rect(Screen.width * .85f, Screen.height * .1f, Screen.width * .15f, Screen.height * .1f),aboutContent, mainButton)) {
				menu = "about";
			}
			GUIContent deleteProgressContent = new GUIContent(label);
			GUIContent keyContent = new GUIContent("KeyBinding");
			GUIContent gameContent = new GUIContent(backLabel);
			GUIContent audioContent = new GUIContent("Music\nVolume");
			//GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height),backgroundTexture);
			float currentAudioVal = 1;
			if (PlayerPrefs.HasKey("audioVol")) {
				currentAudioVal = PlayerPrefs.GetFloat("audioVol");			
//				print(currentAudioVal);

			} else {
				PlayerPrefs.SetFloat("audioVol", currentAudioVal);
			}
			float audioVal = 1;
			GUI.Label(new Rect(Screen.width * .4f, Screen.height * .65f, Screen.width * .2f, Screen.height * .2f), audioContent, sliderStyle);
			audioVal = GUI.HorizontalSlider(new Rect(Screen.width * .45f, Screen.height * .80f, Screen.width * .1f, Screen.height * .05f),currentAudioVal,0f,1f);
			PlayerPrefs.SetFloat("audioVol", audioVal);
			GameObject.Find("MenuMusic").GetComponent<AudioSource>().volume = audioVal;
			if (GUI.Button(new Rect(Screen.width * .1f, Screen.height * .65f, Screen.width * .25f, Screen.height * .2f), deleteProgressContent, mainButton)) {
				this.GetComponent<AudioSource>().PlayOneShot(this.menuSelect);

				if (!clicked) {
					clicked = true;
					label = "Are you sure? \nCannot be \nundone";
				} else {
					GameObject.Find("MenuMusic").GetComponent<AudioSource>().volume = 1;
					PlayerPrefs.DeleteAll();
					menu = "Main";
					
				}
			}
			
			if (GUI.Button(new Rect(Screen.width * .65f, Screen.height * .65f, Screen.width * .25f, Screen.height * .2f), keyContent, mainButton)) {
				////print("Just kidding, no keybindings yet");
				//Application.LoadLevel ("KeyBindings");
				this.GetComponent<AudioSource>().PlayOneShot(this.menuSelect);

				menu = "Keys";
			}
			
			if (GUI.Button (new Rect ((Screen.width - Screen.width*.5f)/2.0f, Screen.height -Screen.height*.1f, Screen.width * .5f, Screen.height * .1f), gameContent, traverseButton)) {
				this.GetComponent<AudioSource>().PlayOneShot(this.menuBack);

				menu =  "Main";
				clicked = false;
				
			}
		}
		if (menu == "Keys") {
			GUIContent labelContent = new GUIContent("Press your button with your desired key!");
			GUIContent keyContent = new GUIContent("KeyBinding");
			GUIContent backContent = new GUIContent(backLabel); 
			GUIContent saveContent = new GUIContent("Save"); 

			GUIContent upContent = new GUIContent("Up: " + KeyCodeUp); 
			GUIContent downContent = new GUIContent("Down: " + KeyCodeDown); 
			GUIContent leftContent = new GUIContent("Right: " + KeyCodeLeft); 
			GUIContent rightContent = new GUIContent("Left: " + KeyCodeRight); 
			GUIContent retractContent = new GUIContent("Cable-: " + KeyCodeRetract); 
			GUIContent extendContent = new GUIContent("Cable+: " + KeyCodeExtend); 
			GUIContent clawContent = new GUIContent("Claw: " + KeyCodeClaw); 
			GUIContent ejectContent = new GUIContent("Eject: " + KeyCodeEject); 
			GUIContent useContent = new GUIContent("Use: " + KeyCodeUse); 



			labelStyle.normal.background = smallLabel;
			labelStyle.hover.background = smallLabel;
			GUI.Label(new Rect (Screen.width * .15f, Screen.height*.05f, Screen.width * .7f, Screen.height * .1f), labelContent, labelStyle);
			labelStyle.normal.background = this.labelTexture;
			labelStyle.hover.background = labelTexture;
			if (GUI.Button(new Rect(Screen.width * .1f, Screen.height * .2f, Screen.width * .25f, Screen.height * .1f), upContent, keybindingButton)) {
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
			if (GUI.Button(new Rect(Screen.width * .1f, Screen.height * .4f, Screen.width * .25f, Screen.height * .1f), downContent, keybindingButton)) {
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
			if (GUI.Button(new Rect(Screen.width * .1f, Screen.height * .6f, Screen.width * .25f, Screen.height * .1f), leftContent, keybindingButton)) {
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
			if (GUI.Button(new Rect(Screen.width * .1f, Screen.height * .8f, Screen.width * .25f, Screen.height * .1f),rightContent, keybindingButton)) {
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
			if (GUI.Button(new Rect(Screen.width * .4f, Screen.height * .2f, Screen.width * .25f, Screen.height * .1f), rightContent, keybindingButton)) {
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
			if (GUI.Button(new Rect(Screen.width * .4f, Screen.height * .4f, Screen.width * .25f, Screen.height * .1f), extendContent, keybindingButton)) {
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
			if (GUI.Button(new Rect(Screen.width * .4f, Screen.height * .6f, Screen.width * .25f, Screen.height * .1f), retractContent, keybindingButton)) {
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
			if (GUI.Button(new Rect(Screen.width * .4f, Screen.height * .8f, Screen.width * .25f, Screen.height * .1f), ejectContent, keybindingButton)) {
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

			if (GUI.Button(new Rect(Screen.width * .7f, Screen.height * .2f, Screen.width * .25f, Screen.height * .1f), useContent, keybindingButton)) {
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
		/*	if (GUI.Button(new Rect(Screen.width * .7f, Screen.height * .4f, Screen.width * .2f, Screen.height * .1f), "[NOT USED]")) {
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
			}*/

			if (!pressing && GUI.Button (new Rect ((Screen.width - Screen.width*.5f)/2.0f, Screen.height -Screen.height*.05f, Screen.width * .25f, Screen.height * .05f), saveContent, keybindingButton)) {
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
			if (!pressing && GUI.Button (new Rect (Screen.width/2, Screen.height - Screen.height * .05f, Screen.width * .25f, Screen.height * .05f), backContent, keybindingButton)) {
				
				this.GetComponent<AudioSource>().PlayOneShot(this.menuBack);
				menu =  "Options";
				clicked = false;
				
			}


		} else if (menu == "about") {
			GUIContent labelContent = new GUIContent("This game was the project for Purdue's SIGGD,\n an ACM special interest group in game development.\n " +
			                                         "Beginning in the spring of 2015,\n this game has had ongoing\n development with several members,\n and will be continued to be updated.");
			GUI.Label(new Rect (Screen.width * .1f, Screen.height*.05f, Screen.width * .8f, Screen.height * .8f), labelContent, labelStyle);
			GUIContent backContent = new GUIContent("Back");
			if (!pressing && GUI.Button (new Rect (Screen.width/2 - Screen.width * .5f * .25f, Screen.height - Screen.height * .05f, Screen.width * .25f, Screen.height * .05f), backContent, mainButton)) {
				
				this.GetComponent<AudioSource>().PlayOneShot(this.menuBack);
				menu =  "Options";
				clicked = false;
				
			}
		} else {
			if (!loading) GUI.DrawTexture(new Rect(Screen.width * .1f, Screen.height * .1f, Screen.width * .8f, Screen.height * .5f), mainLogo);
		}

	}
}
