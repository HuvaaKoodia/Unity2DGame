using UnityEngine;
using System.Collections;

public class CharacterMain : MonoBehaviour {

	public System.Action OnDeath;

	public CharacterGraphicsMain GraphicsMain;
	public float 
		move_acceleration=70,
		move_speed=1,
		jump_speed=60,
		throwing_force=100,
		recoil_multiplier_add_per_shot=2;

	public Transform groundPos,weapon_pos_back,weapon_pos_hip;
	
	bool OnGround,FacingRight;
	WeaponMain weapon_to_pick_up;
	public WeaponMain CurrentWeapon{get; private set;}
	public WeaponMain OtherWeapon{get; private set;}

	float recoil_strength=1;

	bool dead;

	void SetToLayer(Transform t,int layer){
		t.gameObject.layer=layer;
		foreach(Transform c in t){
			SetToLayer(c,layer);
		}
	}

	public bool DEAD
	{
		get {return dead;}
		private set
		{
			var old_dead=dead;
			dead=value;
			if (!old_dead&&dead){
				GraphicsMain.SetDead();
				GraphicsMain.StopWalking();
				GraphicsMain.SetHandTarget(transform.position+Vector3.down*5);

				SetToLayer(transform,LayerMask.NameToLayer("Corpse"));

				if (CurrentWeapon!=null)
					ClearCurrentWeapon(true);
				if (OnDeath!=null)
					OnDeath();
			}
		}
	}
	
	float _hp;
	
	public float HP{
		get {return _hp;}
		set {
			Debug.Log("DMG: "+(_hp-value));
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
		if (weapon_to_pick_up!=null)
		{
			GraphicsMain.SetHandTarget(weapon_to_pick_up.GetComponent<WeaponMain>().HandPos1.transform.position);

			int mask=1<<LayerMask.NameToLayer("Weapon");
			var cols=Physics2D.OverlapCircleAll(GraphicsMain.GetHandPos(),GraphicsMain.HandRadius,mask);
			
			foreach (var c in cols){
				if (c.isTrigger) continue;
				//DEV.TODO check for weapon value
				SetCurrentWeapon(c.gameObject.GetComponent<WeaponMain>());
				weapon_to_pick_up=null;
				break;
			}
		}

		UpdateCurrentWeaponPos();
		UpdateOtherWeaponPos();
	}

	void UpdateCurrentWeaponPos(){
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
	}

	void UpdateOtherWeaponPos(){
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

		UpdateCurrentWeaponPos();

		CurrentWeapon.GetComponent<WeaponHitboxStats>().ClearVelocity();
	}
	
	public void ClearCurrentWeapon(bool changeToWeapon){
		ClearWeapon (CurrentWeapon,changeToWeapon);

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

		OtherWeapon.GetComponent<WeaponHitboxStats>().in_inventory=true;

		UpdateOtherWeaponPos();
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
				ClearCurrentWeapon(false);
				SetOtherWeapon(current);
			}
			else {
				ClearCurrentWeapon(false);
				ClearOtherWeapon();

				SetOtherWeapon(current);
				SetCurrentWeapon(other);
			}
		}
	}

	public void ClearOtherWeapon(){
		ClearWeapon(OtherWeapon,false);

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
		CurrentWeapon.Throw(GraphicsMain.HandVelocityDirection.normalized,throwing_force*GraphicsMain.HandThrowVelocity);
		ClearCurrentWeapon(false);
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
		w.gameObject.layer=gameObject.layer;
	}

	void ClearWeapon(WeaponMain w,bool changeToWeapon){
		w.collider2D.isTrigger=false;
		w.rigidbody2D.isKinematic=false;

		if (changeToWeapon)
			w.gameObject.layer=LayerMask.NameToLayer("Weapon");

		w.SetDepth(-5);
		w.GetComponent<WeaponHitboxStats>().in_inventory=false;
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

	public void PickUpWeapon (WeaponMain weapon)
	{
		weapon_to_pick_up=weapon;
	}

	public Collider2D[] FindWeapons(){
		int mask=1<<LayerMask.NameToLayer("Weapon");
		return Physics2D.OverlapCircleAll(GraphicsMain.GetHandPos(),GraphicsMain.HandRadius,mask);
	}

	bool can_take_meelee_dmg=true;

	public bool TakeMeeleeDMG (float f)
	{
		if (can_take_meelee_dmg){
			HP-=f;
			StartCoroutine(MeeleeDMGTimer());
			return true;
		}
		return false;
	}

	IEnumerator MeeleeDMGTimer(){
		can_take_meelee_dmg=false;
		yield return new WaitForSeconds(1f);
		can_take_meelee_dmg=true;
	}
}
