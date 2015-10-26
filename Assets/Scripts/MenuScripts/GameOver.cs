using UnityEngine;
using System.Collections;

public class GameOver : MonoBehaviour {
	
	public Texture backgroundTexture;

	public float guiPlacementY2;
	public Font fontText;
	private GUIStyle gText, gButton;
	public Texture2D buttonTexture, hoverTexture, labelTexture, bloodTexture;
	void Start() {
		gText = new GUIStyle();
		gButton = new GUIStyle();
		GUIStyleState gbs = new GUIStyleState();
		GUIStyleState gs = new GUIStyleState();
		GUIStyleState gts = new GUIStyleState();
		//gts.background = labelTexture;
		gbs.background = hoverTexture;
		gs.background = buttonTexture;
		gs.textColor = gbs.textColor = Color.grey;
		gts.textColor = Color.red;
		gText.font = fontText;
		gText.normal = gts;
		gText.fontSize = (int)(Screen.height * .35f);
		gText.alignment = TextAnchor.MiddleCenter;
		gButton.alignment = TextAnchor.MiddleCenter;
		gButton.font = fontText;
		gButton.normal = gs;
		gButton.hover = gbs;
		gButton.fontSize = (int)(Screen.height * .1f);
		
	}
	void Update() {
		GameObject.Find("MenuAudio").GetComponent<AudioSource>().volume-=.05f *Time.deltaTime;
	}
	void OnGUI() {
		GUIContent mainContent = new GUIContent("Main Menu");
		GUIContent quitContent = new GUIContent("Quit");
		GUIContent gameOver = new GUIContent("GAME\nOVER");

		GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height),backgroundTexture);
		Vector2 gg = gText.CalcSize(gameOver);
		//GUI.DrawTexture(new Rect((Screen.width / 2) - 1.05f*gg.x*.5f, 0, gg.x*1.05f, gg.y), labelTexture);
		//GUI.DrawTexture(new Rect((Screen.width / 2) - 1.05f*gg.x*.5f, 0, gg.x*1.05f, gg.y), bloodTexture);
		GUI.Box(new Rect((Screen.width / 2) - 1.05f*gg.x*.5f, 0, gg.x*1.05f, gg.y), gameOver, gText);
		if (GUI.Button(new Rect(Screen.width * .1f, (Screen.height * .9f)  , Screen.width * .3f, Screen.height * .1f), mainContent, gButton)) {
			this.GetComponent<AudioSource>().Play();
			Application.LoadLevel("MainMenu");	
		}
		
		
		if (GUI.Button(new Rect(Screen.width * .6f, Screen.height * .9f, Screen.width * .3f, Screen.height * .1f), quitContent, gButton)) {
			this.GetComponent<AudioSource>().Play();

			Application.Quit();
		}
		
	}
}
