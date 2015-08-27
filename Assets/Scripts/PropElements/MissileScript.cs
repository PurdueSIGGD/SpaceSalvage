using UnityEngine;
using System.Collections;

public class MissileScript : MonoBehaviour {
	public bool thrusting = true; //different than fuel because objects can not be thrusting, then be thrusting
	public float thrustrate = 2;
	public float fuel = 3;
	public float damage = .5f; //max damage at close range
	public float radius = 1;
	public Color empcolor;
	public Color damagecolor;
	public GameObject explosion;
	public bool damageoremp = true; //damage if true, emp if false
	private bool homing;
	private GameObject target;
	private Transform thruster;
	private float time = 0;
	private float flashing;
	private bool flashingb;
	// Use this for initialization
	void Start () {
		thruster = transform.FindChild("Thruster");
		SpriteRenderer sp = this.GetComponent<SpriteRenderer>();
		if (damageoremp) { //color of main sprite
			sp.color = damagecolor;
		} else {
			sp.color = empcolor;
		}
	}

	// Update is called once per frame
	void Update () {

		SpriteRenderer c = thruster.GetComponent<SpriteRenderer>();
		SpriteRenderer cc = this.transform.FindChild("Nose").GetComponent<SpriteRenderer>();

		time += Time.deltaTime;
		if (thrusting) {
			if (c.color.a < 1) {

				c.color = new Color(c.color.r, c.color.g, c.color.b, c.color.a + Time.deltaTime);
				cc.color = new Color(cc.color.r, cc.color.g, cc.color.b, 1);

			} 
			if (homing) {
				if (flashingb) {
					flashing+= Time.deltaTime;
				} else {
					flashing-= Time.deltaTime;
				}
				if ((flashing >= .5f && flashingb) || (flashing <= 0 && !flashingb)) {
					flashingb = !flashingb;
				} 				
				cc.color = new Color(cc.color.r, cc.color.g, cc.color.b, flashing/.5f);
			} else {
				cc.color = new Color(cc.color.r, cc.color.g, cc.color.b, 0);
			}
			float angle = this.transform.eulerAngles.z;
			this.fuel -= Time.deltaTime;
			if (fuel <= 0) thrusting = false;
			if (homing && target != null) {
							this.GetComponent<Rigidbody2D>().AddForce(2 * new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)));

				float thetaersnenig;
				Vector3 pz = this.transform.position;
				thetaersnenig = (Mathf.Atan( ((pz.y - (target.transform.position.y)) /(pz.x - target.transform.position.x)))); //angle from mouse to me, formatting later
				thetaersnenig = thetaersnenig/2;
				if (thetaersnenig < 0) {
					thetaersnenig+= Mathf.PI/2;
				}
				if (pz.y - target.transform.position.y < 0) {
					thetaersnenig+= Mathf.PI/2;
				}
				thetaersnenig = thetaersnenig * 2 * Mathf.Rad2Deg; //fooooormatting
				this.transform.eulerAngles = new Vector3(0,0, thetaersnenig + 180);
				this.GetComponent<Rigidbody2D>().velocity = (2 * (target.transform.position - this.transform.position)/Vector3.Distance(target.transform.position, this.transform.position));
				//this.rigidbody2D.AddForce(2 * new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)));

			} else {
				this.GetComponent<Rigidbody2D>().AddForce(2 * new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)));

			}

		} else {
			if (c.color.a >= 0) {
				c.color = new Color(c.color.r, c.color.g, c.color.b, c.color.a - Time.deltaTime);
				cc.color = new Color(cc.color.r, cc.color.g, cc.color.b, cc.color.a - Time.deltaTime);

			} 
		}
		if (time > 300) explode();
	}
	void OnCollisionEnter2D(Collision2D coll) {
		if (Vector3.Magnitude(coll.relativeVelocity) >  2 || (homing && fuel > 0)) {
			explode();
		}

	}
	void OnTriggerEnter2D(Collider2D col) {
		if (col.GetComponent<DestructionStation>() != null ) {
			explode();
		}
	}
	void GetTarget(GameObject g) {
		homing = true;
		target = g;
		this.fuel *= 5;
	}
	void explode() {
		//add force to all objects around

		GameObject thingy = (GameObject)Instantiate(explosion,transform.position, Quaternion.identity);
		thingy.GetComponent<ExplosionScript>().damageoremp = this.damageoremp;
		thingy.GetComponent<ExplosionScript>().damage = damage;
		if (this.GetComponent<RopeScript2D>() != null) {
			this.BroadcastMessage("DestroyRope");

		}
		Destroy(this.gameObject);



	}
}
