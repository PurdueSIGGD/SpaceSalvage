using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	public int position = 0;
	public int sampleRate = 0;
	public float frequency = 440;
	public float moverate = 2;
	public bool emp;
	private SpriteRenderer BackThruster;
	private SpriteRenderer FrontThruster;
	private SpriteRenderer LeftThruster;
	private SpriteRenderer RightThruster;
	private static double BackAngle = 270;
	private static double FrontAngle = 90;
	private static double LeftAngle = 180;
	private static double RightAngle = 0;
	public float emprechargetime = 10;
	private float currentemptime;
	public bool debugmode;
	public AudioClip move;

	// Use this for initialization
	void Start () {
		if (PlayerPrefs.HasKey("moverate")) {
			moverate = PlayerPrefs.GetFloat("moverate");
			if (debugmode) moverate = 3;
		} else {
			PlayerPrefs.SetFloat("moverate", moverate);
		}
		if (PlayerPrefs.HasKey ("emprechargetime")) {
			emprechargetime = PlayerPrefs.GetFloat("emprechargetime");
		} else {
			PlayerPrefs.SetFloat("emprechargetime", emprechargetime);
		}
		BackThruster = GameObject.Find ("BackThruster").GetComponent<SpriteRenderer> ();
		FrontThruster = GameObject.Find ("FrontThruster").GetComponent<SpriteRenderer> ();
		LeftThruster = GameObject.Find ("LeftThruster").GetComponent<SpriteRenderer> ();
		RightThruster = GameObject.Find ("RightThruster").GetComponent<SpriteRenderer> ();
		currentemptime = 0;
	}
	void EMP() {
		currentemptime = 0;
		emp = true;
		GameObject.Find("Ship").GetComponent<RopeTubeController>().emp = true;
		GameObject.Find ("Crane").GetComponent<CraneController>().emp = true;
		this.GetComponent<HealthController>().emp = true;
	}
	private bool up;
	private bool down;
	private bool right;
	private bool left;

	// Update is called once per frame

	void Update() {
		if (emp) {
			currentemptime+= Time.deltaTime;
			if (currentemptime > emprechargetime) {
				emp = false;
				GameObject.Find("Ship").GetComponent<RopeTubeController>().emp = false;
				GameObject.Find ("Crane").GetComponent<CraneController>().emp = false;
				this.GetComponent<HealthController>().emp = false;

			}
			this.GetComponent<HealthController>().emptime = currentemptime;
		}
		up = Input.GetKey (KeyCode.W);
		down = Input.GetKey (KeyCode.S);
		right = Input.GetKey (KeyCode.D);
		left = Input.GetKey (KeyCode.A);
	}
	bool inRange(double d, double rangestart, double rangeend) {
		return (!left && (d + this.transform.rotation.eulerAngles.z + 180) % 360  > rangestart % 360 && (d + this.transform.rotation.eulerAngles.z + 180) % 360 < rangeend % 360);
	}
	bool inRangeLeft(double d, double rangestart, double rangeend) {
		bool arg1 = (d + this.transform.rotation.eulerAngles.z + 180 + 360)  > rangestart % 360 && (d + this.transform.rotation.eulerAngles.z + 180) % 360 < rangeend;
		bool arg2 = (d + this.transform.rotation.eulerAngles.z + 180) % 360  > rangestart % 360 && (d + this.transform.rotation.eulerAngles.z + 180) % 360 < rangeend + 360;

		return ( left && (arg1 || arg2));
	}
	//Handles movement and stuff
	void FixedUpdate () {
		if (!emp) {
			double rangestart = 0;
			double rangeend = 0;
			bool flying = false;
			if (right)
			{
				this.rigidbody2D.AddForce(new Vector2( moverate /  (70 * Time.deltaTime), 0));
				rangestart = 100;
				rangeend = 260;
				flying = true;
			}
			if (left)
			{
				this.rigidbody2D.AddForce(new Vector2(-1 * moverate / (70*Time.deltaTime), 0));
				rangestart = 280;
				rangeend = 80;
				flying = true;
			}
			if (up)
			{
				this.rigidbody2D.AddForce(new Vector2(0, moverate / (70*Time.deltaTime)));
				if (flying) {
					if (right) {
						rangestart = 125;
						rangeend = 315;

					}
					if (left) {
						rangestart = 190;
						rangeend = 80;
					}
				
				} else {
					// ("Only up");
					rangestart = 190;
					rangeend = 350;
				}

				flying = true;
			}
			if (down)
			{
				this.rigidbody2D.AddForce(new Vector2(0, -1 * moverate /(70 * Time.deltaTime)));
				if (flying) {
					if (right) {
						rangestart = 10;
						rangeend = 260;
					}
					if (left) {
						rangestart = 315;
						rangeend = 125;
					}
					
				} else {
					rangestart = 10;
					rangeend = 170;
				}
				flying = true;
			}


			if (flying) {
				//this.GetComponent<AudioSource>().Play();

				//BackAngle
				if (inRange(BackAngle, rangestart, rangeend)|| inRangeLeft(BackAngle, rangestart, rangeend)) {
					if (BackThruster.color.a < 1) {
						BackThruster.color = new Color (BackThruster.color.r, BackThruster.color.g, BackThruster.color.b, BackThruster.color.a + Time.deltaTime * 5);
					}
				} else {
					if (BackThruster.color.a > 0) {
						BackThruster.color = new Color (BackThruster.color.r, BackThruster.color.g, BackThruster.color.b, BackThruster.color.a - Time.deltaTime * 5);
					}
				}
				//FrontAngle
				if (inRange(FrontAngle, rangestart, rangeend)|| inRangeLeft(FrontAngle, rangestart, rangeend)) {
					if (FrontThruster.color.a < 1) {
						FrontThruster.color = new Color (FrontThruster.color.r, FrontThruster.color.g, FrontThruster.color.b, FrontThruster.color.a + Time.deltaTime * 5);
					}
				} else {
					if (FrontThruster.color.a > 0) {
						FrontThruster.color = new Color (FrontThruster.color.r, FrontThruster.color.g, FrontThruster.color.b, FrontThruster.color.a - Time.deltaTime * 5);
					}
				}
				//LeftAngle
				if (inRange(LeftAngle, rangestart, rangeend) || inRangeLeft(LeftAngle, rangestart, rangeend)) {
					if (LeftThruster.color.a < 1) {
						LeftThruster.color = new Color (LeftThruster.color.r, LeftThruster.color.g, LeftThruster.color.b, LeftThruster.color.a + Time.deltaTime * 5);
					}
				} else {
					if (LeftThruster.color.a > 0) {
						LeftThruster.color = new Color (LeftThruster.color.r, LeftThruster.color.g, LeftThruster.color.b, LeftThruster.color.a - Time.deltaTime * 5);
					}
				}
				//RightAngle
				if (inRange(RightAngle, rangestart, rangeend) || inRangeLeft(RightAngle, rangestart, rangeend)) {
					if (RightThruster.color.a < 1) {
						RightThruster.color = new Color (RightThruster.color.r, RightThruster.color.g, RightThruster.color.b, RightThruster.color.a + Time.deltaTime * 5);
					}
				} else {
					if (RightThruster.color.a > 0) {
						RightThruster.color = new Color (RightThruster.color.r, RightThruster.color.g, RightThruster.color.b, RightThruster.color.a - Time.deltaTime * 5);
					}
				}
				
			} else {
				if (BackThruster.color.a > 0) {
					BackThruster.color = new Color (BackThruster.color.r, BackThruster.color.g, BackThruster.color.b, BackThruster.color.a - Time.deltaTime * 5);
				}
				if (FrontThruster.color.a > 0) {
					FrontThruster.color = new Color (FrontThruster.color.r, FrontThruster.color.g, FrontThruster.color.b, FrontThruster.color.a - Time.deltaTime * 5);
				}
				if (LeftThruster.color.a > 0) {
					LeftThruster.color = new Color (LeftThruster.color.r, LeftThruster.color.g, LeftThruster.color.b, LeftThruster.color.a - Time.deltaTime * 5);
				}
				if (RightThruster.color.a > 0) {
					RightThruster.color = new Color (RightThruster.color.r, RightThruster.color.g, RightThruster.color.b, RightThruster.color.a - Time.deltaTime * 5);
				}
			}
		} else {
			if (BackThruster.color.a > 0) {
				BackThruster.color = new Color (BackThruster.color.r, BackThruster.color.g, BackThruster.color.b, BackThruster.color.a - Time.deltaTime * 2);
			}
			if (FrontThruster.color.a > 0) {
				FrontThruster.color = new Color (FrontThruster.color.r, FrontThruster.color.g, FrontThruster.color.b, FrontThruster.color.a - Time.deltaTime * 2);
			}
			if (LeftThruster.color.a > 0) {
				LeftThruster.color = new Color (LeftThruster.color.r, LeftThruster.color.g, LeftThruster.color.b, LeftThruster.color.a - Time.deltaTime * 2);
			}
			if (RightThruster.color.a > 0) {
				RightThruster.color = new Color (RightThruster.color.r, RightThruster.color.g, RightThruster.color.b, RightThruster.color.a - Time.deltaTime * 2);
			}
		}
	}
}
