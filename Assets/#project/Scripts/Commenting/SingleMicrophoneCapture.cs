/**http://www.41post.com/4884/programming/unity-capturing-audio-from-a-microphone**/

using UnityEngine;
using System.Collections;
using System.IO;

[RequireComponent(typeof(AudioSource))]

public class SingleMicrophoneCapture : MonoBehaviour
{
	public int m_MaxRecordTime;

    //A boolean that flags whether there's a connected microphone  
    private bool micConnected = false;

    //The maximum and minimum available recording frequencies  
    private int minFreq;
    private int maxFreq;

    //A handle to the attached AudioSource  
    private AudioSource goAudioSource;

    //save audio
    SavWav savWav;

    private string fileID;
	private string filePath;
	private Vector3 posTemp;	//the position where the comment wil be saved
	private Quaternion rotTemp;

	private float m_RecordedSeconds = 0;

	public GameObject comment;	//comment prefab to spawn
	public MetaDataLogger metaDataLogger;

    //Use this for initialization  
    void Start()
    {
		/*//Get the attached AudioSource component  
		goAudioSource = this.GetComponent<AudioSource>();
		AudioClip c = Resources.Load("testrecord", typeof(AudioClip)) as AudioClip;
		//AudioClip c = Resources.Load("testrecord") as AudioClip;
		goAudioSource.clip = c;
		Debug.Log (c);	*/
    }

	public void record(){
		//reset recordtimer
		m_RecordedSeconds = 0;

		fileID = System.DateTime.Now.Day + "-" + System.DateTime.Now.Month + "-" + System.DateTime.Now.Year + "__" + System.DateTime.Now.Hour + "-" + System.DateTime.Now.Minute + "-" + System.DateTime.Now.Second+"__";
		savWav = GetComponent<SavWav>();

		//Check if there is at least one microphone connected  
		if (Microphone.devices.Length <= 0)
		{
			//Throw a warning message at the console if there isn't  
			Debug.LogWarning("Microphone not connected!");
		}
		else //At least one microphone is present  
		{
			//Set 'micConnected' to true  
			micConnected = true;

			//Get the default microphone recording capabilities  
			Microphone.GetDeviceCaps(null, out minFreq, out maxFreq);

			//According to the documentation, if minFreq and maxFreq are zero, the microphone supports any frequency...  
			if (minFreq == 0 && maxFreq == 0)
			{
				//...meaning 44100 Hz can be used as the recording sampling rate  
				maxFreq = 44100;
			}

			//Get the attached AudioSource component  
			goAudioSource = this.GetComponent<AudioSource>();
		}
		StartCoroutine(StartTimer ());
		StartCoroutine(recordAudio());
	}

	public void SetRecParams(Vector3 pos, Quaternion rot){
		posTemp = pos;
		rotTemp = rot;
		Debug.Log ("Recording - set parameters");
	}

	public void stopRec(){
		if (micConnected) {
			Microphone.End(null); 	//Stop the audio recording  
			Debug.Log ("end rec");
			StopAllCoroutines();	//make sure recordAudioCoroutine not running anymore

			StartCoroutine (SaveFile());	//start saving
		}
	}

    IEnumerator recordAudio()
    {
        //If there is a microphone  
        if (micConnected)
        {
            //If the audio from any microphone isn't being captured  
            if (!Microphone.IsRecording(null))
            {
                //Start recording and store the audio captured from the microphone at the AudioClip in the AudioSource  
				goAudioSource.clip = Microphone.Start(null, true, m_MaxRecordTime, maxFreq);
				yield return new WaitForSeconds(m_MaxRecordTime);
                Microphone.End(null); //Stop the audio recording  
				Debug.Log ("end rec - time ran out");

				yield return StartCoroutine (SaveFile());
            }
            else //Recording is in progress  
            {
				Microphone.End(null); //Stop the audio recording  
				Debug.Log ("end rec - other");

				yield return StartCoroutine (SaveFile());
            }
        }
        else // No microphone  
        {
            //Print a red "Microphone not connected!" message at the center of the screen  
            //GUI.contentColor = Color.red;
            //GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 25, 200, 50), "Microphone not connected!");
        }
    }

	IEnumerator SaveFile(){
		AudioClip AC = goAudioSource.clip;
		//try to save the audiofile
		/*if (SavWav.Save (fileID+"testrecord.wav", AC)) {

			//get filepath
			filePath = SavWav.lastFilePath;
			//instantiate comment
			GameObject GO = Instantiate (comment, posTemp, rotTemp) as GameObject;
			FetchAudio FA = GO.GetComponent<FetchAudio> ();
			FA.audioPath = filePath;
			FA.setAudio ();

		} else {
			yield return null;
		}*/

		yield return SavWav.Save (fileID + "testrecord.wav", AC);

		//get filepath
		filePath = SavWav.lastFilePath;

		//instantiate comment
		GameObject GO = Instantiate (comment, posTemp, rotTemp) as GameObject;
		FetchAudio FA = GO.GetComponent<FetchAudio> ();
		FA.audioPath = filePath;
		FA.setAudio ();

		//log metaData
		string date = System.DateTime.Now.Day + "/" + System.DateTime.Now.Month + "/" + System.DateTime.Now.Year;
		metaDataLogger.LogMetadata("00","00", date, posTemp, rotTemp, m_RecordedSeconds, filePath, "00");
		//Debug.Log ("Seconds recorded: "+m_RecordedSeconds);

		CommentInteraction CI = GO.GetComponentInChildren<CommentInteraction> ();
		CI.SetCommentDuration (m_RecordedSeconds);
	}

	IEnumerator StartTimer(){
		//Debug.Log ("started timer");
		while(m_RecordedSeconds <= m_MaxRecordTime){
			//Debug.Log ("one second passed");
			yield return new WaitForSeconds(1f);
			m_RecordedSeconds++;
		}

		yield break;
	}
}