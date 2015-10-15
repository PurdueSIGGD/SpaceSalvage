using UnityEngine;
using System.Collections;

public class GuiTextStarter : MonoBehaviour {
	private float startingX = -.35f;
	// Use this for initialization
	void Start () {
		//this.GetComponent<GUIText>().pixelOffset = new Vector2(.95f * Screen.width / -2, .95f * Screen.height / 2);
	}
	
	// Update is called once per frame
	void Update () {
		//Handler for needlepoint
		this.transform.FindChild("Needle").transform.localPosition = new Vector3(startingX + (.25f * (GameObject.Find("Player").GetComponent<HealthController>().GetOxyPercent() - .5f)), this.transform.FindChild("Needle").transform.localPosition.y, 1);
		this.transform.FindChild("OxyText").GetComponent<GUIText>().text = "" + GameObject.Find("Player").GetComponent<HealthController>().GetOxy().ToString("F2")	;
	}
}
