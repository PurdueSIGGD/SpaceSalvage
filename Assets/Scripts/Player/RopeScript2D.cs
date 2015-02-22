using UnityEngine;
using System.Collections;

public class RopeScript2D : MonoBehaviour {
	public Transform target;
	public float resolution;///0.5F;							  //  Sets the amount of joints there are in the rope (1 = 1 joint for every 1 unit)
	public float ropeDrag = 0.1F;								 //  Sets each joints Drag
	public float ropeMass = 0.1F;							//  Sets each joints Mass
	public float ropeColRadius = 0.5F;	//  Sets the radius of the collider in the SphereCollider component
	//public float ropeseglength = .025f; 
	//public float ropeBreakForce = 25.0F;					 //-------------- TODO (Hopefully will break the rope in half...
	private Vector3[] segmentPos;			//  DONT MESS!	This is for the Line Renderer's Reference and to set up the positions of the gameObjects
	private GameObject[] joints;			//  DONT MESS!	This is the actual joint objects that will be automatically created
	private LineRenderer line;							//  DONT MESS!	 The line renderer variable is set up when its assigned as a new component
	private int segments = 0;					//  DONT MESS!	The number of segments is calculated based off of your distance * resolution
	private bool rope = false;						 //  DONT MESS!	This is to keep errors out of your debug window! Keeps the rope from rendering when it doesnt exist...
	
	//Joint Settings
	public bool usemotor = false;
	public bool UseLimits = false;

	void Start() {
		line = gameObject.GetComponent<LineRenderer>();
		//target.GetComponent<HingeJoint2D>().anchor = target.transform.position;
	}

	/*void Awake()
	{
		BuildRope();
	}*/
	
	void Update()
	{

	}
	void LateUpdate()
	{
		// Does rope exist? If so, update its position
		if(rope) {
			for(int i=0;i<segments;i++) {
				if(i == 0) {
					line.SetPosition(i,transform.position);
				} else
				if(i == segments-1) {
					line.SetPosition(i,target.transform.position);	
				} else {
					line.SetPosition(i,joints[i].transform.position);
				}
			}
			line.enabled = true;
		} else {
			line.enabled = false;	
		}
	}
	void AddJointPhysics(int n)
	{
		//print ("A new member has joined!");
		joints[n] = new GameObject("Joint_" + n);
		joints[n].transform.parent = transform;
		//joints [n].transform.position = new Vector3 (this.transform.position.x + (this.transform.position.x - target.position.x)/n, 0, 0);
		Rigidbody2D rigid = joints[n].AddComponent<Rigidbody2D>();
		CircleCollider2D col = joints[n].AddComponent<CircleCollider2D>();
		HingeJoint2D ph = joints[n].AddComponent<HingeJoint2D>();
		ph.collideConnected = false;
		ph.useLimits = false;
	
		//ph.anchor = new Vector2(1/(resolution), 1/(resolution));
		ph.anchor = new Vector2 (((this.transform.position.x - target.position.x))/segments, ((this.transform.position.y - target.position.y))/segments);
		//print (ph.anchor);
		
		rigid.gravityScale = 0; 
		joints[n].transform.position = segmentPos[n];
		
		rigid.drag = ropeDrag;
		rigid.mass = ropeMass;
		col.radius = ropeColRadius;
		//print ("value: " + n + " and segments = "+segments);
		if(n==1){		
			ph.connectedBody = transform.rigidbody2D;
		} else
		{
			ph.connectedBody = joints[n-1].rigidbody2D;	
		}
		
	}
	void BuildRope()
	{
		rope = true;
		line = gameObject.GetComponent<LineRenderer>();
		
		// Find the amount of segments based on the distance and resolution
		// Example: [resolution of 1.0 = 1 joint per unit of distance]
		//print("Distance = " + (Vector3.Distance(transform.position, target.position)*resolution));
		segments = (int)(Vector3.Magnitude(transform.position - target.position)*resolution ) + 1;
		print ("segments = " + segments);
		line.SetVertexCount(segments);
		segmentPos = new Vector3[segments];
		joints = new GameObject[segments];
		segmentPos[0] = transform.position;
		segmentPos[segments-1] = target.position;
		
		// Find the distance between each segment
		var segs = segments-1;
		var seperation = ((target.position - transform.position)/segs);
		
		for(int s=1;s < segments;s++)
		{
			// Find the each segments position using the slope from above
			Vector3 vector = (seperation*s) + transform.position;	
			segmentPos[s] = vector;
			
			//Add Physics to the segments
			AddJointPhysics(s);
			Physics2D.IgnoreCollision(joints[s].collider2D, target.collider2D);
			Physics2D.IgnoreCollision(joints[s].collider2D, transform.collider2D);
			if (s>1) {
				Physics2D.IgnoreCollision(joints[s].collider2D, joints[s-1].collider2D);
			}
		}
		
		// Attach the joints to the target object and parent it to this object	
		HingeJoint2D end = target.gameObject.AddComponent<HingeJoint2D>();

		end.connectedBody = joints[joints.Length-1].transform.rigidbody2D;
		target.rigidbody2D.gravityScale = 0;

		//target.parent = transform;
	}
	void DestroyRope()
	{
		// Stop Rendering Rope then Destroy all of its components
		rope = false;
		for(int dj=0;dj<joints.Length-1;dj++)
		{
			Destroy(joints[dj]);	
		}
		
		segmentPos = new Vector3[0];
		joints = new GameObject[0];
		segments = 0;
	}	
}
