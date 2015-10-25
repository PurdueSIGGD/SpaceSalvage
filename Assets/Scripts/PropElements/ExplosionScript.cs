using UnityEngine;
using System.Collections;

public class ExplosionScript : MonoBehaviour {
	// Explosion that is spawned by missiles

	private float time = 0;
	public float speed = 1;
	public float damage = 3;
	public Color cee;
	public bool damageoremp = true; //true for damage, false for emp
	public float radius = 1;
	public GameObject particle;
	// Use this for initialization
	void Start () {

		SpriteRenderer sp = this.GetComponent<SpriteRenderer>();
		Light l = this.GetComponentInChildren<Light>();
		l.intensity = 0;
		if (!damageoremp) {
			sp.color = cee;
			l.color = cee;
		}
		Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius); 
		foreach (Collider2D c in hitColliders) {
			if (damageoremp) {

				//push
				if (c.GetComponent<Rigidbody2D>() != null) c.GetComponent<Rigidbody2D>().AddForce(damage * (c.transform.position - this.transform.position));
				//damage
				if (c.GetComponent<HealthController>() != null) c.SendMessage("changeHealth",-1 * damage * Vector3.Distance(c.transform.position, this.transform.position));
				//break
				if (c.GetComponent<JointScript>() != null) {
					foreach (JointScript j in c.GetComponents<JointScript>()) j.SendMessage("BrokenJoint");
				}
				//explode
				if (c.GetComponent<MissileScript>() != null)  c.SendMessage("explode");
			} else {
				if (c.name.Equals("Player") || c.GetComponent<WallTurret>() != null || c.GetComponent<Chaser>()) c.SendMessage("EMP");
			}
		}
		for (int i = 0; i < damage; i++) {
			GameObject thingy = (GameObject)Instantiate(particle, this.transform.position, Quaternion.identity);
			thingy.GetComponent<SpriteRenderer>().color = this.GetComponent<SpriteRenderer>().color;
			thingy.GetComponent<Rigidbody2D>().AddForce(new Vector2(UnityEngine.Random.Range(-50,50), UnityEngine.Random.Range(-50,50)));
		}

	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime; //meant for the explosion, and the sprite showing
		if (time <  .5f/speed) {
			this.GetComponentInChildren<Light>().intensity = 2;
			this.transform.localScale = new Vector3(this.transform.localScale.x + 6 * Time.deltaTime, this.transform.localScale.y + 6 * Time.deltaTime, 1);
		} else if (time < 2/speed) {
			this.GetComponentInChildren<Light>().intensity = 2 - time;
			this.transform.localScale = new Vector3(this.transform.localScale.x - 3 * Time.deltaTime, this.transform.localScale.y - 3 * Time.deltaTime, 1);
		} else if (time > 2/speed && time < 8/speed) {
			this.transform.localScale = Vector3.zero;
			this.GetComponentInChildren<Light>().intensity = 0;

		} else if (time > 8/speed) {
			Destroy(this.gameObject);
		}
	}
}
