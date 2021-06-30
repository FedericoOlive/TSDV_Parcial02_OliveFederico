using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    public void FinalizeSplash()
    {
        SceneManager.LoadScene("MainMenu");
    }
}