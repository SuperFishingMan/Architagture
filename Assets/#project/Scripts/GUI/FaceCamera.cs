using UnityEngine;
using System.Collections;

public class FaceCamera : MonoBehaviour {

	public Transform target;
	public bool freezeX = true;
	public bool faceCam = false;
	public bool invert = false;
	
	void Update () {
		
		transform.LookAt(target);
		if (faceCam) {
			GameObject player = GameObject.FindGameObjectWithTag("Player"); //locates the player
			transform.LookAt(player.transform);
		}
		if(freezeX)
			transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z);
		if(!invert)
			transform.Rotate (0, 180f, 0);
	}
}
