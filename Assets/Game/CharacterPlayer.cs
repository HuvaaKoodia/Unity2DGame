using UnityEngine;
using System.Collections;

/// <summary>
/// Player Character specific stuff (input and such)
/// </summary>
public class CharacterPlayer : MonoBehaviour {

	CharacterMain Main;
	// Use this for initialization
	void Start () {
		Main=GetComponent<CharacterMain>();
	}

	void Update(){
		if (Main.DEAD) return;

		if (Input.GetMouseButton(0)){
			if (Main.CurrentWeapon==null){
				//pick up weapon
				var cols=Physics2D.OverlapCircleAll(Main.GraphicsMain.GetHandPos(),Main.GraphicsMain.HandRadius);

				foreach (var c in cols){
					if (c.isTrigger) continue;
					if (c.gameObject.tag=="Weapon"){
						//DEV.TODO check for weapon value
						Main.SetCurrentWeapon(c.gameObject.GetComponent<WeaponMain>());
						break;
					}
				}
			}
		}
		if (Input.GetMouseButton(0)){
			if (Main.CurrentWeapon!=null){
				//attack
				Main.CurrentWeapon.AttackPressed();
			}
		}
		if (Input.GetMouseButtonUp(0)){
			if (Main.CurrentWeapon!=null){
				//attack
				Main.CurrentWeapon.AttackReleased();
			}
			Main.ClearRecoil();
		}

		if (Input.GetKeyDown(KeyCode.F)){
			if (Main.CurrentWeapon!=null){
				//throw/drop weapon
				Main.ThrowWeapon();

			}
		}

		if (Input.GetKeyDown(KeyCode.Q)){
			Main.SwapWeapons();
		}
	}

	void FixedUpdate () {
		//hand movement

		Plane p=new Plane(Vector3.forward,Vector3.zero);
		
		float enter;
		var ray=Camera.main.ScreenPointToRay(Input.mousePosition);
		if (p.Raycast(ray,out enter)){
			
			Main.GraphicsMain.SetHandTarget(ray.GetPoint(enter));
		}


		//movement
		Main.Move(Input.GetAxis("Horizontal"));

		if (Input.GetButtonDown("Jump")){
			Main.Jump();

		}
	}
}
