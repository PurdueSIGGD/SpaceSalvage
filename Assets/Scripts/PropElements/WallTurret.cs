using UnityEngine;
using System.Collections;

public class WallTurret : MonoBehaviour {

	//Needs to be redone

	private GameObject barrel, Player;
	public GameObject ammo;
	public bool empshooty = false; //false if damage, true if emp	
	public bool homig;
	public float shotsperburst = 3;
	public float firingspeed = 1.5f;
	public float angleStart = 0;
	public float angleEnd = 0;
	public bool startBigSide;
	public bool clockwise, resetting, resettingStep;
	private bool focused, emp;
	private float timebetweenshots;
	private float shots;
	private float emprecharge;

	// Use this for initialization
	void Start () {
		if (homig) shotsperburst = 1;
		focused = false;
		barrel = this.transform.FindChild("Barrel").gameObject;
		Player = GameObject.Find("Player");
		if (startBigSide) {
			barrel.transform.eulerAngles = new Vector3(0,0,(angleStart + angleEnd)/2 + 180);
		} else {
			barrel.transform.eulerAngles = new Vector3(0,0,(angleStart + angleEnd)/2);
		}
		clockwise = true;
	}

	void Focus(bool b) {
		focused = b;
		if (!b) resetting = true;
	}
	void EMP() {
		emp = true;
	}
	// Update is called once per frame
	void Update () {
		//print(resetting);
		if (!emp) {
			if (focused) {
				timebetweenshots+=Time.deltaTime;
				barrel.GetComponent<Rigidbody2D>().angularVelocity = 0;
				float thetaersnenig;
				Vector3 pz = this.transform.position;
				thetaersnenig = (Mathf.Atan( ((pz.y - (Player.transform.position.y)) /(pz.x - Player.transform.position.x)))); //angle from mouse to me, formatting later
				thetaersnenig = thetaersnenig/2;
				if (thetaersnenig < 0) {
					thetaersnenig+= Mathf.PI/2;
				}
				if (pz.y - Player.transform.position.y < 0) {
					thetaersnenig+= Mathf.PI/2;
				}
				thetaersnenig = thetaersnenig * 2 * Mathf.Rad2Deg; //fooooormatting, make it move around in circles
				barrel.transform.eulerAngles = new Vector3(0,0,thetaersnenig);
				if ((timebetweenshots >= .75f && shots < shotsperburst)|| (timebetweenshots > firingspeed)) { //fire
					this.GetComponent<AudioSource>().Play();
					if (timebetweenshots > 1.5f) shots = 0;
					timebetweenshots = 0;
					shots++;
					GameObject thingy = (GameObject)Instantiate(ammo, barrel.transform.FindChild("OtherBarrel").transform.position, Quaternion.identity);
					thingy.transform.eulerAngles = new Vector3(0,0,barrel.transform.eulerAngles.z + 180);
					if (homig) thingy.SendMessage("GetTarget",Player);
					Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), thingy.GetComponent<Collider2D>());
					if (!ammo.name.Equals("Missile")) {

						thingy.GetComponent<Rigidbody2D>().AddForce(100 * new Vector2(Mathf.Cos(Mathf.Deg2Rad * (barrel.transform.eulerAngles.z+180)),Mathf.Sin(Mathf.Deg2Rad * (barrel.transform.eulerAngles.z+180))));
						thingy.GetComponent<Rigidbody2D>().AddTorque(50);
					} else {
						thingy.GetComponent<MissileScript>().damageoremp = !empshooty;
					}
				}

			} else {
				timebetweenshots = 0;
				//print(barrel.transform.eulerAngles.z + " " + angleStart + " " + angleEnd);
				if (resetting) {
					barrel.transform.eulerAngles = new Vector3(0,0,(angleStart + angleEnd)/2);
					resetting = false;
				}
				if (Mathf.Abs(barrel.transform.localEulerAngles.z - angleStart) < 10) {
					//print("angleStart");
					clockwise = !clockwise;
					barrel.transform.eulerAngles = new Vector3(0,0,this.transform.parent.eulerAngles.z + angleStart + (clockwise?-1:1) * Mathf.Abs(barrel.transform.localEulerAngles.z - angleStart));
				}
				if (Mathf.Abs(barrel.transform.localEulerAngles.z - angleEnd) < 10) {
					//print("angleEnd");
					clockwise = !clockwise;
					barrel.transform.eulerAngles = new Vector3(0,0,this.transform.parent.eulerAngles.z + angleEnd + (clockwise?-1:1) * Mathf.Abs(barrel.transform.localEulerAngles.z - angleEnd));

				}
				barrel.transform.eulerAngles = new Vector3(0,0,barrel.transform.eulerAngles.z + (clockwise?-1:1) * 30 * Time.deltaTime);
				
			}
		} else {
			emprecharge += Time.deltaTime;
			if (emprecharge > 10) {
				emprecharge = 0;
				emp = false;
			}
		}


	}
}
