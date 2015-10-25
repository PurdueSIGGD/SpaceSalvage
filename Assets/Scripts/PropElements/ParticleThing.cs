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

		startScale = this.transform.localScale;
	}
	// Update is called once per frame
	void Update () {
		if (changingColor) this.GetComponent<SpriteRenderer>().color = new Color(startColor.r + (5*time)*(startColor.r - endColor.r), startColor.g + (5*time)*(startColor.g - endColor.g), startColor.b + (5*time)*(startColor.b - endColor.b));
		time+=Time.deltaTime;
		float foo;
		if (time < (1/distanceLasting)) foo = startScale.x * distanceLasting;
		else foo = startScale.x/time;
		this.transform.localScale = new Vector3(foo, foo, 1);
		if (foo <= .1f) {
			Destroy(this.gameObject);
		}
	}
}
