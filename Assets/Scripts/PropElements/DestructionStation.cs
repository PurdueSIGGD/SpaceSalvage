using UnityEngine;
using System.Collections;

public class DestructionStation : MonoBehaviour {
	public float damagerate = 1;	
	private GameObject Player;

	// Use this for initialization
	void Start () {
		
		//Recognize the player in the game
		Player = GameObject.Find("Player"); 
	}

	void OnTriggerStay2D(Collider2D col){
		// create particles at col.OverlapPoint
		if (col.gameObject == Player) {
			attack ();
			return;
		}
		if (col.GetComponent<JointScript> () != null) {
			col.SendMessage("BrokenJoint");
		}
	}
	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject == Player)
			attack ();
	}

	void attack(){
		Player.SendMessage("changeHealth", 10 *  Time.deltaTime * -1 * damagerate);
	}

	// Update is called once per frame
	void Update () {
		//transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, transform.eulerAngles.z + 5);
	}
}
