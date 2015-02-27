using UnityEngine;
using System.Collections;

public class RopeScript2D : MonoBehaviour {
	public Transform target;
	public Material ropemat;
	public PhysicsMaterial2D mate;
	public float resolution;///0.5F;							  //  Sets the amount of joints there are in the rope (1 = 1 joint for every 1 unit)
	public float ropeDrag = 0.1F;								 //  Sets each joints Drag
	public float ropeMass = 0.1F;							//  Sets each joints Mass
	public float ropeColRadius = 0.5F;	//  Sets the radius of the collider in the SphereCollider component
	public float frequency = 4;
	public bool startmade;
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
	//Joint Settings
	public bool usemotor = false;
	public bool UseLimits = false;
	private bool istube;
	public bool ejected;
	private bool death;
	private bool increasing;
	private GameObject connector;
	private float lightintensity;

	void Start() {
		lightintensity = 0;
		if (this.GetComponent<RopeTubeController>() != null) {
			istube = true;
		}
		ropeColRadius = 0.03f;
		line = gameObject.GetComponent<LineRenderer>();
		//target.GetComponent<SpringJoint2D>().anchor = target.transform.position;
		if (startmade) {
			BuildRope();
		}
	}

	/*void Awake()
	{
		BuildRope();
	}*/
	
	void Update()
	{
		timepassed += Time.deltaTime;
		if (isgenerating) { //if it is in the progress of generating
			var segs = segments-1;
			var seperation = ((target.position - transform.position)/segs);

			//for(int s=1;s < segments;s++)
			float timepercalc = .000005f; //value for now, no visible difference in between that and .005
			if (timepassed > indexovertime * (timepercalc)) //if the time passed so far is enough for this "for" loop
			{

				// Find the each segments position using the slope from above
				Vector3 vector = (seperation*indexovertime) + transform.position;	
				//print("segmentadded");
				segmentPos.Add (vector);
				
				//Add Physics to the segments
				AddJointPhysics(indexovertime);
				Physics2D.IgnoreCollision(((GameObject)joints[indexovertime - 1]).collider2D, target.collider2D);
				Physics2D.IgnoreCollision(((GameObject)joints[indexovertime - 1]).collider2D, transform.collider2D);
				if (istube && this.GetComponentInChildren<CraneController>().focus != null) {
					Physics2D.IgnoreCollision(this.GetComponentInChildren<CraneController>().focus.collider2D, ((GameObject)joints[indexovertime-1]).collider2D);
					
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

					end.distance = ((this.transform.position.x - target.position.x))/segments;
					
					end.connectedBody = ((GameObject)joints[joints.Count-1]).transform.rigidbody2D;
					target.rigidbody2D.gravityScale = 0;
					line.SetVertexCount(segments);
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
			print (lightintensity);
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
		}
	}
	void AddRope() {
		segments++;
		line.SetVertexCount(segments);
		segmentPos.Add(target.transform.position); //add to araylist
		print ("segments: " + segments + "segmentpos.count: " + segmentPos.Count);
		AddJointPhysics(joints.Count + 1); //increment physics points
		//target.transform.position = target.transform.position - (target.transform.position - (Vector3)segmentPos[segments - 1]);
		//((GameObject)joints[segments - 2]).transform.position = target.transform.position + Vector3.up;
		((GameObject)joints[segments-2]).GetComponent<SpringJoint2D>().connectedBody = ((GameObject)joints[segments-3]).rigidbody2D; //set the new one's connected body to be the one before
		target.GetComponent<SpringJoint2D>().connectedBody = ((GameObject)joints[segments-2]).rigidbody2D; //set the targets body to this last, new point
		Physics2D.IgnoreCollision(((GameObject)joints[segments-2]).collider2D, target.rigidbody2D.collider2D); //cleaning up collision
		Physics2D.IgnoreCollision(((GameObject)joints[segments-2]).collider2D, this.rigidbody2D.collider2D);
		if (istube && this.GetComponentInChildren<CraneController>().focus != null) {
			Physics2D.IgnoreCollision(this.GetComponentInChildren<CraneController>().focus.collider2D, ((GameObject)joints[segments-2]).collider2D);

		}

	}
	void SubRope() {
		if (segments > 1) {
			Destroy(((GameObject)joints[segments - 1]));
			target.GetComponent<SpringJoint2D>().connectedBody = ((GameObject)joints[segments-2]).rigidbody2D;
			joints.Remove(joints[segments-1]);
			segments--;
			line.SetVertexCount(segments);


			((GameObject)joints[segments-2]).GetComponent<SpringJoint2D>().frequency = frequency * 10;
			((GameObject)joints[segments-3]).GetComponent<SpringJoint2D>().frequency = frequency;
		}
	}
	void LateUpdate()
	{
		// Does rope exist? If so, update its position
		if(rope && !isgenerating) {
			/*//print(collisionpoint);
			if (collisionpoint != Vector3.zero) {
				print("SEetting");
				GameObject.Find("Joint_1").transform.position = collisionpoint;
			}*/
			for(int i=0;i<segments;i++) {
				if(i == 0) {
					if (ejected) {
						line.SetPosition(i,connector.transform.position);
					}
					else {
						line.SetPosition(i,this.transform.position);
					}

				} else
				if(i == segments-1) {
					line.SetPosition(i,target.transform.position);	
				} else {
					line.SetPosition(i,((GameObject)joints[i - 1]).transform.position);
				}
			}
			line.enabled = true;
		} else {
			if (isgenerating) {
				line.SetVertexCount(2);
				line.SetPosition (1, this.transform.position);
				line.SetPosition (0, target.transform.position);
			} else {
				line.enabled = false;
			}
				
		}
	}
	void AddJointPhysics(int n)
	{

		//print ("A new member has joined!");
		joints.Add (new GameObject("Joint_" + n));
		//print("Joint added");
		if (startmade){ 

			((GameObject)joints[n - 1]).transform.parent = target.transform;
		} else {
			((GameObject)joints[n - 1]).transform.parent = transform;
		}

		//joints [n].transform.position = new Vector3 (this.transform.position.x + (this.transform.position.x - target.position.x)/n, 0, 0);
		Rigidbody2D rigid = ((GameObject)joints[n - 1]).AddComponent<Rigidbody2D>();
		CircleCollider2D col = ((GameObject)joints[n - 1]).AddComponent<CircleCollider2D>();
		SpringJoint2D ph = ((GameObject)joints[n - 1]).AddComponent<SpringJoint2D>();
		((GameObject)joints[n-1]).AddComponent<RigidIgnorer>();
		ph.collideConnected = false;
		ph.frequency = frequency;
		ph.dampingRatio = 2;
		rigid.isKinematic = false;
		//ph.useLimits = false;
		ph.distance = ((this.transform.position.x - target.position.x))/segments;
		//ph.anchor = new Vector2(1/(resolution), 1/(resolution));
		//ph.anchor = new Vector2 (((this.transform.position.x - target.position.x))/segments, ((this.transform.position.y - target.position.y))/segments);
		//print (ph.anchor);
		
		rigid.gravityScale = 0; 
		//print(segmentPos.Count);
		((GameObject)joints[n - 1]).transform.position = (((Vector3)segmentPos[n]));

		//segmentPos.

		//rigid.drag = ropeDrag;
		rigid.mass = ropeMass;
		col.radius = .03f;
		col.sharedMaterial = mate;
		//print ("value: " + n + " and segments = "+segments);
		if(n==1){		
			ph.connectedBody = transform.rigidbody2D;
		} else
		{
			ph.connectedBody = ((GameObject)joints[n-2]).rigidbody2D;	
		}

		
	}
	void BuildRope()
	{
		rope = true;
		line = gameObject.GetComponent<LineRenderer>();
		segmentPos = new ArrayList();
		joints = new ArrayList();
		// Find the amount of segments based on the distance and resolution
		// Example: [resolution of 1.0 = 1 joint per unit of distance]
		//print("Distance = " + (Vector3.Distance(transform.position, target.position)*resolution));
		segments = (int)(Vector3.Magnitude(transform.position - target.position)*resolution ) + 1;
		if (segments < 20) {
			segments = 20;
		}
		
		print ("segments " + segments);
		line.SetVertexCount(segments);
		line.material = ropemat;
		segmentPos.Add(transform.position);

		
		// Find the distance between each segment
		var segs = segments-1;
		var seperation = ((target.position - transform.position)/segs);
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
		isgenerating = false;
		GameObject.Find ("Crane").GetComponent<CraneController> ().grabbed = false;
		// Stop Rendering Rope then Destroy all of its components
		rope = false;
		for(int dj=0;dj<joints.Count;dj++)
		{
			Destroy((GameObject)joints[dj]);	
		}
		
		segmentPos.Clear();
		joints.Clear();
		segments = 0;
	}	
	void SetTargetAnchor(Vector3 vec) {
		/*print (this.transform.position);
		print (vec);
		//print (this.transform.position - (Vector3)vec);
		collisionpoint = (vec);*/
	}
	void Eject() {
		if (!ejected) {
			ejected = true;
			connector = new GameObject("Connector");
			Rigidbody2D rg = connector.AddComponent<Rigidbody2D>();
			CircleCollider2D cc = connector.AddComponent<CircleCollider2D>();
			SpriteRenderer sp = connector.AddComponent<SpriteRenderer>();

			rg.gravityScale = 0;
			cc.isTrigger = true;
			//connector.transform.parent = transform;	
			rg.mass = ropeMass;
			sp.sprite = (spriteconnector);
			sp.color = new Color(sp.color.r, sp.color.g, sp.color.b, lightintensity);
			connector.transform.position = this.transform.position + (Vector3.back);
			GameObject.Find("Joint_1").GetComponent<SpringJoint2D>().connectedBody = rg;
		}
	}
	void Connect() {
		if (ejected && !death) {
			GameObject.Find("Joint_1").GetComponent<SpringJoint2D>().connectedBody = GameObject.Find("Player").rigidbody2D;
			Destroy(connector);
			ejected = false;
		}
	}
	void DeathIsSoon() {
		Eject();
		death = true;
	}
	void UpdateFocus() { //so rope can ignore the item that is being carried (some weird glitch happens which makes it ridiculously bouncy) 	

	}
}
