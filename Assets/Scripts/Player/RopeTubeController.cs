using UnityEngine;
using System.Collections;

public class RopeTubeController : MonoBehaviour {
	public bool ejected;
	private bool add, sub;
	private float timepassed;
	private float rate = .2f;
	public bool emp, debugmode;
	public int tubesleft = 100;
	private int startingtubes;
	// Use this for initialization
	void Start () {

		if (PlayerPrefs.HasKey("tubesleft")) {

			tubesleft = PlayerPrefs.GetInt("tubesleft");
			if (debugmode) {
				tubesleft = 90;
			}
			startingtubes = tubesleft;
		} else {
			PlayerPrefs.SetInt("tubesleft",tubesleft);
			startingtubes = tubesleft;

		}
		if (tubesleft == 0)  {
			this.gameObject.GetComponent<RopeScript2D>().SendMessage("DeathIsSoon");
		} 
		GameObject.Find("Player").SendMessage("GetTubesLeft",tubesleft);

	}
	void GiveTubesLeft(int i) {
		if (i + tubesleft > 0) PlayerPrefs.SetInt ("tubesleft",i + tubesleft);
		else PlayerPrefs.SetInt("tubesleft",0);


	}
	// Update is called once per frame
	void Update () {
		ejected = (((RopeScript2D)this.GetComponent("RopeScript2D")).ejected);
		if (Input.GetKeyDown (KeyCode.G) && !emp && !ejected) {
			GameObject.Find("Ship").SendMessage("Eject");
		}
		if (ejected) {

		}
		GameObject.Find("Player").SendMessage("GetTubesLeft",tubesleft);
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
		tubesleft--;
	}
	void SubTheRopeAmt() {
		tubesleft++;
	}
	void RopeIsBuilt() {

	}
	void DeathIsSoon() {

	}
	void BrokenRope() {

	}
}
