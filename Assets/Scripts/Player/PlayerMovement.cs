using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	public KeyCode kup = KeyCode.W;
	public KeyCode kdown = KeyCode.S;
	public KeyCode kleft = KeyCode.A;
	public KeyCode kright = KeyCode.D;

	public int position = 0;
	public int sampleRate = 0;
	public float frequency = 440;
	public float moverate = 2;
	public float startingmoverate;
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
    private AudioClip defaultClip;

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

		if (PlayerPrefs.HasKey("Up")) {
			kup = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Up")) ; //casting from string to keycode
		} else {
			PlayerPrefs.SetString("Up",kup.ToString());
		}
		if (PlayerPrefs.HasKey("Down")) {
			kdown = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Down")) ;
		} else {
			PlayerPrefs.SetString("Down",kup.ToString());
		}
		if (PlayerPrefs.HasKey("Left")) {
			kright = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Left")) ;
		} else {
			PlayerPrefs.SetString("Left",kup.ToString());
		}
		if (PlayerPrefs.HasKey("Right")) {
			kleft = (KeyCode) System.Enum.Parse(typeof(KeyCode), PlayerPrefs.GetString("Right")) ;
		} else {
			PlayerPrefs.SetString("Right",kup.ToString());
		}

		BackThruster = GameObject.Find ("BackThruster").GetComponent<SpriteRenderer> ();
		FrontThruster = GameObject.Find ("FrontThruster").GetComponent<SpriteRenderer> ();
		LeftThruster = GameObject.Find ("LeftThruster").GetComponent<SpriteRenderer> ();
		RightThruster = GameObject.Find ("RightThruster").GetComponent<SpriteRenderer> ();
		currentemptime = 0;
		startingmoverate = moverate;
        defaultClip = this.gameObject.GetComponent<AudioSource>().clip;
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
				currentemptime = 0;
				GameObject.Find("Ship").GetComponent<RopeTubeController>().emp = false;
				GameObject.Find ("Crane").GetComponent<CraneController>().emp = false;
				this.GetComponent<HealthController>().emp = false;
				this.GetComponent<HealthController>().emptime = 0;

			}
			this.GetComponent<HealthController>().emptime = currentemptime;
		}

        if ((left || right || up || down) && !this.gameObject.GetComponent<AudioSource>().isPlaying)
        {
            this.gameObject.GetComponent<AudioSource>().clip = move;
            this.gameObject.GetComponent<AudioSource>().Play();
        }
        else if (!left && !right && !up && !down)
        {
            this.gameObject.GetComponent<AudioSource>().Stop();
        }
        else
        {
            //this.gameObject.GetComponent<AudioSource>().Stop();
            this.gameObject.GetComponent<AudioSource>().clip = defaultClip;
        }

		if (this.GetComponent<Rigidbody2D>().velocity.y < 10) {
			up = Input.GetKey (kup);
		} else {
			up = false;
		}
		if (this.GetComponent<Rigidbody2D>().velocity.y > -	10) {
			down = Input.GetKey (kdown);
		} else {
			down = false;
		}
		if (this.GetComponent<Rigidbody2D>().velocity.x < 10) {
			right = Input.GetKey (kleft);
		} else {
			right = false;
		}
		if (this.GetComponent<Rigidbody2D>().velocity.x > -10) {
			left = Input.GetKey (kright);
		} else {
			left = false;
		}
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
		if (emp){
			moverate = startingmoverate / 4;
		} else  {
			moverate = startingmoverate;
		}
		double rangestart = 0;
		double rangeend = 0;
		bool flying = false;

		if (right)
		{
			this.GetComponent<Rigidbody2D>().AddForce(this.GetComponent<Rigidbody2D>().mass * new Vector2( moverate /  (70 * Time.deltaTime), 0));
			rangestart = 100;
			rangeend = 260;
			flying = true;
		}
		if (left)
		{
			this.GetComponent<Rigidbody2D>().AddForce(this.GetComponent<Rigidbody2D>().mass * new Vector2(-1 * moverate / (70*Time.deltaTime), 0));
			rangestart = 280;
			rangeend = 80;
			flying = true;
		}
		if (up)
		{
			this.GetComponent<Rigidbody2D>().AddForce(this.GetComponent<Rigidbody2D>().mass * new Vector2(0, moverate / (70*Time.deltaTime)));
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
			this.GetComponent<Rigidbody2D>().AddForce(this.GetComponent<Rigidbody2D>().mass * new Vector2(0, -1 * moverate /(70 * Time.deltaTime)));
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
		Light BLight = this.transform.FindChild("BackLight").GetComponent<Light>();
		Light FLight = this.transform.FindChild("FrontLight").GetComponent<Light>();
		Light LLight = this.transform.FindChild("LeftLight").GetComponent<Light>();
		Light RLight = this.transform.FindChild("RightLight").GetComponent<Light>();
		Light CLight = this.transform.FindChild("CWLight").GetComponent<Light>();
		Light CCLight = this.transform.FindChild("CCWLight").GetComponent<Light>();
		if (flying) {


			//this.GetComponent<AudioSource>().Play();
			if (!emp) {
				//BackAngle
				if (inRange(BackAngle, rangestart, rangeend)|| inRangeLeft(BackAngle, rangestart, rangeend)) {
					if (BackThruster.color.a < 1) {
						BackThruster.color = new Color (BackThruster.color.r, BackThruster.color.g, BackThruster.color.b, BackThruster.color.a + Time.deltaTime * 5);

					}
					if (BLight.intensity < 0.00f) {
						BLight.intensity += Time.deltaTime * 5;
					}
				} else {
					if (BackThruster.color.a > 0) {
						BackThruster.color = new Color (BackThruster.color.r, BackThruster.color.g, BackThruster.color.b, BackThruster.color.a - Time.deltaTime * 5);
					}
					if (BLight.intensity > 0) {
						BLight.intensity -= Time.deltaTime * 5;
					}

				}
				//FrontAngle
				if (inRange(FrontAngle, rangestart, rangeend)|| inRangeLeft(FrontAngle, rangestart, rangeend)) {
					if (FrontThruster.color.a < 1) {
						FrontThruster.color = new Color (FrontThruster.color.r, FrontThruster.color.g, FrontThruster.color.b, FrontThruster.color.a + Time.deltaTime * 5);
					}
					if (FLight.intensity < 0.00f) {
						FLight.intensity += Time.deltaTime * 5;
					}
				} else {
					if (FrontThruster.color.a > 0) {
						FrontThruster.color = new Color (FrontThruster.color.r, FrontThruster.color.g, FrontThruster.color.b, FrontThruster.color.a - Time.deltaTime * 5);
					}
					if (FLight.intensity > 0) {
						FLight.intensity -= Time.deltaTime * 5;
					}

				}
				//LeftAngle
				if (inRange(LeftAngle, rangestart, rangeend) || inRangeLeft(LeftAngle, rangestart, rangeend)) {
					if (LeftThruster.color.a < 1) {
						LeftThruster.color = new Color (LeftThruster.color.r, LeftThruster.color.g, LeftThruster.color.b, LeftThruster.color.a + Time.deltaTime * 5);
					}
					if (LLight.intensity < 0.00f) {
						LLight.intensity += Time.deltaTime * 5;
					}
				} else {
					if (LeftThruster.color.a > 0) {
						LeftThruster.color = new Color (LeftThruster.color.r, LeftThruster.color.g, LeftThruster.color.b, LeftThruster.color.a - Time.deltaTime * 5);
					}
					if (LLight.intensity > 0) {
						LLight.intensity -= Time.deltaTime * 5;
					}
				}
				//RightAngle
				if (inRange(RightAngle, rangestart, rangeend) || inRangeLeft(RightAngle, rangestart, rangeend)) {
					if (RightThruster.color.a < 1) {
						RightThruster.color = new Color (RightThruster.color.r, RightThruster.color.g, RightThruster.color.b, RightThruster.color.a + Time.deltaTime * 5);
					}
					if (BLight.intensity < 0.00f) {
						BLight.intensity += Time.deltaTime * 5;
					}
				} else {
					if (RightThruster.color.a > 0) {
						RightThruster.color = new Color (RightThruster.color.r, RightThruster.color.g, RightThruster.color.b, RightThruster.color.a - Time.deltaTime * 5);
					}
					if (RLight.intensity > 0) {
						RLight.intensity -= Time.deltaTime * 5;
					}
				}
			} else {
				if (BackThruster.color.a > 0) {
					BackThruster.color = new Color (BackThruster.color.r, BackThruster.color.g, BackThruster.color.b, BackThruster.color.a - Time.deltaTime * 2);
				}
				if (BLight.intensity > 0) {
					BLight.intensity -= Time.deltaTime * 5;
				}
				if (FrontThruster.color.a > 0) {
					FrontThruster.color = new Color (FrontThruster.color.r, FrontThruster.color.g, FrontThruster.color.b, FrontThruster.color.a - Time.deltaTime * 2);
				}
				if (FLight.intensity > 0) {
					FLight.intensity -= Time.deltaTime * 5;
				}
				if (LeftThruster.color.a > 0) {
					LeftThruster.color = new Color (LeftThruster.color.r, LeftThruster.color.g, LeftThruster.color.b, LeftThruster.color.a - Time.deltaTime * 2);
				}
				if (LLight.intensity > 0) {
					LLight.intensity -= Time.deltaTime * 5;
				}
				if (RightThruster.color.a > 0) {
					RightThruster.color = new Color (RightThruster.color.r, RightThruster.color.g, RightThruster.color.b, RightThruster.color.a - Time.deltaTime * 2);
				}
				if (RLight.intensity > 0) {
					RLight.intensity -= Time.deltaTime * 5;
				}
			}
				
		} else {
				if (BackThruster.color.a > 0) {
					BackThruster.color = new Color (BackThruster.color.r, BackThruster.color.g, BackThruster.color.b, BackThruster.color.a - Time.deltaTime * 5);
				}
				if (BLight.intensity > 0) {
					BLight.intensity -= Time.deltaTime * 5;
				}
				if (FrontThruster.color.a > 0) {
					FrontThruster.color = new Color (FrontThruster.color.r, FrontThruster.color.g, FrontThruster.color.b, FrontThruster.color.a - Time.deltaTime * 5);
				}
				if (FLight.intensity > 0) {
					FLight.intensity -= Time.deltaTime * 5;
				}
				if (LeftThruster.color.a > 0) {
					LeftThruster.color = new Color (LeftThruster.color.r, LeftThruster.color.g, LeftThruster.color.b, LeftThruster.color.a - Time.deltaTime * 5);
				}
				if (LLight.intensity > 0) {
					LLight.intensity -= Time.deltaTime * 5;
				}
				if (RightThruster.color.a > 0) {
					RightThruster.color = new Color (RightThruster.color.r, RightThruster.color.g, RightThruster.color.b, RightThruster.color.a - Time.deltaTime * 5);
				}
				if (RLight.intensity > 0) {
					RLight.intensity -= Time.deltaTime * 5;
				}
		}

			

	}
}
