using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public Dropdown LearnDDown;
	
	public void Compose()
	{
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
