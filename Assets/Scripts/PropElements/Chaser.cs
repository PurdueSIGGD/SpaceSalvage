using UnityEngine;
using System.Collections;

public class Chaser : MonoBehaviour {

	/* An enemy that will chase you until... you know.... You're dead.
	 * 
	 */
	GameObject Player, barrel;
	bool focused, emp;
	float beepTime, beepInterval;
    private bool isLocked;
	float emprecharge;
	// Use this for initialization
	void Start () {
		barrel = this.transform.FindChild("Barrel").gameObject;
		if (GameObject.Find ("Player")) Player = GameObject.Find("Player");
        isLocked = false;
	}
	void Focus(bool b) { //dictated by the barrel, which uses a TurretRanger scipt. Uses that collision to identify the player
		if (!focused || Vector3.Distance(this.transform.position,Player.transform.position) > 4) focused = b;
	}
	void EMP() {
		emp = true;
	}
	// Update is called once per frame
	void Update () {
		if (!emp) {
			this.transform.FindChild("Beam").transform.eulerAngles = new Vector3(0,0,this.transform.FindChild("Beam").transform.eulerAngles.z + Time.deltaTime * 150); //spin child
			if (focused) {
				beepTime += Time.deltaTime;
				if (beepTime > beepInterval) {
					this.GetComponent<AudioSource>().Play();
					beepTime = 0;
				}
				beepInterval = Mathf.Pow(Vector3.Distance(this.transform.position, Player.transform.position)/3, 2);
                if (!isLocked)
                {
                    //this.GetComponent<AudioSource>().PlayOneShot(this.GetComponent<AudioSource>().clip);
                    isLocked = true;
                }
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
			
				barrel.transform.eulerAngles = new Vector3(0,0,thetaersnenig);
				this.GetComponent<Rigidbody2D>().AddForce(6 * Time.deltaTime * (Player.transform.position - this.transform.position) * Vector3.Distance(Player.transform.position, this.transform.position));
			} else {
				this.GetComponent<Rigidbody2D>().AddForce(-1 * this.GetComponent<Rigidbody2D>().velocity); //slow down 
				barrel.transform.eulerAngles = new Vector3(0,0,barrel.transform.eulerAngles.z+ 30 * Time.deltaTime); //spin ourselves
                isLocked = false;
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
