using UnityEngine;
using System.Collections;

public class CharacterGraphicsMain : MonoBehaviour {
	public Animator animator;
	public GameObject flippable,hand,shoulder_pos;
	public float HandReach=1,HandRadius=0.2f,HandMovementSpeed=10;
	
	public float HandVelocity{get;private set;}
	public Vector3 HandVelocityDirection{get;private set;}
	Vector3 old_hand_pos;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		//hand pos calculations
		Plane p=new Plane(Vector3.forward,Vector3.zero);

		float enter;
		var ray=Camera.main.ScreenPointToRay(Input.mousePosition);
		if (p.Raycast(ray,out enter)){

			var t=ray.GetPoint(enter)-shoulder_pos.transform.position;

			if (t.magnitude>HandReach)
				t=t.normalized*HandReach;

			HandVelocity=Vector3.Distance(hand.transform.position,shoulder_pos.transform.position+t);
			HandVelocityDirection=shoulder_pos.transform.position+t-hand.transform.position;

			//hand.rigidbody2D.AddForce(HandVelocityDirection.normalized*Mathf.Min(HandMovementSpeed*Time.deltaTime,HandVelocity));
			hand.transform.position=Vector3.Lerp(hand.transform.position,shoulder_pos.transform.position+t,Time.deltaTime*HandMovementSpeed);
		}


		old_hand_pos=hand.transform.position;
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
	}

	public void StartWalking(){
		animator.SetFloat(Animator.StringToHash("Speed"),1);
	}

	public void StopWalking(){
		animator.SetFloat(Animator.StringToHash("Speed"),0);
	}
}
