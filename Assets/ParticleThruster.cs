using UnityEngine;
using System.Collections;

public class ParticleThruster : MonoBehaviour {
	private float oxyTime;
	private bool firing;

	public Color startingColor, endingColor;
	void Update() {
		oxyTime += Time.deltaTime;
		if (oxyTime > .05f && firing) {
			//GameObject.Find("Camera").SendMessage("Shake",60 * Time.deltaTime);
			GameObject thingy = (GameObject)Instantiate(GameObject.Find("Player").GetComponent<HealthController>().particle, this.transform.position, Quaternion.identity); //spawning particles
			float r = Random.value;
			//thingy.GetComponent<SpriteRenderer>().sprite = ;
			thingy.transform.localScale = new Vector3(1f, 1f, 1f); //typical scale is 5, dont want parts too big or small
			thingy.GetComponent<ParticleThing>().distanceLasting = .5f;
			thingy.GetComponent<SpriteRenderer>().color = new Color(.3f,.3f,.3f); //make it redder if necessary
			thingy.GetComponent<Rigidbody2D>().AddForce(this.transform.parent.GetComponent<Rigidbody2D>().velocity +  100 *((Vector2) (this.transform.position - this.transform.parent.transform.position)) + new Vector2(UnityEngine.Random.Range(-30,30),UnityEngine.Random.Range(-30,30)));
			thingy.GetComponent<ParticleThing>().changingColor = true;
			thingy.GetComponent<ParticleThing>().startColor = this.startingColor;
			thingy.GetComponent<ParticleThing>().endColor = this.endingColor;

			//thingy.GetComponent<Rigidbody2D>().AddTorque(thingy.GetComponent<Rigidbody2D>().mass * UnityEngine.Random.Range(-25,25));
			oxyTime = 0;
		}
	}

	public void spawnParticles(bool b) {
		firing = b;
	}

}
