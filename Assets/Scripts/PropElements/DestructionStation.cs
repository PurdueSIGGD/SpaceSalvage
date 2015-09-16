using UnityEngine;
using System.Collections;

public class DestructionStation : MonoBehaviour {
	//Aka "lasers"
	//Damage player and cut cable


	public float damagerate = 1;	
	private GameObject Player;
	public GameObject particle;

	// Use this for initialization
	void Start () {
		
		//Recognize the player in the game
		Player = GameObject.Find("Player"); 
	}

	void OnTriggerStay2D(Collider2D col){

		if (col.gameObject == Player) {
			attack ();
			GameObject.Find ("Camera").SendMessage ("ShakeOnOff", true); //shaking
			GameObject.Find("Camera").SendMessage("Shake",1);

			return;
		}
		if (col.GetComponent<JointScript> () != null) {
			col.SendMessage("BrokenJoint");
		}
	}
	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject == Player) {
				GameObject.Find ("Camera").SendMessage ("ShakeOnOff", true);

				attack ();
		}
	}
	void OnTriggerExit2D(Collider2D col){
		if (col.gameObject == Player)
			GameObject.Find("Camera").SendMessage("ShakeOnOff", false);

	}

	void attack(){
		Player.SendMessage("changeHealth", 10 *  Time.deltaTime * -1 * damagerate);
	}

	// Update is called once per frame
	void Update () {
		//transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 5);
	}
}
