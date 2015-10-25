using UnityEngine;
using System.Collections;

public class MissionResultsScript : MonoBehaviour {
	private float status; //0 == results, 1 == upgrade

	public AudioClip[] coinSounds;
	public AudioClip buttonSound;
	public Texture backgroundTexture1, backgroundTexture2, loadingTexture;
	public Texture2D labelTexture;
	public Texture2D buttonTexture, hoverButtonTexture;
	public float guiPlacementX1;
	public float guiPlacementY1;
	private float allPlacementX;
	private float healthPlacementY;
	private float walletPlacementY;
	public Font thisFont;


	public float guiPlacementX2;
	public float guiPlacementY2;
	private string[] items;
	private int cash;
	private float suitintegrity;
	private int wallet, collected, started;
	private int healthitems, cashitems, fooditems;
	private float startingoxy;
	private float thrustermoverate;
	private float emprechargetime;
	private int tubesleft;
	private int tubecut;
	private float armor;
	private float cranemovespeed;
	private float cranelength;
	private float startingstartingoxy;
	private float startingthrustermoverate;
	private float startingemprechargetime;
	private int startingtubesleft;
	private float startingarmor;
	private float startingcranelength;
	private int capacity;
	private int startingcapacity;

	private GUIStyle h1, h2, t1, t2, t3, button, cashStyle, fixTubes, fixCranes; //headers, text

	private GUIStyleState gbs;
	private GUIStyleState gs;
	private GUIStyleState labels;

	void Start() {
		float audioVol = 1;
		if (PlayerPrefs.HasKey("audioVol")) {
			audioVol = PlayerPrefs.GetFloat("audioVol");
		} else {
			PlayerPrefs.SetFloat("audioVol", audioVol);
		}
		GameObject.Find("MenuMusic").GetComponent<AudioSource>().volume = audioVol;

		h1 = new GUIStyle();
		h2 = new GUIStyle();
		t1 = new GUIStyle();
		t2 = new GUIStyle();
		t3 = new GUIStyle();
		cashStyle = new GUIStyle();
		button = new GUIStyle();
		h1.stretchHeight = true;
		h2.stretchHeight = true;

		h1.fontSize = (int)(Screen.height * .09f);
		h2.fontSize = (int)(Screen.height * .08f);
		t1.fontSize = (int)(Screen.height * .03f);
		t2.fontSize = (int)(Screen.height * .02f);
		t3.fontSize = (int)(Screen.height * .035f);
		button.fontSize = (int)(Screen.height * .1f);
		cashStyle.fontSize = (int)(Screen.height * .1f);
		h1.font = h2.font = t1.font = t2.font = t3.font = button.font = cashStyle.font = thisFont;
		gbs = new GUIStyleState();
		gs = new GUIStyleState();
		labels = new GUIStyleState();
		gbs.textColor = gs.textColor = new Color(.6f, .6f, .6f);
		gs.background = this.buttonTexture;
		gbs.background = this.hoverButtonTexture;
		labels.background = labelTexture;
		button.hover = gbs;
		button.normal = gs;
		cashStyle.normal = labels;
		h1.normal = labels;
		h2.normal = labels;
		t1.normal = labels;
		t1.normal = labels;
		t2.normal = labels;
		t3.normal = labels;
		t2.alignment = h1.alignment = h2.alignment = cashStyle.alignment = t3.alignment = t1.alignment = TextAnchor.MiddleCenter;
		cashStyle.alignment = TextAnchor.MiddleCenter;
		//t1.contentOffset= new Vector2(7.7f, 7f);
		//cashStyle.contentOffset= new Vector2(0, 7f);
		t1.hover = gbs;
		fixTubes = new GUIStyle();
		fixTubes.fontSize = (int)(Screen.height * .03f);
		GUIStyleState fixTubesStyle = new GUIStyleState();
		fixTubesStyle.textColor = new Color(.6f, .6f, .6f);
		fixTubesStyle.background = buttonTexture;
		fixTubes.normal = fixTubesStyle;
		fixTubes.alignment = TextAnchor.MiddleCenter;
		fixTubes.hover = gbs;
		fixTubes.font = thisFont;

		fixCranes = new GUIStyle();
		fixCranes.fontSize = (int)(Screen.height * .03f);
		GUIStyleState fisCranesStyle = new GUIStyleState();
		fisCranesStyle.textColor = new Color(.6f, .6f, .6f);
		fisCranesStyle.background = buttonTexture;
		fixCranes.normal = fisCranesStyle;
		fixCranes.alignment = TextAnchor.MiddleCenter;
		fixCranes.hover = gbs;
		fixCranes.font = thisFont;


	

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
		if (((string[])PlayerPrefsX.GetStringArray("Items")).Length != 0){
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
		guiPlacementY1 = .9f;
		/*if (items[i] == null) {
			items[i].Equals("Random Space Junk");
		}*/

		capacity = PlayerPrefs.GetInt("capacity");
		suitintegrity = PlayerPrefs.GetFloat("health");
		startingoxy = PlayerPrefs.GetFloat("startingoxy");
		thrustermoverate = PlayerPrefs.GetFloat("moverate");
		emprechargetime = PlayerPrefs.GetFloat("emprechargetime");
		tubesleft = PlayerPrefs.GetInt("tubesleft");
		tubecut = PlayerPrefs.GetInt("tubecut");
		cranemovespeed = PlayerPrefs.GetFloat("movespeed");
		cranelength = PlayerPrefs.GetFloat("cranelength");
		startingcapacity = capacity;
		startingstartingoxy = startingoxy;
		startingthrustermoverate = thrustermoverate;
		startingemprechargetime = emprechargetime;
		startingtubesleft = tubesleft;
		startingcranelength = cranelength;
		
		if (suitintegrity > 100) {
			armor = suitintegrity - 100;
			suitintegrity= 100;
		} else {
			armor = 0;
		}
		startingarmor = armor;
		
	}
	void Update() {
	
	}
	void OnGUI() {
		GUI.skin.label.alignment = TextAnchor.MiddleCenter;

		if (status == 0) {
			float healththing = (Mathf.Round(PlayerPrefs.GetFloat ("health") * 100f) / 100f); //round that stuff
			string value = ((healththing <= 100) ? ("" + healththing) : ("100, Armor: " + (healththing - 100)));
			GUIContent buttonContent = new GUIContent("Next");
			GUIContent integrityContent = new GUIContent("Suit left: "  +  value);
			GUIContent cashContent = new GUIContent("Starting Cash: $ " + PlayerPrefs.GetInt ("startingwallet"));
			GUIContent collectedCashContent = new GUIContent("Collected Cash: $ " + collected);
			GUIContent collectedItemContent = new GUIContent("Collected Items:");
			GUIContent itemContent;
			GUIContent expensesContent = new GUIContent("Travel Expenses: $20");
			GUIContent totalCashContent = new GUIContent("Total Cash: $" + ( wallet - 20));
			float offset;
			GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height),backgroundTexture1);
			//print (healthitems);
			offset = .5f *1.05f *  button.CalcSize(buttonContent).x;
			if (GUI.Button(new Rect(Screen.width * .5f - offset, Screen.height * .9f, 1.05f * button.CalcSize(buttonContent).x,button.CalcSize(buttonContent).y),buttonContent, button)) {
				this.GetComponent<AudioSource>().PlayOneShot(buttonSound);
				for (int i = 0; i < items.Length; i++) {
					items[i] = "";
				}
				PlayerPrefsX.SetStringArray("Items", items);
				//PlayerPrefs.SetInt ("wallet", wallet - 20);
				cash = wallet - 20;
				status = 1;
			}
			offset = .5f * 1.05f * h2.CalcSize(integrityContent).x;
			GUI.Box (new Rect (Screen.width * .5f - offset, Screen.height * healthPlacementY, 1.05f * h2.CalcSize(integrityContent).x,h2.CalcSize(integrityContent).y),integrityContent, this.h2);
			offset = .5f *1.05f *  h2.CalcSize(cashContent).x;
			GUI.Box (new Rect (Screen.width * .5f - offset, Screen.height * walletPlacementY, 1.05f * h2.CalcSize(cashContent).x,h2.CalcSize(cashContent).y),cashContent,this.h2);
			offset = .5f *1.05f *  h2.CalcSize(collectedCashContent).x;
			GUI.Box (new Rect (Screen.width * .5f - offset, Screen.height * .25f, 1.05f * h2.CalcSize(collectedCashContent).x,h2.CalcSize(collectedCashContent).y),collectedCashContent,this.h2);
			offset = .5f *1.05f *  h2.CalcSize(collectedItemContent).x;
			GUI.Box (new Rect (Screen.width * .5f - offset, Screen.height * .35f, 1.05f * h2.CalcSize(collectedItemContent).x,h2.CalcSize(collectedItemContent).y),collectedItemContent,this.h2);

			int cost;
			int f = 0;
			string s = "";

			if (cashitems > 0) {
				if (cashitems > 1) s = "s"; else s = "";
				cost = 100;
				itemContent = new GUIContent(cashitems + " Cash Safe" + s + " for $" + cost + " each");
				offset = .5f * 1.05f * t3.CalcSize(itemContent).x;
				GUI.Box (new Rect (Screen.width * .5f - offset, Screen.height * (.45f + (f * .1f)), 1.05f * t3.CalcSize(itemContent).x, t3.CalcSize(itemContent).y), itemContent, t3);
				f++;
			}
			if (healthitems > 0) {
				if (healthitems > 1) s = "s"; else s = "";
				cost = 50;
				itemContent = new GUIContent(healthitems +" crate" + s + " of Medical Supplies" + " for $" + cost + " each");
				offset = .5f * 1.05f * t3.CalcSize(itemContent).x;
				GUI.Box (new Rect (Screen.width * .5f - offset, Screen.height * (.45f + (f * .1f)), 1.05f * t3.CalcSize(itemContent).x, t3.CalcSize(itemContent).y),itemContent, t3);
				f++;
			}
			if (fooditems > 0) {
				if (fooditems > 1) s = "s"; else s = "";
				cost = 25;
				itemContent = new GUIContent(fooditems + " Food Crate" + s + " for $" + cost + " each");
				offset = .5f *1.05f *  t3.CalcSize(itemContent).x;
				GUI.Box (new Rect (Screen.width * .5f - offset, Screen.height * (.45f + (f * .1f)), 1.05f * t3.CalcSize(itemContent).x, t3.CalcSize(itemContent).y), itemContent, t3);
				f++;
			}
			offset = .5f *1.05f *  h2.CalcSize(expensesContent).x;
			GUI.Box (new Rect (Screen.width * .5f - offset, Screen.height * .7f, 1.05f * h2.CalcSize(expensesContent).x,h2.CalcSize(expensesContent).y), expensesContent, h2);
			offset = .5f *1.05f *  h2.CalcSize(totalCashContent).x;
			GUI.Box (new Rect (Screen.width * .5f - offset, Screen.height * .80f, 1.05f * h2.CalcSize(totalCashContent).x,h2.CalcSize(totalCashContent).y), totalCashContent, h2);

			/*if (GUI.Button(new Rect(Screen.width * guiPlacementX2, Screen.height * guiPlacementY2, Screen.width * .5f, Screen.height * .1f), "Controls")) {
				
			}*/
		} else if (status == 1) { //upgrade script

			string labelVal;
			GUIContent buttonContent = new GUIContent("Depart to Salvage");
			labelVal = "Upgrade Menu";
			GUIContent menuContent = new GUIContent(labelVal);
			labelVal = "Cash = " + cash;
			GUIContent cashContent = new GUIContent(labelVal);
			suitintegrity = (Mathf.Round(suitintegrity * 100f) / 100f); //round that stuff
			labelVal =  "Suit Integrity = " + suitintegrity + " ($5)";
			GUIContent integrityContent = new GUIContent(labelVal);
			GUIContent integrityContent0 = new GUIContent("-5");
			GUIContent integrityContent1 = new GUIContent("+5");
			labelVal = "Armor = " + armor.ToString("F2") + " ($20)";
			GUIContent armorContent = new GUIContent(labelVal);
			GUIContent armorContent0 = new GUIContent("-5");
			GUIContent armorContent1 = new GUIContent("+5");
			labelVal = "Engine Power = " + thrustermoverate + "  ($10)";
			GUIContent engineContent = new GUIContent(labelVal);
			GUIContent engineContent0 = new GUIContent("-.1");
			GUIContent engineContent1 = new GUIContent("+.1");
			labelVal = "Oxy Capacity = " + startingoxy + "  ($10)";
			GUIContent oxyContent = new GUIContent(labelVal);
			GUIContent oxyContent0 = new GUIContent("-1");
			GUIContent oxyContent1 = new GUIContent("+1");
			labelVal = "EMP Recovery = " + emprechargetime + "  ($40)";
			GUIContent empContent = new GUIContent(labelVal);
			GUIContent empContent0 = new GUIContent("-0.5");
			GUIContent empContent1 = new GUIContent("+0.5");
			string cranesucks, cranerep;
			if (cranelength == 0 && startingcranelength == 0) {
				cranesucks = "Repair Crane";
				cranerep = "Fix";
				fixCranes.normal.textColor = Color.red;
				fixCranes.hover.textColor = Color.red;

			} else {
				cranesucks = "Crane length = " + cranelength.ToString("F2");
				cranerep = "+ 0.2";
				fixCranes.normal.textColor = new Color(.6f,.6f,.6f);
				fixCranes.hover.textColor = new Color(.6f,.6f,.6f);
			}
			labelVal = cranesucks + "  ($40)";
			GUIContent craneContent = new GUIContent(labelVal);
			GUIContent craneContent0 = new GUIContent("- 0.2");
			GUIContent craneContent1 = new GUIContent(cranerep);
			string ropesucks, roperep;
			if (tubecut == 1) {
				ropesucks = "Repair Tube";
				roperep = "Fix";
				fixTubes.normal.textColor = Color.red;
				fixTubes.hover.textColor = Color.red;
			} else {
				ropesucks = "Tube length = " + tubesleft + "m";
				roperep = "+10m";
				fixTubes.normal.textColor = new Color(.6f,.6f,.6f);
				fixTubes.hover.textColor = new Color(.6f,.6f,.6f);
			}
			labelVal =  ropesucks + " ($10)";
			GUIContent tubeContent = new GUIContent(labelVal);
			GUIContent tubeContent0 = new GUIContent("-10m");
			GUIContent tubeContent1 = new GUIContent(roperep);
			labelVal = "Item Capacity = " + capacity + "  ($200)";
			GUIContent capContent = new GUIContent(labelVal);
			GUIContent capContent0 = new GUIContent("-1");
			GUIContent capContent1 = new GUIContent("+1");
			labelVal = "Next";
			GUIContent leaveContent = new GUIContent(labelVal);




			GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height),backgroundTexture2);
			//Sprite s =  Resources.Load("player", typeof(Sprite)) as Sprite;
			//GUI.DrawTexture(new Rect(Screen.width * .67f,Screen.height * .35f, 96 * 3/2 , 96 * 3/2 ),player);
			
			if (GUI.Button(new Rect(Screen.width * .5f - .5f * button.CalcSize(buttonContent).x, Screen.height * guiPlacementY1, button.CalcSize(buttonContent).x,button.CalcSize(buttonContent).y), buttonContent, button )) {
				this.GetComponent<AudioSource>().PlayOneShot(buttonSound);

				//suitintegrity = 100;
				//thrustermoverate = 1;
				//startingoxy = 30;
				//cash = 0;
				//cranelength = 1;
				status = 3;
				PlayerPrefs.SetInt("capacity",capacity);
				PlayerPrefs.SetInt("wallet", cash);
				PlayerPrefs.SetFloat("health", suitintegrity + armor);
				PlayerPrefs.SetFloat("startingoxy", startingoxy);
				PlayerPrefs.SetFloat("moverate", thrustermoverate);
				PlayerPrefs.SetFloat("emprechargetime", emprechargetime);
				//print(tubesleft);
				PlayerPrefs.SetInt("tubesleft", tubesleft);
				PlayerPrefs.SetInt("tubecut", tubecut);
				PlayerPrefs.SetFloat("movespeed", cranemovespeed);
				PlayerPrefs.SetFloat("cranelength", cranelength);
				Application.LoadLevel("ProcGen");
			}
			float offset = Screen.width *.5f - .5f * h1.CalcSize(menuContent).x;
			GUI.Box (new Rect (offset, Screen.height * guiPlacementY2,1.05f * h1.CalcSize(menuContent).x,h1.CalcSize(menuContent).y), menuContent, h1);
			float xval1 = .13f, yval1 = .22f;
			GUI.Box (new Rect (Screen.width * (xval1 + .5f), Screen.height * .4f,1.05f *  cashStyle.CalcSize(cashContent).x,cashStyle.CalcSize(cashContent).y), cashContent, cashStyle);

			
			//for health
			t1.normal = labels;
			t1.hover = labels;
			GUI.Box (new Rect (Screen.width * (xval1 + .052f), Screen.height * yval1, Screen.width * .3f, Screen.height * .06f), integrityContent, t1);
			if (suitintegrity >= 100 || cash < 5) {
				t1.normal = labels;
				t1.hover = labels;
				GUI.Box (new Rect (Screen.width * (xval1 +.254f + .1f), Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), integrityContent1, t1);

			} else {

				if (cash >= 5) {
					t1.normal = gs;
					t1.hover = gbs;
					if (GUI.Button (new Rect (Screen.width * (xval1 +.254f + .1f), Screen.height * yval1, Screen.width * .05f, Screen.height * .06f),integrityContent1, t1)) {
						this.GetComponent<AudioSource>().PlayOneShot(coinSounds[Random.Range(0,3)]);

						if (suitintegrity % 5 != 0 && suitintegrity > 95) {
							suitintegrity += (5 -(suitintegrity % 5));
							cash -= 5;
						} else {
							suitintegrity += 5;
							cash -= 5;
						}
					}

				} 
				
			}
			yval1 = .30f;
			//for armor
			if (armor == 0 || armor == startingarmor) {
				t1.normal = labels;
				t1.hover = labels;
				GUI.Box (new Rect (Screen.width * xval1, Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), integrityContent0, t1);
			} else {
				t1.normal = gs;
				t1.hover = gbs;
				if (GUI.Button (new Rect (Screen.width * xval1, Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), integrityContent0, t1)) {
					this.GetComponent<AudioSource>().PlayOneShot(coinSounds[Random.Range(0,3)]);
					if (armor % 5 != 0) {
						armor -= ((armor % 5));
					} else {
						armor -= 5;
					}
					cash += 20;
				}
				
			}
			t1.normal = labels;
			t1.hover = labels;
			GUI.Box (new Rect (Screen.width * (xval1 + .052f), Screen.height * yval1, Screen.width * .3f, Screen.height * .06f), armorContent, t1);
			if (armor >= 200 || cash < 20) { //200 being max armor
			
				GUI.Box (new Rect (Screen.width * (xval1 +.254f + .1f), Screen.height * yval1, Screen.width * .05f, Screen.height * .06f),  armorContent1, t1);
			} else {
				t1.normal = gs;
				t1.hover = gbs;
				if (cash >= 20 && GUI.Button (new Rect (Screen.width * (xval1 +.254f + .1f), Screen.height * yval1, Screen.width * .05f, Screen.height * .06f),  armorContent1, t1)) {
					this.GetComponent<AudioSource>().PlayOneShot(coinSounds[Random.Range(0,3)]);

					if (armor % 5 != 0 && armor > 190) {
						armor += (5 - (armor % 5));
					} else {
						armor += 5;
					}
					cash -= 20;
				}
				
			}
			yval1 = .38f;
			//for thrusterrate
			t1.normal = labels;
			t1.hover = labels;
			if (thrustermoverate == 1 || thrustermoverate == startingthrustermoverate) {
				GUI.Box (new Rect (Screen.width * xval1, Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), armorContent0, t1);
			} else {
				t1.normal = gs;
				t1.hover = gbs;
				if (GUI.Button (new Rect (Screen.width * xval1, Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), armorContent0, t1)) {
					this.GetComponent<AudioSource>().PlayOneShot(coinSounds[Random.Range(0,3)]);

					thrustermoverate -= .1000000000f;
					thrustermoverate = Mathf.Round(thrustermoverate * 100f) / 100f;
					cash += 10;
				}
				
			}
			t1.normal = labels;
			t1.hover = labels;
			GUI.Box (new Rect (Screen.width * (xval1 + .052f), Screen.height * yval1, Screen.width * .3f, Screen.height * .06f), engineContent, t1);
			if (thrustermoverate >= 6 || cash < 10) { // 6 being max speed
				GUI.Box (new Rect (Screen.width * (xval1 +.254f + .1f), Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), engineContent1, t1);
			} else {
				t1.normal = gs;
				t1.hover = gbs;
				if (cash >= 10 && GUI.Button (new Rect (Screen.width * (xval1 +.254f + .1f), Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), engineContent1, t1)) {
					this.GetComponent<AudioSource>().PlayOneShot(coinSounds[Random.Range(0,3)]);
					thrustermoverate += .1f;
					thrustermoverate = Mathf.Round(thrustermoverate * 100f) / 100f;
					cash -=10;
				}
				
			}
			//starting oxygen
			yval1 = .46f;
			if (startingoxy == 30 || startingoxy == startingstartingoxy) {
				t1.normal = labels;
				t1.hover = labels;
				GUI.Box (new Rect (Screen.width * xval1, Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), engineContent0, t1);
			} else {
				t1.normal = gs;
				t1.hover = gbs;
				if (GUI.Button (new Rect (Screen.width * xval1, Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), engineContent0, t1)) {
					this.GetComponent<AudioSource>().PlayOneShot(coinSounds[Random.Range(0,3)]);
					startingoxy -= 1;
					cash += 10;
				}
				
			}
			t1.normal = labels;
			t1.hover = labels;
			GUI.Box (new Rect (Screen.width * (xval1 + .052f), Screen.height * yval1, Screen.width * .3f, Screen.height * .06f), oxyContent, t1);
			if (startingoxy >= 180 || cash < 10) { //180 being max oxy
				GUI.Box (new Rect (Screen.width * (xval1 +.254f + .1f), Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), oxyContent1, t1);
			} else {
				t1.normal = gs;
				t1.hover = gbs;
				if (cash >= 10 && GUI.Button (new Rect (Screen.width * (xval1 +.254f + .1f), Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), oxyContent1, t1)) {
					this.GetComponent<AudioSource>().PlayOneShot(coinSounds[Random.Range(0,3)]);
					startingoxy += 1;
					cash -= 10;
				}
				
			}
			yval1 = .54f;
			if (emprechargetime == 10 || emprechargetime == startingemprechargetime) {
				t1.normal = labels;
				t1.hover = labels;
				GUI.Box (new Rect (Screen.width * xval1, Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), empContent1, t1);
			} else {
				t1.normal = gs;
				t1.hover = gbs;
				if (GUI.Button (new Rect (Screen.width * xval1, Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), empContent1, t1)) {
					this.GetComponent<AudioSource>().PlayOneShot(coinSounds[Random.Range(0,3)]);
					emprechargetime += 0.5f;
					cash += 40;
				}
				
			}
			t1.normal = labels;
			t1.hover = labels;
			GUI.Box (new Rect (Screen.width * (xval1 + .052f), Screen.height * yval1, Screen.width * .3f, Screen.height * .06f), empContent, t1);
			if (emprechargetime == 2 || cash < 40) { //180 being min time

				GUI.Box (new Rect (Screen.width * (xval1 +.254f + .1f), Screen.height * yval1, Screen.width * .05f, Screen.height * .06f),empContent0, t1);
			} else {
				t1.normal = gs;
				t1.hover = gbs;
				if (cash >= 40 && GUI.Button (new Rect (Screen.width * (xval1 +.254f + .1f), Screen.height * yval1, Screen.width * .05f, Screen.height * .06f),empContent0, t1)) {
					this.GetComponent<AudioSource>().PlayOneShot(coinSounds[Random.Range(0,3)]);
					emprechargetime -= 0.5f;
					cash -= 40;
				}
				
			}
			yval1+= .08f;
			if (cranelength == 3 || cranelength == startingcranelength) {
				t1.normal = labels;
				t1.hover = labels;
				GUI.Box (new Rect (Screen.width * xval1, Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), craneContent0, t1);
			} else {
				t1.normal = gs;
				t1.hover = gbs;
				if (GUI.Button (new Rect (Screen.width * xval1, Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), craneContent0, t1)) {
					this.GetComponent<AudioSource>().PlayOneShot(coinSounds[Random.Range(0,3)]);
					if (startingcranelength == 0 && cranelength == 1) {
						cranelength -= 1;
						cash += 40;
					} else {
						cranelength -= 0.2f;
						cash += 40;
					}
					
					
				}
				
			}
			t1.normal = labels;
			t1.hover = labels;
			GUI.Box (new Rect (Screen.width * (xval1 + .052f), Screen.height * yval1, Screen.width * .3f, Screen.height * .06f), craneContent, t1);
			if (cranelength == 6 || cash < 40) { //6 being max length
				GUI.Box (new Rect (Screen.width * (xval1 +.254f + .1f), Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), craneContent1, fixCranes);
			} else {
				t1.normal = gs;
				t1.hover = gbs;
				if (cash >= 40 && GUI.Button (new Rect (Screen.width * (xval1 +.254f + .1f), Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), craneContent1, fixCranes)) {
					this.GetComponent<AudioSource>().PlayOneShot(coinSounds[Random.Range(0,3)]);
					if (startingcranelength == 0 && cranelength == 0) {
						cranelength += 1;
						cash -= 40;
					} else {
						cranelength += 0.2f;
						cash -= 40;
					}
				} 
				
			}
			yval1+= .08f;
			if (tubesleft == startingtubesleft) {
				t1.normal = labels;
				t1.hover = labels;
				GUI.Box (new Rect (Screen.width * xval1, Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), tubeContent0, t1);
			} else {
				t1.normal = gs;
				t1.hover = gbs;
				if (GUI.Button (new Rect (Screen.width * xval1, Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), tubeContent0, t1)) {
					this.GetComponent<AudioSource>().PlayOneShot(coinSounds[Random.Range(0,3)]);
					tubesleft -= 10;
					cash += 10;
				}
				
			}
			t1.normal = labels;
			t1.hover = labels;
			GUI.Box (new Rect (Screen.width * (xval1 + .052f), Screen.height * yval1, Screen.width * .3f, Screen.height * .06f), tubeContent, t1);
			if (tubesleft == 500 || cash < 10) { //4 being max time
				GUI.Box (new Rect (Screen.width * (xval1 +.254f + .1f), Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), tubeContent1, fixTubes);
			} else {
				t1.normal = gs;
				t1.hover = gbs;
				if (cash >= 10 && GUI.Button (new Rect (Screen.width * (xval1 +.254f + .1f), Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), tubeContent1, fixTubes)) {
					this.GetComponent<AudioSource>().PlayOneShot(coinSounds[Random.Range(0,3)]);
					if (tubecut == 1) {
						tubecut = 0;
						cash -= 10;
					} else {
						tubesleft += 10;
						cash -= 10;
					}
				}
			}
			yval1+= .08f;
			if (capacity == 3 || capacity == startingcapacity) {
				t1.normal = labels;
				t1.hover = labels;
				GUI.Box (new Rect (Screen.width * xval1, Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), capContent0, t1);
			} else {
				t1.normal = gs;
				t1.hover = gbs;
				if (GUI.Button (new Rect (Screen.width * xval1, Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), capContent0, t1)) {
					this.GetComponent<AudioSource>().PlayOneShot(coinSounds[Random.Range(0,3)]);
					capacity -= 1;
					cash += 200;
				}
				
			}
			t1.normal = labels;
			t1.hover = labels;
			GUI.Box (new Rect (Screen.width * (xval1 + .052f), Screen.height * yval1, Screen.width * .3f, Screen.height * .06f), capContent, t1);
			if (capacity == 10 || cash < 200) { //4 being max time

				GUI.Box (new Rect (Screen.width * (xval1 +.254f + .1f), Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), capContent1, t1);
			} else {
				t1.normal = gs;
				t1.hover = gbs;
				if (cash >= 200 && GUI.Button (new Rect (Screen.width * (xval1 +.254f + .1f), Screen.height * yval1, Screen.width * .05f, Screen.height * .06f),capContent1, t1)) {
					this.GetComponent<AudioSource>().PlayOneShot(coinSounds[Random.Range(0,3)]);
					capacity += 1;
					cash -= 200;
				}
				
			}
			
			/*GUI.Box (new Rect (Screen.width * xval1, Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), "-");
		GUI.Box (new Rect (Screen.width * (xval1 + .052f), Screen.height * yval1, Screen.width * .2f, Screen.height * .06f), "Armor = " + armor);
		GUI.Box (new Rect (Screen.width * (xval1 +.254f), Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), "+");
*/

		} else {
			GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height),loadingTexture);

		}
	}
}
