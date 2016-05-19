using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class DataLogger : MonoBehaviour {

	public Transform cursor;
	public Transform player;
	public float logFrequency = 1f;
	public float saveFrequency = 10f;
	//upload
	public bool vibrateOnUpload = false;
	public bool saveDataLocally = false;
	public bool uploadData = true;
	public string uploadDataURL;

	string dataString;
	string filePath;
	string fileName;
	//upload
	string sessionName;

	// Use this for initialization
	void Start () 
	{
		//Debug.Log (Application.dataPath);
		filePath = Application.dataPath + "/";
		fileName = "data_"+System.DateTime.Now.Day+"-"+System.DateTime.Now.Month+"-"+System.DateTime.Now.Year+"_"+System.DateTime.Now.Hour+"-"+System.DateTime.Now.Minute+"-"+System.DateTime.Now.Second+".txt";
		sessionName = "session_date_"+System.DateTime.Now.Day+"-"+System.DateTime.Now.Month+"-"+System.DateTime.Now.Year+"_time_"+System.DateTime.Now.Hour+"-"+System.DateTime.Now.Minute+"-"+System.DateTime.Now.Second;

		InvokeRepeating ("logData", logFrequency, logFrequency);
		InvokeRepeating ("saveData", saveFrequency, saveFrequency);
	}
	
	// Update is called once per frame
	public void logData() 
	{
		//Debug.Log ("logged data");
		dataString += player.position.x + "," + player.position.y + "," + player.position.z + "," + cursor.position.x + "," + cursor.position.y + "," + cursor.position.z + "\n";
	}

	public void saveData()
	{
		if (saveDataLocally) {
			System.IO.File.WriteAllText(filePath+fileName, dataString);
		}
		if (uploadData) {
			StartCoroutine(uploadDataToWeb());
		}
	}

	IEnumerator uploadDataToWeb()
	{
        WWWForm form = new WWWForm();
        form.AddField("session", sessionName);
        form.AddField("data", dataString);

        // Upload to a cgi script
        WWW w = new WWW(uploadDataURL, form);
        yield return w;
        if (!string.IsNullOrEmpty(w.error))
        {
            print(w.error);

            Handheld.Vibrate();
            yield return new WaitForSeconds(0.5f);
            Handheld.Vibrate();
            yield return new WaitForSeconds(0.5f);
            Handheld.Vibrate();
        }
        else
        {
            print("Finished Uploading Data: "+dataString);
			if(vibrateOnUpload)
            	Handheld.Vibrate();
        }

		dataString = "";
	}
}
