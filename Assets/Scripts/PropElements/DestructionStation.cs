using UnityEngine;
using System.Collections;

public class DestructionStation : MonoBehaviour {
	
	private GameObject Player;

	// Use this for initialization
	void Start () {
		
		//Recognize the player in the game
		Player = GameObject.Find("Player"); 
	}

	void OnTriggerStay2D(Collider2D col){
		if (col.gameObject == Player)
						attack ();
	}
	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject == Player)
			attack ();
	}

	void attack(){

		if (Player.GetComponent<PlayerCollisionController> ().health > 0) {
				Player.GetComponent<PlayerCollisionController> ().health -= Time.deltaTime * 20;


			/*When the health is greater than 0, the oxygen only decreases when the player is attached
			 to the tube*/
			if(Player.GetComponent<RopeTubeController>().ejected)
				Player.GetComponent<PlayerCollisionController> ().oxy -= Time.deltaTime*10;
	
		}

		else {
			Player.GetComponent<PlayerCollisionController> ().health = 0;
			Player.GetComponent<PlayerCollisionController> ().oxy -= Time.deltaTime*10;


			/*the "DeathIsSoon" message is not sent when the player is not connected to the tube controller*/
			if(Player.GetComponent<RopeTubeController>().ejected == false){
				Player.GetComponent<RopeTubeController>().SendMessage("DeathIsSoon");
			}
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
