using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.IO;

public class ButtonInteractionToStop : MonoBehaviour {

	public AudioSource audioSource;

	void Awake()
	{
		if (audioSource == null)
		{
			audioSource = GetComponent<AudioSource>();
		}        
	}	

	void Start () {
	}

	void Update(){
		//check if player clicks mouse (for devellopment only) or taps screen
		if (Input.GetMouseButtonDown (0) || Input.GetButtonDown ("Fire1")) {
			//if (hoverOn)
				ClickToStop();
			/*
			if (Input.GetKeyDown(KeyCode.Space))
			{
				PlaySound();
			}
			*/
		}
	}

	public void ClickToStop(){

		//the button is pressed, and the user drags it to the tag position
		//buttonActivated = true;

		//disactivate player movement
		//playerMovement.enabled = false;

		StopSound();

		//StartCoroutine(streamAudio("file://Users/timdebleser/Desktop/Tim/SLL/ArchiTAGture/Assets/Resources/audio.wav"));
			

		// Update is called once per frame
	}

		public void StopSound()
		{
			if (audioSource.isPlaying)
			{
				audioSource.Stop();
			}        
		}
	/*
		public void PlaySound(AudioClip loadedClip)
		{
			audioSource.clip = loadedClip;
			audioSource.Stop();
		}
		*/
	}