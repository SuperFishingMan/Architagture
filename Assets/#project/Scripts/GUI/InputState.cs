using UnityEngine;
using System.Collections;

public class InputState : MonoBehaviour {

	public float m_HoldTime = 0.1f;
	public float m_DoubleTapTime = 0.1f;

	public static bool tap = false;
	public static bool doubleTap = false;
	public static bool hold = false;

	private float m_LastClickTime = -1f;
	private bool m_ButtonDown = false;


	void Update () {

		if (Input.GetButton("Fire1")) {
			if (!m_ButtonDown) {
				//double tap
				if (Time.time - m_LastClickTime <= m_DoubleTapTime) {
					doubleTap = true;
					Debug.Log ("InputState: double tap");
				} else {
					doubleTap = false;
				}
				//tap
				m_ButtonDown = true;
				m_LastClickTime = Time.time;
				tap = true; 
				Debug.Log ("InputState: tap");

			} else {
				tap = false;
			}
			//hold
			if (Time.time - m_LastClickTime >= m_HoldTime) {
				hold = true;
				Debug.Log ("InputState: hold");
			} else {
				hold = false;
			}
		
		} else {
			tap = false;
			doubleTap = false;
			hold = false;
			m_ButtonDown = false;
		}
	}
}
