using UnityEngine;
using System.Collections;

public class CraneController : MonoBehaviour {
	public GameObject Player;
	public GameObject focus;
	public Vector3 current;
	private Vector3 pz;
	private Vector3 delta;
	private Vector3 playerdelta;
	private Vector3 difference;
	public float movespeed = .5f;
	public float changedmovespeed;
	public float cranelength = 2;
	public bool grabbed = false;
	private bool rot = false; 
	private bool ended = false;

	// Use this for initialization
	void Start () {
		Player = GameObject.Find("Player");
		current = Player.transform.position;
		changedmovespeed = movespeed;
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
							print ("Got one");
							focus = hitCollider.gameObject;
							grabbed = true;
						 
							focus.transform.rigidbody2D.angularVelocity = 0;
							Physics2D.IgnoreCollision(focus.collider2D, GameObject.Find("Player").collider2D);
							((PlayerMovement)Player.GetComponent("PlayerMovement")).moverate = 2;
						}
					}
			} else {
				grabbed = false;
				((PlayerMovement)Player.GetComponent("PlayerMovement")).moverate = 1;
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
		;
		}
		current += ((pz - current) * Time.deltaTime * changedmovespeed);

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
			if (ended) {
				Physics2D.IgnoreCollision(focus.collider2D, GameObject.Find("Player").collider2D, false);
				((Rigidbody2D)focus.GetComponent("Rigidbody2D")).velocity = 60 * (focus.transform.position - delta);
				ended = false;
			}
		}

		Transform ending = transform.FindChild("Ending");
		ending.position = (new Vector3(current.x, current.y, 0));
		//print ( Mathf.Asin((ending.position.x - Player.transform.position.x )/( ending.position.y - Player.transform.position.y)));
		//ending.Rotate(new Vector3(0,0,45));

		playerdelta = Player.transform.position;
	}
	void FixedUpdate () {

		if (rot && grabbed) {
			print ("Rotate");
			//focus.transform.rotation = new Quaternion(0,0,Time.DeltaTime,0);
			focus.transform.Rotate(Vector3.back);
			//focus.transform.RotateAround(Vector3.zero, current, Time.deltaTime * 50);
		}
	
		


	}
}
