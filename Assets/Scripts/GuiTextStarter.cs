using UnityEngine;
using System.Collections;

public class GuiTextStarter : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.GetComponent<GUIText>().pixelOffset = new Vector2(.95f * Screen.width / -2, .95f * Screen.height / 2);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
