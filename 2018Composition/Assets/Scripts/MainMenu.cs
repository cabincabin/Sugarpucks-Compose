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
			SceneManager.LoadScene(LearnDDown.value+1);
	}

	public void QuitGame()
	{
		Debug.Log("QUIT!");
		Application.Quit();
	}

}
