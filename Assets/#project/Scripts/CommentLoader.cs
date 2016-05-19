using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class CommentLoader : MonoBehaviour {

	public string fileName = "metaData";
	public GameObject comment;

	private string filePath;
	private string fullPath;
	private string dataString;

	// Use this for initialization
	void Start () {
		if (Application.isEditor) {
			filePath = Application.dataPath + "/Resources/"; 			//persistentDataPath
		} else {
			filePath = Application.persistentDataPath + "/Resources/"; 
		}

		fullPath = filePath + fileName + ".txt";

		//load file if it exists
		if (System.IO.File.Exists (fullPath)) {
			dataString = LoadData();									//get textasset from resources folder 
			if (dataString != "" || dataString != null) {				//make sure datastring isent empty
				string[] lines = dataString.Split ("\n" [0]);			//split text up at linebreaks
				for (int i = 0; i < lines.Length; i++) {
					if (lines [i] != "") {
						string[] data = lines [i].Split ("," [0]);
						Debug.Log (data.Length);
						//instantiate comment
						Vector3 pos = new Vector3 (float.Parse (data [3]), float.Parse (data [4]), float.Parse (data [5]));
						Quaternion rot = new Quaternion (float.Parse (data [6]), float.Parse (data [7]), float.Parse (data [8]), float.Parse (data [9]));
						GameObject GO = Instantiate (comment, pos, rot) as GameObject;
						//set audioclip
						FetchAudio FA = GO.GetComponent<FetchAudio> ();
						Debug.Log (data [11]);
						FA.audioPath = data [11];
						FA.setAudio ();
						//set comment duration
						CommentInteraction CI = GO.GetComponentInChildren<CommentInteraction>();
						CI.SetCommentDuration (float.Parse (data [10]));
					}
				}
			}
		}
	}

	/**
	 * https://support.microsoft.com/en-us/kb/304430
	 *
	 * @param
	 * @return string
	 */
	public string LoadData(){
		StreamReader reader = new StreamReader(fullPath);//winDir + "\\system.ini"
		string returnString = "";
		try   
		{    
			do
			{
				returnString += ("\n" + reader.ReadLine());
			}   
			while(reader.Peek() != -1);
		}      

		catch 
		{ 
			Debug.Log ("File is empty");
		}

		finally
		{
			reader.Close();
		}
		return returnString;
	}
}
