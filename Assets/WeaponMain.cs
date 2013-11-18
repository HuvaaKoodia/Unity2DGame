using UnityEngine;
using System.Collections;

public class WeaponMain : MonoBehaviour {

	public GameObject HandPos1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetPosition(Vector3 position){
		transform.position=transform.position-HandPos1.transform.position+position;
	}
}
