using UnityEngine;
using System.Collections;

public class ParticleThing : MonoBehaviour {
	private float time;
	public float distanceLasting = 2.5f;
	private Vector3 startScale; 
	public bool changingColor;
	public Color startColor = new Color(255,255,255);
	public Color endColor = new Color(255,255,255);
	//particle that will move away and get smaller
	void Start() {
		//this.transform.localScale = new Vector3(1.5f,1.5f,1.5f);
		startScale = this.transform.localScale;
	}
	// Update is called once per frame
	void Update () {
		if (changingColor) this.GetComponent<SpriteRenderer>().color = new Color(startColor.r + (5*time)*(startColor.r - endColor.r), startColor.g + (5*time)*(startColor.g - endColor.g), startColor.b + (5*time)*(startColor.b - endColor.b));
		Color c = this.GetComponent<SpriteRenderer>().color;

		time+=Time.deltaTime;
		float foo;
		//if (time < (1/distanceLasting)) foo = c.a * distanceLasting;
		//else foo = c.a/time;
		foo = c.a - (time/distanceLasting);
		//this.transform.localScale = new Vector3(foo, foo, 1);
		this.GetComponent<SpriteRenderer>().color = new Color(c.r,c.g,c.b,foo);
		if (foo <= 0) {
			Destroy(this.gameObject);
		}
	}
}
