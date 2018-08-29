using UnityEngine;
using UnityEngine.SceneManagement;
public class BackToMenu : MonoBehaviour
{
    //when the back button is pressed, go back to the main menue
    private void OnMouseUpAsButton()
    {
        SceneManager.LoadScene(0);
    }
}
