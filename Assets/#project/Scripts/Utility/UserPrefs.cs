using UnityEngine;
using System.IO;
using System.Collections;

public class UserPrefs : MonoBehaviour {

	public string _FileName = "UserPrefs";

	public static string username;
	public static string cardboardType;
	public static bool startWithTutorial;

	private string _FilePath;
	private string _FullPath;

	// Use this for initialization
	void Awake () {
		if (Application.isEditor) {
			_FilePath = Application.dataPath + "/Resources/"; 			//persistentDataPath
		} else {
			_FilePath = Application.persistentDataPath + "/Resources/"; 
		}

		_FullPath = _FilePath + _FileName + ".txt";

		//load previously entered values
		LoadUserPrefValues();
	}

	public void LoadUserPrefValues(){
		if (LoadData() != null) {
			string[] data = LoadData().Split(',');
			int L = data.Length;
			if(L>0)
				username = data[0];
			if(L>1)
				cardboardType = data[1];
			if(L>2)
				startWithTutorial = (data[2].Contains("True")) ? true : false;
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
