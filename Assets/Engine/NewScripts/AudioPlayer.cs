using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioPlayer : MonoBehaviour {

	public List<AudioClip> Clips=new List<AudioClip>();

	public AudioSource source;

	public void Start(){
		if (source==null){
			source =gameObject.AddComponent<AudioSource>();
		}
	}

	public void PlayRandom(){
		source.clip=Subs.GetRandom(Clips);
		source.Play();
	}
}
