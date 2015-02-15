using UnityEngine;
using System.Collections;
using System;

public class TubeController : MonoBehaviour {
	public int tubesleft = 50;
	public Vector3 tubeorigin;
	private float ejectcooldown;
	public bool emp;
	private bool returnkey;
	private bool lastkey; //also toggle
	private bool returning; //toggle
	private bool reelkey;
	private bool ejectkey;
	public bool ejected = false;
	private bool death = false;
	private int tubecount = 0; //because I am stupid and +- 1 errors are the bane of my existence
	private float tubelength = .5f;
	private Vector3[] tubes; //There is no function to get a value at the index for LineRenderer
	private float dist = 0;
	// Use this for initialization
	void Start () {
		if (PlayerPrefs.HasKey("tubesleft")) {
			tubesleft = PlayerPrefs.GetInt("tubesleft");
		} else {
			PlayerPrefs.SetInt("tubesleft",tubesleft);
		}

		tubes = new Vector3[tubesleft + 1];
		LineRenderer l = (LineRenderer) GetComponent<LineRenderer>();
		l.SetColors(Color.cyan, Color.cyan);
		l.SetVertexCount (2);
		l.SetPosition (0, new Vector3(tubeorigin.x, tubeorigin.y, 1));
		l.SetPosition (1, new Vector3(this.transform.position.x, this.transform.position.y, 1));
		tubes[0] = (tubeorigin); //Start with two values, one being in ship, one on you (free to change);
		tubecount = 2;
		tubesleft -= tubecount;
		ejectkey = false;
		returnkey = false;
		lastkey = false;
		reelkey = false;
	}
	
	// Update is called once per frame
	void Update () {

		LineRenderer l = (LineRenderer)GetComponent<LineRenderer> ();
		returnkey = Input.GetKey (KeyCode.E);
		reelkey = Input.GetKey (KeyCode.Q);
		if (!ejectkey && !returning) { //Stop everything if ejected
					ejectkey = Input.GetKey (KeyCode.G);
					dist = Vector3.Magnitude ((tubes [tubecount - 2] - transform.position)); //far away enough?
					tubes [tubecount - 1] = this.transform.position; //Update position
					
					if (dist > tubelength  && tubesleft > 0) { //can we drop it like it is hot?
							tubesleft--;
							tubecount++; 
							l.SetVertexCount (tubecount);
							l.SetPosition(tubecount - 1, this.transform.position); //If I don't call it (even when it is called right after that) it flashes for a second. Bothers me.

							dist = 0;
					}
					if ( tubesleft == 0 || dist > tubelength) {
						this.transform.rigidbody2D.AddForce((tubes[tubecount - 2] - transform.position));
					}
						
		}


	}
	void DeathIsSoon() {
		ejectkey = true;
		death = true;
	}
	void FixedUpdate() {
		LineRenderer l = (LineRenderer)GetComponent<LineRenderer> ();
		 
		if (returning && !ejectkey) {
			if (tubecount != 2 && Vector3.Magnitude (transform.position - tubes[tubecount - 2]) < tubelength && Vector3.Magnitude (tubes [tubecount - 3] - (tubes[tubecount - 2] + (transform.position - tubes[tubecount - 2]))) < tubelength + tubelength/4) {
				tubes[tubecount - 2] += (transform.position - tubes[tubecount - 2	]) / 20	;
				l.SetPosition (tubecount - 2, tubes[tubecount - 2]);
			}
			
		}
		/*for (int i = 1; i < tubecount; i++) { //line bending
			if (returnkey && !ejectkey) {
				if (Vector3.Magnitude(tubes[i] - tubes[i + 1]) < 2.5 && Vector3.Magnitude(tubes[i] - tubes[i - 1]) < 2.5 && !(tubecount == 2)) {
					print ("moooooooove");
					tubes[i] += (tubes[i + 1] - tubes[i]) / 20;
				}

			} else {
				if (Vector3.Magnitude(tubes[i] - tubes[i + 1]) > 2 && Vector3.Magnitude(tubes[i] - tubes[i - 1]) > 2 && !(tubecount == 2)) {
					print ("returnnnnnnn");
					tubes[i] += (tubes[i - 1] - tubes[i]) / 20;
				}
			}

			//tubes[i] = tubes[i] + tubevelocity[i];
			l.SetPosition(i, tubes[i]);
		}*/

		if (ejected) {
			ejectcooldown += Time.fixedDeltaTime;
			if (!death && ejectcooldown >= 4 && (Math.Abs(this.transform.position.x - ((Vector3)tubes [tubecount - 1]).x) < .6) && (Math.Abs (this.transform.position.y - ((Vector3)tubes [tubecount - 1]).y) < .6 )) {
				ejectkey = false; //come back from ejecting
				ejected = false;
				ejectcooldown = 0;
			}
		} 
		else {

			tubes [tubecount - 1] = this.transform.position; //Update position
			l.SetPosition (tubecount - 1, this.transform.position);
			if (returnkey != lastkey) { //toggle
				if (returnkey) {
					returning = !returning;
				}
				lastkey = returnkey;
			}

			if (returning) { //In order to pick up tubes
				if (tubecount > 2 && Math.Abs (this.transform.position.x - ((Vector3)tubes [tubecount - 2]).x) < .2 && this.transform.position.y - ((Vector3)tubes [tubecount - 2]).y < .2 ) {
					tubecount--;
					dist = 0;
					tubesleft++;
					l.SetVertexCount (tubecount);
					tubes [tubecount] = new Vector3 (0, 0, 0); //clear value


				}
				if (Vector3.Magnitude (tubes [tubecount - 2] - transform.position) > tubelength + tubelength/2	) {
					this.transform.rigidbody2D.AddForce(2 * (tubes[tubecount - 2] - transform.position));
				}
				if (reelkey) { 
					this.transform.rigidbody2D.AddForce(3 * (tubes[tubecount - 2] - transform.position));
				}

					
				
			}

		}

		if (ejectkey && !emp	) {
			if (!ejected) {
				this.transform.rigidbody2D.AddForce(80 * (transform.position - tubes[tubecount - 2]));
			}
			ejected = true;


		}
	}

}
