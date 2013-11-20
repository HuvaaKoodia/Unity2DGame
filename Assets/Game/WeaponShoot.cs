using UnityEngine;
using System.Collections;

public class WeaponShoot : MonoBehaviour {

	public delegate void WeaponRecoidEvent(Vector3 force);

	public GameObject ShootPos,CasingPos;
	public GameObject Projectile_prefab,Casing_prefab,Smoke_prefab,MuzzleFlash_prefab;

	public float Projectile_velocity,Casing_velocity,Recoil_force;
	
	public bool automatic=false;
	public int firerate=100;
	
	Timer firerate_timer;

	public WeaponRecoidEvent OnWeaponRecoilEvent; 
	
	bool can_shoot,trigger_down;

	// Use this for initialization
	void Start () {
		firerate_timer=new Timer(firerate);
	}

	void Update() {
		if (!can_shoot){
			if (firerate_timer.Update()){
				can_shoot=true;
			}
		}
	}

	public void PressTrigger(){
		if (!can_shoot||(!automatic&&trigger_down)) return;
		can_shoot=false;
		trigger_down=true;

		var go=Instantiate(Projectile_prefab,ShootPos.transform.position,transform.rotation) as GameObject;
		var bullet=go.GetComponent<BulletMain>();
		var angle=transform.rotation.eulerAngles.z*Mathf.Deg2Rad;
		bullet.Launch(new Vector3(Mathf.Cos(angle),Mathf.Sin(angle)),Projectile_velocity);//DEV.OPTMZ get rid of trig.

		if (Casing_prefab){
			go=Instantiate(Casing_prefab,CasingPos.transform.position,transform.rotation) as GameObject;
			go.transform.rigidbody2D.AddForce(
				transform.TransformDirection(transform.localScale.y>0?Vector3.up:Vector3.down)
				*Casing_velocity);

			if (Subs.RandomPercent()<40){
				go.transform.rigidbody2D.AddTorque(100);
			}
		}

		if (Smoke_prefab){
			go=Instantiate(Smoke_prefab,ShootPos.transform.position,transform.rotation) as GameObject;
		}

		if (MuzzleFlash_prefab){
			Instantiate(MuzzleFlash_prefab,ShootPos.transform.position,transform.rotation);
		}

		if (OnWeaponRecoilEvent!=null)
			OnWeaponRecoilEvent(transform.TransformDirection(transform.localScale.y>0?Vector3.up:Vector3.down)*Recoil_force);
	}

	public void ReleaseTrigger(){
		trigger_down=false;
	}
}
