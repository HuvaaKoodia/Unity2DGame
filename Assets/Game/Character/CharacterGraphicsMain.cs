using UnityEngine;
using System.Collections;

public class CharacterGraphicsMain : MonoBehaviour {
	public Animator animator;
	public GameObject flippable,hand,head,shoulder_pos,head_pos;
	public float HandReach=1,HandRadius=0.2f,HandMovementSpeed=10,HeadMovementSpeed=10;
	
	public float HandVelocity{get;private set;}
	public Vector3 HandVelocityDirection{get;private set;}
	Vector3 hand_target;

	bool dead=false;

	public void SetDead(){
		dead=true;
		//hand.rigidbody2D.gravityScale=1;
	}

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate () {

		head.transform.position=Vector3.Lerp(head.transform.position,head_pos.transform.position,Time.deltaTime*HeadMovementSpeed);

		var t=hand_target-shoulder_pos.transform.position;

		if (t.magnitude>HandReach)
			t=t.normalized*HandReach;

		HandVelocity=Vector3.Distance(hand.transform.position,shoulder_pos.transform.position+t);
		HandVelocityDirection=shoulder_pos.transform.position+t-hand.transform.position;

		
		hand.transform.position=Vector3.Lerp(hand.transform.position,shoulder_pos.transform.position+t,Time.deltaTime*HandMovementSpeed);


		//limit inside range
		/*
		if (Vector3.Distance(shoulder_pos.transform.position,hand.transform.position)>HandReach)
		{
			hand.transform.position=shoulder_pos.transform.position+(hand.transform.position-shoulder_pos.transform.position).normalized*HandReach;
			hand.rigidbody2D.velocity=Vector3.zero;
		}
		 */
	}



	public void SetHandTarget(Vector3 target){
		hand_target=target;
	}

	public Vector2 GetHandPos ()
	{
		return new Vector2(hand.transform.position.x,hand.transform.position.y);
	}

	public Vector3 GetHandDirection ()
	{
		return hand.transform.position-shoulder_pos.transform.position;
	}

	public void FlipHorizontal(){
		var scale=flippable.transform.localScale;
		scale.x*=-1;
		flippable.transform.localScale=scale;
		head.transform.localScale=scale;
	}

	public void StartWalking(){
		animator.SetFloat(Animator.StringToHash("Speed"),1);
	}

	public void StopWalking(){
		animator.SetFloat(Animator.StringToHash("Speed"),0);
	}
}
