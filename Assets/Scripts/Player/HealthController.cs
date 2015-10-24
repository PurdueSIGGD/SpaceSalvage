using UnityEngine;
using System.Collections;

public class HealthController : MonoBehaviour {

	/* This controls several different values inside of the player, including:
	 * Health, oxygen, armor, tubesleft.
	 * It also controls the GUIText values displayed in-game.
	 * It will also save values at the end of the game
	 */
	public float health = 100; //suit integrity
	public float med = 100; //actual health
	private float startingoxy = 30;
	public float oxy;
	private GameObject text;
	private int wallet = 50;
	private int startingwallet, tubesleft;
	private float startinghealth, time, cranetime;
	private bool oxywarning, oxyerror, suitwarning, suiterror, medwarning, cranewarning;
	private static string okmessage = "All systems operational";
	private static string oxymessage = "WARNING: LOW OXYGEN\n";
	private static string oxymessagegone = "WARNING: NO OXYGEN\n";
	private static string suitmessage = "WARNING: LOW SUIT INTEGRITY\n";
	private static string suitmessagegone = "WARNING: SUIT LOST\n";
	private static string medmessage = "WARNING: VITAL SIGNS ARE LOW\n";
	private static string cranemessage = "WARNING: CRANE IS DESTROYED\n";
	//private static string cranedis = "DISCONNECTING CRANE IN ";
	private static string empmessage = "WARNING: EMP DETECTED\n";
	private static string empmessage2 = "TIME UNTIL SYSTEM REBOOT: ";
	private bool ejected;
	private bool emergency, on;
	private float timesince, timeerror, oxytime;
	private float timesincelastdamage;
	private float rechargetime;
	private bool pause;
    private AudioSource vitalsLowAudioSource;
    private AudioSource oxyLowAudioSource;
	private float rechargeEmpDiff; // This is the difference between the recharge time/emp time, which is never negative -- Anna
	public GameObject particle, oxyparticle;
	public bool acceptingOxy, emp = false, gettingOxy; //is oxy less than startingoxy?
	public float emptime = 0;
    public GameObject vitalsLowAudio;
    public GameObject oxyLowAudio;
	public Sprite j1, j2, j3, j4, j5;
	// Use this for initialization
	void Start () { 
		timesincelastdamage = -1;
		if (PlayerPrefs.HasKey("startingoxy")) { //getting initial values
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
		if (PlayerPrefs.HasKey ("emprechargetime")) {
			//print (PlayerPrefs.GetFloat("emprechargetime"));
			rechargetime = PlayerPrefs.GetFloat("emprechargetime");
		} else {
			PlayerPrefs.SetFloat("emprechargetime", 10);
		}
		timesince = 0;
		startinghealth = health;
		oxy = startingoxy;
		text = GameObject.Find("GuiText");
        vitalsLowAudioSource = vitalsLowAudio.GetComponent<AudioSource>();
        oxyLowAudioSource = oxyLowAudio.GetComponent<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {
		//if (this.cranewarning) cranetime+=Time.deltaTime;
		if (time < .5f) time+=Time.deltaTime; //used for particles later
		acceptingOxy = (oxy < startingoxy); //if we are able to take in oxygen
		string words = "";
		if (timesincelastdamage >= 0) {
			timesincelastdamage += Time.deltaTime;
			if (timesincelastdamage > 5 && med < 100) { //regen health

				if (med > 1) med +=  (8 * Time.deltaTime)/med;
				if (med <= 1) med +=  Time.deltaTime;
			}

		}
        if (medwarning && !vitalsLowAudioSource.isPlaying)
        {
            vitalsLowAudioSource.Play();
        }
        else if (!medwarning)
        {
            vitalsLowAudioSource.Stop();
        }
        
		if (!(medwarning || oxywarning || oxyerror || suitwarning || suiterror || cranewarning || (emp && !pause))) { //no warnings
			emergency = false;
			words += okmessage;

		} else { //see what issue we may have
 			emergency = true;
			if (emp && !pause){
				//print(transform.FindChild("SoundEffectController").FindChild("EMPSound") + " " + emptime/rechargetime);
				transform.FindChild("SoundEffectController").FindChild("EMPSound").GetComponent<AudioSource>().volume = 1 - emptime/rechargetime;
				rechargeEmpDiff = rechargetime - emptime;
				if(rechargeEmpDiff < 0.0f) rechargeEmpDiff=0.0f;
				words += (empmessage2 +  (rechargeEmpDiff).ToString("F2") + "\n" + empmessage);
			}
			if (suitwarning) words += suitmessage;
			if (suiterror) words += suitmessagegone;
			if (oxywarning) words += oxymessage;
			if (oxyerror) words += oxymessagegone;
			if (medwarning) words += medmessage;
			if (cranewarning) words += cranemessage;
			//if (cranewarning && cranetime < 5) words += cranedis + (5 - cranetime) + "\n";

		}
		if (emergency) { //flashing lights

			timesince+=Time.deltaTime;
			if (timesince > .25f) {
				if (timesince > .5f) timesince = 0;
				if (emp && !pause) {

					
					rechargeEmpDiff = rechargetime - emptime;
					if(rechargeEmpDiff < 0.0f) rechargeEmpDiff=0.0f;

					words = empmessage2 + (rechargeEmpDiff).ToString("F2") + "\n";

				} else {
					words = "";
				}
			}
		}
		string m = "m"; //for meters
		string tubesmessage = ejected?" (disconnected)":"";
		float newoxy = (oxy >= startingoxy - 3*Time.deltaTime)?startingoxy:oxy; //so it doesnt go from 29 to 30 constantly
		string final = 
			"Suit Integrity: " + health.ToString("F2") + "/" + startinghealth.ToString("F2") + "\n" +
				/*"Oxygen Levels: " + newoxy.ToString("F2") + "/" + startingoxy.ToString("F2") + "\n" +*/
				"Health: " + med.ToString("F2") + "/100.00\n" +
				"Cash: " + wallet + "\n" + 
				"Tube length left: " + tubesleft + m + tubesmessage + "\n" + 
				words;
		((GUIText)text.GetComponent("GUIText")).text = final;

		if (med != 100 && !pause) { 
			this.SendMessage("FaderTime",med/100); //so the screen can fade, sends to playercollisioncontroller and kills the player if necessary
		}
		medwarning = (med < 35);
		suitwarning = (health < 20 && health > 0);
		suiterror = (health <= 0);
		oxyerror  = (oxy <= 0);

		if (emp && !pause && med <= 0) emp = false; //we cannot be affected by an emp if we aren't paused or dead
		cranewarning = (this.GetComponentInChildren<CraneController>().broken); 

		ejected = ((RopeScript2D)GameObject.Find("Ship").GetComponent("RopeScript2D")).ejected || ((RopeScript2D)GameObject.Find("Ship").GetComponent("RopeScript2D")).brokenrope; //finding if rope is disconneted
		if (ejected) { //change oxygen from being ejected
			if (((RopeScript2D)GameObject.Find("Ship").GetComponent("RopeScript2D")).ejected) {
				oxytime+=Time.deltaTime;
				if (oxytime > .15f) {
					if (oxy > 0) {
							GameObject thingy = (GameObject)Instantiate(oxyparticle, this.transform.position, Quaternion.identity); //spawning particles
							float r = Random.value;
							//thingy.GetComponent<SpriteRenderer>().sprite = ;
							thingy.transform.localScale = new Vector3(.7f,.7f,.7f); //typical scale is 5, dont want parts too big or small
							thingy.GetComponent<SpriteRenderer>().color = new Color(1,1,1); //make it redder if necessary
							thingy.GetComponent<Rigidbody2D>().AddForce(new Vector2(UnityEngine.Random.Range(-50,50), UnityEngine.Random.Range(-50,50)));
							//thingy.GetComponent<Rigidbody2D>().AddTorque(thingy.GetComponent<Rigidbody2D>().mass * UnityEngine.Random.Range(-25,25));
							oxytime = 0;
					}	
					if (GameObject.Find("Connector")) {
						GameObject thingy1 = (GameObject)Instantiate(oxyparticle, GameObject.Find("Connector").transform.position, Quaternion.identity); //spawning particles
						float r1 = Random.value;
						//thingy.GetComponent<SpriteRenderer>().sprite = ;
						thingy1.transform.localScale = new Vector3(.7f,.7f,.7f); //typical scale is 5, dont want parts too big or small
						thingy1.GetComponent<SpriteRenderer>().color = new Color(1,1,1); //make it redder if necessary
						thingy1.GetComponent<Rigidbody2D>().AddForce(new Vector2(UnityEngine.Random.Range(-50,50), UnityEngine.Random.Range(-50,50)));
						//thingy.GetComponent<Rigidbody2D>().AddTorque(thingy.GetComponent<Rigidbody2D>().mass * UnityEngine.Random.Range(-25,25));
					}
				}
			} 

			if ((health < 50 && health > 1 )|| health == 0) {

				changeOxy(-1 * Time.deltaTime * (50 - health)/10 ); 
				//spawn particles here

			} else {
				changeOxy(-1 * Time.deltaTime);
			}
		} else {
			if (health < 15) {
				changeOxy(-.2f * Time.deltaTime);
				//spawn particles here
				oxytime+=Time.deltaTime;
				if (oxytime > .15f && oxy>0) {
					GameObject thingy = (GameObject)Instantiate(oxyparticle, this.transform.position, Quaternion.identity); //spawning particles
					float r = Random.value;
					//thingy.GetComponent<SpriteRenderer>().sprite = ;
					thingy.transform.localScale = new Vector3(.7f/(health+1),.7f/(health+1),.7f/(health+1)); //typical scale is 5, dont want parts too big or small
					thingy.GetComponent<SpriteRenderer>().color = new Color(1,1,1); //make it redder if necessary
					thingy.GetComponent<Rigidbody2D>().AddForce(new Vector2(UnityEngine.Random.Range(-50,50), UnityEngine.Random.Range(-50,50)));
					//thingy.GetComponent<Rigidbody2D>().AddTorque(thingy.GetComponent<Rigidbody2D>().mass * UnityEngine.Random.Range(-25,25));
					oxytime = 0;
				}
			} else {
				changeOxy(3 * 	Time.deltaTime);

			}
		}

		if (oxy < 15) {
			changeMed(-1 * Time.deltaTime * (15-oxy)/3);
			oxywarning = (!oxyerror);
		} else {
			oxywarning = false;
		}

	}

	void changeHealth(float f) { //not actual health, this is just the suit itegrity


		if (!pause) { //pause is set if we are in the ship, leaving
			if (time > .25f && particle != null) {				
				GameObject thingy = (GameObject)Instantiate(particle, this.transform.position, Quaternion.identity); //spawning particles
				float r = Random.value;
				if (r >= 0 && r < .2f) { //find random damage sprite
					thingy.GetComponent<SpriteRenderer>().sprite = j1;
				} 
				if (r >= .2f && r < .4f) {
					thingy.GetComponent<SpriteRenderer>().sprite = j2;
				} 
				if (r >= .4f && r < .6f) {
					thingy.GetComponent<SpriteRenderer>().sprite = j3;
				} 
				if (r >= .6f && r < .8f) {
					thingy.GetComponent<SpriteRenderer>().sprite = j4;
				} 
				if (r >= .8f && r < 1) {
					thingy.GetComponent<SpriteRenderer>().sprite = j5;
				} 
				//thingy.GetComponent<SpriteRenderer>().sprite = ;
				thingy.transform.localScale = new Vector3(.8f,.8f,.8f); //typical scale is 1, dont want parts too big or small
				thingy.GetComponent<SpriteRenderer>().color = new Color(1, (health/100), (health/100)); //make it redder if necessary
				thingy.GetComponent<Rigidbody2D>().AddForce(new Vector2(UnityEngine.Random.Range(-50,50), UnityEngine.Random.Range(-50,50)));
				thingy.GetComponent<Rigidbody2D>().AddTorque(thingy.GetComponent<Rigidbody2D>().mass * UnityEngine.Random.Range(-25,25));
				time = 0;
			}
			if (health > 1 && health + f > 0) {
				if (med >= 3*f/health) { //your health can be damaged by attacks, but your suit works like armor plus your armor already
					med += 3*f/health;
				} else {
					med = 0;
				}
			} else {
				health = 0; //if no armor left, change your actual health
				changeMed(f);
			}
			if (health != 0) {
				health+=f;
			}
		}
	}
	void changeOxy(float f) {
		//print(oxy);
		if (!pause) {
			if (f < 0 && gettingOxy) return; //if getting oxygen and losing some, forget about the losing oxygen
			if (f < 0 && !oxyLowAudioSource.isPlaying)
            {
                oxyLowAudioSource.Play();
            }
            else if (f >= 0)
            {
                oxyLowAudioSource.Stop();
            }
            if (f + oxy > startingoxy) {
				oxy = startingoxy;
			} else if (f + oxy <= 0) { // if it is more than we can give, so no negatives
				oxy = 0;
			} else {
				oxy += f;
			}
		}
	}
	void changeMed(float f) { //local use ONLY
		if (!pause) {

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
	void StopDoingThat() { //from the ship
		pause = true;
	}
	void GetTubesLeft(int tubes) {
		tubesleft = tubes;
	}
	void GettingOxy(bool b) { //accepting oxygen, look in oxygenstation.cs
		gettingOxy = b;
	}
	public float GetOxy() {
		return oxy;
	}
	public float GetOxyPercent() {
		return oxy/this.startingoxy;
	}
	void Im_Leaving() { //last function to pass
		PlayerPrefs.SetInt ("wallet", wallet);
		PlayerPrefs.SetFloat ("health", health);
		PlayerPrefs.SetInt ("startingwallet", startingwallet);
	}
}
