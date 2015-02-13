using UnityEngine;
using System.Collections;

public class Airlock : MonoBehaviour {

	public bool on = true;
	public bool closed;
	public bool leftside = true;
	private Vector3 topposleft;
	private Vector3 bottomposleft;
	private Vector3 topposright;
	private Vector3 bottomposright;
	private float time = 0;
	private float cooldowntime = 10;
	// Use this for initialization
	void Start () {

		Physics2D.IgnoreCollision(transform.FindChild("Door1left").collider2D  , collider2D);
		Physics2D.IgnoreCollision(transform.FindChild("Door2left").collider2D  , collider2D);
		Physics2D.IgnoreCollision(transform.FindChild("Door1right").collider2D  , collider2D);
		Physics2D.IgnoreCollision(transform.FindChild("Door2right").collider2D  , collider2D); //doors won't set off the airlock

		Physics2D.IgnoreCollision(transform.FindChild("TopWall").collider2D  , collider2D);
		Physics2D.IgnoreCollision(transform.FindChild("BottomWall").collider2D  , collider2D); //top and bottom parts won't set off the airlock

		/*Transform[] ts = this.transform.GetComponentsInChildren<Transform>(); //This code isn't working, just gonna edit the collision boxes instead
		foreach (Transform t in ts) {
			Physics2D.IgnoreCollision(transform.FindChild ("Top").collider2D, t.collider2D);
		}
		/*Physics2D.IgnoreCollision(transform.FindChild("Top").collider2D  , transform.FindChild("Door1left").collider2D);
		Physics2D.IgnoreCollision(transform.FindChild("Bottom").collider2D  , transform.FindChild("Door2left").collider2D);
		Physics2D.IgnoreCollision(transform.FindChild("Top").collider2D  , transform.FindChild("Door1right").collider2D);
		Physics2D.IgnoreCollision(transform.FindChild("Bottom").collider2D  , transform.FindChild("Door2right").collider2D);
		*/
		topposleft = transform.FindChild("Door1left").position;
		bottomposleft = transform.FindChild("Door2left").position;
		topposright = transform.FindChild("Door1right").position;
		bottomposright = transform.FindChild("Door2right").position;

	}
	
	// Update is called once per frame
	void OnTriggerStay2D(Collider2D col) {
		//print ("Poop");
		if (on && col.name.Equals ("Player")) {

			time+=Time.deltaTime;
			if (time > 3 && closed != true && cooldowntime > 10) {
				//print (time);
				closed = true;


			}
			if (cooldowntime < 10) {
				time = 0;
			}
		}
	}
	void OnTriggerExit2D(Collider2D col) {
		if (col.name.Equals ("Player")) {
			time = 0;
		}
	}
	void Update () {

		if (closed) {
			((Rigidbody2D)transform.FindChild("Door1left").GetComponent("Rigidbody2D")).velocity =(Time.deltaTime * 20 * ((Vector2)(transform.FindChild("Door2left").position - transform.FindChild("Door1left").position)));
			((Rigidbody2D)transform.FindChild("Door2left").GetComponent("Rigidbody2D")).velocity =(Time.deltaTime * 20 * ((Vector2)(transform.FindChild("Door1left").position - transform.FindChild("Door2left").position)));
			
			((Rigidbody2D)transform.FindChild("Door1right").GetComponent("Rigidbody2D")).velocity =(Time.deltaTime * 20 * ((Vector2)(transform.FindChild("Door2right").position - transform.FindChild("Door1right").position)));
			((Rigidbody2D)transform.FindChild("Door2right").GetComponent("Rigidbody2D")).velocity =(Time.deltaTime * 20 * ((Vector2)(transform.FindChild("Door1right").position - transform.FindChild("Door2right").position)));
			if (leftside) {

				//print (time);

				if (time > 10) {
					leftside = false;
					time = 0;
					cooldowntime = 0;
					closed = false;
					//print ("Open plz");
				}
				//print ("Push");
			} else {
				if (time > 10) {
					leftside = true;
					time = 0;
					cooldowntime = 0;
					closed = false;
					//print ("Open plz");
				}

			}

		} else {
			cooldowntime+= Time.deltaTime;
			if (leftside) {
				((Rigidbody2D)transform.FindChild("Door1left").GetComponent("Rigidbody2D")).velocity = (Time.deltaTime * 30 * ((Vector2)(topposleft - transform.FindChild("Door1left").position))); //opens left doors
				((Rigidbody2D)transform.FindChild("Door2left").GetComponent("Rigidbody2D")).velocity = (Time.deltaTime * 30 * ((Vector2)(bottomposleft - transform.FindChild("Door2left").position)));
				//print ("Opening left side");
				((Rigidbody2D)transform.FindChild("Door1right").GetComponent("Rigidbody2D")).velocity = (Time.deltaTime * 30 * ((Vector2)(transform.FindChild("Door2right").position - transform.FindChild("Door1right").position))); //closes right doors
				((Rigidbody2D)transform.FindChild("Door2right").GetComponent("Rigidbody2D")).velocity = (Time.deltaTime * 30 * ((Vector2)(transform.FindChild("Door1right").position - transform.FindChild("Door2right").position)));
				print (transform.FindChild("Door2right").position);
			} else {
				((Rigidbody2D)transform.FindChild("Door1right").GetComponent("Rigidbody2D")).velocity = (Time.deltaTime * 30 * ((Vector2)(topposright - transform.FindChild("Door1right").position))); //opens right doors
				((Rigidbody2D)transform.FindChild("Door2right").GetComponent("Rigidbody2D")).velocity = (Time.deltaTime * 30 * ((Vector2)(bottomposright - transform.FindChild("Door2right").position)));
				//print ("Opening right side");
				((Rigidbody2D)transform.FindChild("Door1left").GetComponent("Rigidbody2D")).velocity = (Time.deltaTime * 30 * ((Vector2)(transform.FindChild("Door2left").position - transform.FindChild("Door1left").position))); //closes left doors
				((Rigidbody2D)transform.FindChild("Door2left").GetComponent("Rigidbody2D")).velocity = (Time.deltaTime * 30 * ((Vector2)(transform.FindChild("Door1left").position - transform.FindChild("Door2left").position)));

			}

		}
	}
}
