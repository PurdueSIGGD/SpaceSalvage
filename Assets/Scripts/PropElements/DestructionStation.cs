using UnityEngine;
using System.Collections;

public class DestructionStation : MonoBehaviour {
	public float damagerate = 1;	
	private GameObject Player;
	public GameObject particle;
	private float time;

	// Use this for initialization
	void Start () {
		
		//Recognize the player in the game
		Player = GameObject.Find("Player"); 
	}

	void OnTriggerStay2D(Collider2D col){
		time+=Time.deltaTime;
		// create particles at col.OverlapPoint

		/*if (time > .4f && col.rigidbody2D != null && particle != null && col.gameObject.Equals(Player)) {

			GameObject thingy = (GameObject)Instantiate(particle, col.transform.position, Quaternion.identity);
			thingy.GetComponent<SpriteRenderer>().color = new Color(255, 0, 0);
			thingy.rigidbody2D.AddForce(new Vector2(UnityEngine.Random.Range(-50,50), UnityEngine.Random.Range(-50,50)));
			time = 0;
		}*/

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
