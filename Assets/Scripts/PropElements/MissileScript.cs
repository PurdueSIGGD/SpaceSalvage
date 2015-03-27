using UnityEngine;
using System.Collections;

public class MissileScript : MonoBehaviour {
	public bool thrusting = true; //different than fuel because objects can not be thrusting, then be thrusting
	public float thrustrate = 2;
	public float fuel = 3;
	public float damage = .5f; //max damage at close range
	public float radius = 1;
	private float time = 0;
	public GameObject explosion;
	public bool damageoremp = true; //damage if true, emp if false
	private Transform thruster;
	// Use this for initialization
	void Start () {
		thruster = transform.FindChild("Thruster");
	}

	// Update is called once per frame
	void Update () {
		SpriteRenderer c = thruster.GetComponent<SpriteRenderer>();
		time += Time.deltaTime;
		if (thrusting) {
			//print (c.ToString());
			if (c.color.a < 1) {
				//print("Chaaange");
				c.color = new Color(c.color.r, c.color.g, c.color.b, c.color.a + Time.deltaTime);
			}
			float angle = this.transform.eulerAngles.z;
			//print(angle = this.transform.eulerAngles.z);
			this.rigidbody2D.AddForce(2 * new Vector2(Mathf.Cos(Mathf.Deg2Rad * angle), Mathf.Sin(Mathf.Deg2Rad * angle)));
			this.fuel -= Time.deltaTime;
			if (fuel <= 0) thrusting = false;
		} else {
			if (c.color.a >= 0) {
				c.color = new Color(c.color.r, c.color.g, c.color.b, c.color.a - Time.deltaTime);
			}
		}
		if (time > 300) explode();
	}
	void OnCollisionEnter2D(Collision2D coll) {
		Collider2D col = coll.collider.collider2D;
		if (Vector3.Magnitude(coll.relativeVelocity) >  2) {
			explode();
		}

	}
	void explode() {
		//add force to all objects around

		GameObject thingy = (GameObject)Instantiate(explosion,transform.position, Quaternion.identity);
		thingy.GetComponent<ExplosionScript>().damageoremp = this.damageoremp;
		if (this.GetComponent<RopeScript2D>() != null) {
			this.BroadcastMessage("DestroyRope");

		}
		Destroy(this.gameObject);



	}
}
