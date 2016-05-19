using System;
using System.IO;
using UnityEngine;
using System.Collections.Generic;

public class MetaDataLogger : MonoBehaviour {

	[SerializeField] private string fileName = "metaData";

	private string filePath;
	private string fullPath;
	private string dataString;
	private TextAsset data;

	// Use this for initialization
	void Start () {

		if (Application.isEditor) {
			filePath = Application.dataPath + "/Resources/"; 			//persistentDataPath
		} else {
			filePath = Application.persistentDataPath + "/Resources/"; 
		}

		fullPath = filePath + fileName + ".txt";

		// Make sure directory exists if user is saving to sub dir.
		Directory.CreateDirectory(Path.GetDirectoryName(filePath));


		//check if file exists
		if (System.IO.File.Exists(fullPath)) {
			dataString = LoadData();	//get textasset from resources folder 
		} 

		//string date = System.DateTime.Now.Day + "/" + System.DateTime.Now.Month + "/" + System.DateTime.Now.Year;
		//LogMetadata ("01", "01", date, new Vector3 (0, 0, 0), new Quaternion (0, 0, 0, 0), 5f, "test", "01");
		//LogMetadata ("02", "02", date, new Vector3 (0, 0, 0), new Quaternion (0, 0, 0, 0), 5f, "test", "01");
	}

	/**
	 * Logs comment's metadata, allowing future users to load previously recorded local comments
	 * 
	 * @param
	 * @return
	 */
	public void LogMetadata (string modelID, string authorID, string date, Vector3 pos, Quaternion rot, float duration, string localFilePath, string onlineID){

		dataString += "\n" + modelID +","+ authorID +","+ date +","+ pos.x +","+ pos.y +"," + pos.z +"," + rot.x + "," + rot.y + "," + rot.z + "," + rot.w + "," + duration + "," + localFilePath + "," + onlineID;
		//save the data locally
		System.IO.File.WriteAllText(fullPath, dataString);
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
				returnString += ("\n"+reader.ReadLine());
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

	/*
 public void SaveItemInfo(){
     string path = null;
     #if UNITY_EDITOR
     path = "Assets/Resources/GameJSONData/ItemInfo.json";
     #endif
     #if UNITY_STANDALONE
         // You cannot add a subfolder, at least it does not work for me
         path = "MyGame_Data/Resources/ItemInfo.json"
     #endif
  
     string str = ItemInfo.ToString();
     using (FileStream fs = new FileStream(path, FileMode.Create)){
         using (StreamWriter writer = new StreamWriter(fs)){
             writer.Write(str);
         }
     }
     #if UNITY_EDITOR
     UnityEditor.AssetDatabase.Refresh ();
     #endif
 }

*/
}
