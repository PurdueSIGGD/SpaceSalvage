using UnityEngine;
using System.Collections;
using System;

public class TubeController : MonoBehaviour {
	public int tubesleft = 50;
	public Vector3 tubeorigin;
	private float ejectcooldown;
	private bool returnkey;
	private bool ejectkey;
	private bool ejected = false;
	private int tubecount = 0; //because I am stupid and +- 1 errors are the bane of my existence
	private float tubelength = .5f;
	private Vector3[] tubes; //There is no function to get a value at the index for LineRenderer
	private float dist = 0;
	private float timesincelast = 4;
	private float timesincedrop = 4	;
	// Use this for initialization
	void Start () {
		tubes = new Vector3[tubesleft + 1];
		LineRenderer l = (LineRenderer) GetComponent<LineRenderer>();
		l.SetColors(Color.cyan, Color.cyan);
		l.SetVertexCount (2);
		l.SetPosition (0, tubeorigin);
		l.SetPosition (1, this.transform.position);
		tubes[0] = (tubeorigin); //Start with two values, one being in ship, one on you (free to change);
		tubecount = 2;
		tubesleft -= tubecount;
		ejectkey = false;
		returnkey = false;
		
	}
	
	// Update is called once per frame
	void Update () {
			
		returnkey = Input.GetKey (KeyCode.Q);
		if (!ejectkey && !returnkey) { //Stop everything if ejected
					ejectkey = Input.GetKey (KeyCode.G);
					tubes [tubecount - 1] = this.transform.position; //Update position
					LineRenderer l = (LineRenderer)GetComponent<LineRenderer> ();
					dist = Vector3.Magnitude ((tubes [tubecount - 2] - transform.position)); //far away enough?
					l.SetPosition (tubecount - 1, this.transform.position);
					timesincelast += Time.deltaTime;
					timesincedrop += Time.deltaTime;

					if (dist > tubelength  && tubesleft > 0) { //can we drop it like it is hot?

							timesincedrop = 0;
							tubesleft--;
							tubecount++; 
							l.SetVertexCount (tubecount);
							l.SetPosition (tubecount - 1, this.transform.position);
							dist = 0;
					}
						
					if ( tubesleft == 0 || dist > tubelength) {
						this.transform.rigidbody2D.AddForce((tubes[tubecount - 2] - transform.position));
				
					}
						
		}

	}
	void FixedUpdate() {
		if (ejected) {
			ejectcooldown += Time.fixedDeltaTime;
			if (ejectcooldown >= 4 && (Math.Abs(this.transform.position.x - ((Vector3)tubes [tubecount - 1]).x) < .6) && (Math.Abs (this.transform.position.y - ((Vector3)tubes [tubecount - 1]).y) < .6 )) {
				ejectkey = false; //come back from ejecting
				ejected = false;
				ejectcooldown = 0;
			}
		} 
		else {
			LineRenderer l = (LineRenderer)GetComponent<LineRenderer> ();
			tubes [tubecount - 1] = this.transform.position; //Update position
			l.SetPosition (tubecount - 1, this.transform.position);
			if (returnkey) { //In order to pick up tubes
				if (tubecount > 2 && Math.Abs (this.transform.position.x - ((Vector3)tubes [tubecount - 2]).x) < .4 && this.transform.position.y - ((Vector3)tubes [tubecount - 2]).y < .4 ) {
					tubecount--;
					dist = 0;
					tubesleft++;
					l.SetVertexCount (tubecount);
					timesincelast = 0;
					tubes [tubecount] = new Vector3 (0, 0, 0); //clear value


				}
				this.transform.rigidbody2D.AddForce(5 * (tubes[tubecount - 2] - transform.position));
				returnkey = true;
					
				
			}

		}

		if (ejectkey) {
			if (!ejected) {
				this.transform.rigidbody2D.AddForce(80 * (transform.position - tubes[tubecount - 2]));
			}
			ejected = true;


		}
	}

}
