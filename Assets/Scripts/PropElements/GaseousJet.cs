using UnityEngine;
using System.Collections;

public class GaseousJet : MonoBehaviour {
	// to push the player
	// has to transform children, and they choose where the airjet will be blowing
	public float maxspeed = 20;
	public float force = 1;
	private Transform top;
	private Transform bottom;
	public GameObject particle;
	private float time;

	// Use this for initialization
	void Start () {
		top = transform.FindChild("Top");
		bottom = transform.FindChild("Bottom");
	}
	void OnTriggerStay2D(Collider2D col) {
		if (col.GetComponent<Rigidbody2D>() && Vector2.SqrMagnitude(col.GetComponent<Rigidbody2D>().velocity) <= maxspeed) {
			col.GetComponent<Rigidbody2D>().AddForce(force * (top.position - bottom.position));
		}
	}
	// Update is called once per frame
	void Update () {
		time+=Time.deltaTime;
		if (time > .1f) {
			time = 0;
			Vector3 placement = transform.FindChild("Bottom").localPosition;
			placement += Vector3.right * Random.Range(-.5f,.5f);
			GameObject g = (GameObject)GameObject.Instantiate(particle, bottom.position + placement, new Quaternion());
			g.GetComponent<SpriteRenderer>().color = new Color(10,10,10);
			g.transform.localScale = new Vector3(.6f,.6f,.6f);
			g.GetComponent<ParticleThing>().distanceLasting = 2.5f;
			g.GetComponent<Rigidbody2D>().AddForce(40 * force  * (top.position-bottom.position));
		}
	}
}
