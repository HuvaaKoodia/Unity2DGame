using UnityEngine;
using System.Collections;

public class WeaponHitboxStats : MonoBehaviour {

	public bool is_blade=false;
	public float hit_speed_threshold=1f,meelee_damage=1;
	
	public float Velocity{get;private set;}
	public Vector3 Direction{get;private set;}

	Vector3 old_pos;
	bool skip_this_frame_HAX;

	public bool in_inventory{get;set;}
	
	void Update (){
		if(skip_this_frame_HAX){
			old_pos=transform.position;
			skip_this_frame_HAX=false;
			return;
		}

		Direction=transform.position-old_pos;
		Velocity=Mathf.Abs(Direction.magnitude);
		old_pos=transform.position;
	}

	public void ClearVelocity ()
	{
		skip_this_frame_HAX=true;
	}
}
