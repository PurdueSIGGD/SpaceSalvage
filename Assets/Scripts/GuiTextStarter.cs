using UnityEngine;
using System.Collections;

public class GuiTextStarter : MonoBehaviour {
	private float startingX = -.35f;
	private float startingHealthX = .2625f;
	private float startingBloodX = .3685f;
	private float startingTubeX = -.15f;
	// Use this for initialization
	void Start () {
		//this.GetComponent<GUIText>().pixelOffset = new Vector2(.95f * Screen.width / -2, .95f * Screen.height / 2);
	}
	
	// Update is called once per frame
	void Update () {
		//Handler for needlepoint
		this.transform.FindChild("Needle").transform.localPosition = new Vector3(startingX + (.215f * (GameObject.Find("Player").GetComponent<HealthController>().GetOxyPercent() - .65f)), -0.89f, .9f);
		this.transform.FindChild("OxyText").GetComponent<GUIText>().text = "Oxy:" + GameObject.Find("Player").GetComponent<HealthController>().GetOxy().ToString("F2")	+ "%";

		this.transform.FindChild("HealthNeedle").transform.localPosition = new Vector3(startingHealthX + (1-GameObject.Find("Player").GetComponent<HealthController>().getIntegrityPercent()) * .215f, -0.89f, .9f);
		this.transform.FindChild("HealthText").GetComponent<GUIText>().text = "Suit:" + GameObject.Find("Player").GetComponent<HealthController>().getIntegrity().ToString("F2") + "%";
		this.transform.FindChild("HealthRedBar").transform.localPosition = new Vector3(startingBloodX + (.47f-.37f) * (1- (GameObject.Find("Player").GetComponent<HealthController>().GetHealth()/100)), this.transform.FindChild("HealthRedBar").transform.localPosition.y, .5f);
		this.transform.FindChild("HealthRedBar").transform.localScale = new Vector3(.23f - .22f * (1-GameObject.Find("Player").GetComponent<HealthController>().GetHealth()/100), .09f, .5f);
		Color rgb = this.transform.FindChild("HealthBlood").GetComponent<GUITexture>().color;
		this.transform.FindChild("HealthBlood").GetComponent<GUITexture>().color = new Color(rgb.r, rgb.g, rgb.b, 1-GameObject.Find("Player").GetComponent<HealthController>().GetHealth()/100);
		this.transform.FindChild("CoinText").GetComponent<GUIText>().text = GameObject.Find("Player").GetComponent<HealthController>().GetWallet()+ " Coins";
		this.transform.FindChild("TubesText").GetComponent<GUIText>().text = GameObject.Find("Ship").GetComponent<RopeScript2D>().brokenrope ||GameObject.Find("Ship").GetComponent<RopeScript2D>().ejected?"X":GameObject.Find("Ship").GetComponent<RopeTubeController>().tubesleft.ToString();
		//print((((float)GameObject.Find("Ship").GetComponent<RopeTubeController>().tubesleft) / (float)GameObject.Find("Ship").GetComponent<RopeTubeController>().startingtubes));
		this.transform.FindChild("TubeCover").transform.localPosition = new Vector3(startingTubeX + .5f * (.07f + .15f)*(1-((float)GameObject.Find("Ship").GetComponent<RopeTubeController>().tubesleft/(float)GameObject.Find("Ship").GetComponent<RopeTubeController>().startingtubes)),-0.92f,.5f);
		this.transform.FindChild("TubeCover").transform.localScale = new Vector3(.24f *(1 - (float)GameObject.Find("Ship").GetComponent<RopeTubeController>().tubesleft/(float)GameObject.Find("Ship").GetComponent<RopeTubeController>().startingtubes) ,0.1f, 0.2f);
		this.transform.FindChild("ArmorHealthBar").GetComponent<GUITexture>().color = new Color(1,1,1,GameObject.Find("Player").GetComponent<HealthController>().GetArmor()/50);
	
	}
}
