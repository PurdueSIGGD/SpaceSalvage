using UnityEngine;
using System.Collections;

public class CraneController : MonoBehaviour {
	public GameObject Player;
	public GameObject focus;
	public Vector3 current;
	public bool emp;
	private Vector3 pz;
	private Vector3 delta;
	private Vector3 playerdelta;
	private Vector3 difference;
	private float lastTheta;
	private float lastDeltaTheta;
	public float movespeed = .5f;
	public float changedmovespeed;
	public float cranelength = 2;
	public bool grabbed = false;
	private bool rot = false; 
	public bool ended = false;
	private bool heislettinggo = false;

	// The boolean deadThrusters is here to make the clockwise/counter-clockwise thrusters
	// not appear when the screen turns black
	public bool deadThrusters = false;

	// Use this for initialization
	void Start () {
		Player = GameObject.Find("Player");
		current = Player.transform.position;
		changedmovespeed = movespeed;
		lastTheta = Player.transform.rotation.z;
		lastDeltaTheta = 0;
		//emp = false;
	}

	// Update is called once per frame
	void Update () {
		this.transform.position = Player.transform.position;
		pz = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		pz.z = 0;
		if (Input.GetMouseButtonDown(0)) {
			if (!grabbed) {
				Collider2D hitCollider = Physics2D.OverlapCircle(current, .1f);
					if (hitCollider != null) {
						if (hitCollider.GetComponent("ItemPickup") != null) {
							//print ("Got one");
							focus = hitCollider.gameObject;
							grabbed = true;
						 
							focus.transform.rigidbody2D.angularVelocity = 0;
							Physics2D.IgnoreCollision(focus.collider2D, GameObject.Find("Player").collider2D);
						((PlayerMovement)Player.GetComponent("PlayerMovement")).moverate = ((PlayerMovement)Player.GetComponent("PlayerMovement")).moverate * 2;
						}
					}
			} else {
				grabbed = false;
				((PlayerMovement)Player.GetComponent("PlayerMovement")).moverate = ((PlayerMovement)Player.GetComponent("PlayerMovement")).moverate /2 ;
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
		if (!emp) {
			current += ((pz - current) * Time.deltaTime * changedmovespeed);
		}
		//current = pz + transform.position;
		current.z = .02f;
		l.SetPosition(0, Player.transform.position);
		l.SetPosition(1, current);
	

		if (grabbed) {
			//print (focus.transform.rigidbody2D.angularVelocity);
			if (!ended) {

				difference = focus.transform.position - current;
			}
			delta = focus.transform.position;
			focus.transform.position = current + difference;
			ended = true;
			
		} else {
			if (ended ) {

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
		//print	(thetaersnenig + "   " +  ending.rotation.eulerAngles.z);
		SpriteRenderer ThrusterCW = (SpriteRenderer)GameObject.Find ("ThrusterCW").GetComponent ("SpriteRenderer");
		SpriteRenderer ThrusterCCW = (SpriteRenderer)GameObject.Find ("ThrusterCCW").GetComponent ("SpriteRenderer");
		//print (ThrusterCW.color.a);
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
	}
	void I_am_letting_go_now () {
		heislettinggo = true;


	}
	void FixedUpdate () {

		if (rot && grabbed) {

			//print ("Rotate");
			//focus.transform.rotation = new Quaternion(0,0,Time.DeltaTime,0);
			focus.transform.Rotate(Time.deltaTime * 40 * Vector3.back);
			//focus.transform.RotateAround(Vector3.zero, current, Time.deltaTime * 50);
		}
	
		


	}
}
