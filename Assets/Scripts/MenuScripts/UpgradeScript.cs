using UnityEngine;
using System.Collections;

public class UpgradeScript : MonoBehaviour {
	
	public Texture backgroundTexture;
	public float guiPlacementX1;
	public float guiPlacementY1;
	public float guiPlacementX2;
	public float guiPlacementY2;
	private string[] items;
	private int cash;
	private float suitintegrity;
	void Start() {
		
		items = new string[50];
		if (PlayerPrefsX.GetBool("Items")){
			items = PlayerPrefsX.GetStringArray("Items");
		} else {
			items[0] = "No packages returned this mission";
		}
		
		guiPlacementX1 = .25f;
		guiPlacementY1 = .90f;
		guiPlacementX2 = .25f;
		guiPlacementY2 = .10f;
		
	}
	void OnGUI() {
		GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height),backgroundTexture);
		
		if (GUI.Button(new Rect(Screen.width * guiPlacementX1, Screen.height * guiPlacementY1, Screen.width * .5f, Screen.height * .1f), "Next")) {
			Application.LoadLevel("ReturnTest");	
		}
		
		if (GUI.Button(new Rect(Screen.width * guiPlacementX2, Screen.height * guiPlacementY2, Screen.width * .5f, Screen.height * .1f), "Upgrade Menu")) {
			
		}
		
	}
}
