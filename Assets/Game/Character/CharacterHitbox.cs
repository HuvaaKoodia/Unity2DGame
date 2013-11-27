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
			
			main.HP-=Mathf.Min(c.gameObject.GetComponent<BulletMain>().MAXDMG,(c.relativeVelocity.magnitude));
			
			Destroy(c.gameObject);

			if (Blood_prefab)
				Instantiate(Blood_prefab,c.contacts[0].point,Quaternion.identity);
		}

		if (c.gameObject.tag=="Weapon"){
			if (rigidbody2D!=null)
				rigidbody2D.AddForce(-c.relativeVelocity);

			main.HP-=(Mathf.Min(c.gameObject.GetComponent<WeaponHitboxStats>().meelee_damage,c.relativeVelocity.magnitude));
		}
	}

	void OnTriggerStay2D(Collider2D c){
		if (c.gameObject.tag=="Weapon"){
			var weapon=c.GetComponent<WeaponHitboxStats>();
			if (weapon.in_inventory) return;

			float vel=weapon.Velocity*100;

			if (vel<weapon.hit_speed_threshold) return;

			//Debug.Log("mag: "+vel);

				main.rigidbody2D.AddForce(weapon.Direction*vel*10);
			
			
			if (main.TakeMeeleeDMG(weapon.meelee_damage)){

				if (weapon.is_blade&&Blood_prefab){
					Instantiate(Blood_prefab,c.transform.position,Quaternion.identity);
				}
			}
		}
	}
}
