using UnityEngine;
using System.Collections;

public class ExitScript : MonoBehaviour {
	//Attached to the ship, interacted to exit the area and take items home

	private bool exiting;
	private GameObject faderObject;
	private GUITexture Fader;
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
		Fader = faderObject.GetComponent<GUITexture> ();

	}


	void Use() {
		if (!exiting && Player.GetComponent<HealthController>().GetWallet() >= 20) {
			Player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			Player.GetComponent<Rigidbody2D>().drag = .4f;
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
			if (Player.GetComponent<HealthController>().GetWallet() >= 20) {
				col.SendMessage("GetMessage", "Press " + this.usekey.ToString () + " to exit map");
			} else {
				col.SendMessage("GetMessage", "Must have at least $20 to exit map");

			}
			col.SendMessage("GetGO", this.gameObject);

		}
	}
	void OnTriggerStay2D(Collider2D col) {
		if ( col.GetComponent<InteractController>() != null) {
			if (exiting) {
				col.SendMessage("GetMessage", "");
			} else {
				if (Player.GetComponent<HealthController>().GetWallet() >= 20) {
					col.SendMessage("GetMessage", "Press " + this.usekey.ToString () + " to exit map");
				} else {
					col.SendMessage("GetMessage", "Must have at least $20 to exit map");
					
				}					
				col.SendMessage("GetGO", this.gameObject);
					

			}
		}
	}
	// Update is called once per frame
	void Update () {

		if (exiting) {

			Player.SendMessage("StopDoingThat"); //stop the health and oxy loss
			Player.SendMessage("EMP");
			Player.GetComponent<Rigidbody2D>().AddForce(playerseat- Player.transform.position);
			//Player.transform.position = playerseat + Vector3.back;
			Player.transform.rotation = Quaternion.Euler(0,0,90);
			Player.GetComponentInChildren<CraneController>().current = 10 *  new Vector3(playerseat.x, playerseat.y + .5f, playerseat.z);
			//print(Fader.color.a);
			if (Fader.color.a < .5f) {
				
				Fader.transform.localScale = new Vector3(442.6756f, 163.451f, 10);
				Fader.color = new Color(Fader.color.r, Fader.color.g, Fader.color.b,  Fader.color.a + Time.deltaTime / 4);
				
			} else {
			
				PlayerPrefs.Save(); 
				GameObject.Find("Ship").BroadcastMessage("Im_Leaving"); //get items from the ship
				GameObject.Find("Player").BroadcastMessage("Im_Leaving"); //get player information
				
				
				Application.LoadLevel("MissionResults");
			}
		}
	}
}
