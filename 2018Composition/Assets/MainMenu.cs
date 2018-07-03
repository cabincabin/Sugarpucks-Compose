using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

	public void Compose()
	{
		SceneManager.LoadScene(0);
	}

	public void Learn()
	{
		//SceneManager.LoadScene(//Still have to create this scene);
	}

	public void QuitGame()
	{
		Debug.Log("QUIT!");
		Application.Quit();
	}

}
