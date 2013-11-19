using UnityEngine;
using System.Collections;

public class BulletMain : MonoBehaviour {


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Launch(Vector3 direction,float speed){
		rigidbody2D.AddForce(direction*speed);
	}

	public void OnCollisionEnter2D(Collision2D c){

		if (c.gameObject.tag=="Ground"){
			if (c.relativeVelocity.magnitude>5)
				Destroy(gameObject);
		}
	}
}
