using UnityEngine;
using System.Collections;

public class LoadScene : MonoBehaviour {

	public CanvasGroup _InfoScreen;
	public string _SceneToLoad;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LoadLevel(){
		StartCoroutine (LoadLevelCR ());
	}

	IEnumerator LoadLevelCR(){
		while (_InfoScreen.alpha < 1f) {
			_InfoScreen.alpha += Time.deltaTime*1.5f;
			yield return null;
		}

		yield return new WaitForSeconds (2f);

		Application.LoadLevel(_SceneToLoad);
	}
}
