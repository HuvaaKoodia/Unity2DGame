using UnityEngine;
using System.Collections;

public class CharacterHitbox : MonoBehaviour {

	public CharacterMain main;
	public GameObject Blood_prefab;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnCollisionEnter2D(Collision2D c){
		if (c.gameObject.tag=="Projectile"){
			if (rigidbody2D!=null)
				rigidbody2D.AddForce(-c.relativeVelocity);

			//DEV.temp
			main.HP-=(c.relativeVelocity.magnitude);
		
			Destroy(c.gameObject);

			if (Blood_prefab)
				Instantiate(Blood_prefab,c.contacts[0].point,Quaternion.identity);
		}

		if (c.gameObject.tag=="Weapon"){
			if (rigidbody2D!=null)
				rigidbody2D.AddForce(-c.relativeVelocity);
			

			main.HP-=(c.relativeVelocity.magnitude);
		}

	}
}
