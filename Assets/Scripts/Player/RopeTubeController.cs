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
		if (tubesleft == 0)  {
			//this.gameObject.GetComponent<RopeScript2D>().SendMessage("DestroyRope");
			this.gameObject.GetComponent<RopeScript2D>().SendMessage("DeathIsSoon");
		} else {

		}

	}
	void GiveTubesLeft(int i) {
		if (i + tubesleft > 0) PlayerPrefs.SetInt ("tubesleft",i + tubesleft);
		else PlayerPrefs.SetInt("tubesleft",0);
		//print(i+tubesleft);


	}
	// Update is called once per frame
	void Update () {
		ejected = (((RopeScript2D)this.GetComponent("RopeScript2D")).ejected);
		if (Input.GetKeyDown (KeyCode.G) && !emp && !ejected) {
			GameObject.Find("Ship").SendMessage("Eject");
		}
		//print(tubesleft);
		if (ejected) {

		}

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
		if (sub && timepassed > rate && !ejected && !emp && tubesleft <= startingtubes - 1) {
			SendMessage("SubRope");
			timepassed = 0;
			tubesleft++;
		}
	}
	void RopeIsBuilt() {

	}
	void DeathIsSoon() {

	}
	void BrokenRope() {

	}
}
