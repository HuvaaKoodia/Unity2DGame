using UnityEngine;
using System.Collections;

public class HeadGraphics : MonoBehaviour {

	public CharacterMain main;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D c){
		if (c.gameObject.tag=="Projectile"){
			rigidbody2D.AddForce(-c.relativeVelocity);

			//DEV.temp
			main.HP-=1000;
		}

	}
}
