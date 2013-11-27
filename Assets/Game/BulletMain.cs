using UnityEngine;
using System.Collections;

public class BulletMain : MonoBehaviour {

	public float MAXDMG{get;private set;}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Launch(Vector3 direction,float speed,float maxdmg){
		rigidbody2D.velocity=direction*speed;
		MAXDMG=maxdmg;
	}

	public void OnCollisionEnter2D(Collision2D c){

		if (c.gameObject.tag=="Ground"){
			if (c.relativeVelocity.magnitude>5)
				Destroy(gameObject);
		}
	}
}
