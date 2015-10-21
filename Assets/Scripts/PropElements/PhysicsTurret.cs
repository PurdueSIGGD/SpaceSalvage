using UnityEngine;
using System.Collections;

public class PhysicsTurret : MonoBehaviour {

	public double rotationUpperLim = 275;
	public double rotationLowerLim = 90;
	private int initialTurnSpeed = 1;
	private GameObject Player;
	private GameObject BarrelPivot;
	private GameObject Barrel;
	private ConstantForce2D pivotForce;
	private Rigidbody2D pivotRB;
	private bool checkAngle = true;

	//firing variables
	public bool empshooty = false; //false if damage, true if emp
	public GameObject ammo;
	public bool homig;
	public bool focused, emp;
	public float shotsperburst = 3;
	public float firingspeed = 1.5f;
	private float timebetweenshots;
	private float shots;
	private float emprecharge;


	// Use this for initialization
	void Start () {
		Player = GameObject.Find ("Player");
		BarrelPivot = transform.FindChild ("BarrelPivot").gameObject; //get child GameObject BarrelPivot
		Barrel = BarrelPivot.transform.FindChild ("Barrel").gameObject;
		pivotForce = BarrelPivot.GetComponent<ConstantForce2D> (); //get ConstantForce2D on BarrelPivot
		pivotRB = BarrelPivot.GetComponent<Rigidbody2D> (); //get Rigidbody2D on BarrelPivot
		pivotForce.torque = -1 * initialTurnSpeed; //initial turret rotation
		//InvokeRepeating ("angleCap", 0, angleCheckRate);

		//firing setup
		if (homig) shotsperburst = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameObject.Find ("Player") && Vector3.Distance(GameObject.Find("Player").transform.position, this.transform.position) > 7) focused = false;
		BarrelPivot.transform.localPosition = Vector2.zero;
		angleCap ();
		if (!emp) {
			if (focused) {
				//Track player
				Quaternion trackRotation = getTrackingRotation(Player);
				BarrelPivot.transform.rotation = trackRotation;
				FiringSequence();
			}
		}

	}
	

	public void angleCap() {
		//Debug.Log ("checking");
		if (BarrelPivot.transform.localEulerAngles.z > rotationLowerLim && BarrelPivot.transform.localEulerAngles.z < rotationUpperLim) {
			if (checkAngle) {
			torqueReverse ();
			}
			checkAngle = false;
		} else {
			checkAngle = true;
		}
	}

	public void torqueReverse() {
		pivotForce.torque = pivotForce.torque * -1;
	}

	void Focus(bool b) {

		focused = b;
		//if (!b) resetting = true;
	}

	void EMP() {
		emp = true;
	}

	public Quaternion getTrackingRotation (GameObject target) {
		Quaternion look = Quaternion.LookRotation(target.transform.position - BarrelPivot.transform.position, BarrelPivot.transform.TransformDirection(Vector3.up));
		Quaternion lookRotation = new Quaternion(0, 0, look.z, look.w);
		return lookRotation;
	}

	public void FiringSequence() {
		if (!emp) {
			if (focused) {
				timebetweenshots+=Time.deltaTime;
				/*BarrelPivotthing.GetComponent<Rigidbody2D>().angularVelocity = 0;
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
				BarrelPivot.transform.eulerAngles = new Vector3(0,0,thetaersnenig);*/
				if ((timebetweenshots >= .75f && shots < shotsperburst)|| (timebetweenshots > firingspeed)) { //fire
					this.GetComponent<AudioSource>().Play();
					if (timebetweenshots > 1.5f) shots = 0;
					timebetweenshots = 0;
					shots++;
					GameObject projectile = (GameObject)Instantiate(ammo, Barrel.transform.FindChild("OtherBarrel").transform.position, Quaternion.identity);
					projectile.transform.eulerAngles = new Vector3(0,0,Barrel.transform.eulerAngles.z + 180);
					if (homig) projectile.SendMessage("GetTarget",Player);
					Physics2D.IgnoreCollision(this.GetComponent<Collider2D>(), projectile.GetComponent<Collider2D>());
					Physics2D.IgnoreCollision(BarrelPivot.GetComponent<Collider2D>(), projectile.GetComponent<Collider2D>());
					Physics2D.IgnoreCollision(Barrel.GetComponent<Collider2D>(), projectile.GetComponent<Collider2D>());
					if (!ammo.name.Equals("Missile")) {
						
						projectile.GetComponent<Rigidbody2D>().AddForce(100 * new Vector2(Mathf.Cos(Mathf.Deg2Rad * (Barrel.transform.eulerAngles.z+180)),Mathf.Sin(Mathf.Deg2Rad * (Barrel.transform.eulerAngles.z+180))));
						projectile.GetComponent<Rigidbody2D>().AddTorque(50);
					} else {
						projectile.GetComponent<MissileScript>().damageoremp = !empshooty;
					}
				}
				
			} else {
				timebetweenshots = 0;
				//print(BarrelPivot.transform.eulerAngles.z + " " + angleStart + " " + angleEnd);
				/*if (resetting) {
					BarrelPivot.transform.eulerAngles = new Vector3(0,0,(angleStart + angleEnd)/2);
					resetting = false;
				}
				if (Mathf.Abs(BarrelPivot.transform.localEulerAngles.z - angleStart) < 10) {
					//print("angleStart");
					clockwise = !clockwise;
					BarrelPivot.transform.eulerAngles = new Vector3(0,0,this.transform.parent.eulerAngles.z + angleStart + (clockwise?-1:1) * Mathf.Abs(BarrelPivot.transform.localEulerAngles.z - angleStart));
				}
				if (Mathf.Abs(BarrelPivot.transform.localEulerAngles.z - angleEnd) < 10) {
					//print("angleEnd");
					clockwise = !clockwise;
					BarrelPivot.transform.eulerAngles = new Vector3(0,0,this.transform.parent.eulerAngles.z + angleEnd + (clockwise?-1:1) * Mathf.Abs(BarrelPivot.transform.localEulerAngles.z - angleEnd));
					
				}
				BarrelPivot.transform.eulerAngles = new Vector3(0,0,BarrelPivot.transform.eulerAngles.z + (clockwise?-1:1) * 30 * Time.deltaTime);
				*/
				
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