using UnityEngine;
using System.Collections;

public class CharacterMain : MonoBehaviour {

	public CharacterGraphicsMain GraphicsMain;
	public float move_acceleration=70,move_speed=1,jump_speed=60;

	public Transform groundPos;
	
	bool OnGround;

	WeaponMain CurrentWeapon=null;

	// Use this for initialization
	void Start () {
		OnGround=false;
	}

	void Update(){
		//OnGround=Physics2D.Linecast(transform.position, groundPos.position, 1 << LayerMask.NameToLayer("Ground"));
		if (Input.GetMouseButtonDown(0)){
			if (CurrentWeapon!=null){
				//attack

			}
			else{
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

		if (Input.GetKeyDown(KeyCode.F)){
			if (CurrentWeapon!=null){
				//throw/drop weapon
				//CurrentWeapon.rigidbody2D.velocity=Vector3.one*100f;
				ClearCurrentWeapon();
			}
		}

		if (CurrentWeapon!=null)
		{
			CurrentWeapon.SetPosition(GraphicsMain.hand.transform.position);
			CurrentWeapon.transform.rotation=Quaternion.LookRotation(
				GraphicsMain.hand.transform.position-transform.position,Vector3.up)*Quaternion.AngleAxis(-90,Vector3.up);
		}
	}

	void SetCurrentWeapon(WeaponMain weapon){
		CurrentWeapon=weapon;
		CurrentWeapon.collider2D.isTrigger=true;
		CurrentWeapon.rigidbody2D.isKinematic=true;
	}
	
	void ClearCurrentWeapon(){
		CurrentWeapon.collider2D.isTrigger=false;
		CurrentWeapon.rigidbody2D.isKinematic=false;
		CurrentWeapon=null;
	}

	// Update is called once per frame
	void FixedUpdate () {

		//if (Physics2D.Linecast(transform.position, groundPos.position, 1 << LayerMask.NameToLayer("Ground")))  
		//   OnGround=true;

		//rigidbody2D.gravityScale=OnGround ? 0 : 1;

		if (Input.GetAxis("Horizontal")!=0){
			if (rigidbody2D.velocity.magnitude<=move_speed){
				rigidbody2D.AddForce(
					Vector2.right*Input.GetAxis("Horizontal")*move_acceleration
				);
			}
		}

		if (Input.GetButtonDown("Jump")){
			if (OnGround){
				rigidbody2D.AddForce(
					Vector2.up*jump_speed
				);
			}
		}
	}

	void OnCollisionEnter2D(Collision2D c){
		OnGround=true;
	}

	void OnCollisionExit2D(Collision2D c){
		OnGround=false;
	}
}
