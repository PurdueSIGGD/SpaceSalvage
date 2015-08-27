using UnityEngine;
using System.Collections;

public class MapCreation : MonoBehaviour {
	public GameObject coin;
	public GameObject CoinCrate;
	public GameObject HealthCrate;
	public GameObject OxyStation;
	public GameObject Airlock;
	public GameObject Laser;

	private bool blerp;
	void Start () {





	}

	void Update () {
		if (Input.GetKey(KeyCode.Y) && !blerp) {
			Vector3 vec = new Vector3(2, 0,  0);
			GameObject thingy = (GameObject)Instantiate(HealthCrate, vec, Quaternion.identity);
			thingy.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, 50));
			blerp = true;
		}
	}
}
