using UnityEngine;
using System.Collections;

public class WallTurret : MonoBehaviour {
	private GameObject barrel, Player;
	public GameObject ammo;
	private bool focused;
	private float timebetweenshots;
	private float shots;
	// Use this for initialization
	void Start () {
		focused = false;
		barrel = this.transform.FindChild("Barrel").gameObject;
		Player = GameObject.Find("Player");
		Physics2D.IgnoreCollision(Player.collider2D,this.collider2D,false);
	}

	void Focus(bool b) {
		focused = b;
	}
	// Update is called once per frame
	void Update () {

		if (focused) {
			timebetweenshots+=Time.deltaTime;
			barrel.rigidbody2D.angularVelocity = 0;
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
			thetaersnenig = thetaersnenig * 2 * Mathf.Rad2Deg; //fooooormatting
			//print(thetaersnenig);
			//print( new Vector2(Mathf.Cos(Mathf.Deg2Rad * (thetaersnenig+180)),Mathf.Sin(Mathf.Deg2Rad * (thetaersnenig+180))));
		
			/*if (barrel.transform.eulerAngles.z != thetaersnenig) {
				barrel.transform.eulerAngles = new Vector3(0,0,barrel.transform.eulerAngles.z+Time.deltaTime);
			}*/
			barrel.transform.eulerAngles = new Vector3(0,0,thetaersnenig);
			if ((timebetweenshots >= .75f && shots < 3)|| (timebetweenshots > 1.5f)) {
				if (timebetweenshots > 1.5f) shots = 0;
				timebetweenshots = 0;
				shots++;
				GameObject thingy = (GameObject)Instantiate(ammo, barrel.transform.FindChild("OtherBarrel").transform.position, Quaternion.identity);
				thingy.rigidbody2D.AddForce(100 * new Vector2(Mathf.Cos(Mathf.Deg2Rad * (barrel.transform.eulerAngles.z+180)),Mathf.Sin(Mathf.Deg2Rad * (barrel.transform.eulerAngles.z+180))));
				thingy.rigidbody2D.AddTorque(22);
			}
		} else {
			timebetweenshots = 0;
			barrel.transform.eulerAngles = new Vector3(0,0,barrel.transform.eulerAngles.z+ 30 * Time.deltaTime);
		}


	}
}
