using UnityEngine;
using System.Collections;

public class CraneController : MonoBehaviour {
	public GameObject Player;
	public GameObject focus;
	public Vector3 current;
	public bool emp;
	public float rotspeed = 300;
	private bool firing;
	private Vector3 pz;
	private Vector3 delta;
	private Vector3 playerdelta;
	private Vector3 difference;
	private float lastTheta;
	private float lastDeltaTheta;
	public float movespeed = .5f;
	public float changedmovespeed;
	public float cranelength = 2;
	public float HarpoonSpeed;
	private float lengthx, lengthy;
	public bool grabbed = false;
	private bool rot = false; 
	public bool ended = false;
	private bool heislettinggo = false;
	private bool retracting;
	private float thetaersnenig;
	private bool releaseready;
	private float launchangle;
	private float lastval;

	// The boolean deadThrusters is here to make the clockwise/counter-clockwise thrusters
	// not appear when the screen turns black
	public bool deadThrusters = false;

	// Use this for initialization
	void Start () {

		if (PlayerPrefs.HasKey("movespeed")) {
			movespeed = PlayerPrefs.GetFloat("movespeed");
		} else {
			PlayerPrefs.SetFloat("movespeed",movespeed);
		}
		if (PlayerPrefs.HasKey ("cranelength")) {
			cranelength = PlayerPrefs.GetFloat("cranelength");
		} else {
			PlayerPrefs.SetFloat("cranelength", cranelength);
		}
		Player = GameObject.Find("Player");
		Transform ending = transform.FindChild("Ending"); //sprite at the end 
		//Physics2D.IgnoreCollision (Player.collider2D, ending.collider2D);
		current = Player.transform.position;
		changedmovespeed = movespeed;
		lastTheta = Player.transform.rotation.z;
		lastDeltaTheta = 0;
		//emp = false;
	}

	// Update is called once per frame
	void Update () {
		this.transform.position = Player.transform.position;
		//current = this.transform.position;
		Transform ending = transform.FindChild("Ending"); //sprite at the end 
		//////print (ending.localPosition);
		////print ("regular - " + ending.position);
		//ending.position = transform.position; //for now

		pz = Camera.main.ScreenToWorldPoint(Input.mousePosition); //the current mouse position
		pz.z = 0;
		if (!firing && !retracting) {
			thetaersnenig = (Mathf.Atan( ((pz.y - Player.transform.position.y) /(pz.x - Player.transform.position.x)))); //angle from mouse to me, formatting later
			thetaersnenig = thetaersnenig/2;
			if (thetaersnenig < 0) {
				thetaersnenig+= Mathf.PI/2;
			}
			if (pz.y - Player.transform.position.y < 0) {
				thetaersnenig+= Mathf.PI/2;
			}
			thetaersnenig = thetaersnenig * 2 * Mathf.Rad2Deg; //fooooormatting

			lastDeltaTheta = thetaersnenig - lastTheta; //change in angles
			//lastTheta = thetaersnenig; //set the last angle
		}
		float newangle = Player.transform.eulerAngles.z;
		//print ((((newangle % 360) - (thetaersnenig % 360)) % 360) + 360);
		/*float val = (((newangle % 360) - (thetaersnenig % 360)) % 360) + 360;
		if ( !(val < 451 && val > 449) && !(val < 91 && val > 89)) {
			if ( val < lastval) {
				newangle += rotspeed * 120 *  Time.deltaTime;
				//print ("adding");
			} else {
				newangle -= rotspeed * 120 *  Time.deltaTime;
				//print ("subtracting");
			}
		}
		lastval = val;*/
		Player.transform.rotation = Quaternion.Euler(0,0,  (thetaersnenig + 90)); //set player rotation, 90 because they did not start at 0 degrees
		float dist = Vector3.Magnitude (Player.transform.position - ending.position);
		////print (Mathf.Cos(thetaersnenig) * dist * Time.deltaTime);

		if (Input.GetMouseButton(0) && !grabbed && !retracting) {
			if (!firing) {
				//print("Initial force shot");
				launchangle = thetaersnenig;
				////print (40 * HarpoonSpeed * new Vector3(this.transform.position.x + Mathf.Cos (Mathf.Deg2Rad * thetaersnenig) , this.transform.position.y + Mathf.Sin (Mathf.Deg2Rad * thetaersnenig), 0) - this.transform.position);
				ending.rigidbody2D.AddForce(40 * HarpoonSpeed * new Vector3(Player.rigidbody2D.velocity.x + Mathf.Cos (Mathf.Deg2Rad * launchangle) , Player.rigidbody2D.velocity.y + Mathf.Sin (Mathf.Deg2Rad * launchangle), 0) - this.transform.position);
			}
			firing = true;
			////print ("pshhhh");
			//print (dist);
			if (dist > cranelength) {
				retracting = true;
			}
			/*lengthx = lengthx + (Mathf.Cos (Mathf.Deg2Rad * thetaersnenig) * HarpoonSpeed * Time.deltaTime);
			lengthy = lengthy + (Mathf.Sin (Mathf.Deg2Rad * thetaersnenig) * HarpoonSpeed * Time.deltaTime);
			current = new Vector3(ending.position.x + lengthx,ending.position.y+lengthy, ending.position.z);*/
		} else {

			if (firing) {
				//print("Done firing");
				firing = false;
				retracting = true;
			}

			if (retracting) {
				//lengthx = lengthx - (Mathf.Cos (Mathf.Deg2Rad * thetaersnenig) * 5 * HarpoonSpeed * Time.deltaTime);
				//lengthy = lengthy - (Mathf.Sin (Mathf.Deg2Rad * thetaersnenig) * 5 * HarpoonSpeed * Time.deltaTime);
				//current = new Vector3(ending.position.x + lengthx,ending.position.y+lengthy, ending.position.z);
				ending.rigidbody2D.velocity = ((this.transform.position - ending.position) + ( (.5f) * (this.transform.position - ending.position )));
				//print("rectracting");
				////print (this.transform.position);
				//current =  (this.transform.position - current);
				if (Mathf.Abs(this.transform.position.x - ending.position.x) < .5f && Mathf.Abs(this.transform.position.y - ending.position.y) < .5f) {
					//print("back home");
					retracting = false;
					firing = false;

				}
			} 
			if (!Input.GetMouseButton(0) && grabbed) {
				releaseready = true;
			}
			if (grabbed && Input.GetMouseButton(0) && releaseready) {
				focus.SendMessage("DestroyRope");
			}
		} 
		/*if (grabbed && Input.GetMouseButton(0)) {
			focus.SendMessage("DestroyRope");
		}*/
		if (!firing && !retracting) {
			ending.position = this.transform.position;
			ending.rotation = Quaternion.Euler(0,0,  (thetaersnenig));
		} else {
			ending.rotation = Quaternion.Euler(0,0,  (launchangle));
		}
		//ending.position = current;
		LineRenderer l = (LineRenderer)GetComponent<LineRenderer> ();
		l.SetPosition(0, Player.transform.position);
		l.SetPosition(1, ending.position);
		if (!grabbed && (firing || retracting)) {
			Collider2D hitCollider = Physics2D.OverlapCircle(ending.transform.position, .1f);
			if (hitCollider != null && hitCollider.gameObject != Player && !hitCollider.isTrigger) {
				retracting = true;
				firing = false;
				if (hitCollider.GetComponent("ItemPickup") != null) {
					releaseready = false;
					//print ("Got one");
					focus = hitCollider.gameObject;
					firing = false;
					retracting = false;
					ending.transform.position = Player.transform.position;
					ending.transform.rigidbody2D.velocity = Vector2.zero;
					grabbed = true;

					LineRenderer lr = focus.gameObject.GetComponent<LineRenderer>();
					if (lr == null) {
						lr = focus.gameObject.AddComponent<LineRenderer>();
					}

					lr.SetWidth(.05f,.05f);

					//lr.material = new Material(Resources.Load<Material>("/Sprites/Materials/Tubemat.mat"));
					lr.SetColors(new Color(0,0,0),new Color(0,0,0));
					lr.sortingLayerName = "Foreground";
					RopeScript2D rp = focus.GetComponent<RopeScript2D>();
					if (rp == null) {
						rp = focus.gameObject.AddComponent<RopeScript2D>();
					}


					rp.target = Player.transform;
					rp.resolution = 3;
					rp.ropeDrag = 0.01f;
					rp.ropeMass = .05f;
					rp.ropeColRadius = 0.1f;
					rp.SendMessage("BuildRope");

					rp.SendMessage("SetTargetAnchor",(Player.transform.position + ending.position));
				}
			}
		} else {

			
		}
		/*

		if (Input.GetMouseButtonDown(0)) {
			if (!grabbed) {
				Collider2D hitCollider = Physics2D.OverlapCircle(current, .1f);
					if (hitCollider != null) {
						if (hitCollider.GetComponent("ItemPickup") != null) {
							////print ("Got one");
							focus = hitCollider.gameObject;
							grabbed = true;
						 
							focus.transform.rigidbody2D.angularVelocity = 0;
							Physics2D.IgnoreCollision(focus.collider2D, GameObject.Find("Player").collider2D);
						((PlayerMovement)Player.GetComponent("PlayerMovement")).moverate = ((PlayerMovement)Player.GetComponent("PlayerMovement")).moverate / 2;
						}
					}
			} else {
				grabbed = false;

			}
		}
	
		rot = Input.GetMouseButton(1);
		if (playerdelta != Player.transform.position) { //keep it up with the player
			current += (Player.transform.position - playerdelta);
		}
LineRenderer l = (LineRenderer)GetComponent<LineRenderer> ();
		float dist = Vector3.Magnitude (Player.transform.position - pz);
		if (dist > cranelength) {
		
			int xval = 1, yval = 1;

			if (pz.x + Player.transform.position.x < 0) {
				xval *= -1;
			}
			if (pz.y + Player.transform.position.y < 0) {
				yval *= -1;
			}
			if (Mathf.Abs(Player.transform.position.x) > Mathf.Abs(pz.x)) {
				xval*=-1;
			}
			if (Mathf.Abs(Player.transform.position.y) > Mathf.Abs(pz.y)) {
				yval*=-1;
			}
	
			float theta = Mathf.Atan((Player.transform.position.y - pz.y)/ (Player.transform.position.x - pz.x));
		
			pz = new Vector3((xval * Mathf.Abs((cranelength) * Mathf.Cos(theta))) + Player.transform.position.x, (yval * Mathf.Abs((cranelength) * 1 * Mathf.Sin (theta))) + Player.transform.position.y);

		
		} else {
			changedmovespeed = movespeed;
		
		}
		////print (delta);

		if (!emp) {

			current += ((pz - current) * Time.deltaTime * changedmovespeed);
		}
		//current = pz + transform.position;
		current.z = .02f;
		l.SetPosition(0, Player.transform.position);
		l.SetPosition(1, current);
	

		if (grabbed) {
			////print (focus.transform.rigidbody2D.angularVelocity);
			if (!ended) {
				difference = focus.transform.position - current;
			}
			/*bool canmove = true;
			if (focus) {
				////print (focus.collider2D.bounds.max);
				Collider2D projectedItem = Physics2D.OverlapArea(focus.collider2D.bounds.min + difference, focus.collider2D.bounds.min + difference);
				if (projectedItem != null && projectedItem.transform.position.z == focus.transform.position.z) {
					//print("Gonna collide");
					canmove = false;
				}
			}
			delta = focus.transform.position;
			//if (canmove) {
			focus.transform.position = current + difference;
			//}
			ended = true;
			
		} else {
			if (ended ) {
				((PlayerMovement)Player.GetComponent("PlayerMovement")).moverate = ((PlayerMovement)Player.GetComponent("PlayerMovement")).moverate * 2 ;
				Physics2D.IgnoreCollision(focus.collider2D, GameObject.Find("Player").collider2D, false);
				if (!heislettinggo) ((Rigidbody2D)focus.GetComponent("Rigidbody2D")).velocity = 60 * (focus.transform.position - delta);
				heislettinggo = false;
				ended = false;
			}
		}

		Transform ending = transform.FindChild("Ending");
		ending.position = (new Vector3(current.x, current.y, 0));
		float thetaersnenig = (Mathf.Atan( ((ending.position.y - Player.transform.position.y) /(ending.position.x - Player.transform.position.x))));
		thetaersnenig = thetaersnenig/2;
		if (thetaersnenig < 0) {
			thetaersnenig+= Mathf.PI/2;
		}
		if (current.y - Player.transform.position.y < 0) {
			thetaersnenig+= Mathf.PI/2;
		}
		thetaersnenig = thetaersnenig * 2 * Mathf.Rad2Deg;
		////print	(thetaersnenig + "   " +  ending.rotation.eulerAngles.z);
		SpriteRenderer ThrusterCW = (SpriteRenderer)GameObject.Find ("ThrusterCW").GetComponent ("SpriteRenderer");
		SpriteRenderer ThrusterCCW = (SpriteRenderer)GameObject.Find ("ThrusterCCW").GetComponent ("SpriteRenderer");
		////print (ThrusterCW.color.a);
		if (Mathf.Abs (lastTheta - thetaersnenig) > 1) {
				if (thetaersnenig < lastTheta) {
						if (ThrusterCW.color.a < 1 && !deadThrusters)
								ThrusterCW.color = new Color (ThrusterCW.color.r, ThrusterCW.color.g, ThrusterCW.color.b, ThrusterCW.color.a + Time.deltaTime * 10);
						if (ThrusterCCW.color.a > 0)
								ThrusterCCW.color = new Color (ThrusterCCW.color.r, ThrusterCCW.color.g, ThrusterCCW.color.b, ThrusterCCW.color.a - Time.deltaTime * 10);

				} else {
						if (ThrusterCW.color.a > 0)
								ThrusterCW.color = new Color (ThrusterCW.color.r, ThrusterCW.color.g, ThrusterCW.color.b, ThrusterCW.color.a - Time.deltaTime * 10);
				if (ThrusterCCW.color.a < 1 && !deadThrusters)
								ThrusterCCW.color = new Color (ThrusterCCW.color.r, ThrusterCCW.color.g, ThrusterCCW.color.b, ThrusterCCW.color.a + Time.deltaTime * 10);
			    }
		} else {

			if (ThrusterCW.color.a > 0) ThrusterCW.color = new Color(ThrusterCW.color.r, ThrusterCW.color.g, ThrusterCW.color.b, ThrusterCW.color.a - Time.deltaTime * 8);
			if (ThrusterCCW.color.a > 0) ThrusterCCW.color = new Color(ThrusterCCW.color.r, ThrusterCCW.color.g, ThrusterCCW.color.b, ThrusterCCW.color.a - Time.deltaTime * 8);
		}
		lastDeltaTheta = thetaersnenig - lastTheta;
		lastTheta = thetaersnenig;
		ending.rotation = Quaternion.Euler(0,0,  (thetaersnenig));
		Player.transform.rotation = Quaternion.Euler(0,0,  (thetaersnenig + 90));

		playerdelta = Player.transform.position;
	*/
		SpriteRenderer ThrusterCW = (SpriteRenderer)GameObject.Find ("ThrusterCW").GetComponent ("SpriteRenderer");
		SpriteRenderer ThrusterCCW = (SpriteRenderer)GameObject.Find ("ThrusterCCW").GetComponent ("SpriteRenderer");
		////print (ThrusterCW.color.a);
		/// 
		//if (lastTheta != thetaersnenig) {
		if (Mathf.Abs (lastTheta - thetaersnenig) > 1) {
			if (thetaersnenig < lastTheta) {
				if (ThrusterCW.color.a < 1 && !deadThrusters)
					ThrusterCW.color = new Color (ThrusterCW.color.r, ThrusterCW.color.g, ThrusterCW.color.b, ThrusterCW.color.a + Time.deltaTime * 5);
				if (ThrusterCCW.color.a > 0)
					ThrusterCCW.color = new Color (ThrusterCCW.color.r, ThrusterCCW.color.g, ThrusterCCW.color.b, ThrusterCCW.color.a - Time.deltaTime * 5);
				
			} else {
				if (ThrusterCW.color.a > 0)
					ThrusterCW.color = new Color (ThrusterCW.color.r, ThrusterCW.color.g, ThrusterCW.color.b, ThrusterCW.color.a - Time.deltaTime * 5);
				if (ThrusterCCW.color.a < 1 && !deadThrusters)
					ThrusterCCW.color = new Color (ThrusterCCW.color.r, ThrusterCCW.color.g, ThrusterCCW.color.b, ThrusterCCW.color.a + Time.deltaTime * 5);
			}
		} else {
			
			if (ThrusterCW.color.a > 0) ThrusterCW.color = new Color(ThrusterCW.color.r, ThrusterCW.color.g, ThrusterCW.color.b, ThrusterCW.color.a - Time.deltaTime * 8);
			if (ThrusterCCW.color.a > 0) ThrusterCCW.color = new Color(ThrusterCCW.color.r, ThrusterCCW.color.g, ThrusterCCW.color.b, ThrusterCCW.color.a - Time.deltaTime * 8);
		}
		lastTheta = thetaersnenig;
	}
	void I_am_letting_go_now () {
		heislettinggo = true;


	}
	void FixedUpdate () {

		if (rot && grabbed) {

			////print ("Rotate");
			//focus.transform.rotation = new Quaternion(0,0,Time.DeltaTime,0);
			focus.transform.Rotate(Time.deltaTime * 40 * Vector3.back);
			//focus.transform.RotateAround(Vector3.zero, current, Time.deltaTime * 50);
		}
	
		


	}
}
