using UnityEngine;
using System.Collections;

public class CraneController : MonoBehaviour {

	public bool opened;
	public AudioClip firecrane;
	public GameObject Player, focus;
	public Material ropemat;
	public PhysicsMaterial2D mate;
	public Vector3 current;
	public bool emp, debugmode, grabbed = false, ended = false, broken, deadThrusters = false;
	public float rotspeed = 300, movespeed = .5f, changedmovespeed, cranelength = 2, HarpoonSpeed, linewidth;
	private Vector3 pz, delta, playerdelta, difference;
	private float lastTheta, lengthx, lengthy, thetaersnenig, launchangle, lastendingangle, firstfocusangle, brokentime, ClosingTime;
	private bool retracting, releaseready, firing, pause;
	private Transform ending;

	// The boolean deadThrusters is here to make the clockwise/counter-clockwise thrusters
	// not appear when the screen turns black


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
		ending = transform.FindChild("Ending"); //sprite at the end 

		current = Player.transform.position;
		changedmovespeed = movespeed;
		lastTheta = Player.transform.rotation.z;
	}

	// Update is called once per frame
	void Update () {
		//print(ending.FindChild("TopCranePart").transform.eulerAngles.z - ending.transform.eulerAngles.z);
		Transform BottomCranePart = ending.FindChild("BottomCranePart").transform;
		Transform TopCranePart = ending.FindChild("TopCranePart").transform;

		if (opened) {
			if (TopCranePart.localRotation.eulerAngles.z < 30) {
				Quaternion BottomRot = BottomCranePart.localRotation;
				BottomRot.eulerAngles = new Vector3(0,0,BottomRot.eulerAngles.z - 100 * Time.deltaTime);
				BottomCranePart.localRotation = BottomRot;
				Quaternion TopRot = TopCranePart.localRotation	;
				TopRot.eulerAngles = new Vector3(0,0,TopRot.eulerAngles.z + 100 * Time.deltaTime); 
				TopCranePart.localRotation = TopRot;
				ClosingTime = 0;
			}

		} else {
			if (Mathf.Abs(BottomCranePart.localRotation.eulerAngles.z) > 10 || TopCranePart.localRotation.eulerAngles.z > 300) {
				ClosingTime+=Time.deltaTime;
				//print(ClosingTime);
				Quaternion BottomRot = BottomCranePart.localRotation;
				BottomRot.eulerAngles = new Vector3(0,0,BottomRot.eulerAngles.z + 100 * Time.deltaTime);
				Quaternion TopRot = TopCranePart.localRotation	;
				TopRot.eulerAngles = new Vector3(0,0,TopRot.eulerAngles.z - 100 * Time.deltaTime);
				if (ClosingTime > .25f) {
					TopRot.eulerAngles = new Vector3(0,0,0);
					BottomRot.eulerAngles = new Vector3(0,0,0);
				}
				BottomCranePart.localRotation = BottomRot;
				TopCranePart.localRotation = TopRot;

			}
		}

		SpringJoint2D[] myjoints = Player.GetComponents<SpringJoint2D>();
		foreach (SpringJoint2D j in myjoints) {
			//if (j.connectedBody == null) Destroy(j); //see line 136
		}
		if (emp && ending != null) {
			ending.transform.localPosition = Vector3.zero;
		}
		if (broken) brokentime+=Time.deltaTime;
		if (brokentime > 5) {
			grabbed = false;
			Destroy(focus.GetComponent<LineRenderer>());
			Player.transform.FindChild("SubLine").GetComponent<LineRenderer>().enabled = false;
			JointScript[] jss = Player.GetComponents<JointScript>();
			foreach (JointScript jass in jss) {
				if (!jass.shiprope) 	Destroy(jass);
				
			}
			if (focus != null) {
				focus.GetComponent<RopeScript2D>().SendMessage("Disconnect");
			}
		}
		if (focus != null) {
			if (focus.GetComponent<RopeScript2D>() != null) {
				if (broken = (focus.GetComponent<RopeScript2D>().brokenrope)) {
					cranelength = 0;
					//LineRenderer lr = this.GetComponent<LineRenderer>();
					//lr.SetVertexCount(0);
				}
			}
		} else {
			// release grip
			if (grabbed) {
				Physics2D.IgnoreCollision(focus.GetComponent<Collider2D>(), ending.GetComponent<Collider2D>(), false);
				//Player.GetComponent<LineRenderer>().enabled = false;
				JointScript[] jss = Player.GetComponents<JointScript>();
				foreach (JointScript jass in jss) {
					if (!jass.shiprope) Destroy(jass);
					
				}
				Player.transform.FindChild("SubLine").GetComponent<LineRenderer>().enabled = false;
				Destroy(focus.GetComponent<LineRenderer>());

				grabbed = false;
				firing = false;
				retracting = true;
				opened = false;
			}	

		}
		this.transform.position = Player.transform.position;
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


		}
		if (!emp && !pause) Player.transform.rotation = Quaternion.Euler(0,0,  (thetaersnenig + 90)); //set player rotation, 90 because they did not start at 0 degrees
		float dist = Vector3.Distance(new Vector3(ending.transform.position.x, ending.transform.position.y, 0) - new Vector3(Player.transform.position.x, Player.transform.position.y, 0), Vector3.zero);

		if (!broken) {
			if (Input.GetMouseButton(0) && !grabbed && !retracting && !emp) {
				if (!firing) {

					launchangle = thetaersnenig;
					if (!broken) Player.GetComponent<AudioSource>().PlayOneShot(firecrane);
					
					this.opened = true;
					ending.GetComponent<Rigidbody2D>().AddForce(40 * HarpoonSpeed * new Vector2(Mathf.Cos (Mathf.Deg2Rad * launchangle) , Mathf.Sin (Mathf.Deg2Rad * launchangle)));
					ending.GetComponent<Rigidbody2D>().AddForce(-1 * Player.GetComponent<Rigidbody2D>().velocity);
				}
				firing = true;
				if (dist > cranelength) {
					retracting = true;
					opened = false;
				}
			} else {

				if (firing) {
					firing = false;
					retracting = true;
					opened = false;
				}

				if (retracting) {
					//this.GetComponent<LineRenderer>().enabled = !broken;
					ending.GetComponent<Rigidbody2D>().velocity = (Vector3)Player.GetComponent<Rigidbody2D>().velocity + ((this.transform.position - ending.position) + ( (.5f) * (this.transform.position - ending.position )));
					//ending.GetComponent<Rigidbody2D>().velocity = (-10 * ending.transform.localPosition);
					//Quaternion q = ending.transform.rotation;
					//print(ending.transform.position + "  " + ending.transform.localPosition + " " + ending.localPosition);
					//q.eulerAngles = new Vector3(0,0,180 + Vector2.Angle(ending.transform.position, Player.transform.position));
					//ending.transform.rotation = q;
					if (Mathf.Abs(this.transform.position.x - ending.position.x) < .5f && Mathf.Abs(this.transform.position.y - ending.position.y) < .5f) {
						releaseready = false;
						retracting = false;
						firing = false;

					}
				} 
				if (focus != null) {
					if (!Input.GetMouseButton(0) && grabbed && !focus.GetComponent<RopeScript2D>().brokenrope) {
						releaseready = true;
					}
				}
				if (grabbed && Input.GetMouseButton(0) && releaseready && !focus.GetComponent<RopeScript2D>().brokenrope) {
					grabbed = false;
					Player.transform.FindChild("SubLine").GetComponent<LineRenderer>().enabled = false;
					Destroy(focus.GetComponent<LineRenderer>());
					this.opened = false;					
					//Player.GetComponent<LineRenderer>().enabled = false;
					// normally I would destroy the component here, however I am not sure how to get the specific component for springjoing2D without destroying my other one.
					firing = false;
					retracting = true;
					focus.BroadcastMessage("DestroyRope");
					Player.GetComponent<LineRenderer>().material = this.ropemat; //so player isnt left with crane mat
					JointScript[] jss = Player.GetComponents<JointScript>();
					foreach (JointScript jass in jss) {
						if (!jass.shiprope) 	Destroy(jass);
					}
}
			} 

			if (!firing && !retracting && !grabbed && !pause && !emp) {
				
				this.opened = false;
				ending.position = this.transform.position + new Vector3(0,0,.01f);
				//ending.rotation = Quaternion.Euler(0,0,  (thetaersnenig));
			} else {
				if (grabbed) {
					if (focus.GetComponent<RopeScript2D>().hinger != null) { //update while connected
						
						this.opened = false;
						ending.transform.position = focus.GetComponent<RopeScript2D>().hinger.transform.position;
						if (!pause) {
							ending.eulerAngles = new Vector3(0,0, lastendingangle + (focus.transform.eulerAngles.z - firstfocusangle ));
						}
					}
				} else {
					if (!pause) {
						ending.eulerAngles = new Vector3(0,0,launchangle);
					}
				}
			}
			LineRenderer l = (LineRenderer)GetComponent<LineRenderer> ();
			if (!grabbed) {
				//Player.GetComponent<LineRenderer>().SetWidth(GameObject.Find("Ship").GetComponent<RopeScript2D>().linewidth, GameObject.Find("Ship").GetComponent<RopeScript2D>().linewidth);
				l.enabled = true;
				l.SetPosition(0, Player.transform.position + new Vector3(0,0,.02f));
				l.SetPosition(1, ending.position);	
			} else {
				l.enabled = false;
			}
			if (!grabbed && (firing || retracting)) {
				Collider2D[] hitColliders = Physics2D.OverlapCircleAll(ending.transform.position, .25f); 
				foreach (Collider2D c in hitColliders) {
					if (c != null && c.gameObject != Player && !c.isTrigger && c.GetComponent<RigidIgnorer>() == null && !releaseready) {
						retracting = true;
						this.opened = false;
						firing = false;
						if (c.GetComponent("ItemPickup") != null) {
							Physics2D.IgnoreCollision(c, ending.GetComponent<Collider2D>());
							releaseready = false;
							focus = c.gameObject;
							firing = false;
							retracting = false;
							focus.GetComponent<Rigidbody2D>().isKinematic = false;
							grabbed = true;

							this.lastendingangle = ending.eulerAngles.z;
							firstfocusangle = focus.transform.eulerAngles.z;
							LineRenderer lr = focus.gameObject.GetComponent<LineRenderer>();
							if (lr == null) {
								lr = focus.gameObject.AddComponent<LineRenderer>();
							}
							lr.enabled = false;

							lr.SetWidth(linewidth,linewidth);
							lr.material = this.ropemat;
							lr.SetColors(new Color(0,0,0),new Color(0,0,0));
							lr.sortingLayerName = "Foreground";
							RopeScript2D rp = focus.GetComponent<RopeScript2D>();
							if (rp == null) {
								rp = focus.gameObject.AddComponent<RopeScript2D>();
								rp.parent = this.transform;
							}
							rp.SendMessage("SetTargetAnchor",(ending.position));
							rp.linewidth = this.linewidth;
							Rigidbody2D rg = focus.GetComponent<Rigidbody2D>();
							if (rg == null) {
								rg = focus.gameObject.AddComponent<Rigidbody2D>();
							}
							rg.gravityScale = 0;
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
							rp.rope = true;
							rp.SendMessage("BuildRope");
							rp.mate = mate;
							if (GameObject.Find("Ship").GetComponent<RopeScript2D>().brokenrope) Player.GetComponent<LineRenderer>().SetWidth(linewidth, linewidth); //so we don't get awkward rope when we use it
							break;
						}
					} else {
						if (c.isTrigger && c.GetComponent<DestructionStation>() != null) {
							//cranelength = 0;
							//broken = true;
							//brokentime = 0;
							firing = false;
							retracting = true;
							this.opened = false;
							//LineRenderer lr = this.GetComponent<LineRenderer>();
							//lr.SetVertexCount(0);
						}
					}


				}
			} 
		} else {
			if (focus != null && focus.GetComponent<RopeScript2D>() != null) {
				ending.transform.position = focus.GetComponent<RopeScript2D>().hinger.transform.position;
				if (!pause) {
					ending.eulerAngles = new Vector3(0,0, lastendingangle + (focus.transform.eulerAngles.z - firstfocusangle ));
				}
			} else {
				ending.transform.position = Player.transform.position + new Vector3(0,0,.01f);
			}
		}

		SpriteRenderer ThrusterCW = (SpriteRenderer)GameObject.Find ("ThrusterCW").GetComponent ("SpriteRenderer");
		SpriteRenderer ThrusterCCW = (SpriteRenderer)GameObject.Find ("ThrusterCCW").GetComponent ("SpriteRenderer");

		if (Mathf.Abs (lastTheta - thetaersnenig) > 1 && !emp) {
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

	void Im_Leaving() {
		PlayerPrefs.SetFloat ("cranelength", cranelength);
	}
	void PauseGame(bool b) {
		pause = b;
	}
	void ObjectTakenByShip(GameObject g) {
		if (grabbed && focus.Equals(g)) {
			grabbed = false;
			Player.transform.FindChild("SubLine").GetComponent<LineRenderer>().enabled = false;
			Destroy(focus.GetComponent<LineRenderer>());
			this.opened = false;	
					//Player.GetComponent<LineRenderer>().enabled = false;
			// normally I would destroy the component here, however I am not sure how to get the specific component for springjoing2D without destroying my other one.
			firing = false;
			retracting = true;
			focus.BroadcastMessage("DestroyRope");
			Player.GetComponent<LineRenderer>().material = this.ropemat; //so player isnt stuck with crane mat
			JointScript[] jss = Player.GetComponents<JointScript>();
			foreach (JointScript jass in jss) {
				if (!jass.shiprope) 	Destroy(jass);
			}
		}
	}
}
