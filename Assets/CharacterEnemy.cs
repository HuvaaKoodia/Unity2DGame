using UnityEngine;
using System.Collections;

public class CharacterEnemy : MonoBehaviour {

	public CharacterMain main,player;


	//ai sys
	bool stop,pick_up_weapon,attack=true;
	Timer moveTimer,attackTimer;

	// Use this for initialization
	void Start () {
		moveTimer=new Timer();
		moveTimer.Delay=Random.Range(1000,3000);
		moveTimer.Active=true;

		attackTimer=new Timer();
		attackTimer.Delay=Random.Range(1000,3000);
		attackTimer.Active=true;

		stop=Subs.RandomBool();

	}
	
	// Update is called once per frame
	void Update () {

		if(player.DEAD){
			main.Move(0);
			return;
		}

		if (main.DEAD) return;

		bool clear_sight_to_player=false;
		//raycast to player
		{
			int mask=1<<LayerMask.NameToLayer("Ground")|1<<LayerMask.NameToLayer("Player")|1<<LayerMask.NameToLayer("Enemy");
			var hits=Physics2D.RaycastAll(transform.position,player.transform.position-transform.position,100,mask);

			if (hits.Length>0){
				clear_sight_to_player=true;
			}
			//if (hits.collider!=null&&hit.collider.gameObject.tag=="Player"){
			//	clear_sight_to_player=true;
			//}
		}

		if (pick_up_weapon){
			stop=true;
			if (main.CurrentWeapon!=null){
				pick_up_weapon=false;
			}
			return;
		}

		if (main.CurrentWeapon==null){
			if (!pick_up_weapon){
				int mask=1<<LayerMask.NameToLayer("Weapon");
				var weapons=Physics2D.OverlapCircleAll(main.GraphicsMain.shoulder_pos.transform.position,main.GraphicsMain.HandReach,mask);

				foreach (var w in weapons){
					if (w.isTrigger) continue;
					//DEV.TODO find best weapon
					var comp=w.GetComponent<WeaponMain>();

					pick_up_weapon=true;
					main.PickUpWeapon(comp);
					break;
				}
			}
		}
		else{
			if (main.CurrentWeapon.IsProjectileWeapon()){
				//if () noammo drop weapon

				//aim

				main.GraphicsMain.SetHandTarget(player.transform.position);

				if (clear_sight_to_player){
					if (attackTimer.Update())
					{
						attack=!attack;
						if (attack){
							attackTimer.Delay=Random.Range(100,200);
							attackTimer.Reset();
						}
						else{
							attackTimer.Delay=Random.Range(1000,2000);
							attackTimer.Reset();
							main.CurrentWeapon.AttackReleased();
						}
					}

					if (attack){
						main.CurrentWeapon.AttackPressed();
					}
				}
				else{

				}

			}

		}

		if (moveTimer.Update()){
			moveTimer.Delay=Random.Range(1000,3000);
			moveTimer.Reset(true);
			stop=Subs.RandomBool();

			if (stop)
				main.Move(0);
		}

		if (!stop)
			main.Move(player.transform.position.x-main.transform.position.x);
	}

	public void OnCollisionStay2D(Collision2D c){
		//if (gameObject.tag=="Weapon"&&main.CurrentWeapon==null){
		//	pick_up_weapon=true;
		//}
	}
}
