using UnityEngine;
using System.Collections;

public class CoinCluster : MonoBehaviour {
	public float density = 1;
	public int count;
	public GameObject coin;
	// Use this for initialization
	void Start () {
		count = Random.Range(0, 10);
		for (int i = 0; i < count; i++) {
			GameObject g = (GameObject)GameObject.Instantiate(coin, this.transform.position + new Vector3(Random.Range(-1 * density, density),Random.Range(-1 * density, density),Random.Range(-1 * density, density)), Quaternion.Euler(0,0,Random.Range(0,360)));
			g.transform.parent = this.transform;
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
