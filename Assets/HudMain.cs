using UnityEngine;
using System.Collections;

public class HudMain : MonoBehaviour {

	public GameObject TurnLabel,TextLabel;
	public UISprite turn_number_spr,text_spr;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetTurnLabel(bool on ,int round){

		TurnLabel.SetActive(on);
		if (on)
			turn_number_spr.spriteName=""+round;
	}

	public void SetTextLabel(bool on){
		
		TextLabel.SetActive(on);
	}

	public void SetTextWin(){
		text_spr.spriteName="UWIN";
	}
	public void SetTextLose(){
		text_spr.spriteName="ULOOSE";
	}
	public void SetTextAllOver(){
		text_spr.spriteName="RoundAllOver";
	}
}
