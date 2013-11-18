using UnityEngine;
using System.Collections;

public class CharacterGraphicsMain : MonoBehaviour {


	public GameObject hand,shoulder_pos;

	public float MaxReach=1,HandRadius=0.2f;


	public float HandSpeed{get;private set;}
	Vector3 old_hand_pos;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


		//hand pos calculations
		Plane p=new Plane(Vector3.forward,Vector3.zero);

		float enter;
		var ray=Camera.main.ScreenPointToRay(Input.mousePosition);
		if (p.Raycast(ray,out enter)){

			var t=ray.GetPoint(enter)-shoulder_pos.transform.position;

			if (t.magnitude>MaxReach)
				t=t.normalized*MaxReach;

			hand.transform.position=shoulder_pos.transform.position+t;
		}
		HandSpeed=Vector3.Distance(hand.transform.position,old_hand_pos);

		old_hand_pos=hand.transform.position;
	}

	public Vector2 GetHandPos ()
	{
		return new Vector2(hand.transform.position.x,hand.transform.position.y);
	}
}
