using UnityEngine;
using System.Collections;

public class Animation_DestroyInState : MonoBehaviour {

	public GameObject Target;
	public Animator animator;
	public string EndState;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		var st=animator.GetCurrentAnimatorStateInfo(0).nameHash;
		var str=Animator.StringToHash(EndState);

		if (animator.GetCurrentAnimatorStateInfo(0).nameHash==Animator.StringToHash(EndState))
			Destroy(Target);
	}
}
