using UnityEngine;
using System.Collections;

public class NormalHitPosition : MonoBehaviour {

	private GazeCursor gazeCursor;
	private Transform head;

	void Start(){
		GameObject player = GameObject.FindGameObjectWithTag("Player"); //locates the player
		gazeCursor = player.GetComponent<GazeCursor>();

		GameObject headGO = GameObject.FindGameObjectWithTag("Head"); //locates the player
		head = headGO.transform;
	}

	// Update is called once per frame
	void Update () {
		//rotate sprite perpendic to geom.
		transform.position = gazeCursor.focusPoint;												//position cursor on hitlocation
		transform.LookAt(head); 																//rotate towards player
		transform.rotation = Quaternion.Euler( Quaternion.FromToRotation (Vector3.forward, gazeCursor.hitNormal).eulerAngles.x,  Quaternion.FromToRotation (Vector3.forward, gazeCursor.hitNormal).eulerAngles.y,  transform.rotation.eulerAngles.z); //rotate from cursors foreward direction to the hit normal

		//rotate image so it faced player - sort of
		if (transform.forward == new Vector3 (0, 1, 0)) { 
			float angle = head.eulerAngles.y;
			transform.Rotate (0f, 0f, angle - 180f);
		} 
	}
}
