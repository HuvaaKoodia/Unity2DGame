using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour 
{
	public Transform Target;

	public float xMargin = 1f;		// Distance in the x axis the player can move before the camera follows.
	public float yMargin = 1f;		// Distance in the y axis the player can move before the camera follows.
	public float xSmooth = 8f;		// How smoothly the camera catches up with it's target movement in the x axis.
	public float ySmooth = 8f;		// How smoothly the camera catches up with it's target movement in the y axis.

	public bool ClampPosition=true;
	public Vector2 maxXAndY;		// The maximum x and y coordinates the camera can have.
	public Vector2 minXAndY;		// The minimum x and y coordinates the camera can have.

	public float max_view_distance_x=1f,max_view_distance_y=0.5f;

	bool CheckXMargin(float x)
	{
		// Returns true if the distance between the camera and the player in the x axis is greater than the x margin.
		return Mathf.Abs(transform.position.x - x) > xMargin;
	}

	bool CheckYMargin(float y)
	{
		// Returns true if the distance between the camera and the player in the y axis is greater than the y margin.
		return Mathf.Abs(transform.position.y - y) > yMargin;
	}


	void FixedUpdate ()
	{
		TrackPlayer();
	}
	
	
	void TrackPlayer ()
	{
		// By default the target x and y coordinates of the camera are it's current x and y coordinates.
		float targetX = transform.position.x;
		float targetY = transform.position.y;

		Plane p=new Plane(Vector3.forward,Vector3.zero);

		bool x_mouse_off=false,y_mouse_off=false;
		float enter;
		var ray=Camera.main.ScreenPointToRay(Input.mousePosition);
		if (p.Raycast(ray,out enter))
		{	
			var mouse_pos=ray.GetPoint(enter);
			var dir=mouse_pos-Target.position;
			if(CheckXMargin(mouse_pos.x)){

				if (Mathf.Abs(dir.x)>xMargin+max_view_distance_x) {

					mouse_pos.x=xMargin+max_view_distance_x;
				}

				targetX = Mathf.Lerp(targetX, mouse_pos.x, xSmooth * Time.deltaTime);
				x_mouse_off=true;
			}
			if(CheckYMargin(mouse_pos.y)){

				if (Mathf.Abs(dir.y)>yMargin+max_view_distance_y) {
					mouse_pos.y=yMargin+max_view_distance_y;
				}

				targetY = Mathf.Lerp(targetY, mouse_pos.y, ySmooth * Time.deltaTime);
				y_mouse_off=true;
			}
		}

		bool ok=!x_mouse_off&&!y_mouse_off;

		if(ok&&CheckXMargin(Target.position.x)){
			targetX = Mathf.Lerp(targetX, Target.position.x, xSmooth * Time.deltaTime);
		}
		if(ok&&CheckYMargin(Target.position.y)){
			targetY = Mathf.Lerp(targetY, Target.position.y, ySmooth * Time.deltaTime);
		}

		if (ClampPosition){
			targetX = Mathf.Clamp(targetX, minXAndY.x, maxXAndY.x);
			targetY = Mathf.Clamp(targetY, minXAndY.y, maxXAndY.y);
		}

		// Set the camera's position to the target position with the same z component.
		transform.position = new Vector3(targetX, targetY, transform.position.z);
	}
}
