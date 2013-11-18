using UnityEngine;
using System.Collections;

public class CharacterGraphicsMain : MonoBehaviour {

	public GameObject hand;
	public float MaxReach=1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		Plane p=new Plane(Vector3.forward,Vector3.zero);

		float enter;
		var ray=Camera.main.ScreenPointToRay(Input.mousePosition);
		if (p.Raycast(ray,out enter)){

			var t=ray.GetPoint(enter)-transform.position;

			if (t.magnitude>MaxReach)
				t=t.normalized*MaxReach;

			hand.transform.position=transform.position+t;
		}

	}
}
