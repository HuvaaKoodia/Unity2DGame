using UnityEngine;
using System.Collections;

public class RandomPositionJitter : MonoBehaviour {

	public float seconds_min=0.1f,seconds_max=0.5f;
	public float jitter_amount=0.1f;

	Vector3 start_pos;

	// Use this for initialization
	void Start () {
		start_pos=transform.position;

		StartCoroutine(Act());
	}

	IEnumerator Act(){
		while (true)
		{
			transform.position=start_pos+new Vector3(Random.Range(-jitter_amount,jitter_amount),Random.Range(-jitter_amount,jitter_amount),0)*0.5f;

			yield return new WaitForSeconds(Random.Range(seconds_min,seconds_max));
		}
	}
}
