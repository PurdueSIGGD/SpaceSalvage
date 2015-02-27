using UnityEngine;
using System.Collections;

public class Airlock : MonoBehaviour {

	public bool on = true;
	public bool closed;
	public bool leftside = true;
	private Vector2 topposleft;
	private Vector2 bottomposleft;
	private Vector2 topposright;
	private Vector2 bottomposright;
	private float time = 0;
	private float cooldowntime = 10;
	private Rigidbody2D Door1left;
	private Rigidbody2D Door2left;
	private Rigidbody2D Door1right;
	private Rigidbody2D Door2right;

	// Use this for initialization
	void Start () {
		Door1left = ((Rigidbody2D)transform.FindChild("Door1left").rigidbody2D);
		Door2left = ((Rigidbody2D)transform.FindChild("Door2left").rigidbody2D);
		Door1right = ((Rigidbody2D)transform.FindChild("Door1right").rigidbody2D);
		Door2right = ((Rigidbody2D)transform.FindChild("Door2right").rigidbody2D);
		RopeCrusher D1l = Door1left.gameObject.AddComponent<RopeCrusher>();
		D1l.partner = Door2left.gameObject;
		RopeCrusher D2l = Door2left.gameObject.AddComponent<RopeCrusher>();
		D2l.partner = Door1left.gameObject;
		RopeCrusher D1r = Door1right.gameObject.AddComponent<RopeCrusher>();
		D1r.partner = Door2right.gameObject;
		RopeCrusher D2r = Door2right.gameObject.AddComponent<RopeCrusher>();
		D2r.partner = Door1right.gameObject;


		Physics2D.IgnoreCollision(Door1left.collider2D  , collider2D);
		Physics2D.IgnoreCollision(Door2left.collider2D  , collider2D);
		Physics2D.IgnoreCollision(Door1right.collider2D  , collider2D);
		Physics2D.IgnoreCollision(Door2right.collider2D  , collider2D); //doors won't set off the airlock

		Physics2D.IgnoreCollision(transform.FindChild("TopWall").collider2D  , collider2D);
		Physics2D.IgnoreCollision(transform.FindChild("BottomWall").collider2D  , collider2D); //top and bottom parts won't set off the airlock

		/*Transform[] ts = this.transform.GetComponentsInChildren<Transform>(); //This code isn't working, just gonna edit the collision boxes instead
		foreach (Transform t in ts) {
			Physics2D.IgnoreCollision(transform.FindChild ("Top").collider2D, t.collider2D);
		}
		/*Physics2D.IgnoreCollision(transform.FindChild("Top").collider2D  , Door1left.collider2D);
		Physics2D.IgnoreCollision(transform.FindChild("Bottom").collider2D  , Door2left.collider2D);
		Physics2D.IgnoreCollision(transform.FindChild("Top").collider2D  , Door1right.collider2D);
		Physics2D.IgnoreCollision(transform.FindChild("Bottom").collider2D  , Door2right.collider2D);
		*/
		topposleft = Door1left.position;
		bottomposleft = Door2left.position;
		topposright = Door1right.position;
		bottomposright = Door2right.position;

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

			//Door1left.AddForce (Time.deltaTime * 200 * ((Vector2)Door2left.position - Door1left.position));
			//Door2left.AddForce(Time.deltaTime * 200 * ((Vector2)Door1left.position - Door2left.position));

			Door1left.velocity = (Time.deltaTime * 20 * ((Vector2)Door2left.position - Door1left.position));
			Door2left.velocity = (Time.deltaTime * 20 * ((Vector2)Door1left.position - Door2left.position));


			Door1left.SendMessage("Closing");
			Door2left.SendMessage("Closing");
			Door1right.SendMessage("Closing");
			Door2right.SendMessage("Closing");
			//Door1right.AddForce(Time.deltaTime * 200 * ((Vector2)Door2right.position - Door1right.position));
			//Door2right.AddForce(Time.deltaTime * 200 * ((Vector2)Door1right.position - Door2right.position));

			Door1right.velocity =(Time.deltaTime * 20 * ((Vector2)Door2right.position - Door1right.position));
			Door2right.velocity =(Time.deltaTime * 20 * ((Vector2)Door1right.position - Door2right.position));
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
				Door1left.velocity = (Time.deltaTime * 30 * (topposleft - Door1left.position)); //opens left doors
				Door2left.velocity = (Time.deltaTime * 30 * (bottomposleft - Door2left.position));
				Door1left.SendMessage("Opening");
				Door2left.SendMessage("Opening");
				//print ("Opening left side");
				Door1right.velocity = (Time.deltaTime * 30 * (Door2right.position - Door1right.position)); //closes right doors
				Door2right.velocity = (Time.deltaTime * 30 * (Door1right.position - Door2right.position));

				Door1right.SendMessage("Closing");
				Door2right.SendMessage("Closing");

				//print (Door2right.position);
			} else {
				Door1right.velocity = (Time.deltaTime * 30 * ((topposright - Door1right.position))); //opens right doors
				Door2right.velocity = (Time.deltaTime * 30 * ((bottomposright - Door2right.position)));
				Door1right.SendMessage("Opening");
				Door2right.SendMessage("Opening");
				//print ("Opening right side");
				Door1left.SendMessage("Closing");
				Door2left.SendMessage("Closing");

				Door1left.velocity = (Time.deltaTime * 30 * (Door2left.position - Door1left.position)); //closes left doors
				Door2left.velocity = (Time.deltaTime * 30 * (Door1left.position - Door2left.position));

			}

		}
	}
}
