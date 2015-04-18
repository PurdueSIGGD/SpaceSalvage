using UnityEngine;
using System.Collections;

public class Airlock : MonoBehaviour {
	public GameObject particle;
	public bool on = true;
	public bool closed;
	public bool leftside = true;

	private bool sound;
	private bool buttontrigger;
	private bool started;
	private float time = 0;
	public float cooldowntime = 10;
	private float particletime;
	private float timesincestart;
	private Transform Door1left;
	private Transform Door2left;
	private Transform Door1right;
	private Transform Door2right;
	private Transform spawner1;
	private Transform spawner2;


	// Use this for initialization
	void Start () {
	
		spawner1 = transform.FindChild("Spawner1");
		spawner2 = transform.FindChild("Spawner2");
		Door1left = transform.FindChild("Door1Left");
		Door2left = transform.FindChild("Door2Left");
		Door1right = transform.FindChild("Door1Right");
		Door2right = transform.FindChild("Door2Right");
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




		if (leftside) {
			Door1left.SendMessage("Open");
			Door2left.SendMessage("Open");

		} else {
			Door1right.SendMessage("Open");
			Door2right.SendMessage("Open");

		}

	}
	void Open() {
		if (cooldowntime > 10) 	buttontrigger = true;

	} 
	void Close() {
		if (cooldowntime > 10)	buttontrigger = true;
	}
	void OnTriggerStay2D(Collider2D col) {
		if (on && col.name.Equals ("Player")) {

			time+=Time.deltaTime;
			if (buttontrigger && closed != true && cooldowntime > 10) {
				started = true;
				buttontrigger = false;
				closed = true;
				time = 0;


			}

		}
	}
	void OnTriggerExit2D(Collider2D col) {

		if (col.name.Equals ("Player")) {
			if (started) {
				started = false;
				if (leftside) {
					leftside = true;
					time = 0;
					cooldowntime = 0;
					closed = false;
					Door1left.SendMessage("Open");
					Door2left.SendMessage("Open");
					sound = false;
				} else {
					leftside = false;
					time = 0;
					cooldowntime = 0;
					closed = false;
					Door1right.SendMessage("Open");
					Door2right.SendMessage("Open");
					sound = false;

				}
			}
			time = 0;
		}
	}
	void Update () {


			if (closed) {



				Door1left.SendMessage("Close");
				Door2left.SendMessage("Close");
				Door1right.SendMessage("Close");
				Door2right.SendMessage("Close");
				
				if (leftside) {


					if (time > 10) {
						
					sound = false;
						started = false;
						leftside = false;
						time = 0;
						cooldowntime = 0;
						closed = false;
						Door1right.SendMessage("Open");
						Door2right.SendMessage("Open");
					} else {
						if (!sound && started) {
							this.GetComponent<AudioSource>().Play();
							sound = true;
						} 
						if (time > 3 && time < 8) {
							this.particletime+= Time.deltaTime;
							if (this.particletime > .2f) {
								GameObject thingy = (GameObject)Instantiate(particle, spawner1.position, Quaternion.identity);
								GameObject thingy2 = (GameObject)Instantiate(particle, spawner2.position, Quaternion.identity);
								thingy.rigidbody2D.AddForce(new Vector2(UnityEngine.Random.Range(-50,50), UnityEngine.Random.Range(-80,0)));
								thingy2.rigidbody2D.AddForce(new Vector2(UnityEngine.Random.Range(-50,50), UnityEngine.Random.Range(0,80)));

							}
						}
					}
				} else {
					if (time > 10) {
					sound = false;
					started = false;
						leftside = true;
						time = 0;
						cooldowntime = 0;
						closed = false;
						Door1left.SendMessage("Open");
						Door2left.SendMessage("Open");
					} else {
						if (!sound && started) {
							this.GetComponent<AudioSource>().Play();
							sound = true;
						} 
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
				//Door1right.SendMessage("Close");
				//Door2right.SendMessage("Close");

				} else {
					//Door1left.SendMessage("Close");
					//Door2left.SendMessage("Close");

				}

			}
		
	}
}
