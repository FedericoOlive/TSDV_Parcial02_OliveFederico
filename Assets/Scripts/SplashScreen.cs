using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SplashScreen : MonoBehaviour
{
    //private Animator anim;
    void Start()
    {
        //anim = GetComponent<Animator>();
        
    }
    void Update()
    {
        
    }

    public void FinalizeSplash()
    {
        SceneManager.LoadScene("MainMenu");
    }
}