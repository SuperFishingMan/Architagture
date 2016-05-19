using UnityEngine;
using System.Collections;

public class StateSetup : MonoBehaviour {

	public GameObject m_DisableOnStart;

	// Use this for initialization
	void Start () {
		if(UserPrefs.cardboardType == "Gen2")
			m_DisableOnStart.SetActive (false);
	}

}
