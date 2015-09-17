using UnityEngine;
using System.Collections;

public class LightExample : MonoBehaviour {
	//what we would use for lights, say in the environment
	public bool on = true;
	public bool spinning = true;
	public bool twolights;
	public Light light1;
	public Light light2;
	public float spinspeed = 10;
	public float range = 5;
	public float intensity = 0;
	// Use this for initialization
	void Start () {
		light1 = this.GetComponentInChildren<Light>();
		light1.intensity = intensity;
		light1.range = range;
		if (twolights) {
			light2.intensity = intensity;
			light2.range = range;
		}
	}
	
	// Update is called once per frame
	void Update () {
		light1.intensity = on?intensity:0; //on or off
		if (twolights) light2.intensity = on?intensity:0;
		if (spinning) {
			this.transform.eulerAngles = this.transform.eulerAngles + Vector3.forward * spinspeed * Time.deltaTime;
		}
	}
}
