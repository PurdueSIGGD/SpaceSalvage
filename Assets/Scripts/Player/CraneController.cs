using UnityEngine;
using System.Collections;

public class CraneController : MonoBehaviour {
	public GameObject Player;
	public GameObject focus;
	public Material ropemat;
	public PhysicsMaterial2D mate;
	public Vector3 current;
	public bool emp; 
	public bool debugmode;
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
	//private Vector3 lastending;
	private bool retracting;
	private float thetaersnenig;
	private bool releaseready;
	private bool phisretract;
	private float launchangle;
	private float lastendingangle;
	private float firstfocusangle;
	public float linewidth;

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
			if (debugmode) {
				cranelength = 3;

			}
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
		//print (ending.position);
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

		Player.transform.rotation = Quaternion.Euler(0,0,  (thetaersnenig + 90)); //set player rotation, 90 because they did not start at 0 degrees
		float dist = Vector3.Distance(new Vector3(ending.transform.position.x, ending.transform.position.y, 0) - new Vector3(Player.transform.position.x, Player.transform.position.y, 0), Vector3.zero);
		////print (Mathf.Cos(thetaersnenig) * dist * Time.deltaTime)

		if (Input.GetMouseButton(0) && !grabbed && !retracting && !emp) {
			if (!firing) {
				//print("Initial force shot");
				launchangle = thetaersnenig;
				////print (40 * HarpoonSpeed * new Vector3(this.transform.position.x + Mathf.Cos (Mathf.Deg2Rad * thetaersnenig) , this.transform.position.y + Mathf.Sin (Mathf.Deg2Rad * thetaersnenig), 0) - this.transform.position);
				ending.rigidbody2D.AddForce(40 * HarpoonSpeed * new Vector3(/*Player.rigidbody2D.velocity.x*/ + Mathf.Cos (Mathf.Deg2Rad * launchangle) , /*Player.rigidbody2D.velocity.y*/ + Mathf.Sin (Mathf.Deg2Rad * launchangle), 0) - this.transform.position);
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
				this.GetComponent<LineRenderer>().enabled = true;
				ending.rigidbody2D.velocity = (Vector3)Player.rigidbody2D.velocity + ((this.transform.position - ending.position) + ( (.5f) * (this.transform.position - ending.position )));
				//print("rectracting");
				////print (this.transform.position);
				//current =  (this.transform.position - current);
				if (Mathf.Abs(this.transform.position.x - ending.position.x) < .5f && Mathf.Abs(this.transform.position.y - ending.position.y) < .5f) {
					//print("back home");
					releaseready = false;
					retracting = false;
					firing = false;

				}
			} 
			if (!Input.GetMouseButton(0) && grabbed && !focus.GetComponent<RopeScript2D>().brokenrope) {
				releaseready = true;
			}
			if (grabbed && Input.GetMouseButton(0) && releaseready && !focus.GetComponent<RopeScript2D>().brokenrope) {
				// release grip
				grabbed = false;
				firing = false;
				retracting = true;
				focus.BroadcastMessage("DestroyRope");

			}
		} 

		if (!firing && !retracting && !grabbed) {
			//print("Coming Back");
			ending.position = this.transform.position + (2 * Vector3.back);
			ending.rotation = Quaternion.Euler(0,0,  (thetaersnenig));
		} else {
			if (grabbed) {
				if (focus.GetComponent<RopeScript2D>().hinger != null) { //update while connected

					ending.transform.position = focus.GetComponent<RopeScript2D>().hinger.transform.position;
					ending.eulerAngles = new Vector3(0,0, lastendingangle + (focus.transform.eulerAngles.z - firstfocusangle ));
					//print(lastendingangle);
				}
			} else {
				//ending.rotation = Quaternion.Euler(0,0,45 + Vector3.Angle(Player.transform.position, ending.transform.position));
				ending.eulerAngles = new Vector3(0,0,launchangle);
				//print(Vector3.Angle(Player.transform.position, ending.transform.position));

			}
			//lastending = ending.position;
		}
		
		LineRenderer l = (LineRenderer)GetComponent<LineRenderer> ();
		if (!grabbed) {
			l.enabled = true;
			l.SetPosition(0, Player.transform.position);
			l.SetPosition(1, ending.position);	
		} else {
			l.enabled = false;
		}
		if (!grabbed && (firing || retracting)) {
			Collider2D[] hitColliders = Physics2D.OverlapCircleAll(ending.transform.position, .1f); 
			foreach (Collider2D c in hitColliders) {
				//print(c);
				if (c != null && c.gameObject != Player && !c.isTrigger && c.GetComponent<RigidIgnorer>() == null && !releaseready) {
					retracting = true;
					firing = false;
					if (c.GetComponent("ItemPickup") != null) {
						releaseready = false;
						//print ("Got one");
						focus = c.gameObject;
						firing = false;
						retracting = false;
						focus.rigidbody2D.isKinematic = false;
						//ending.transform.position = Player.transform.position;
						//ending.transform.rigidbody2D.velocity = Vector2.zero;
						grabbed = true;
						this.lastendingangle = ending.eulerAngles.z;
						firstfocusangle = focus.transform.eulerAngles.z;
						//GameObject child = new GameObject("Connector");

						//focus.transform.parent = focus.transform;

						//print (Player.transform.position);
						//print(ending.transform.localPosition);
						//print("Crane : " + ending.transform.position + "  sent coords: " + child.transform.position);

						LineRenderer lr = focus.gameObject.GetComponent<LineRenderer>();
						if (lr == null) {
							lr = focus.gameObject.AddComponent<LineRenderer>();
						}

						lr.SetWidth(linewidth,linewidth);
						lr.material = this.ropemat;
						//lr.material = new Material(Resources.Load<Material>("/Sprites/Materials/Tubemat.mat"));
						lr.SetColors(new Color(0,0,0),new Color(0,0,0));
						lr.sortingLayerName = "Foreground";
						RopeScript2D rp = focus.GetComponent<RopeScript2D>();
						if (rp == null) {

							rp = focus.gameObject.AddComponent<RopeScript2D>();
							rp.parent = this.transform;

						}
						rp.linewidth = this.linewidth;
						Rigidbody2D rg = focus.GetComponent<Rigidbody2D>();
						if (rg == null) {
							rg = focus.gameObject.AddComponent<Rigidbody2D>();
						}
						rg.gravityScale = 0;
						//rg.isKinematic = true;
						/*RigidIgnorer ri = focus.gameObject.GetComponent<RigidIgnorer>();
						if (ri == null) {
							focus.gameObject.AddComponent<RigidIgnorer>();
						}*/
						rp.iscrane = true;

						rp.ropemat = this.ropemat;
						rp.ropeColRadius = 0.03f;
						rp.target = Player.transform;
						rp.frequency = .5f;
						rp.dampening = 10;
						rp.resolution = 2;
						rp.ropeDrag = 0.01f;
						rp.ropeMass = .5f;
						rp.ropeColRadius = 0.1f;
						//print(ending.position);
						rp.SendMessage("SetTargetAnchor",(ending.position));
						rp.SendMessage("BuildRope");
						rp.mate = mate;
						lr.enabled = false;

					}
				}


			}
			//Collider2D hitCollider = Physics2D.OverlapCircle(Vector2.zero,0);
			//Physics2D.IgnoreCollision(hitCollider, GameObject.Find("Ship").collider2D);
			//hitCollider = Physics2D.OverlapCircle(ending.transform.position, .1f);

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

	void FixedUpdate () {

		if (rot && grabbed) {

			////print ("Rotate");
			//focus.transform.rotation = new Quaternion(0,0,Time.DeltaTime,0);
			focus.transform.Rotate(Time.deltaTime * 40 * Vector3.back);
			//focus.transform.RotateAround(Vector3.zero, current, Time.deltaTime * 50);
		}
	
		


	}
}
