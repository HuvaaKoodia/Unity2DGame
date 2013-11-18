using UnityEngine;
using System.Collections;

public class CharacterMain : MonoBehaviour {

	public float move_acceleration=70,move_speed=1,jump_speed=60;

	public Transform groundPos;

	bool OnGround;

	// Use this for initialization
	void Start () {
		OnGround=false;
	}

	void Update(){
		//OnGround=Physics2D.Linecast(transform.position, groundPos.position, 1 << LayerMask.NameToLayer("Ground"));  


	}

	// Update is called once per frame
	void FixedUpdate () {

		//if (Physics2D.Linecast(transform.position, groundPos.position, 1 << LayerMask.NameToLayer("Ground")))  
		//   OnGround=true;

		//rigidbody2D.gravityScale=OnGround ? 0 : 1;

		if (Input.GetAxis("Horizontal")!=0){
			if (rigidbody2D.velocity.magnitude<=move_speed){
				rigidbody2D.AddForce(
					Vector2.right*Input.GetAxis("Horizontal")*move_acceleration
				);
			}
		}

		if (Input.GetButtonDown("Jump")){
			if (OnGround){
				rigidbody2D.AddForce(
					Vector2.up*jump_speed
				);
			}
		}
	}

	void OnCollisionEnter2D(Collision2D c){
		OnGround=true;
	}

	void OnCollisionExit2D(Collision2D c){
		OnGround=false;
	}
}
