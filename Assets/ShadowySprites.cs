using UnityEngine;
using System.Collections;

public class ShadowySprites : MonoBehaviour {

	// Use this for initialization
	void Start () {

		if (this.GetComponent<Renderer>() != null) {
			this.GetComponent<Renderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.On;
			GetComponent<Renderer>().receiveShadows = true;
		}
		//UnityEngine.Shader shader = this.GetComponent<SpriteRenderer>().material.shader;
		//shader.
		//this.GetComponent<SpriteRenderer>().material.shader.
		//this.GetComponent<SpriteRenderer>().shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.TwoSided;
		//this.GetComponent<SpriteRenderer>().receiveShadows = true;
	}
	
	// Update is called once per frame
	void Update () {
		//if (this.GetComponent<SpriteRenderer>() != null) print(this.GetComponent<SpriteRenderer>().shadowCastingMode);
	}
}
