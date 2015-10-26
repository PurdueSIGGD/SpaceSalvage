using UnityEngine;
using System.Collections;

public class RopeTubeController : MonoBehaviour {
	//Controls the rope extending or contracting

	public KeyCode kextend = KeyCode.LeftShift;
	public KeyCode kretract = KeyCode.LeftControl;
	public KeyCode keject = KeyCode.G;

	public bool ejected;
	private bool add, sub;
	private float timepassed;
	private float rate = .2f;
	public bool emp, debugmode;
	public int tubesleft = 100, tubecut, endingtubes;
	public int startingtubes;
	// Use this for initialization
	void Start () {

		if (PlayerPrefs.HasKey("tubesleft")) {

			tubesleft = PlayerPrefs.GetInt("tubesleft");
			if (debugmode) {
				tubesleft = 90;
			}
			startingtubes = tubesleft;
		} else {
			print(tubesleft);
			PlayerPrefs.SetInt("tubesleft",tubesleft);
			startingtubes = tubesleft;

		}

		if (PlayerPrefs.HasKey("tubecut")) {
			tubecut = PlayerPrefs.GetInt("tubecut");
		} else {
			PlayerPrefs.SetInt("tubecut",0);
			
		}

		if (PlayerPrefs.HasKey("Extend")) {
			kextend = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Extend")) ;
		} else {
			PlayerPrefs.SetString("Extend",kextend.ToString());
		}
		if (PlayerPrefs.HasKey("Retract")) {
			kretract = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Retract")) ;
		} else {
			PlayerPrefs.SetString("Retract",kretract.ToString());
		}
		if (PlayerPrefs.HasKey("Eject")) {
			keject = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Eject")) ;
		} else {
			PlayerPrefs.SetString("Eject",keject.ToString());
		}

		if (tubesleft == 0 || tubecut == 1)  {
			this.gameObject.GetComponent<RopeScript2D>().SendMessage("DeathIsSoon");
		} 
		GameObject.Find("Player").SendMessage("GetTubesLeft",tubesleft);
	}
	void GiveTubesLeft(int i) {
		//print(i);
		if (i > 0) tubecut = 1;
		endingtubes = tubesleft + i;
		//print("endingtubes: " + endingtubes);
	}
	// Update is called once per frame
	void Update () {
		if (tubesleft < 0) tubesleft = 0;
		ejected = (((RopeScript2D)this.GetComponent("RopeScript2D")).ejected);
		if (Input.GetKeyDown (keject) && !emp && !ejected) {
			GameObject.Find("Ship").SendMessage("Eject");
		}
		if (ejected) {

		}
		GameObject.Find("Player").SendMessage("GetTubesLeft",tubesleft);
		timepassed += Time.deltaTime;
		add = (Input.GetKey(kextend));
		sub = (Input.GetKey(kretract));
	}
	void FixedUpdate() {
		if (add && timepassed > rate && !ejected && !emp && tubesleft > 0 && !this.GetComponent<RopeScript2D>().brokenrope) {
			SendMessage("AddRope");
			timepassed = 0;
			tubesleft--;
		}
		if (sub && timepassed > rate && !ejected && !emp && tubesleft <= startingtubes - 1&& !this.GetComponent<RopeScript2D>().brokenrope) {
			SendMessage("SubRope");
			timepassed = 0;
		} else {
			if (sub) {
				this.GetComponent<RopeScript2D>().pushing = true;
			} else {
				this.GetComponent<RopeScript2D>().pushing = false;
			}
		}
	}
	void ReconnectAdd() {
		SendMessage("AddRope");
		timepassed = 0;
	
	}
	void SubTheRopeAmt() {
		tubesleft++;
	}
	void Im_Leaving() {
		GiveTubesLeft(0); //so we don't have an ending tube amount of 0;
		if (tubecut == 1) {
			//print(tubesleft);
			PlayerPrefs.SetInt("tubesleft",endingtubes);
			PlayerPrefs.SetInt("tubecut",1);


		}
	}

	void RopeIsBuilt() {

	}
	void DeathIsSoon() {

	}
	void BrokenRope() {

	}
}
