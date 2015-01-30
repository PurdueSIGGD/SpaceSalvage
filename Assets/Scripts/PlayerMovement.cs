using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	public float health = 100;
	public int position = 0;
	public int sampleRate = 0;
	public float frequency = 440;
	public float moverate = 1;
	//AudioClip move = AudioClip.Create ("Meow", 100000, 2, 44100, true, false);

	// Use this for initialization
	void Start () {

	}

	private bool up;
	private bool down;
	private bool right;
	private bool left;

	// Update is called once per frame

	void Update() {
		AudioClip move = AudioClip.Create ("Meow", 100000, 2, 44100, true, false);

		up = Input.GetKey (KeyCode.W);
		down = Input.GetKey (KeyCode.S);
		right = Input.GetKey (KeyCode.D);
		left = Input.GetKey (KeyCode.A);

		if (right) {
			audio.clip = move;
			audio.Play();
		}
		if (left) {
			audio.clip = move;
			audio.Play();
		}
		if (up) {
			audio.clip = move;
			audio.Play();
		}
		if (down) {
			audio.clip = move;
			audio.Play();
		}

	}

	//Handles physics and stuff
	void FixedUpdate () {


		if (right)
		{
			this.rigidbody2D.AddForce(new Vector2(80 * 1/moverate * Time.deltaTime, 0));

		}
		if (left)
		{
			this.rigidbody2D.AddForce(new Vector2(-80 * 1/moverate * Time.deltaTime, 0));
		}
		if (up)
		{
			this.rigidbody2D.AddForce(new Vector2(0,80 * 1/moverate * Time.deltaTime));
		}
		if (down)
		{
			this.rigidbody2D.AddForce(new Vector2(0,-80 * 1/moverate * Time.deltaTime));
		}

	}
}
