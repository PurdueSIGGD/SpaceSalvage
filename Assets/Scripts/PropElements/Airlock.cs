using UnityEngine;
using System.Collections;

public class Airlock : MonoBehaviour {
	public GameObject particle;
	public bool on = true;
	public bool closed;
	public bool leftside = true;
	private bool startingclosed;
	private bool startingleftside;
	private bool warmup;
	private bool buttontrigger;
	private Vector2 topposleft;
	private Vector2 bottomposleft;
	private Vector2 topposright;
	private Vector2 bottomposright;
	private float time = 0;
	private float cooldowntime = 10;
	private float particletime;
	private float timesincestart;
	private Rigidbody2D Door1left;
	private Rigidbody2D Door2left;
	private Rigidbody2D Door1right;
	private Rigidbody2D Door2right;
	private Transform spawner1;
	private Transform spawner2;


	// Use this for initialization
	void Start () {
		warmup = true;
		spawner1 = transform.FindChild("Spawner1");
		spawner2 = transform.FindChild("Spawner2");
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
		startingclosed = closed;
		startingleftside = leftside;

		Physics2D.IgnoreCollision(Door1left.collider2D  , collider2D);
		Physics2D.IgnoreCollision(Door2left.collider2D  , collider2D);
		Physics2D.IgnoreCollision(Door1right.collider2D  , collider2D);
		Physics2D.IgnoreCollision(Door2right.collider2D  , collider2D); //doors won't set off the airlock

		Physics2D.IgnoreCollision(transform.FindChild("TopWall").collider2D  , collider2D);
		Physics2D.IgnoreCollision(transform.FindChild("BottomWall").collider2D  , collider2D); //top and bottom parts won't set off the airlock


		topposleft = Door1left.position;
		bottomposleft = Door2left.position;
		topposright = Door1right.position;
		bottomposright = Door2right.position;

	}
	void Open() {
		if (cooldowntime > 10)	buttontrigger = true;

	} 
	void Close() {
		if (cooldowntime > 10)	buttontrigger = true;
	}
	void OnTriggerStay2D(Collider2D col) {
		if (on && col.name.Equals ("Player")) {

			time+=Time.deltaTime;
			if (buttontrigger && closed != true && cooldowntime > 10) {
				buttontrigger = false;
				closed = true;
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
		if (warmup) timesincestart += Time.deltaTime; //this is to make sure the airlock starts properly
		if (timesincestart > 0 && timesincestart < .5f) closed = !startingclosed; 
		if (timesincestart >= .5f && timesincestart <=1) closed = startingclosed; 
		if (timesincestart >= 1 && timesincestart < 1.5f) leftside = !startingleftside; 
		if (timesincestart >= 1.5f && timesincestart < 2) leftside = startingleftside;

		

			if (closed) {


				Door1left.velocity = (Time.deltaTime * 20 * ((Vector2)Door2left.position - Door1left.position));
				Door2left.velocity = (Time.deltaTime * 20 * ((Vector2)Door1left.position - Door2left.position));


				if (time > 10) {
					leftside = false;
					time = 0;
					cooldowntime = 0;
					closed = false;
				} else {
					if (time > 3 && time < 8) {
						this.particletime+= Time.deltaTime;
						if (this.particletime > .2f) {
							GameObject thingy = (GameObject)Instantiate(particle, spawner1.position, Quaternion.identity);
							GameObject thingy2 = (GameObject)Instantiate(particle, spawner2.position, Quaternion.identity);
							thingy.rigidbody2D.AddForce(new Vector2(UnityEngine.Random.Range(-50,50), UnityEngine.Random.Range(-50,0)));
							thingy2.rigidbody2D.AddForce(new Vector2(UnityEngine.Random.Range(-50,50), UnityEngine.Random.Range(-0,50)));

				if (leftside) {


					if (time > 10) {
						leftside = false;
						time = 0;
						cooldowntime = 0;
						closed = false;
					} else {
						if (time > 3 && time < 8) {
							this.particletime+= Time.deltaTime;
							if (this.particletime > .2f) {
								GameObject thingy = (GameObject)Instantiate(particle, spawner1.position, Quaternion.identity);
								GameObject thingy2 = (GameObject)Instantiate(particle, spawner2.position, Quaternion.identity);
								thingy.rigidbody2D.AddForce(new Vector2(UnityEngine.Random.Range(-50,50), UnityEngine.Random.Range(-50,0)));
								thingy2.rigidbody2D.AddForce(new Vector2(UnityEngine.Random.Range(-50,50), UnityEngine.Random.Range(-0,50)));

							}
						}
					}
				} else {
					if (time > 10) {
						leftside = true;
						time = 0;
						cooldowntime = 0;
						closed = false;
					} else {
						if (time > 3 && time < 8) {

							this.particletime+= Time.deltaTime;
							if (this.particletime > .2f) {
								GameObject thingy = (GameObject)Instantiate(particle, spawner1.position, Quaternion.identity);
								GameObject thingy2 = (GameObject)Instantiate(particle, spawner2.position, Quaternion.identity);
								thingy.rigidbody2D.AddForce(new Vector2(UnityEngine.Random.Range(-50,50), UnityEngine.Random.Range(-50,50)));
								thingy2.rigidbody2D.AddForce(new Vector2(UnityEngine.Random.Range(-50,50), UnityEngine.Random.Range(-50,50)));
								Physics2D.IgnoreCollision(thingy.collider2D, GameObject.Find("Player").collider2D);
								Physics2D.IgnoreCollision(thingy2.collider2D, GameObject.Find("Player").collider2D);

							}

						}
					}

				}

			} else {
				cooldowntime+= Time.deltaTime;
				if (leftside) {
					Door1left.velocity = (Time.deltaTime * 30 * (topposleft - Door1left.position)); //opens left doors
					Door2left.velocity = (Time.deltaTime * 30 * (bottomposleft - Door2left.position));
					Door1left.SendMessage("Opening");
					Door2left.SendMessage("Opening");
					Door1right.velocity = (Time.deltaTime * 30 * (Door2right.position - Door1right.position)); //closes right doors
					Door2right.velocity = (Time.deltaTime * 30 * (Door1right.position - Door2right.position));

					Door1right.SendMessage("Closing");
					Door2right.SendMessage("Closing");

				} else {
					Door1right.velocity = (Time.deltaTime * 30 * ((topposright - Door1right.position))); //opens right doors
					Door2right.velocity = (Time.deltaTime * 30 * ((bottomposright - Door2right.position)));
					Door1right.SendMessage("Opening");
					Door2right.SendMessage("Opening");
					Door1left.SendMessage("Closing");
					Door2left.SendMessage("Closing");
					Door1left.velocity = (Time.deltaTime * 30 * (Door2left.position - Door1left.position)); //closes left doors
					Door2left.velocity = (Time.deltaTime * 30 * (Door1left.position - Door2left.position));

				}

			}
		
	}
}
