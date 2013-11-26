using UnityEngine;
using System.Collections;

public class Sprite_RandomStartRotation : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.rotation=Quaternion.AngleAxis(Subs.GetRandom(360),Vector3.forward);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
