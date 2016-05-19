using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SaveUserPrefs : MonoBehaviour {

	public string _FileName = "UserPrefs";
	[Header("Parameters")]
	public InputField _UserName;
	public RadioButtonValues _CardboardType;
	public Toggle _StartWithTutorial;

	public static string username;
	public static string cardboardType;
	public static bool startWithTutorial;

	private string _FilePath;
	private string _FullPath;
	private TextAsset Data;

	// Use this for initialization
	void Start () {
		if (Application.isEditor) {
			_FilePath = Application.dataPath + "/Resources/"; 			//persistentDataPath
		} else {
			_FilePath = Application.persistentDataPath + "/Resources/"; 
		}

		_FullPath = _FilePath + _FileName + ".txt";

		// Make sure directory exists if user is saving to sub dir.
		Directory.CreateDirectory(Path.GetDirectoryName(_FilePath));

		//check if file exists
		if (System.IO.File.Exists(_FullPath)) {
		}

		//load previously entered values
		LoadUserPrefValues();
	}

	/**
	 * Logs user preferences
	 * 
	 * @param
	 * @return
	 */
	public void LogUserPrefs(){

		string dataString = ""+_UserName.text+","+_CardboardType.GetActiveValue()+","+_StartWithTutorial.isOn;
		//save the data locally
		System.IO.File.WriteAllText(_FullPath, dataString);
	}

	/**
	 * Sets user preferences
	 * 
	 * @param
	 * @return
	 */
	public void LoadUserPrefValues(){
		if (LoadData() != null) {
			string[] data = LoadData().Split(',');
			int L = data.Length;
			if(L>0)
				_UserName.text = data[0];
			if(L>1)
				_CardboardType.SetActiveValue(data[1]);
			if(L>2)
				_StartWithTutorial.isOn = (data[2].Contains("True")) ? true : false;
		}
	}

	/**
	 * https://support.microsoft.com/en-us/kb/304430
	 *
	 * @param
	 * @return string
	 */
	public string LoadData(){
		StreamReader reader = new StreamReader(_FullPath);//winDir + "\\system.ini"
		string returnString = null;
		try   
		{    
			do
			{
				returnString += (reader.ReadLine()+"\n");
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
		Debug.Log (returnString);
		return returnString;
	}
		
}
