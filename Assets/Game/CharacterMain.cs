using UnityEngine;
using System.Collections;

public class CharacterMain : MonoBehaviour {

	public CharacterGraphicsMain GraphicsMain;
	public float 
		move_acceleration=70,
		move_speed=1,
		jump_speed=60,
		throwing_force=100,
		recoil_multiplier_add_per_shot=2;

	public Transform groundPos,weapon_pos_back,weapon_pos_hip;
	
	bool OnGround,FacingRight;
	public WeaponMain CurrentWeapon{get; private set;}
	public WeaponMain OtherWeapon{get; private set;}

	float recoil_strength=1;

	public bool DEAD{get;private set;}
	
	int _hp;
	
	public int HP{
		get {return _hp;}
		set {
			_hp=value;
			if (_hp<0){
				DEAD=true;
				_hp=0;
				rigidbody2D.fixedAngle=false;
			}
		}
	}


	// Use this for initialization
	void Start () {
		OnGround=false;
		FacingRight=true;
		DEAD=false;
		_hp=100;
	}

	void Update(){

		if (CurrentWeapon!=null)
		{
			CurrentWeapon.SetPosition(GraphicsMain.hand.transform.position);
			var dir=GraphicsMain.GetHandDirection();
			float angle=Mathf.Atan2(dir.y,dir.x);
			//angle=FacingRight?angle:angle+Mathf.PI;
			CurrentWeapon.transform.rotation=Quaternion.AngleAxis(Mathf.Rad2Deg*angle,Vector3.forward);
			//Quaternion.LookRotation(GraphicsMain.GetHandDirection(),)
			//CurrentWeapon.transform.rotation=Quaternion.AngleAxis(Time.time*20,Vector3.forward);
		}

		if (OtherWeapon!=null)
		{
			if (OtherWeapon.CarryOnHip)
				OtherWeapon.SetPosition(weapon_pos_hip.position);
			else
				OtherWeapon.SetPosition(weapon_pos_back.position);
			OtherWeapon.transform.rotation=Quaternion.AngleAxis(-90,Vector3.forward);
		}
	}

	public void SetCurrentWeapon(WeaponMain weapon){
		CurrentWeapon=weapon;

		CurrentWeapon.SetVertical(FacingRight);

		SetWeapon(CurrentWeapon);

		if (CurrentWeapon.IsProjectileWeapon())
			CurrentWeapon.ProjectileComp.OnWeaponRecoilEvent+=OnRecoil;
	}
	
	public void ClearCurrentWeapon(){
		ClearWeapon (CurrentWeapon);

		if (CurrentWeapon.IsProjectileWeapon())
			CurrentWeapon.ProjectileComp.OnWeaponRecoilEvent-=OnRecoil;

		CurrentWeapon=null;
	}

	public void SetOtherWeapon(WeaponMain weapon){
		OtherWeapon=weapon;

		if (OtherWeapon.CarryOnHip)
			OtherWeapon.SetDepth(5);

		OtherWeapon.SetVertical(false);
		SetWeapon(OtherWeapon);

		OtherWeapon.SetVertical(FacingRight);
	}

	public void SwapWeapons(){
		var other=OtherWeapon;
		var current=CurrentWeapon;

		if (CurrentWeapon==null){
			if(OtherWeapon==null){
				return;
			}
			else {

				ClearOtherWeapon();
				SetCurrentWeapon(other);
				return;
			}
		}
		else{
			if(OtherWeapon==null){
				ClearCurrentWeapon();
				SetOtherWeapon(current);
			}
			else {
				ClearCurrentWeapon();
				ClearOtherWeapon();

				SetOtherWeapon(current);
				SetCurrentWeapon(other);
			}
		}
	}

	public void ClearOtherWeapon(){
		ClearWeapon(OtherWeapon);

		OtherWeapon.SetVertical(false);

		OtherWeapon=null;
	}

	public void SetFacing(bool right){
		if (FacingRight==right) return;

		if (CurrentWeapon!=null)
			CurrentWeapon.SetVertical(right);

		if (OtherWeapon!=null)
			OtherWeapon.SetVertical(right);

		GraphicsMain.FlipHorizontal();

		FacingRight=right;
	}

	public void ThrowWeapon ()
	{
		CurrentWeapon.Throw(GraphicsMain.HandVelocityDirection.normalized,throwing_force*GraphicsMain.HandVelocity);
		ClearCurrentWeapon();
	}

	
	public void Jump ()
	{
		if (OnGround){
			rigidbody2D.AddForce(
				Vector2.up*jump_speed
			);
		}
	}
	
	public void Move (float direction)
	{
		if (direction!=0){
			if (rigidbody2D.velocity.magnitude<=move_speed){
				rigidbody2D.AddForce(
					Vector2.right*direction*move_acceleration
					);
				SetFacing(direction>0);
			}
			GraphicsMain.StartWalking();
		}
		else{
			GraphicsMain.StopWalking();
		}
	}

	public void ClearRecoil ()
	{
		recoil_strength=1;
	}


	//members

	void SetWeapon(WeaponMain w){
		w.collider2D.isTrigger=true;
		w.rigidbody2D.isKinematic=true;
	}

	void ClearWeapon(WeaponMain w){
		w.collider2D.isTrigger=false;
		w.rigidbody2D.isKinematic=false;
		
		w.SetDepth(-5);
	}

	void OnCollisionEnter2D(Collision2D c){
		OnGround=true;
	}

	void OnCollisionExit2D(Collision2D c){
		OnGround=false;
	}

	void OnRecoil(Vector3 force){
		GraphicsMain.hand.rigidbody2D.AddForce(force*recoil_strength);
		recoil_strength+=recoil_multiplier_add_per_shot;
	}
}
