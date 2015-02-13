using UnityEngine;
using System.Collections;

public class PlayerCollisionController : MonoBehaviour {
	public float health = 100;
	public float startingoxy = 60;
	public float oxy;
	private GameObject text;
	public int wallet = 0;
	private SpriteRenderer Fader;
	private SpriteRenderer PlayerSprite,Arrow,Back,Front,Left,Right;
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

		PlayerSprite = GameObject.Find ("Player").GetComponent<SpriteRenderer> ();
		Arrow = GameObject.Find ("Ending").GetComponent<SpriteRenderer> ();
		Crane = GameObject.Find ("Crane").GetComponent<LineRenderer> ();
		Left = GameObject.Find ("LeftThruster").GetComponent<SpriteRenderer> ();
		Right = GameObject.Find ("RightThruster").GetComponent<SpriteRenderer> ();
		Front = GameObject.Find ("FrontThruster").GetComponent<SpriteRenderer> ();
		Back = GameObject.Find ("BackThruster").GetComponent<SpriteRenderer> ();
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

		//Fader.transform.position = 

		Fader.transform.position = this.transform.position;
		if (((TubeController)this.GetComponent("TubeController")).ejected) {
			if (oxy > 0){
				((GUIText)text.GetComponent("GUIText")).text = "Oxygen Left = " + oxy.ToString ("F2");
			}
			else{
				((GUIText)text.GetComponent("GUIText")).text = "Oxygen Left = 0.00";
			}
			//oxy -= Time.deltaTime;
			oxy -= Time.deltaTime;
			if (oxy <= 0) {
				GameObject.Find ("Player").rigidbody2D.velocity = new Vector2(0,0);
				PlayerSprite.color = new Color(PlayerSprite.color.r, PlayerSprite.color.g, PlayerSprite.color.b, 0);
				Arrow.color = new Color(Arrow.color.r, Arrow.color.g, Arrow.color.b, 0);
				Front.color = new Color(Front.color.r, Front.color.g, Front.color.b, 0);
				Back.color = new Color(Back.color.r, Back.color.g, Back.color.b, 0);
				Left.color = new Color(Left.color.r, Left.color.g, Left.color.b, 0);
				Right.color = new Color(Right.color.r, Right.color.g, Right.color.b, 0);
				Crane.SetColors (new Color(0,0,0,0), new Color(0,0,0,0));
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
