using UnityEngine;
using System.Collections;

public class ExitScript : MonoBehaviour {
	//private GameObject display;
	// Use this for initialization
	private bool exiting;
	private GameObject faderObject;
	private SpriteRenderer Fader;
	private GameObject Player;
	public KeyCode usekey = KeyCode.F;
	public string usestring = "Press 'f' to exit map";
	public Vector3 playerseat;
	void Start () {
		Player = GameObject.Find ("Player");
		if (transform.FindChild("PlayerSeat") != null) {
			playerseat = transform.FindChild("PlayerSeat").position;
		}
		if (PlayerPrefs.HasKey("Use")) {
			usekey = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Use")) ;
		} else {
			PlayerPrefs.SetString("Use",usekey.ToString());
		}
		faderObject = GameObject.Find ("Fader");
		Fader = faderObject.GetComponent<SpriteRenderer> ();

	}


	void Use() {
		if (!exiting) {
			exiting = true;
		}
	}
	void OnTriggerExit2D (Collider2D col) {
		if (col.GetComponent<InteractController>() != null) {

			col.SendMessage("GetMessage", "");
			col.SendMessage("LoseGO");
		}
	}
	void OnTriggerEnter2D(Collider2D col) {
		if (col.GetComponent<InteractController>() != null) {
			col.SendMessage("GetMessage", "Press " + this.usekey.ToString () + " to exit map!");
			col.SendMessage("GetGO", this.gameObject);

		}
	}
	void OnTriggerStay2D(Collider2D col) {
		if ( col.GetComponent<InteractController>() != null) {
			if (exiting) {
				col.SendMessage("GetMessage", "");
			} else {
				col.SendMessage("GetMessage", "Press " + this.usekey.ToString () + " to exit map!");
					col.SendMessage("GetGO", this.gameObject);
					

			}
		}
	}
	// Update is called once per frame
	void Update () {

		if (exiting) {
				
			Player.SendMessage("StopDoingThat"); //stop the health and oxy loss
			Player.SendMessage("EMP");
			Player.transform.position = playerseat + Vector3.back;
			Player.transform.rotation = Quaternion.Euler(0,0,90);
			Player.GetComponentInChildren<CraneController>().current = new Vector3(playerseat.x, playerseat.y + .5f, playerseat.z);
			
			if (Fader.color.a < 1) {
				
				Fader.transform.localScale = new Vector3(442.6756f, 163.451f, 10);
				Fader.color = new Color(Fader.color.r, Fader.color.g, Fader.color.b, Fader.color.a + Time.deltaTime / 3);
				
			} else {
			
				PlayerPrefs.Save(); 
				GameObject.Find("Ship").BroadcastMessage("Im_Leaving");
				GameObject.Find("Player").BroadcastMessage("Im_Leaving");
				
				
				Application.LoadLevel("MissionResults");
			}
		}
	}
}
