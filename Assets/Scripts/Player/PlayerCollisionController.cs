using UnityEngine;
using System.Collections;

public class PlayerCollisionController : MonoBehaviour {
	public float health = 100;
	public float startingoxy = 60;
	public float oxy;
	private GameObject text;
	public int wallet = 0;

	// Creating SpriteRender (and a LineRenderer) variables for the Fading Screen,
	//the Player sprite,
	//and the sprites of all of the parts of the Player's Crane,
	// so that I can make them disappear when the screen fades to black
	private SpriteRenderer Fader;
	private SpriteRenderer PlayerSprite,Arrow,Back,Front,Left,Right,Cw,Ccw;
	private LineRenderer Crane;

	private GameObject faderObject;


	void OnCollisionStay2D(Collision2D col) {

	}
	void OnCollisionEnter2D(Collision2D col) {
		if (Vector3.Magnitude(col.relativeVelocity) > 4) {

			//print ("Ow don't do that, I have " + health + " health left");
			if (health < 0) {
				oxy -= Vector3.Magnitude(col.relativeVelocity);
				((GUIText)text.GetComponent("GUIText")).text = "Suit Integrity = " + 0;
				((TubeController)this.GetComponent("TubeController")).SendMessage("DeathIsSoon");
			} else {
				health -= Vector3.Magnitude(col.relativeVelocity);
				((GUIText)text.GetComponent("GUIText")).text = "Suit Integrity = " + health.ToString ("F2");
			}

		}
		
	}
	void OnCollisionExit2D(Collision2D col) {}
	void OnTriggerEnter2D(Collider2D col) {
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
		faderObject = GameObject.Find ("Fader");
		Fader = faderObject.GetComponent<SpriteRenderer> ();


		oxy = startingoxy;
		text = GameObject.Find("GuiText");
		Physics2D.IgnoreLayerCollision(0,8);
		//((GUIText)text.GetComponent("GUIText")).text = "Suit Integrity = " + health;
	}
	
	// Update is called once per frame

	void Update () {
		//pickupkey = Input.GetKey (KeyCode.F);
 
		// Always setting the fading screen sprite's x/y coordinates to those of the player
		Fader.transform.position = this.transform.position;
		if (((TubeController)this.GetComponent("TubeController")).ejected) {

			/*The following if-else statements ensure that the oxygen amount displayed on screen
			 is never negative*/
			if (oxy > 0){
				((GUIText)text.GetComponent("GUIText")).text = "Oxygen Left = " + oxy.ToString ("F2");
			}
			else{
				((GUIText)text.GetComponent("GUIText")).text = "Oxygen Left = 0.00";
			}
			oxy -= Time.deltaTime*10;
			if (oxy <= 0) {

				/*  When the player ship dies, it stops moving */
				GameObject.Find ("Player").rigidbody2D.velocity = new Vector2(0,0);


				/*Here is where, during the 'GAME OVER' stage,
				 * the entire ship disappears (by _immediately_ becoming transparent)*/
				PlayerSprite.color = new Color(PlayerSprite.color.r, PlayerSprite.color.g, PlayerSprite.color.b, 0);
				Arrow.color = new Color(Arrow.color.r, Arrow.color.g, Arrow.color.b, 0);
				Front.color = new Color(Front.color.r, Front.color.g, Front.color.b, 0);
				Back.color = new Color(Back.color.r, Back.color.g, Back.color.b, 0);
				Left.color = new Color(Left.color.r, Left.color.g, Left.color.b, 0);
				Right.color = new Color(Right.color.r, Right.color.g, Right.color.b, 0);
				GameObject.Find("Crane").GetComponent <CraneController>().deadThrusters = true;
				Cw.color = new Color(Cw.color.r, Cw.color.g, Cw.color.b, 0);
				Ccw.color = new Color(Ccw.color.r, Ccw.color.g, Ccw.color.b, 0);
				Crane.SetColors (new Color(0,0,0,0), new Color(0,0,0,0));


				/*Here is where, during the 'GAME OVER' stage,
				 the fading screen _gradually_ blackens. Once the fader's color reaches
				 the point when it's completely black, the (completely invisible) 
				 player ship finally destroys itself*/
				if(Fader.color.a < 255) Fader.color = new Color(Fader.color.r, Fader.color.g, Fader.color.b, Fader.color.a + Time.deltaTime);
				else GameObject.Destroy (this.gameObject);
				
			}
		} else {
			((GUIText)text.GetComponent("GUIText")).text = "Suit Integrity = " + health.ToString ("F2");
			oxy = startingoxy;
		}

	}
	void FixedUpdate() {


	}
}
