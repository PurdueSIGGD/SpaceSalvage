using UnityEngine;
using System.Collections;

public class ExitScript : MonoBehaviour {
	//private GameObject display;
	// Use this for initialization
	private bool exiting;
	private GameObject go;
	private GameObject faderObject;
	private SpriteRenderer Fader;
	public Vector3 playerseat;
	void Start () {
		if (playerseat == Vector3.zero) {
			playerseat = this.transform.position;
		}
		faderObject = GameObject.Find ("Fader");
		Fader = faderObject.GetComponent<SpriteRenderer> ();
		//go = new GameObject("Return");
		//display = GameObject.Find ("display");
		//go.AddComponent("GUIText");

	}
	void OnCollisionEnter2D(Collision2D col) {

	}

	void OnTriggerExit2D (Collider2D col) {
		//Physics2D.IgnoreCollision(this.collider2D, col);
		//go.guiText.text = "";
	}
	void OnTriggerStay2D(Collider2D col) {
		//go.transform.position = new Vector3 (col.transform.position.x, col.transform.position.y + 40, col.transform.position.z);
		//print ("hi");


		if (Input.GetKey (KeyCode.F) && col.name.Equals("Player") && !exiting) {

			exiting = true;
			print ("Loading new level");
		}
		if (exiting) {
			if (col.name.Equals("Player")) {

				col.SendMessage("EMP");
				col.transform.position = playerseat;
				col.transform.rotation = Quaternion.Euler(0,0,180);
				col.GetComponentInChildren<CraneController>().current = new Vector3(playerseat.x, playerseat.y + .5f, playerseat.z);
			}
			if (Fader.color.a < 1) {
				Fader.transform.localScale = new Vector3(442.6756f, 163.451f, 10);
				Fader.color = new Color(Fader.color.r, Fader.color.g, Fader.color.b, Fader.color.a + Time.deltaTime / 3);
				
			} else {
				Application.LoadLevel("MainMenu");
			}
		}
	}
	// Update is called once per frame
	void Update () {
	
	}
}
