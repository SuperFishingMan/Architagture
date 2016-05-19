//

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class FetchAudio : MonoBehaviour {

    public string audioPath = "";
	string absolutePath = "./"; //relative path to where app is running
	AudioSource AS;
	FileInfo[] files;

	//compatible file extensions
	string[] fileTypes = {"wav"};

	void Start(){
		if (Application.isEditor) //if in editor - allows testing
			absolutePath = "Assets/";
		AS = GetComponent<AudioSource>();
	}

    public void setAudio() {
        Debug.Log ("fetching: " + audioPath);

		DirectoryInfo info = new DirectoryInfo (absolutePath);
		files = info.GetFiles ();

		Debug.Log ("Directoryinfo made");

		StartCoroutine (loadFile (audioPath));

		//check if the file is valid and load it
		/*foreach(FileInfo f in files) {
			Debug.Log ("file checked");
			if(validFileType(f.FullName)) {
				Debug.Log ("wav found");
				//Debug.Log("Start loading "+f.FullName);
				StartCoroutine(loadFile(f.FullName));
			}
		}*/
    }

	bool validFileType(string filename) {
		foreach(string ext in fileTypes) {
			if(filename.IndexOf(ext) > -1) return true;
		}
		return false;
	}

	IEnumerator loadFile(string path){
		WWW www = new WWW ("file://"+path);

		AudioClip AC = www.audioClip;
		while (!AC.isReadyToPlay)
			yield return www;

		Debug.Log ("assigning clip");

		AudioClip clip = www.GetAudioClip (false);
		string[] parts = path.Split ('\\');
		clip.name = parts [parts.Length - 1];
		AS.clip = clip;
	}
}