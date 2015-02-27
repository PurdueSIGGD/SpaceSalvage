using UnityEngine;
using System.Collections;

public class RopeTubeController : MonoBehaviour {
	public bool ejected;
	private bool add, sub;
	private float timepassed;
	private float rate = .2f;
	public bool emp;
	public int tubesleft;
	private int startingtubes;
	// Use this for initialization
	void Start () {
		if (PlayerPrefs.HasKey("tubesleft")) {
			tubesleft = PlayerPrefs.GetInt("tubesleft");
			startingtubes = tubesleft;
			//PlayerPrefs.SetInt("tubesleft",50);
		} else {
			PlayerPrefs.SetInt("tubesleft",tubesleft);
			startingtubes = tubesleft;
		}

	}
	
	// Update is called once per frame
	void Update () {
		ejected = ((RopeScript2D)this.GetComponent("RopeScript2D")).ejected;
		if (Input.GetKeyDown (KeyCode.G) && !emp) {
			this.SendMessage("Eject");
			print ("eject");
		}
		//print(tubesleft);


		timepassed += Time.deltaTime;
		add = (Input.GetKey(KeyCode.E));
		sub = (Input.GetKey(KeyCode.Q));
	}
	void FixedUpdate() {
		if (add && timepassed > rate && !ejected && !emp && tubesleft > 0) {
			SendMessage("AddRope");
			timepassed = 0;
			tubesleft--;
		}
		if (sub && timepassed > rate && !ejected && !emp && tubesleft <= startingtubes) {
			SendMessage("SubRope");
			timepassed = 0;
			tubesleft++;
		}
	}
	void RopeIsBuilt() {

	}
	void DeathIsSoon() {

	}
}
