using UnityEngine;
using System.Collections;

public class PlayerCollisionController : MonoBehaviour {

	private bool ejected;
	public AudioClip bump;
    public AudioClip bumbFaster;
	// Creating SpriteRender (and a LineRenderer) variables for the Fading Screen,
	//the Player sprite,
	//and the sprites of all of the parts of the Player's Crane,
	// so that I can make them disappear when the screen fades to black
	private GUITexture Fader;
	private SpriteRenderer Arrow,Back,Front,Left,Right,Cw,Ccw;
	private LineRenderer Crane;
	
	private float ejectcooldown;
	private GameObject faderObject;
    private RandomPitch randomPitch;

	void OnCollisionStay2D(Collision2D col) {

	}
	void OnCollisionEnter2D(Collision2D col) {
		if (col.gameObject.GetComponent<SpringJoint2D>() == null) { //not cord
			//this.gameObject.GetComponent<AudioSource>().volume = .5f;
			if (Vector3.Magnitude(col.relativeVelocity) > .5f) {
				if (Vector3.Magnitude(col.relativeVelocity) > 4) {
					this.SendMessage("changeHealth",-1 * col.relativeVelocity.magnitude);
					this.gameObject.GetComponent<AudioSource>().volume = 1f;
				}
				float pitchtmp = this.GetComponent<AudioSource>().pitch;
                this.GetComponent<AudioSource>().pitch = randomPitch.RandomPitchValue();
				this.GetComponent<AudioSource>().PlayOneShot(bump);
				this.gameObject.GetComponent<AudioSource>().volume = 1f;
				this.GetComponent<AudioSource>().pitch = pitchtmp;
			}

			GameObject.Find("Camera").SendMessage("Shake",Vector3.Magnitude(col.relativeVelocity)); //shake the camera and jiggle

		}
		
	}
	void OnCollisionExit2D(Collision2D col) {} //no uses yet
	void OnTriggerStay2D(Collider2D col) {
		if (ejected) {

			if (col.name.Equals("Innercol")) { //find the connector if ejected
				if (ejectcooldown > 5) {
					ejectcooldown = 0;
					GameObject.Find("Ship").BroadcastMessage("Connect");
					print ("Connecting");
				}
			}
		}
	}
	void OnTriggerExit2D(Collider2D col) {
	}
	// Use this for initialization
	void Start () {
		randomPitch = this.gameObject.AddComponent<RandomPitch>();
		/*Here I initialize my many, many SpriteRenderer and LineRenderer variables
		 that help make the ship look like it disappears right before the screen turns black*/

		Arrow = GameObject.Find ("Ending").GetComponent<SpriteRenderer> ();
		Crane = GameObject.Find ("Crane").GetComponent<LineRenderer> ();
		Left = GameObject.Find ("LeftThruster").GetComponent<SpriteRenderer> ();
		Right = GameObject.Find ("RightThruster").GetComponent<SpriteRenderer> ();
		Front = GameObject.Find ("FrontThruster").GetComponent<SpriteRenderer> ();
		Back = GameObject.Find ("BackThruster").GetComponent<SpriteRenderer> ();
		Cw = GameObject.Find ("ThrusterCW").GetComponent<SpriteRenderer> ();
		Ccw = GameObject.Find ("ThrusterCCW").GetComponent<SpriteRenderer> ();
		faderObject = GameObject.Find("Fader");
		Fader = faderObject.GetComponent<GUITexture> ();


		Physics2D.IgnoreLayerCollision(0,8);
		//Physics2D.IgnoreLayerCollision(0,10);
		//Physics2D.IgnoreLayerCollision(10,8);
	}

	// Update is called once per frame

	void Update () {


		ejected = ((RopeScript2D)GameObject.Find("Ship").GetComponent("RopeScript2D")).ejected || ((RopeScript2D)GameObject.Find("Ship").GetComponent("RopeScript2D")).brokenrope;

		// Always setting the fading screen sprite's x/y coordinates to those of the player
		//Fader.transform.position = new Vector3(GameObject.Find("Camera").transform.position.x,GameObject.Find("Camera").transform.position.y,GameObject.Find("Camera").transform.position.z + 7);
		if (ejected) {
			ejectcooldown += Time.deltaTime;



		}

	}
	void FaderTime(float f) {
	//	Fader.transform.localScale = new Vector3(442.6756f, 163.451f, 10); //large

		if (f <= 0) { // if we are dead

			Fader.color = new Color(Fader.color.r, Fader.color.g, Fader.color.b, Fader.color.a + Time.deltaTime/3);
			this.SendMessage("EMP");
			this.BroadcastMessage("wearedying");
			Arrow.color = new Color(Arrow.color.r, Arrow.color.g, Arrow.color.b, 0);
			Front.color = new Color(Front.color.r, Front.color.g, Front.color.b, 0);
			Back.color = new Color(Back.color.r, Back.color.g, Back.color.b, 0);
			Left.color = new Color(Left.color.r, Left.color.g, Left.color.b, 0);
			Right.color = new Color(Right.color.r, Right.color.g, Right.color.b, 0);
			GameObject.Find("Crane").GetComponent <CraneController>().deadThrusters = true;
			Cw.color = new Color(Cw.color.r, Cw.color.g, Cw.color.b, 0);
			Ccw.color = new Color(Ccw.color.r, Ccw.color.g, Ccw.color.b, 0);
			Crane.SetColors (new Color(0,0,0,0), new Color(0,0,0,0));
			if (Fader.color.a >= 1) {
				Application.LoadLevel("GameOver");
			}
			
		} else {
			Fader.color = new Color(Fader.color.r, Fader.color.g, Fader.color.b, (.5f - f)); //1 = opaque, 0 = transparent
		}
	}

}
