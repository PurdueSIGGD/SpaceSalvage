using UnityEngine;
using System.Collections;

public class PlayerCollisionController : MonoBehaviour {

	private bool ejected;

	// Creating SpriteRender (and a LineRenderer) variables for the Fading Screen,
	//the Player sprite,
	//and the sprites of all of the parts of the Player's Crane,
	// so that I can make them disappear when the screen fades to black
	private SpriteRenderer Fader;
	private SpriteRenderer PlayerSprite,Arrow,Back,Front,Left,Right,Cw,Ccw;
	private LineRenderer Crane;
	
	private float ejectcooldown;
	private GameObject faderObject;


	void OnCollisionStay2D(Collision2D col) {

	}
	void OnCollisionEnter2D(Collision2D col) {
		if (Vector3.Magnitude(col.relativeVelocity) > 4) {
			this.SendMessage("changeHealth",-1 * col.relativeVelocity);


		}
		
	}
	void OnCollisionExit2D(Collision2D col) {}
	void OnTriggerStay2D(Collider2D col) {
		if (ejected) {

			if (col.name.Equals("Connector")) {
				if (ejectcooldown > 5) {
					ejectcooldown = 0;
					GameObject.Find("Ship").BroadcastMessage("Connect");
					print ("Connecting");
				}
			}
		}
		//abletopickup = true;
	}
	void OnTriggerExit2D(Collider2D col) {
		//abletopickup = false;
	}
	// Use this for initialization
	void Start () {
		
		/*Here I initialize my many, many SpriteRenderer and LineRenderer variables
		 that help make the ship look like it disappears right before the screen turns black*/

		PlayerSprite = GameObject.Find ("Player").GetComponent<SpriteRenderer> ();
		Arrow = GameObject.Find ("Ending").GetComponent<SpriteRenderer> ();
		Crane = GameObject.Find ("Crane").GetComponent<LineRenderer> ();
		Left = GameObject.Find ("LeftThruster").GetComponent<SpriteRenderer> ();
		Right = GameObject.Find ("RightThruster").GetComponent<SpriteRenderer> ();
		Front = GameObject.Find ("FrontThruster").GetComponent<SpriteRenderer> ();
		Back = GameObject.Find ("BackThruster").GetComponent<SpriteRenderer> ();
		Cw = GameObject.Find ("ThrusterCW").GetComponent<SpriteRenderer> ();
		Ccw = GameObject.Find ("ThrusterCCW").GetComponent<SpriteRenderer> ();
		faderObject = GameObject.Find("Fader");
		Fader = faderObject.GetComponent<SpriteRenderer> ();


		Physics2D.IgnoreLayerCollision(0,8);
		//((GUIText)text.GetComponent("GUIText")).text = "Suit Integrity = " + health;
	}

	// Update is called once per frame

	void Update () {


		//pickupkey = Input.GetKey (KeyCode.F);
		ejected = ((RopeScript2D)GameObject.Find("Ship").GetComponent("RopeScript2D")).ejected || ((RopeScript2D)GameObject.Find("Ship").GetComponent("RopeScript2D")).brokenrope;

		// Always setting the fading screen sprite's x/y coordinates to those of the player
		Fader.transform.position = this.transform.position;
		if (ejected) {
			ejectcooldown += Time.deltaTime;



		}

	}
	void FaderTime(float f) {
		Fader.transform.localScale = new Vector3(442.6756f, 163.451f, 10);

		if (f <= 0) {

			Fader.color = new Color(Fader.color.r, Fader.color.g, Fader.color.b, Fader.color.a + Time.deltaTime/3);
			this.SendMessage("EMP");
			/*Here is where, during the 'GAME OVER' stage,
				 * the entire ship disappears (by _immediately_ becoming transparent)*/
			//PlayerSprite.color = new Color(PlayerSprite.color.r, PlayerSprite.color.g, PlayerSprite.color.b, 0);
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
	void FixedUpdate() {


	}

}
