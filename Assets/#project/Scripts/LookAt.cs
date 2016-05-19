using UnityEngine;
using System.Collections;

public class LookAt : MonoBehaviour {

	public Transform m_Target;
	
	// Update is called once per frame
	void Update () {
		transform.LookAt (m_Target);
		transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
	}
}
