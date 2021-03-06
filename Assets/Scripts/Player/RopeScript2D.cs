﻿using UnityEngine;
using System.Collections;
//using UnityEditor;

public class RopeScript2D : MonoBehaviour {
	//this code is funky. I would not recommend re-using it, unless you take out a lot.
	//based off of http://wiki.unity3d.com/index.php/3D_Physics_Based_Rope, except used for 2d and many, many more things changed.
	public KeyCode keject = KeyCode.G;

	public GameObject hinger; //the hinge to make the connector stick out instead of being on the center
	public Vector3 vec; //where our cable starts
	public Transform target;
	public Transform parent;
	public Material ropemat;
	public PhysicsMaterial2D mate;
	public Sprite spriteconnector;
	public float resolution;///0.5F;							  //  Sets the amount of joints there are in the rope (1 = 1 joint for every 1 unit)
	public float ropeDrag = 0.1F;								 //  Sets each joints Drag
	public float ropeMass = 0.1F;							//  Sets each joints Mass
	public float ropeColRadius = 0.5F;							//  Sets the radius of the collider in the SphereCollider component
	public float frequency = 4;
	public float dampening;
	public float linewidth;
	public bool startmade;
	public bool debugmode;
	public bool rope;
	public bool isgenerating;
	public bool ejected;
	public bool shiprope =  false;
	public bool brokenrope;
	public bool iscrane;
	public bool retract_on_death;
	private Vector3 seperation;
	private ArrayList segmentPos;
	private ArrayList joints;
	private LineRenderer line;	
	private int segments = 0;	
	private int indexovertime;
	private float timepassed;
	private bool deadlines;
	private bool hasgotvec;
	//Joint Settings
	public bool usemotor = false;
	public bool UseLimits = false;
	public bool pushing;	
	private bool istube;
	private bool death;
	private bool increasing;
	private GameObject connector;
	private GameObject lastnew;
	private GameObject firstjoint;
	private float lightintensity;
	private Vector2 lastvel;
	private int retractindex;
	private SpringJoint2D spree;
	private float startingdistance;
	private int tubecut;

	public AudioClip brokenRopeClip, brokenRopeAir;
	void Start() {
		if (shiprope) {
			if (PlayerPrefs.HasKey("tubecut")) {
				tubecut = PlayerPrefs.GetInt("tubecut");
			} else {
				PlayerPrefs.SetInt("tubecut",0);
			}
		} else {
			tubecut = 0;
		}


		Transform tee;
		if ((tee = transform.FindChild("RopeStart")) != null) { 
			vec = tee.transform.position;
		}
		rope = true;
		retractindex = 0;
		lightintensity = 0;
		if (parent.GetComponent<RopeTubeController>() != null) {
			istube = true;
		}
		ropeColRadius = 0.03f;
		//line = this.GetComponent<LineRenderer>();
		if (startmade && tubecut == 0) {
			BuildRope();
		} 
		//print(tubecut);
		if (shiprope && tubecut == 1) {
			this.brokenrope = true;

		}
	}
	void Update()
	{
		if (pushing) {
			if (spree != null) {
				if (spree.frequency < 5) spree.frequency += Time.deltaTime;
			}

		}
		if (retract_on_death && brokenrope) { //code in order to retract the cable if shit goes down and one is broken
			if (timepassed > .8f) {
				if (joints != null) {
					if (joints[retractindex] != null) {
						if (!(retractindex == joints.Count - 3) && (GameObject)joints[retractindex + 1] != null && joints.Count > retractindex + 1 && ((GameObject)joints[retractindex + 1]).GetComponent<SpringJoint2D>() != null && ((GameObject)joints[retractindex]).GetComponent<SpringJoint2D>().connectedBody != null) {
							timepassed = 0;
						
							((GameObject)joints[retractindex + 1]).GetComponent<SpringJoint2D>().connectedBody = hinger.GetComponent<Rigidbody2D>();
							if (joints[retractindex] != null) Destroy((GameObject)joints[retractindex]);
							retractindex++;
							if (!iscrane) this.BroadcastMessage("GiveTubesLeft",retractindex);
						} else { //done with retracting, this is the last one
							Destroy((GameObject)joints[retractindex]); 
							if (!iscrane) this.BroadcastMessage("GiveTubesLeft",retractindex);
							retract_on_death = false;
						}
					} else {
						retract_on_death = false;
					}
				}
			}

		}
		timepassed += Time.deltaTime;
		if (isgenerating) { //if it is in the progress of generating

			seperation.z = 0;
			//as if it was for(int s=1;s < segments;s++)
			float timepercalc = .0005f; //value for now, no visible difference in between .000005f and .005
			if (timepassed > indexovertime * (timepercalc)) //if the time passed so far is enough for this "for" loop
			{

				// Find the each segments position using the slope from above
				Vector3 vector = (seperation*indexovertime) + vec;	
				if (indexovertime==1) {
					hinger = new GameObject("Hinger"); //the hinger's purpose is to connect the rope to the center of gravity, as if it was the outside of the object
					hinger.transform.position = vec;
					hinger.layer = 11;
					//do this thing
					DistanceJoint2D dj = hinger.AddComponent<DistanceJoint2D>();
					dj.autoConfigureDistance = false;
					hinger.GetComponent<Rigidbody2D>().gravityScale = 0;
					dj.connectedBody = this.GetComponent<Rigidbody2D>();
					dj.maxDistanceOnly = true;
					dj.distance = 0;
					if (!shiprope || true) {
						Vector2 vectorry = new Vector2((this.vec.x - this.transform.position.x)/this.transform.localScale.x,(this.vec.y-this.transform.position.y)/this.transform.localScale.y);
						float _x = vectorry.x; //math
						float _y = vectorry.y;

						float _angle = -1 * this.transform.eulerAngles.z * Mathf.Deg2Rad;
						float _cos = Mathf.Cos(_angle);
						float _sin = Mathf.Sin (_angle);
						
						float _x2 = _x * _cos - _y * _sin;
						float _y2 = _x * _sin + _y * _cos;
						
						vectorry =  new Vector2(_x2, _y2);
						dj.connectedAnchor = vectorry; //this is to get the correct rotation on the connection in between the two
					}
					hinger.transform.parent = transform;

				}


				segmentPos.Add (vector);
				
				//Add Physics to the segments
			
				GameObject newie = AddJointPhysics(indexovertime);
				Physics2D.IgnoreCollision(((GameObject)joints[indexovertime - 1]).GetComponent<Collider2D>(), target.GetComponent<Collider2D>());
				Physics2D.IgnoreCollision(((GameObject)joints[indexovertime - 1]).GetComponent<Collider2D>(), transform.GetComponent<Collider2D>());
				if (parent.GetComponentInChildren<CraneController>() != null) {
					Physics2D.IgnoreCollision(parent.GetComponentInChildren<CraneController>().focus.GetComponent<Collider2D>(), ((GameObject)joints[indexovertime-1]).GetComponent<Collider2D>());
					
				}
				if (indexovertime>1) { //if it is not the first
					foreach (GameObject gee in joints) {
						Physics2D.IgnoreCollision(newie.GetComponent<Collider2D>(), gee.GetComponent<Collider2D>());

					}
					//Physics2D.IgnoreCollision(((GameObject)joints[indexovertime - 1]).collider2D, ((GameObject)joints[indexovertime-2]).collider2D);
					//Physics2D.IgnoreCollision(((GameObject)joints[indexovertime - 1]).collider2D, ((GameObject)joints[1]).collider2D);
				} 
				if (indexovertime >= segments)  { //if we are DONE
					isgenerating = false;
					segmentPos.Add (target.position);
					firstjoint.GetComponent<LineRenderer>().enabled = true;
					// Attach the joints to the target object and parent it to this object	
					SpringJoint2D end = target.gameObject.AddComponent<SpringJoint2D>();
					end.autoConfigureDistance = false;
					spree = end;
					end.frequency = frequency;
					LineRenderer lr = target.gameObject.GetComponent<LineRenderer>();
					JointScript js = target.gameObject.AddComponent<JointScript>();
					js.shipRopeIndex = indexovertime;
					js.material = this.ropemat;
					js.shiprope = this.shiprope;
					js.SendMessage("GiveFocus",this.gameObject);
					js.linewidth = this.linewidth;

					if (lr == null) {
						lr = target.gameObject.AddComponent<LineRenderer>();
					}
					lr.material = line.material;
					if (shiprope) {
						lr.SetWidth(linewidth,linewidth);
					}
					startingdistance = (((vec.x - target.position.x))/segments)/3;
					end.distance = (((vec.x - target.position.x))/segments)/3;
					end.connectedBody = ((GameObject)joints[joints.Count-1]).transform.GetComponent<Rigidbody2D>();

						((GameObject)joints[0]).GetComponent<SpringJoint2D>().connectedBody = hinger.GetComponent<Rigidbody2D>();
						
					target.GetComponent<Rigidbody2D>().gravityScale = 0;
				
				}
				indexovertime++; //increment (like a for loop)

			}


		}
		if (ejected) { //the pulsing light of the connector
			if (increasing) {
				lightintensity += Time.deltaTime;
				if (lightintensity >= 1) {
					increasing = false;
				}
			} else {
				lightintensity -= Time.deltaTime;
				if (lightintensity <= 0) {
					increasing = true;
				}
			}

		} else {

		}
		if (connector != null) {
			SpriteRenderer sp = connector.GetComponent<SpriteRenderer>();
			if (sp != null) {
				// add light properties here
				connector.transform.localScale = new Vector3(2 * lightintensity,2 * lightintensity,2 * lightintensity);
				sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, 1);
			}
		}
	}
	void AddRope() {
		if (!isgenerating && !this.brokenrope) {
			segments++;
			segmentPos.Add(GameObject.Find("Player").transform.position); //add to araylist
			GameObject newobj = AddJointPhysics(joints.Count + 1); //increment physics points
			target.GetComponent<SpringJoint2D>().connectedBody = newobj.GetComponent<Rigidbody2D>();
			newobj.GetComponent<SpringJoint2D>().connectedBody = ((GameObject)joints[segments-2]).GetComponent<Rigidbody2D>();
			spree.frequency = frequency;
			newobj.transform.position = target.transform.position;
			LineRenderer lr = target.GetComponent<LineRenderer>();
			lr.SetVertexCount (2);
			lr.SetPosition (0, target.transform.position);
			lr.SetPosition (1, newobj.transform.position);
			Physics2D.IgnoreCollision(newobj.GetComponent<Collider2D>(), target.GetComponent<Collider2D>()); //cleaning up collision
			Physics2D.IgnoreCollision(newobj.GetComponent<Collider2D>(), parent.GetComponent<Collider2D>());
			if (istube && parent.GetComponentInChildren<CraneController>() != null) {
				Physics2D.IgnoreCollision(parent.GetComponentInChildren<CraneController>().focus.GetComponent<Collider2D>(), (newobj).GetComponent<Collider2D>());
			}
		}

	}

	void SubRope() {

		if (segments > 3 && !isgenerating && !this.brokenrope) {
			if ( Vector3.Distance(((GameObject)joints[segments - 1]).transform.position,target.transform.position) < .4f) {
				pushing = false;
				Destroy(((GameObject)joints[segments - 1]));
				target.GetComponent<SpringJoint2D>().connectedBody = ((GameObject)joints[segments-2]).GetComponent<Rigidbody2D>();
				joints.RemoveAt(segments-1);
				segmentPos.RemoveAt(segments - 1);
				segments--;
				spree.frequency = frequency;
				this.GetComponent<RopeTubeController>().SendMessage("SubTheRopeAmt");
				lastnew = ((GameObject)joints[segments - 1]);
				//((GameObject)joints[segments-2]).GetComponent<SpringJoint2D>().frequency = frequency * 10;
				//((GameObject)joints[segments-3]).GetComponent<SpringJoint2D>().frequency = frequency;
			} 
		}
	}
	void LateUpdate()
	{
		line = this.GetComponent<LineRenderer>();
		// Does rope exist? If so, update its position
		if (!isgenerating || deadlines ) {
			if (line != null) line.enabled = false;

		} else {
			if (this.hasgotvec && !this.isgenerating) { //so no rope appears before it has the proper plaements

				line.SetVertexCount(2);
				if (vec == Vector3.zero) {
					line.SetPosition(0,this.transform.position);
				} else {

					line.SetPosition(0,vec);
				}
				line.enabled = true;
				line.SetPosition(1,target.transform.position);
			}
		}
	}
	GameObject AddJointPhysics(int n)
	{
		//creating the game objects to initiate physics, function called upon earlier
		GameObject newie = new GameObject("Joint_" + n);
		newie.layer = 11;
		joints.Add (newie);
		lastnew = newie;
		Rigidbody2D rigid = newie.AddComponent<Rigidbody2D>();
		CircleCollider2D col = newie.AddComponent<CircleCollider2D>();
		//EdgeCollider2D col = newie.AddComponent<EdgeCollider2D>();
		SpringJoint2D ph = newie.AddComponent<SpringJoint2D>();
		LineRenderer ln = newie.AddComponent<LineRenderer>();
		JointScript js = newie.gameObject.AddComponent<JointScript>();
		js.SendMessage("GiveFocus",this.gameObject); 
		js.shiprope = this.shiprope;
		js.linewidth = this.linewidth;					
		js.material = this.ropemat;

		js.shipRopeIndex = indexovertime;
		if (!this.isgenerating) indexovertime++;
		line = parent.GetComponent<LineRenderer> ();
		ln.material = line.material;
		ln.SetWidth(linewidth,linewidth);
		col.radius = .03f;

		if (debugmode) {
			SpriteRenderer sp = newie.AddComponent<SpriteRenderer>();
			sp.sprite = spriteconnector;
		}
		//newie.AddComponent<RigidIgnorer>();
		ph.enableCollision = false;
		ph.frequency = frequency;
		ph.dampingRatio = dampening;
		ph.distance = startingdistance;
		rigid.isKinematic = false;
		rigid.gravityScale = 0; 
		rigid.mass = ropeMass;


		newie.transform.position = new Vector3(((Vector3)segmentPos[n]).x, ((Vector3)segmentPos[n]).y, target.transform.position.z);
		col.sharedMaterial = mate;
		if(n==1){		
			this.firstjoint = newie;
			ph.connectedBody = hinger.GetComponent<Rigidbody2D>();
			//js.SendMessage("GiveConnected",this.gameObject); //remove if rolling back
			//col.points = new Vector2[2] {new Vector2(0,0), (Vector2)(this.transform.position - newie.transform.position)}; //remove if rolling back

		} else
		{
			ph.connectedBody = ((GameObject)joints[n-2]).GetComponent<Rigidbody2D>();	
			//js.SendMessage("GiveConnected",((GameObject)joints[n-2])); //remove if rolling back
			//col.points = new Vector2[2] {new Vector2(0,0), (Vector2)(this.transform.position - ((GameObject)joints[n-2]).transform.position)}; //remove if rolling back


		}
		ln.SetVertexCount(2);
		ln.SetPosition(0,newie.transform.position);
		ln.SetPosition(1,ph.connectedBody.transform.position);
		line = this.GetComponent<LineRenderer> ();
		if (n == 1) {
			ln.enabled = false;
		}
		return newie;
	}

	void BuildRope()
	{

		if (rope) {
			segmentPos = new ArrayList();
			joints = new ArrayList();

			// Find the amount of segments based on the distance and resolution
			// Example: [resolution of 1.0 = 1 joint per unit of distance]



			segments = (int)(Vector3.Distance( vec , target.position)*resolution ) + 1;
			seperation = ((target.position - vec)/(segments));

			segmentPos.Add(vec);
			// Find the distance between each segment
			var segs = segments-1;
			isgenerating = true;
			indexovertime = 1;
			if (this.GetComponent<RopeTubeController>() != null) {
				this.GetComponent<RopeTubeController>().tubesleft = this.GetComponent<RopeTubeController>().tubesleft - segs - 1 ;

			}
			//EditorApplication.isPaused = true;
		}
	}

	void DestroyRope()
	{


		isgenerating = false;
		GameObject.Find ("Crane").GetComponent<CraneController> ().grabbed = false;
		// Stop Rendering Rope then Destroy all of its components
		Destroy(hinger);
		rope = false;
		for(int dj=0;dj<joints.Count;dj++)
		{
			Destroy((GameObject)joints[dj]);	
		}
		this.line.SetVertexCount(0);
		Destroy(spree);
		segmentPos.Clear();
		joints.Clear();
		segments = 0;
		deadlines = true;
		line.enabled = false;
		Destroy(this.GetComponent<RopeScript2D>());
	}	
	void SetTargetAnchor(Vector3 vec) {
		this.hasgotvec = true;
		this.vec = vec;

	}
	void Eject() {
		if (!isgenerating) {
			if (!ejected) {
					GameObject.Find("SoundEffectController").transform.FindChild("EjectSound").GetComponent<AudioSource>().Play();

				if (!brokenrope) { //spawn a connector, float away

					ejected = true;
					connector = new GameObject ("Connector");
					GameObject innercol = new GameObject("Innercol");
					connector.layer = 11;
					CircleCollider2D icc = innercol.AddComponent<CircleCollider2D>();
					Rigidbody2D rg = connector.AddComponent<Rigidbody2D> ();
					CircleCollider2D cc = connector.AddComponent<CircleCollider2D> ();
					SpriteRenderer sp = connector.AddComponent<SpriteRenderer> ();
					SpringJoint2D sj = connector.AddComponent<SpringJoint2D> ();
					LineRenderer lr = connector.AddComponent<LineRenderer> ();
					//connector.AddComponent<RigidIgnorer>();
					lr.material = line.material;
					lr.SetWidth (linewidth, linewidth);
					GameObject.Find("Player").GetComponent<LineRenderer>().enabled = false;
					JointScript js = connector.gameObject.AddComponent<JointScript>();
					js.SendMessage("GiveFocus",this.gameObject);	
					js.shiprope = this.shiprope;
					js.linewidth = this.linewidth;
					js.material = this.ropemat;

					rg.gravityScale = 0;
					cc.radius = .15f;
					Physics2D.IgnoreCollision(GameObject.Find("Player").GetComponent<Collider2D>(), cc.GetComponent<Collider2D>());
					LineRenderer[] lers = target.GetComponentsInChildren<LineRenderer>();
					LineRenderer luhr = null;
					foreach (LineRenderer l in lers) {
						if (l.name == "SubLine") luhr = l;
					}
					if (luhr != null) {
						luhr.enabled = false;
					}
					icc.isTrigger = true;
					innercol.transform.parent = connector.transform;	
					rg.mass = ropeMass;
					sp.sprite = (spriteconnector);
					sp.color = new Color (sp.color.r, sp.color.g, sp.color.b, lightintensity);
					connector.transform.position = target.transform.position;
					sj.distance = startingdistance;
					sj.connectedBody = lastnew.GetComponent<Rigidbody2D>();
				
				} 
				ejected = true;
				target.GetComponent<LineRenderer>().enabled = false;
				LineRenderer ls = target.GetComponent<LineRenderer>();
				SpringJoint2D[] sjs = target.GetComponents<SpringJoint2D>();
				foreach (SpringJoint2D sj in sjs) {
					if (sj.frequency == frequency) Destroy(sj);
				}
				JointScript[] jss = target.GetComponents<JointScript>();
				foreach (JointScript jass in jss) {
					if (jass.shiprope) 	Destroy(jass);
					
				}

				ls.SetVertexCount(0);
				// detach from rope

			}
		}
	}
	void Connect() { //reconnect if we register
		if (ejected && !death) {
			if (!brokenrope) {
				if ((spree = target.gameObject.GetComponent<SpringJoint2D>()) == null) {
					spree = target.gameObject.AddComponent<SpringJoint2D>();
				}
				spree.distance = (((vec.x - target.position.x))/segments)/3;
				spree.connectedBody = lastnew.transform.GetComponent<Rigidbody2D>();
				Destroy(connector);
				ejected = false;
				LineRenderer[] lers = target.GetComponentsInChildren<LineRenderer>();
				LineRenderer luhr = null;
				foreach (LineRenderer l in lers) {
					if (l.name == "SubLine") luhr = l;
				}
				if (luhr != null && !shiprope) {
					luhr.material = this.ropemat;
				}
				/*LineRenderer lr = target.GetComponent<LineRenderer>();
				lr.SetVertexCount(2);
				lr.SetPosition(0,target.transform.position);
				lr.SetPosition(1,lastnew.transform.position);*/
				JointScript js = target.gameObject.AddComponent<JointScript>();
				js.SendMessage("ReconnectJoint");
				js.material = this.ropemat;
				js.linewidth = this.linewidth;
				js.shiprope = true;
				target.SendMessage("GiveFocus",this.gameObject);
				this.SendMessage("ReconnectAdd");
			} 

		}
	}
	void DeathIsSoon() { 
		brokenrope = true;
		Eject();
		death = true;
	}
	void BrokenRope() {
		brokenrope = true;
		if (connector != null) {
			Destroy(connector);
		}
	}
	void BrokenJoint(int i) {
		joints[i] = null;
	}
	void Disconnect() {
		Destroy(((GameObject)joints[joints.Count-1]));
	}
	
}
