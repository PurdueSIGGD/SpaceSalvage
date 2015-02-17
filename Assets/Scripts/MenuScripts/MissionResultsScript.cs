using UnityEngine;
using System.Collections;

public class MissionResultsScript : MonoBehaviour {
	
	public Texture backgroundTexture;
	public float guiPlacementX1;
	public float guiPlacementY1;
	private float allPlacementX;
	private float healthPlacementY;
	private float walletPlacementY;
	private string[] items;
	private int cash;
	private float suitintegrity;
	private int wallet, collected, started;

	void Start() {
		//PlayerPrefs.SetInt ("wallet", 0);
		//PlayerPrefs.SetInt ("startingwallet", 0);

		wallet = PlayerPrefs.GetInt ("wallet");
		started = PlayerPrefs.GetInt ("startingwallet");
		//print ("wallet: " + wallet);
		//print ("started: " + started);
		collected = wallet - started;
		allPlacementX = .30f;
		healthPlacementY = .05f;
		walletPlacementY = .15f;
		items = new string[50];
		if (PlayerPrefsX.GetStringArray("Items")[0] != null){
			items = PlayerPrefsX.GetStringArray("Items");
		} else {
			items[0] = "No packages returned this mission";
		}
		for (int i = 0; i < items.Length; i++) {
			int cost = 0;
			if (items[i].Equals("Cash Safe") ){
				cost = 100;
			} else {
				if (items[i].Equals("Medical Supplies")) {
					cost = 50;
				} else {
					if (items[i].Equals("Food")) {
						cost = 25;
					} else {
						break;
					}
				}
			}
			wallet += cost;
			//GUI.Box (new Rect (Screen.width * allPlacementX, Screen.height * (.45f + (i * .1f)), Screen.width * .40f, Screen.height * .08f), items[i] + " for $" + cost);
			
		}
		print (wallet);

		guiPlacementX1 = .25f;
		guiPlacementY1 = .90f;
		/*if (items[i] == null) {
			items[i].Equals("Random Space Junk");
		}*/
		
	}
	void OnGUI() {
		GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height),backgroundTexture);
		
		if (GUI.Button(new Rect(Screen.width * guiPlacementX1, Screen.height * guiPlacementY1, Screen.width * .5f, Screen.height * .1f), "Next")) {
			for (int i = 0; i < items.Length; i++) {
				items[i] = "";
			}
			PlayerPrefsX.SetStringArray("Items", items);
			PlayerPrefs.SetInt ("wallet", wallet);
			Application.LoadLevel("UpgradeMenu");	
		}
		string value = ((PlayerPrefs.GetFloat ("health") <= 100) ? "" + (PlayerPrefs.GetFloat ("health") - PlayerPrefs.GetFloat ("health") % .01) : "100, Armor: " + ((PlayerPrefs.GetFloat ("health") - PlayerPrefs.GetFloat ("health") % .01) - 100));
		GUI.Box (new Rect (Screen.width * allPlacementX, Screen.height * healthPlacementY, Screen.width * .40f, Screen.height * .08f), ("Health left: "  +  value));

		GUI.Box (new Rect (Screen.width * allPlacementX, Screen.height * walletPlacementY, Screen.width * .40f, Screen.height * .08f), "Starting Cash: $ " + PlayerPrefs.GetInt ("startingwallet"));
		GUI.Box (new Rect (Screen.width * allPlacementX, Screen.height * .25f, Screen.width * .40f, Screen.height * .08f), "Collected Cash: $ " + collected);
		GUI.Box (new Rect (Screen.width * .2f, Screen.height * .35f, Screen.width * .60f, Screen.height * .08f), "Collected Items:");
		for (int i = 0; i < items.Length; i++) {
			int cost = 0;
			if (items[i].Equals("Cash Safe") ){
				cost = 100;
			} else {
				if (items[i].Equals("Medical Supplies")) {
					cost = 50;
				} else {
					if (items[i].Equals("Food")) {
						cost = 25;
					} else {
						break;
					}
				}
			}

			GUI.Box (new Rect (Screen.width * allPlacementX, Screen.height * (.45f + (i * .1f)), Screen.width * .40f, Screen.height * .08f), items[i] + " for $" + cost);
			
		}
		GUI.Box (new Rect (Screen.width * .2f, Screen.height * .80f, Screen.width * .60f, Screen.height * .08f), "Total Cash: $" + ( wallet));

		/*if (GUI.Button(new Rect(Screen.width * guiPlacementX2, Screen.height * guiPlacementY2, Screen.width * .5f, Screen.height * .1f), "Controls")) {
			
		}*/
		
	}
}
