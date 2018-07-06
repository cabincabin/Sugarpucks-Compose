using UnityEngine;
using UnityEngine.SceneManagement;
public class BackToMenu : MonoBehaviour
{
    private void OnMouseUpAsButton()
    {
        SceneManager.LoadScene(0);
    }
}
