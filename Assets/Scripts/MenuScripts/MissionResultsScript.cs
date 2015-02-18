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
	private int healthitems, cashitems, fooditems;

	void Start() {
		healthitems = 0;
		cashitems = 0;
		fooditems = 0;
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
				cashitems++;
			} else {
				if (items[i].Equals("Medical Supplies")) {
					cost = 50;
					healthitems++;
				} else {
					if (items[i].Equals("Food")) {
						fooditems++;
						cost = 25;
					} else {
						break;
					}
				}
			}
			wallet += cost;
			//GUI.Box (new Rect (Screen.width * allPlacementX, Screen.height * (.45f + (i * .1f)), Screen.width * .40f, Screen.height * .08f), items[i] + " for $" + cost);
			
		}
		//print (wallet);

		guiPlacementX1 = .25f;
		guiPlacementY1 = .90f;
		/*if (items[i] == null) {
			items[i].Equals("Random Space Junk");
		}*/
		
	}
	void OnGUI() {
		GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height),backgroundTexture);
		//print (healthitems);
		if (GUI.Button(new Rect(Screen.width * guiPlacementX1, Screen.height * guiPlacementY1, Screen.width * .5f, Screen.height * .1f), "Next")) {
			for (int i = 0; i < items.Length; i++) {
				items[i] = "";
			}
			PlayerPrefsX.SetStringArray("Items", items);
			PlayerPrefs.SetInt ("wallet", wallet);
			Application.LoadLevel("UpgradeMenu");	
		} 
		float healththing = (Mathf.Round(PlayerPrefs.GetFloat ("health") * 100f) / 100f); //round that stuff
		string value = ((healththing <= 100) ? ("" + healththing) : ("100, Armor: " + (healththing - 100)));
		GUI.Box (new Rect (Screen.width * allPlacementX, Screen.height * healthPlacementY, Screen.width * .40f, Screen.height * .08f), ("Health left: "  +  value));

		GUI.Box (new Rect (Screen.width * allPlacementX, Screen.height * walletPlacementY, Screen.width * .40f, Screen.height * .08f), "Starting Cash: $ " + PlayerPrefs.GetInt ("startingwallet"));
		GUI.Box (new Rect (Screen.width * allPlacementX, Screen.height * .25f, Screen.width * .40f, Screen.height * .08f), "Collected Cash: $ " + collected);
		GUI.Box (new Rect (Screen.width * .2f, Screen.height * .35f, Screen.width * .60f, Screen.height * .08f), "Collected Items:");

		int cost;
		int f = 0;
		string s = "";
		if (cashitems > 0) {
			if (cashitems > 1) s = "s"; else s = "";
			cost = 100;
			GUI.Box (new Rect (Screen.width * allPlacementX, Screen.height * (.45f + (f * .1f)), Screen.width * .40f, Screen.height * .08f), cashitems + " Cash Safe" + s + " for $" + cost + " each");
			f++;
		}
		if (healthitems > 0) {
			if (healthitems > 1) s = "s"; else s = "";
			cost = 50;
			GUI.Box (new Rect (Screen.width * allPlacementX, Screen.height * (.45f + (f * .1f)), Screen.width * .40f, Screen.height * .08f), healthitems +" crate" + s + " of Medical Supplies" + " for $" + cost + " each");
			f++;
		}
		if (fooditems > 0) {
			if (fooditems > 1) s = "s"; else s = "";
			cost = 25;
			GUI.Box (new Rect (Screen.width * allPlacementX, Screen.height * (.45f + (f * .1f)), Screen.width * .40f, Screen.height * .08f), fooditems + " Food Crate" + s + " for $" + cost + " each");
			f++;
		}
		
		GUI.Box (new Rect (Screen.width * .2f, Screen.height * .80f, Screen.width * .60f, Screen.height * .08f), "Total Cash: $" + ( wallet));

		/*if (GUI.Button(new Rect(Screen.width * guiPlacementX2, Screen.height * guiPlacementY2, Screen.width * .5f, Screen.height * .1f), "Controls")) {
			
		}*/
		
	}
}
