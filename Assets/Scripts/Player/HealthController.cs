using UnityEngine;
using System.Collections;

public class HealthController : MonoBehaviour {
	private float health = 100; //suit integrity
	private float med = 100; //actual health
	private float startingoxy = 30;
	private float oxy;
	private GameObject text;
	private int wallet;
	private int startingwallet;
	private float startinghealth;
	private bool oxywarning, suitwarning, medwarning, cranewarning;
	private static string okmessage = "All systems operational";
	private static string oxymessage = "WARNING: LOW OXYGEN\n";
	private static string suitmessage = "WARNING: LOW SUIT INTEGRITY\n";
	private static string medmessage = "WARNING: VITAL SIGNS ARE DECREASING\n";
	private static string cranemessage = "WARNING: CRANE IS DESTROYED\n";
	private bool ejected;
	private bool emergency, on;
	private float timesince;
	private float timesincelastdamage;
	private bool pause;
	public bool acceptingOxy; //is oxy less than startingoxy?
	// Use this for initialization
	void Start () {
		timesincelastdamage = -1;
		if (PlayerPrefs.HasKey("startingoxy")) {
			startingoxy = PlayerPrefs.GetFloat("startingoxy");
		} else {
			PlayerPrefs.SetFloat("startingoxy",startingoxy);
		}
		if (PlayerPrefs.HasKey ("wallet")) {
			wallet = PlayerPrefs.GetInt("wallet");
			startingwallet =  PlayerPrefs.GetInt("wallet");
		} else {
			PlayerPrefs.SetInt("wallet", wallet);
		}	
		if (PlayerPrefs.HasKey ("health")) {
			health = PlayerPrefs.GetFloat("health");
			//health = 100; //REMOVE LATER
		} else {
			PlayerPrefs.SetFloat("health", health);
		}	
		timesince = 0;
		startinghealth = health;
		oxy = startingoxy;
		text = GameObject.Find("GuiText");
	}
	
	// Update is called once per frame
	void Update () {
		acceptingOxy = (oxy < startingoxy);
		string words = "";
		if (timesincelastdamage >= 0) {
			timesincelastdamage += Time.deltaTime;
			if (timesincelastdamage > 5 && med < 100) { //regen health

				if (med > 1) med +=  (8 * Time.deltaTime)/med;
				if (med <= 1) med +=  Time.deltaTime;
			}
		}
		if (!(medwarning || oxywarning || suitwarning || cranewarning)) {
			emergency = false;
			words += okmessage;
		} else {
			emergency = true;
			if (suitwarning) words += suitmessage;
			if (oxywarning) words += oxymessage;
			if (medwarning) words += medmessage;
			if (cranewarning) words += cranemessage;
		}
		if (emergency) { //flashing lights

			timesince+=Time.deltaTime;
			if (timesince > .25f) {
				if (timesince > .5f) timesince = 0;
				words = "";
			}
		}
		((GUIText)text.GetComponent("GUIText")).text = 
			"Suit Integrity: " + health.ToString("F2") + "/" + startinghealth.ToString("F2") + "\n" +
				"Oxygen Levels: " + oxy.ToString("F2") + "/" + startingoxy.ToString("F2") + "\n" +
			"Health: " + med.ToString("F2") + "/100.00\n" +
			"Cash: " + wallet + "\n" + 
				words;

		if (med != 100 && !pause) {
			this.SendMessage("FaderTime",med/100);
		}
		medwarning = (med < 35);
		suitwarning = (health < 20);
		cranewarning = (this.GetComponentInChildren<CraneController>().broken);
		ejected = ((RopeScript2D)GameObject.Find("Ship").GetComponent("RopeScript2D")).ejected || ((RopeScript2D)GameObject.Find("Ship").GetComponent("RopeScript2D")).brokenrope;
		if (ejected) { //change oxygen from being ejected
			//this.GetComponent<LineRenderer>().enabled = false;
			if ((health < 50 && health > 1 )|| health == 0) {
				changeOxy(-1 * Time.deltaTime * (50 - health)/10 );
			} else {
				changeOxy(-1 * Time.deltaTime);
			}
		} else {
			//this.GetComponent<LineRenderer>().enabled = true;
			changeOxy(3 * 	Time.deltaTime);
		}

		if (oxy < 15) {
			changeMed(-1 * Time.deltaTime * (15-oxy)/3);
			oxywarning = true;
		} else {
			oxywarning = false;
		}

	}

	void changeHealth(float f) { //not actual health, this is just the suit itegrity
		if (!pause) {
			if (health > 1) {
				if (med >= 3*f/health) { //your health can be damaged by attacks, but your suit works like armor
					med += 3*f/health;
				} else {
					med = 0;
				}
			} else {
				health = 0;
				changeMed(f);
			}
			if (health != 0) {
				health+=f;
			}
		}
	}
	void changeOxy(float f) {
		if (!pause) {
			if (f + oxy > startingoxy) {
				oxy = startingoxy;
			} else if (f + oxy <= 0) {
				oxy = 0;
			} else {
				oxy += f;
			}
		}
	}
	void changeMed(float f) { //local use ONLY
		if (!pause) {
			timesincelastdamage = 0;
			if (med + f >= 100) {
				med = 100;
			} else if (med+f <=0) {
				med = 0;
			} else {
				med += f;
			}
		}
	}
	void changeWallet(int i) {
		wallet += i;
	}
	void StopDoingThat() {
		pause = true;
	}
	void Im_Leaving() {

		//print ("wallet: " + wallet);
		//print ("startingwallet: " + startingwallet);
		PlayerPrefs.SetInt ("wallet", wallet);
		PlayerPrefs.SetFloat ("health", health);
		PlayerPrefs.SetInt ("startingwallet", startingwallet);
	}
}
