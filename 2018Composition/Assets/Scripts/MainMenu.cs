using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public Dropdown LearnDDown;
	public Text CurrSig;
	static int Tsig = 4;

	private void Start()
	{
		//Set a default time signature
		TimelinePositioning.TimeSig = 4;
	}

	public void Compose()
	{
		//try to interpret the user's input to the time signature box
		try
		{
			int TextSig = Int32.Parse(CurrSig.text);
			Tsig = TextSig;
			if(Tsig>0)
				//if interpreted and logical, set the time signature
				TimelinePositioning.TimeSig = Tsig;
		}
		catch (Exception)
		{
			
		}
		//load the composition tool, which should be the 1st index in the build file in unity
		SceneManager.LoadScene(1);
		Debug.Log(SceneManager.sceneCount);
	}

	//Learning tool, the order in buid should be from two up as follows
	/* Quarter Notes
	 * 8th Notes
	 * 16th notes
	 * Syncopation
	 * Polyrhythm 2:3
	 * Polyrhythm 3:4
	 * Polyrhythm 4:5
	 * Create A Key
	 * Create A Chord
	 */
	public void Learn()
	{
		//for each of the learning lessons, set up the time signature correctly
		if(LearnDDown.value !=0)
			switch (LearnDDown.value)
			{
				case 1:
					TimelinePositioning.TimeSig = 4;
					break;
				case 2:
					TimelinePositioning.TimeSig = 8;
					break;
				case 3:
					TimelinePositioning.TimeSig = 16;
					break;
				case 4:
					TimelinePositioning.TimeSig = 16;
					break;
				case 5:
					TimelinePositioning.TimeSig = 6;
					break;
				case 6:
					TimelinePositioning.TimeSig = 12;
					break;
				case 7:
					TimelinePositioning.TimeSig = 20;
					break;
			}
			SceneManager.LoadScene(LearnDDown.value+1);
	}

	//When quit is pressed
	public void QuitGame()
	{
		Debug.Log("QUIT!");
		Application.Quit();
	}

}
