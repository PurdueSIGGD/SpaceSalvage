using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {


	// Use this for initialization
	void Start () {

	}
	private bool up;
	private bool down;
	private bool right;
	private bool left;
	// Update is called once per frame
	void Update() {
		up = Input.GetKey (KeyCode.W);
		down = Input.GetKey (KeyCode.S);
		right = Input.GetKey (KeyCode.D);
		left = Input.GetKey (KeyCode.A);


		}
	void FixedUpdate () {

		if (right)
		{
			this.rigidbody2D.AddForce(new Vector2(80 * Time.deltaTime, 0));

		}
		if (left)
		{
			this.rigidbody2D.AddForce(new Vector2(-80 * Time.deltaTime, 0));
		}
		if (up)
		{
			this.rigidbody2D.AddForce(new Vector2(0,80 * Time.deltaTime));
		}
		if (down)
		{
			this.rigidbody2D.AddForce(new Vector2(0,-80 * Time.deltaTime));
		}

	}
}
