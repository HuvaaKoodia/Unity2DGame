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

	bool picked_up_weapon=false;

	void Update(){
		if (Main.DEAD) return;

		if (Input.GetMouseButton(0)){
			if (!picked_up_weapon&&Main.CurrentWeapon!=null){
				//attack
				Main.CurrentWeapon.AttackPressed();
			}
		}

		if (Input.GetMouseButton(0)){
			if (Main.CurrentWeapon==null){
				//pick up weapon
				var weapons=Main.FindWeapons();

				foreach (var w in weapons){
					if (w.isTrigger) continue;

					//DEV.TODO check for weapon value
					Main.SetCurrentWeapon(w.gameObject.GetComponent<WeaponMain>());
					picked_up_weapon=true;
					break;
				}
			}
		}

		if (Input.GetMouseButtonUp(0)){
			if (Main.CurrentWeapon!=null){
				//attack
				Main.CurrentWeapon.AttackReleased();
			}
			Main.ClearRecoil();
			picked_up_weapon=false;
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
		if (Main.DEAD) return;

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
