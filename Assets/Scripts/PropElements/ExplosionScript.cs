using UnityEngine;
using System.Collections;

public class ExplosionScript : MonoBehaviour {
	private float time = 0;
	public float speed = 1;
	public float damage = 3;
	public Color cee;
	public bool damageoremp = true; //true for damage, false for emp
	public float radius = 1;
	// Use this for initialization
	void Start () {
		this.GetComponent<AudioSource>().Play();

		SpriteRenderer sp = this.GetComponent<SpriteRenderer>();
		if (!damageoremp) sp.color = cee;
		Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius); 
		foreach (Collider2D c in hitColliders) {
			if (damageoremp) {

				//push
				if (c.GetComponent<Rigidbody2D>() != null) c.GetComponent<Rigidbody2D>().AddForce(damage * (c.transform.position - this.transform.position));
				//damage
				if (c.GetComponent<HealthController>() != null) c.SendMessage("changeHealth",-1 * damage * Vector3.Distance(c.transform.position, this.transform.position));
				//break
				if (c.GetComponent<JointScript>() != null) c.SendMessage("BrokenJoint");
				//explode
				if (c.GetComponent<MissileScript>() != null)  c.SendMessage("explode");
			} else {
				if (c.name.Equals("Player") || c.GetComponent<WallTurret>() != null || c.GetComponent<Chaser>()) c.SendMessage("EMP");
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if (time <  .5f/speed) {
			this.transform.localScale = new Vector3(this.transform.localScale.x + 6 * Time.deltaTime, this.transform.localScale.y + 6 * Time.deltaTime, 1);
		} else if (time < 1.5f/speed) {
			this.transform.localScale = new Vector3(this.transform.localScale.x - 3 * Time.deltaTime, this.transform.localScale.y - 3 * Time.deltaTime, 1);
		} else if (time > 1.5f/speed) {
			Destroy(this.gameObject);
		}
	}
}
