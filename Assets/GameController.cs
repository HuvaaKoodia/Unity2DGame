using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public bool PAUSE=false;

	public CharacterMain Player;

	public HudMain HudController;
	public GameObject Enemy_prefab;

	public GameObject[] Weapon_prefabs;
	public GameObject[] Enemy_spawns;
	
	bool gameover;

	public int enemy_start_create_amount,enemy_spawn_rate,round_over_time,round_number;
	int enemy_amount,enemy_amount_to_create;

	Timer enemy_spawn_timer,round_timer;

	bool round_over;

	// Use this for initialization
	void Start (){
		enemy_spawn_timer=new Timer(enemy_spawn_rate);
		enemy_amount_to_create=enemy_start_create_amount;

		round_timer=new Timer(round_over_time);

		round_number=1;
		StartRoundTimer();

		Player.OnDeath+=OnPlayerDeath;
	}
	
	// Update is called once per frame
	void Update () {

		if (PAUSE) return;

		if(round_over){
			if(round_timer.Update()){
				//next round

				if (round_number>1)
					enemy_amount_to_create=enemy_start_create_amount*2;

				round_number++;
				round_over=false;
				round_timer.Active=false;
				HudController.SetTurnLabel(false,0);
			}
		}
		else{
			if (enemy_spawn_timer.Update()){
				CreateEnemy(Subs.GetRandom(Enemy_spawns).transform.position);
				enemy_amount_to_create--;
				enemy_spawn_timer.Reset();

				if(enemy_amount_to_create==0)
				{
					//round over
					round_over=true;
				}
			}
		}

		if (Input.GetMouseButtonDown(2))
		{
			Plane p=new Plane(Vector3.forward,Vector3.zero);
			
			float enter;
			var ray=Camera.main.ScreenPointToRay(Input.mousePosition);
			if (p.Raycast(ray,out enter)){
				CreateEnemy(ray.GetPoint(enter));
			}
		}
	}

	void CreateEnemy(Vector3 pos){
		var go=Instantiate(Enemy_prefab,pos,Quaternion.identity) as GameObject;
		var enemy=go.GetComponent<CharacterMain>();
		
		var go2=Instantiate(Subs.GetRandom(Weapon_prefabs),pos,Quaternion.identity) as GameObject;
		
		//enemy.SetCurrentWeapon(go2.GetComponent<WeaponMain>());
		
		go.GetComponent<CharacterEnemy>().player=Player;

		enemy_amount++;
		enemy.OnDeath+=OnEnemyDeath;
	}

	void OnEnemyDeath(){
		enemy_amount--;
		if (enemy_amount==0){
			if (round_over){

				StartRoundTimer();
			}
		}
	}

	void OnPlayerDeath(){
		HudController.SetTextLabel(true);
		HudController.SetTextLose();
	}

	void StartRoundTimer(){
		round_over=true;
		HudController.SetTurnLabel(true,round_number);
		round_timer.Reset(true);
	}
}
