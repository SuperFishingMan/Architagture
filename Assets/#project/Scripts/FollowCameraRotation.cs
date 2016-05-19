using UnityEngine;
using System.Collections;

public class FollowCameraRotation : MonoBehaviour {

	[SerializeField] private Transform head;
	[Header("Easing")]
	[SerializeField] private bool enableEasing = false;
	[SerializeField] private float rotSpeed = 0.5f;
	[SerializeField] private float correction = 5f;

	private Quaternion currentRot;


	void Awake(){
		currentRot = transform.rotation;
	}

	// Update is called once per frame
	void Update () {
		if (enableEasing) {
			float rotY = Mathf.LerpAngle (currentRot.eulerAngles.y, head.eulerAngles.y + correction, Time.deltaTime * rotSpeed);
			currentRot = Quaternion.Euler (0f, rotY, 0f);
			transform.rotation = currentRot;
		} else{ 
			transform.rotation = Quaternion.LookRotation (head.position);
			transform.rotation = Quaternion.Euler (0f, head.rotation.eulerAngles.y, 0f);
		}
	}


}
