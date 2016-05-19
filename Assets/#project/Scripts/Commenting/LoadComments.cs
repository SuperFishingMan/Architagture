using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LoadComments : MonoBehaviour {

    [SerializeField] private GameObject comment;

	private List<TAG> tags;

	public AudioClip[] audio;
	public Transform[] commentPos;

    // Use this for initialization
    void Start () {
		//Instantiate (comment, new Vector3 (5, 5, 5), Quaternion.identity);
		tags = new List<TAG> ();

		//add all fake comments
		for (int i = 0; i < audio.Length; i++) {
			tags.Add( new TAG(audio [i], commentPos [i].position.x, commentPos [i].position.y, commentPos [i].position.z, commentPos [i].rotation.eulerAngles.x, commentPos [i].rotation.eulerAngles.y, commentPos [i].rotation.eulerAngles.z));
		}

		displayTags ();
	}

	public void displayTags(){
		foreach (TAG t in tags){ 
			GameObject GO = Instantiate (comment, new Vector3 (t.x, t.y, t.z), Quaternion.Euler (t.xn, t.yn, t.zn)) as GameObject;
			AudioSource AS = GO.GetComponent<AudioSource> ();
			AS.clip = t.audio;
		}
	}	
}

public class TAG {

	public AudioClip audio;
	public float x;
	public float y;
	public float z;
	public float xn; 
	public float yn; 
	public float zn;

	public TAG(AudioClip audio, float x, float y, float z, float xn, float yn, float zn) {
		this.audio = audio;
		this.x = x;
		this.y = y;
		this.z = z;
		this.x = xn; 
		this.y = yn; 
		this.z = zn; 
	}  
}