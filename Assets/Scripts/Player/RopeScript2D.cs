using UnityEngine;
using System.Collections;

public class RopeScript2D : MonoBehaviour {
	public Transform target;
	public Transform parent;
	public Material ropemat;
	public PhysicsMaterial2D mate;
	public float resolution;///0.5F;							  //  Sets the amount of joints there are in the rope (1 = 1 joint for every 1 unit)
	public float ropeDrag = 0.1F;								 //  Sets each joints Drag
	public float ropeMass = 0.1F;							//  Sets each joints Mass
	public float ropeColRadius = 0.5F;	//  Sets the radius of the collider in the SphereCollider component
	public float frequency = 4;
	public bool startmade;
	public bool debugmode;
	public float dampening;
	public Sprite spriteconnector;
	//public float ropeseglength = .025f; 
	//public float ropeBreakForce = 25.0F;//-------------- TODO (Hopefully will break the rope in half...
	private Vector3 collisionpoint;
	//private Vector3[] segmentPos;//  DONT MESS!	This is for the Line Renderer's Reference and to set up the positions of the gameObjects
	//private Vector3[] segmentPos;
	private ArrayList segmentPos;
	//private GameObject[] joints;		 	//  DONT MESS!	This is the actual joint objects that will be automatically created
	private ArrayList joints;
	private LineRenderer line;							//  DONT MESS!	 The line renderer variable is set up when its assigned as a new component
	private int segments = 0;					//  DONT MESS!	The number of segments is calculated based off of your distance * resolution
	private bool rope = false;						 //  DONT MESS!	This is to keep errors out of your debug window! Keeps the rope from rendering when it doesnt exist...
	private int indexovertime;
	private float timepassed;
	private bool isgenerating;
	private bool deadlines;
	//Joint Settings
	public bool usemotor = false;
	public bool UseLimits = false;
	private bool istube;
	public bool ejected;
	private bool death;
	private bool increasing;
	public bool shiprope =  false;
	private GameObject connector;
	public GameObject hinger;
	private GameObject lastnew;
	private float lightintensity;
	private Vector2 lastvel;
	private Vector3 relativestartpos;
	public Vector3 vec; //where our cable starts
	private Transform Ending;
	private Transform theparent;
	public float linewidth;
	private bool brokenrope;


	void Start() {

		lightintensity = 0;
		if (parent.GetComponent<RopeTubeController>() != null) {
			istube = true;
		}
		ropeColRadius = 0.03f;
		line = parent.GetComponent<LineRenderer>();
		//target.GetComponent<SpringJoint2D>().anchor = target.transform.position;
		if (startmade) {
			BuildRope();
		}
	}
	void Update()
	{





		timepassed += Time.deltaTime;
		if (isgenerating) { //if it is in the progress of generating
			var segs = segments-1;
			var seperation = ((target.position - vec)/segs);


			seperation.z = 0;
			//for(int s=1;s < segments;s++)
			float timepercalc = .000005f; //value for now, no visible difference in between .000005f and .005
			if (timepassed > indexovertime * (timepercalc)) //if the time passed so far is enough for this "for" loop
			{

				// Find the each segments position using the slope from above
				Vector3 vector = (seperation*indexovertime) + vec;	
				//print("segements: " + segments + " index: " + indexovertime + "position: " + vector);
				//print(vec);
				if (indexovertime==1) {
					hinger = new GameObject("Hinger"); //the hinger's purpose is to connect the rope to the center of gravity, as if it was the outside of the object
					Vector3 newvec = collisionpoint + this.transform.position;
					hinger.transform.position = vec;
					//print(collisionpoint);
					//do this thing
					DistanceJoint2D dj = hinger.AddComponent<DistanceJoint2D>();
					hinger.GetComponent<Rigidbody2D>().gravityScale = 0;
					dj.connectedBody = this.rigidbody2D;
					dj.maxDistanceOnly = true;
					dj.distance = 0;
					if (!shiprope) {
						Vector2 vectorry = new Vector2((this.vec.x - this.transform.position.x)/this.transform.localScale.x,(this.vec.y-this.transform.position.y)/this.transform.localScale.y);
						float _x = vectorry.x;
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

					//SpriteRenderer sp = hinger.AddComponent<SpriteRenderer>();
					//sp.sprite = this.spriteconnector;


					//print (segs);
					//print(vector + "   " + this.transform.position);
				}

				segmentPos.Add (vector);
				
				//Add Physics to the segments
				AddJointPhysics(indexovertime);
				Physics2D.IgnoreCollision(((GameObject)joints[indexovertime - 1]).collider2D, target.collider2D);
				Physics2D.IgnoreCollision(((GameObject)joints[indexovertime - 1]).collider2D, transform.collider2D);
				if (parent.GetComponentInChildren<CraneController>() != null) {
					Physics2D.IgnoreCollision(parent.GetComponentInChildren<CraneController>().focus.collider2D, ((GameObject)joints[indexovertime-1]).collider2D);
					
				}
				if (indexovertime>1) { //if it is not the first
					Physics2D.IgnoreCollision(((GameObject)joints[indexovertime - 1]).collider2D, ((GameObject)joints[indexovertime-2]).collider2D);
					Physics2D.IgnoreCollision(((GameObject)joints[indexovertime - 1]).collider2D, ((GameObject)joints[1]).collider2D);
				} 
				if (indexovertime >= segments)  { //if we are DONE
					isgenerating = false;
					segmentPos.Add (target.position);

					// Attach the joints to the target object and parent it to this object	
					SpringJoint2D end = target.gameObject.AddComponent<SpringJoint2D>();
					LineRenderer lr = target.gameObject.GetComponent<LineRenderer>();
					target.gameObject.AddComponent<JointScript>();
					if (lr == null) {
						lr = target.gameObject.AddComponent<LineRenderer>();
					}
					lr.material = line.material;
					lr.SetWidth(linewidth,linewidth);


					end.distance = (((vec.x - target.position.x))/segments)/3;



					end.connectedBody = ((GameObject)joints[joints.Count-1]).transform.rigidbody2D;

						((GameObject)joints[0]).GetComponent<SpringJoint2D>().connectedBody = hinger.rigidbody2D;
						for (int s = 1; s < segments; s++) {
							((GameObject)joints[s]).transform.position =  (seperation*s) + vec;
						}
						//dj.distance = Vector2.Distance(new Vector2(this.collisionpoint.x/this.transform.localScale.x,this.collisionpoint.y/this.transform.localScale.y), Vector2.zero);
					

					/*SpringJoint2D myend = this.gameObject.AddComponent<SpringJoint2D>();
					end.distance = ((this.transform.position.x - target.position.x))/segments;
					end.connectedBody = ((GameObject)joints[0]).transform.rigidbody2D;
					*/

					target.rigidbody2D.gravityScale = 0;
					//line.SetVertexCount(segments);
				}
				indexovertime++; //increment (like a for loop)

			}


		}



		/*if (Input.GetKeyDown(KeyCode.E)) {
			AddRope();
		}
		if (Input.GetKeyDown (KeyCode.Q)) {
			SubRope();
		}*/
		if (ejected) {
			//print (lightintensity);
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
			SpriteRenderer sp = connector.GetComponent<SpriteRenderer>();
			sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, lightintensity);
		} else {
			if (connector != null) {
				SpriteRenderer sp = connector.GetComponent<SpriteRenderer>();
				if (sp != null) {
					sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, lightintensity);
				}
			}
		}
	}
	void AddRope() {
		segments++;
		//print(segments);
		//line.SetVertexCount(segments);
		segmentPos.Add(GameObject.Find("Player").transform.position); //add to araylist
		//printprint(segmentPos[joints.Count + 2] + "  " + GameObject.Find("Player").transform.position);
		//print ("segments: " + segments + "segmentpos.count: " + segmentPos.Count);
		GameObject newobj = AddJointPhysics(joints.Count + 1); //increment physics points
		//target.transform.position = target.transform.position - (target.transform.position - (Vector3)segmentPos[segments - 1]);
		//((GameObject)joints[segments - 2]).transform.position = target.transform.position + Vector3.up;
		//lastnew.GetComponent<SpringJoint2D>().connectedBody = newobj.rigidbody2D;
		target.GetComponent<SpringJoint2D>().connectedBody = newobj.rigidbody2D;
		newobj.GetComponent<SpringJoint2D>().connectedBody = ((GameObject)joints[segments-2]).rigidbody2D;
		newobj.transform.position = target.transform.position;
		LineRenderer lr = target.GetComponent<LineRenderer>();
		lr.SetVertexCount (2);
		lr.SetPosition (0, target.transform.position);
		lr.SetPosition (1, newobj.transform.position);
		//lastnew = newobj;
		//((GameObject)joints[segments-2]).GetComponent<SpringJoint2D>().connectedBody = ((GameObject)joints[segments-3]).rigidbody2D; //set the new one's connected body to be the one before
		//target.GetComponent<SpringJoint2D>().connectedBody = ((GameObject)joints[segments-2]).rigidbody2D; //set the targets body to this last, new point
		Physics2D.IgnoreCollision(newobj.collider2D, target.rigidbody2D.collider2D); //cleaning up collision
		Physics2D.IgnoreCollision(newobj.collider2D, parent.rigidbody2D.collider2D);
		if (istube && parent.GetComponentInChildren<CraneController>() != null) {
			Physics2D.IgnoreCollision(parent.GetComponentInChildren<CraneController>().focus.collider2D, (newobj).collider2D);
		}

	}

	void SubRope() {
		if (segments > 3) {

			Destroy(((GameObject)joints[segments - 1]));
			target.GetComponent<SpringJoint2D>().connectedBody = ((GameObject)joints[segments-2]).rigidbody2D;
			joints.RemoveAt(segments-1);
			segmentPos.RemoveAt(segments - 1);
			segments--;
			//line.SetVertexCount(segments);


			((GameObject)joints[segments-2]).GetComponent<SpringJoint2D>().frequency = frequency * 10;
			((GameObject)joints[segments-3]).GetComponent<SpringJoint2D>().frequency = frequency;
		}
	}
	void LateUpdate()
	{
		line = parent.GetComponent<LineRenderer>();
		// Does rope exist? If so, update its position
		if(rope && !isgenerating && !deadlines && false) {

			line.enabled = true;
		} else {
			if (isgenerating && !deadlines) {
				line.SetVertexCount(2);
				line.SetPosition (1, this.transform.position);
				print(vec);

				line.SetPosition(0,vec);

			} else {
				line.enabled = false;
			}
				
		}
	}
	GameObject AddJointPhysics(int n)
	{

		//print ("A new member has joined!");
		GameObject newie = new GameObject("Joint_" + n);
		joints.Add (newie);
		lastnew = newie;

		//print("Joint added");
		/*if (this.shiprope){ 

			((GameObject)joints[n - 1]).transform.parent = target.transform;
			//print("I am target");
		} else {
			((GameObject)joints[n - 1]).transform.parent = transform;
		}*/

		//joints [n].transform.position = new Vector3 (this.transform.position.x + (this.transform.position.x - target.position.x)/n, 0, 0);

		Rigidbody2D rigid = newie.AddComponent<Rigidbody2D>();
		CircleCollider2D col = newie.AddComponent<CircleCollider2D>();
		SpringJoint2D ph = newie.AddComponent<SpringJoint2D>();
		LineRenderer ln = newie.AddComponent<LineRenderer>();
		newie.AddComponent<JointScript>();
		ln.material = line.material;
		ln.SetWidth(linewidth,linewidth);

		if (debugmode) {
			SpriteRenderer sp = newie.AddComponent<SpriteRenderer>();
			sp.sprite = spriteconnector;
		}
		newie.AddComponent<RigidIgnorer>();
		ph.collideConnected = false;
		ph.frequency = frequency;
		ph.dampingRatio = dampening;
		rigid.isKinematic = false;
		//ph.useLimits = false;

		ph.distance = (((vec.x - target.position.x))/segments)/3;


		//ph.anchor = new Vector2(1/(resolution), 1/(resolution));
		//ph.anchor = new Vector2 (((this.transform.position.x - target.position.x))/segments, ((this.transform.position.y - target.position.y))/segments);
		//print (ph.anchor);
		
		rigid.gravityScale = 0; 
		//print(segmentPos.Count);

		newie.transform.position = (((Vector3)segmentPos[n]));
//		((GameObject)joints[n-1]).transform.position.z = 0;
		//segmentPos.

		//rigid.drag = ropeDrag;
		rigid.mass = ropeMass;
		col.radius = .03f;
		col.sharedMaterial = mate;
		//print ("value: " + n + " and segments = "+segments);
		if(n==1){		

			ph.connectedBody = this.transform.rigidbody2D;
		} else
		{
			ph.connectedBody = ((GameObject)joints[n-2]).rigidbody2D;	
		}
		ln.SetVertexCount(2);
		ln.SetPosition(0,newie.transform.position);
		ln.SetPosition(1,ph.connectedBody.transform.position);
		return newie;
	}

	void BuildRope()
	{

		rope = true;
		line = parent.GetComponent<LineRenderer>();
		segmentPos = new ArrayList();
		joints = new ArrayList();
		if (vec.Equals(Vector3.zero)){
			//print(vec);
			vec = this.transform.position;
		}
		// Find the amount of segments based on the distance and resolution
		// Example: [resolution of 1.0 = 1 joint per unit of distance]
		//print("Distance = " + (Vector3.Distance(transform.position, target.position)*resolution));
		//print(Vector3.Magnitude(this.transform.position + target.position));




		segments = (int)(Vector3.Distance( vec , target.position)*resolution ) + 1;

		//print(vec);
		//segments = 20;
		
		//print ("segments " + segments);
		//line.SetVertexCount(segments);
		line.material = ropemat;
		segmentPos.Add(vec);

		
		// Find the distance between each segment
		var segs = segments-1;
		var seperation = ((target.position - vec)/segs);



		//print(seperation);
		//if (this.GetComponent<ItemHolder>() == null) Physics2D.IgnoreCollision(this.collider2D, target.collider2D);
		isgenerating = true;
		indexovertime = 1;
		if (this.GetComponent<RopeTubeController>() != null) {

			this.GetComponent<RopeTubeController>().tubesleft = this.GetComponent<RopeTubeController>().tubesleft - segs + 1 ;

		}
		//target.parent = transform;
	}
	void CreateRopeOverTime() {

	}
	void DestroyRope()
	{
		if (Ending != null) {
			Ending.parent = theparent;
		}
		isgenerating = false;
		GameObject.Find ("Crane").GetComponent<CraneController> ().grabbed = false;
		// Stop Rendering Rope then Destroy all of its components
		Destroy(hinger);
		rope = false;
		for(int dj=0;dj<joints.Count;dj++)
		{
			Destroy((GameObject)joints[dj]);	
		}
		//line.isVisible = false;
		segmentPos.Clear();
		joints.Clear();
		segments = 0;
		//line.SetVertexCount(0);
		deadlines = true;
		line.enabled = false;
		//line.enabled = false;
		//Destroy(parent.GetComponent<LineRenderer>());
		Destroy(this.GetComponent<RopeScript2D>());
	}	
	void SetTargetAnchor(Vector3 vec) {
		//print (this.transform.position);
//		print (vec);
		this.vec = vec;
		collisionpoint = (vec - this.transform.position);
		//print("Sent vector: " + vec);
		relativestartpos = new Vector3(this.collisionpoint.x/this.transform.localScale.x,this.collisionpoint.y/this.transform.localScale.y, this.collisionpoint.z/this.transform.localScale.z);
		//new Vector3(this.collisionpoint.x/this.transform.localScale.x,this.collisionpoint.y/this.transform.localScale.y, this.collisionpoint.z/this.transform.localScale.z);
		
	}
	void Eject() {
		if (!ejected) {
			if (!brokenrope) {
				ejected = true;
				connector = new GameObject ("Connector");
				Rigidbody2D rg = connector.AddComponent<Rigidbody2D> ();
				CircleCollider2D cc = connector.AddComponent<CircleCollider2D> ();
				SpriteRenderer sp = connector.AddComponent<SpriteRenderer> ();
				SpringJoint2D sj = connector.AddComponent<SpringJoint2D> ();
				LineRenderer lr = connector.AddComponent<LineRenderer> ();
				lr.material = line.material;
				lr.SetWidth (linewidth, linewidth);
				connector.AddComponent<JointScript> ();								
				rg.gravityScale = 0;
				cc.isTrigger = true;
				//connector.transform.parent = transform;	
				rg.mass = ropeMass;
				sp.sprite = (spriteconnector);
				sp.color = new Color (sp.color.r, sp.color.g, sp.color.b, lightintensity);
				connector.transform.position = target.transform.position;
				sj.distance = ((GameObject)joints [0]).GetComponent<SpringJoint2D> ().distance;
				sj.connectedBody = lastnew.rigidbody2D;

				LineRenderer ls = target.GetComponent<LineRenderer>();

				Destroy(target.GetComponent<SpringJoint2D>());
				ls.SetVertexCount(0);
			} else {
			
				ejected = true;

				LineRenderer ls = target.GetComponent<LineRenderer>();
				Destroy(target.GetComponent<SpringJoint2D>());
				ls.SetVertexCount(0);
			     // detach from rope

			}

		}
	}
	void Connect() {
		//print("Connect!");
		if (ejected && !death) {
			SpringJoint2D end = target.gameObject.AddComponent<SpringJoint2D>();
			end.distance = (((vec.x - target.position.x))/segments)/3;
			end.connectedBody = lastnew.transform.rigidbody2D;
			Destroy(connector);
			ejected = false;
			LineRenderer lr = target.GetComponent<LineRenderer>();
			lr.SetVertexCount(2);
			lr.SetPosition(0,target.transform.position);
			lr.SetPosition(1,lastnew.transform.position);
		}
	}
	void DeathIsSoon() {
		Eject();
		death = true;
	}
	void BrokenRope() {
		brokenrope = true;
		bool kaputt = false, deadones = false;
		GameObject lastg = null, remove = null;
		foreach (GameObject e in joints)
		{
			print("what");

			if (e.GetComponent<SpringJoint2D>() == null);
			{
				kaputt = true;
			}

			if (kaputt) {
				deadones = true;
				e.GetComponent<JointScript>().severed = true;
				lastnew = lastg;
			} else {
				lastg = e;
			}


		}
		joints.Remove (remove);
		
		ejected = true;
		connector = new GameObject ("Connector");
		Rigidbody2D rg = connector.AddComponent<Rigidbody2D> ();
		CircleCollider2D cc = connector.AddComponent<CircleCollider2D> ();
		SpriteRenderer sp = connector.AddComponent<SpriteRenderer> ();
		SpringJoint2D sj = connector.AddComponent<SpringJoint2D> ();
		LineRenderer lr = connector.AddComponent<LineRenderer> ();
		lr.material = line.material;
		lr.SetWidth (linewidth, linewidth);
		connector.AddComponent<JointScript> ();								
		rg.gravityScale = 0;
		cc.isTrigger = true;
		//connector.transform.parent = transform;	
		rg.mass = ropeMass;
		sp.sprite = (spriteconnector);
		sp.color = new Color (sp.color.r, sp.color.g, sp.color.b, lightintensity);
		connector.transform.position = lastnew.transform.position;
		sj.distance = ((GameObject)joints [0]).GetComponent<SpringJoint2D> ().distance;
		sj.connectedBody = lastnew.rigidbody2D;
	}
	
}
