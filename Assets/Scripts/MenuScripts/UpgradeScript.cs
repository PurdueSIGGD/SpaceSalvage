using UnityEngine;
using System.Collections;

public class UpgradeScript : MonoBehaviour {
	
	public Texture backgroundTexture;
	public Texture player;
	public float guiPlacementX1;
	public float guiPlacementY1;
	public float guiPlacementX2;
	public float guiPlacementY2;
	private string[] items;
	private int cash;
	private float suitintegrity;
	private float startingoxy;
	private float thrustermoverate;
	private float emprechargetime;
	private int tubesleft;
	private float armor;
	private float cranemovespeed;
	private float cranelength;
	private float startingstartingoxy;
	private float startingthrustermoverate;
	private float startingemprechargetime;
	private int startingtubesleft;
	private float startingarmor;
	private float startingcranemovespeed;
	private float startingcranelength;
	private int capacity;
	private int startingcapacity;
	void Start() {
		capacity = PlayerPrefs.GetInt("capacity");
		cash = PlayerPrefs.GetInt("wallet");
		suitintegrity = PlayerPrefs.GetFloat("health");
		startingoxy = PlayerPrefs.GetFloat("startingoxy");
		thrustermoverate = PlayerPrefs.GetFloat("moverate");
		emprechargetime = PlayerPrefs.GetFloat("emprechargetime");
		tubesleft = PlayerPrefs.GetInt("tubesleft");
		cranemovespeed = PlayerPrefs.GetFloat("movespeed");
		cranelength = PlayerPrefs.GetFloat("cranelength");
		startingcapacity = capacity;
		startingstartingoxy = startingoxy;
		startingthrustermoverate = thrustermoverate;
		startingemprechargetime = emprechargetime;
		startingtubesleft = tubesleft;
		startingcranemovespeed = cranemovespeed;
		startingcranelength = cranelength;

		if (suitintegrity > 100) {
			armor = suitintegrity - 100;
			suitintegrity= 100;
		} else {
			armor = 0;
		}
		startingarmor = armor;

		
		guiPlacementX1 = .25f;
		guiPlacementY1 = .90f;
		guiPlacementX2 = .25f;
		guiPlacementY2 = .10f;
		
	}
	void OnGUI() {
		GUI.DrawTexture(new Rect(0,0,Screen.width, Screen.height),backgroundTexture);
		//Sprite s =  Resources.Load("player", typeof(Sprite)) as Sprite;
		GUI.DrawTexture(new Rect(Screen.width * .67f,Screen.height * .35f, 96 * 3/2 , 96 * 3/2 ),player);
		if (GUI.Button(new Rect(Screen.width * guiPlacementX1, Screen.height * guiPlacementY1, Screen.width * .5f, Screen.height * .1f), "Next")) {
			//suitintegrity = 100;
			//thrustermoverate = 1;
			//startingoxy = 30;
			//cash = 0;
			//cranelength = 1;
			print (suitintegrity + "   " + armor);
			PlayerPrefs.SetInt("capacity",capacity);
			PlayerPrefs.SetInt("wallet", cash);
			PlayerPrefs.SetFloat("health", suitintegrity + armor);
			PlayerPrefs.SetFloat("startingoxy", startingoxy);
			PlayerPrefs.SetFloat("moverate", thrustermoverate);
			PlayerPrefs.SetFloat("emprechargetime", emprechargetime);
			PlayerPrefs.SetInt("tubesleft", tubesleft);
			PlayerPrefs.SetFloat("movespeed", cranemovespeed);
			PlayerPrefs.SetFloat("cranelength", cranelength);
			Application.LoadLevel("ReturnTest");
		}
		
		GUI.Box (new Rect (Screen.width * guiPlacementX2, Screen.height * guiPlacementY2, Screen.width * .5f, Screen.height * .1f), "Upgrade Menu");
		float xval1 = .13f, yval1 = .22f;
		GUI.Box (new Rect (Screen.width * (xval1 + .5f), Screen.height * yval1, Screen.width * .2f, Screen.height * .1f), "Cash = $" + cash);
		suitintegrity = (Mathf.Round(suitintegrity * 100f) / 100f); //round that stuff

		//for health
		GUI.Box (new Rect (Screen.width * (xval1 + .052f), Screen.height * yval1, Screen.width * .2f, Screen.height * .06f), "Health = " + suitintegrity + " ($5)");
		if (suitintegrity >= 100) {
			GUI.Box (new Rect (Screen.width * (xval1 +.254f), Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), "+ 5");
		} else {
			if (cash >= 5 && GUI.Button (new Rect (Screen.width * (xval1 +.254f), Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), "+ 5")) {
				if (suitintegrity % 5 != 0 && suitintegrity > 95) {
					suitintegrity += (5 -(suitintegrity % 5));
					cash -= 5;
				} else {
					suitintegrity += 5;
					cash -= 5;
				}
			} 

		}
		yval1 = .30f;
		//for armor
		if (armor == 0 || armor == startingarmor) {
			GUI.Box (new Rect (Screen.width * xval1, Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), "- 5");
		} else {
			if (GUI.Button (new Rect (Screen.width * xval1, Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), "- 5")) {
				if (armor % 5 != 0) {
					armor -= ((armor % 5));
				} else {
					armor -= 5;
				}
				cash += 20;
			}
			
		}
		GUI.Box (new Rect (Screen.width * (xval1 + .052f), Screen.height * yval1, Screen.width * .2f, Screen.height * .06f), "Armor = " + armor + " ($20)");
		if (armor >= 200) { //200 being max armor
			GUI.Box (new Rect (Screen.width * (xval1 +.254f), Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), "+ 5");
		} else {
			if (cash >= 20 && GUI.Button (new Rect (Screen.width * (xval1 +.254f), Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), "+ 5")) {
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
		if (thrustermoverate == 1 || thrustermoverate == startingthrustermoverate) {
			GUI.Box (new Rect (Screen.width * xval1, Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), "- .1");
		} else {
			if (GUI.Button (new Rect (Screen.width * xval1, Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), "- .1")) {
				thrustermoverate -= .1000000000f;
				thrustermoverate = Mathf.Round(thrustermoverate * 100f) / 100f;
				cash += 10;
			}
			
		}
		GUI.Box (new Rect (Screen.width * (xval1 + .052f), Screen.height * yval1, Screen.width * .2f, Screen.height * .06f), "Engine Power = " + thrustermoverate + "  ($10)");
		if (thrustermoverate >= 5) { // 5 being max speed
			GUI.Box (new Rect (Screen.width * (xval1 +.254f), Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), "+ .1");
		} else {
			if (cash >= 10 && GUI.Button (new Rect (Screen.width * (xval1 +.254f), Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), "+ .1")) {
				thrustermoverate += .1f;
				thrustermoverate = Mathf.Round(thrustermoverate * 100f) / 100f;
				cash -=10;
			}

		}
		//starting oxygen
		yval1 = .46f;
		if (startingoxy == 30 || startingoxy == startingstartingoxy) {
			GUI.Box (new Rect (Screen.width * xval1, Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), "- 1");
		} else {
			if (GUI.Button (new Rect (Screen.width * xval1, Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), "- 1")) {
				startingoxy -= 1;
				cash += 10;
			}
			
		}
		GUI.Box (new Rect (Screen.width * (xval1 + .052f), Screen.height * yval1, Screen.width * .2f, Screen.height * .06f), "Oxy Capacity = " + startingoxy + "  ($10)");
		if (startingoxy >= 180) { //180 being max oxy
			GUI.Box (new Rect (Screen.width * (xval1 +.254f), Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), "+ 1");
		} else {
			if (cash >= 10 && GUI.Button (new Rect (Screen.width * (xval1 +.254f), Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), "+ 1")) {
				startingoxy += 1;
				cash -= 10;
			}
			
		}
		yval1 = .54f;
		if (emprechargetime == 10 || emprechargetime == startingemprechargetime) {
			GUI.Box (new Rect (Screen.width * xval1, Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), "+ 0.5");
		} else {
			if (GUI.Button (new Rect (Screen.width * xval1, Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), "+ 0.5")) {
				emprechargetime += 0.5f;
				cash += 40;
			}
			
		}
		GUI.Box (new Rect (Screen.width * (xval1 + .052f), Screen.height * yval1, Screen.width * .2f, Screen.height * .06f), "EMP Recovery = " + emprechargetime + "  ($40)");
		if (emprechargetime == 2) { //180 being min time
			GUI.Box (new Rect (Screen.width * (xval1 +.254f), Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), "- 0.5");
		} else {
			if (cash >= 40 && GUI.Button (new Rect (Screen.width * (xval1 +.254f), Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), "- 0.5")) {
				emprechargetime -= 0.5f;
				cash -= 40;
			}
			
		}
		yval1+= .08f;
		if (cranelength == 1 || cranelength == startingcranelength) {
			GUI.Box (new Rect (Screen.width * xval1, Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), "- 0.2");
		} else {
			if (GUI.Button (new Rect (Screen.width * xval1, Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), "- 0.2")) {
				cranelength -= 0.2f;
				cash += 40;
			}
			
		}

		GUI.Box (new Rect (Screen.width * (xval1 + .052f), Screen.height * yval1, Screen.width * .2f, Screen.height * .06f), "Crane Length = " + cranelength + "  ($40)");
		if (cranelength == 4) { //4 being max time
			GUI.Box (new Rect (Screen.width * (xval1 +.254f), Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), "+ 0.25");
		} else {
			if (cash >= 40 && GUI.Button (new Rect (Screen.width * (xval1 +.254f), Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), "+ 0.2")) {
				cranelength += 0.2f;
				cash -= 40;
			}
			
		}
		yval1+= .08f;
		if (tubesleft == startingtubesleft) {
			GUI.Box (new Rect (Screen.width * xval1, Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), "- 10");
		} else {
			if (GUI.Button (new Rect (Screen.width * xval1, Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), "- 10")) {
				tubesleft -= 10;
				cash += 10;
			}
			
		}
		
		GUI.Box (new Rect (Screen.width * (xval1 + .052f), Screen.height * yval1, Screen.width * .2f, Screen.height * .06f), "Tube Length= " + tubesleft + "  ($10)");
		if (tubesleft == 500) { //4 being max time
			GUI.Box (new Rect (Screen.width * (xval1 +.254f), Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), "+ 10");
		} else {
			if (cash >= 10 && GUI.Button (new Rect (Screen.width * (xval1 +.254f), Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), "+ 10")) {
				tubesleft += 10;
				cash -= 10;
			}
		}
		yval1+= .08f;
		if (capacity == 3 || capacity == startingcapacity) {
			GUI.Box (new Rect (Screen.width * xval1, Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), "- 1");
		} else {
			if (GUI.Button (new Rect (Screen.width * xval1, Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), "- 1")) {
				capacity -= 1;
				cash += 200;
			}
			
		}
		
		GUI.Box (new Rect (Screen.width * (xval1 + .052f), Screen.height * yval1, Screen.width * .2f, Screen.height * .06f), "Item Capacity = " + capacity + "  ($200)");
		if (capacity == 10) { //4 being max time
			GUI.Box (new Rect (Screen.width * (xval1 +.254f), Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), "+ 1");
		} else {
			if (cash >= 200 && GUI.Button (new Rect (Screen.width * (xval1 +.254f), Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), "+ 1")) {
				capacity += 1;
				cash -= 200;
			}
			
		}

		/*GUI.Box (new Rect (Screen.width * xval1, Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), "-");
		GUI.Box (new Rect (Screen.width * (xval1 + .052f), Screen.height * yval1, Screen.width * .2f, Screen.height * .06f), "Armor = " + armor);
		GUI.Box (new Rect (Screen.width * (xval1 +.254f), Screen.height * yval1, Screen.width * .05f, Screen.height * .06f), "+");
*/
		
	}
}
