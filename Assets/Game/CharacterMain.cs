using UnityEngine;
using System.Collections;

public class CharacterMain : MonoBehaviour {



	public CharacterGraphicsMain GraphicsMain;
	public float move_acceleration=70,move_speed=1,jump_speed=60,throwing_force=100;

	public Transform groundPos;
	
	bool OnGround,FacingRight;
	WeaponMain CurrentWeapon=null;



	// Use this for initialization
	void Start () {
		OnGround=false;
		FacingRight=true;
	}

	void Update(){
		//OnGround=Physics2D.Linecast(transform.position, groundPos.position, 1 << LayerMask.NameToLayer("Ground"));
		if (Input.GetMouseButton(0)){
			if (CurrentWeapon==null){
				//pick up weapon
				var cols=Physics2D.OverlapCircleAll(GraphicsMain.GetHandPos(),GraphicsMain.HandRadius);

				foreach (var c in cols){
					if (c.gameObject.tag=="Weapon"){
						//DEV.TODO check for weapon value
						SetCurrentWeapon(c.gameObject.GetComponent<WeaponMain>());
						break;
					}
				}
			}
		}
		if (Input.GetMouseButton(0)){
			if (CurrentWeapon!=null){
				//attack
				CurrentWeapon.AttackPressed();
			}
		}
		if (Input.GetMouseButtonUp(0)){
			if (CurrentWeapon!=null){
				//attack
				CurrentWeapon.AttackReleased();
			}
		}

		if (Input.GetKeyDown(KeyCode.F)){
			if (CurrentWeapon!=null){
				//throw/drop weapon
				Debug.Log(""+GraphicsMain.HandVelocity);
				CurrentWeapon.Throw(GraphicsMain.HandVelocityDirection.normalized,throwing_force*GraphicsMain.HandVelocity);
				ClearCurrentWeapon();
			}
		}

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

	void SetCurrentWeapon(WeaponMain weapon){
		CurrentWeapon=weapon;

		CurrentWeapon.SetVertical(FacingRight);
		CurrentWeapon.collider2D.isTrigger=true;
		CurrentWeapon.rigidbody2D.isKinematic=true;

		if (CurrentWeapon.IsProjectileWeapon())
			CurrentWeapon.ProjectileComp.OnWeaponRecoilEvent+=OnRecoil;
	}
	
	void ClearCurrentWeapon(){
		CurrentWeapon.collider2D.isTrigger=false;
		CurrentWeapon.rigidbody2D.isKinematic=false;

		if (CurrentWeapon.IsProjectileWeapon())
			CurrentWeapon.ProjectileComp.OnWeaponRecoilEvent-=OnRecoil;

		CurrentWeapon=null;
	}
	
	void FixedUpdate () {
		//movement
		if (Input.GetAxis("Horizontal")!=0){
			if (rigidbody2D.velocity.magnitude<=move_speed){
				rigidbody2D.AddForce(
					Vector2.right*Input.GetAxis("Horizontal")*move_acceleration
				);
				
				SetFacing(Input.GetAxis("Horizontal")>0);
			}
			GraphicsMain.StartWalking();
		}
		else{
			GraphicsMain.StopWalking();
		}

		if (Input.GetButtonDown("Jump")){
			if (OnGround){
				rigidbody2D.AddForce(
					Vector2.up*jump_speed
				);
			}
		}
	}

	void SetFacing(bool right){
		if (FacingRight==right) return;

		if (CurrentWeapon!=null)
			CurrentWeapon.SetVertical(right);

		GraphicsMain.FlipHorizontal();

		FacingRight=right;
	}

	void OnCollisionEnter2D(Collision2D c){
		OnGround=true;
	}

	void OnCollisionExit2D(Collision2D c){
		OnGround=false;
	}

	void OnRecoil(Vector3 force){
		GraphicsMain.hand.rigidbody2D.AddForce(force);
	}
}
