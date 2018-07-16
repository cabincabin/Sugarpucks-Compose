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
		TimelinePositioning.TimeSig = 4;
	}

	public void Compose()
	{
		
		try
		{
			int TextSig = Int32.Parse(CurrSig.text);
			Tsig = TextSig;
			if(Tsig>0)
				TimelinePositioning.TimeSig = Tsig;
		}
		catch (Exception)
		{
			
		}
		SceneManager.LoadScene(1);
		Debug.Log(SceneManager.sceneCount);
	}

	public void Learn()
	{
		
		if(LearnDDown.value !=0)
			switch (LearnDDown.value)
			{
				case 1:
					Debug.Log("GHejhv");
					TimelinePositioning.TimeSig = 4;
					break;
				case 2:
					TimelinePositioning.TimeSig = 8;
					break;
				case 3:
					TimelinePositioning.TimeSig = 16;
					break;
				case 4:
					Debug.Log("1111111111");
					TimelinePositioning.TimeSig = 4;
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

	public void QuitGame()
	{
		Debug.Log("QUIT!");
		Application.Quit();
	}

}
