using UnityEngine;
using System.Collections;

public class WeaponMain : MonoBehaviour {

	public GameObject HandPos1,GraphicsMain;

	public float Damage=10;

	Vector3 start_scale;

	public bool IsProjectileWeapon(){return ProjectileComp!=null;}
	public WeaponShoot ProjectileComp{get;private set;}

	public bool CarryOnHip;
	
	// Use this for initialization
	void Start () {
		start_scale=transform.localScale;
		ProjectileComp=GetComponent<WeaponShoot>();
	}

	public void SetPosition(Vector3 position){
		transform.position=transform.position-HandPos1.transform.position+position;
	}

	public void SetVertical(bool right) {
		int i=right?1:-1;
		var scale=start_scale;
		scale.y*=i;
		transform.localScale=scale;	
	}

	public void Throw (Vector3 direction,float speed)
	{
		rigidbody2D.isKinematic=false;
		rigidbody2D.AddForce(direction*speed);
	}

	public void AttackPressed(){
		if (ProjectileComp!=null)
			ProjectileComp.PressTrigger();
	}

	public void AttackReleased()
	{
		if (ProjectileComp!=null)
			ProjectileComp.ReleaseTrigger();
	}

	public void SetDepth (int d)
	{
		GraphicsMain.GetComponent<SpriteRenderer>().sortingOrder=d;
	}

	public void OnCollisionStay2D(Collision2D c){
		if (gameObject.layer!=LayerMask.NameToLayer("Weapon")&&
		    rigidbody2D.velocity.magnitude<0.1f&&c.gameObject.tag=="Ground"){
			gameObject.layer=LayerMask.NameToLayer("Weapon");
		}
	}
}
