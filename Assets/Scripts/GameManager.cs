using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    #region unity_functions
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else if(instance!= this)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(gameObject);
    }
    #endregion

    #region sceneTransition
    public void StartGame()
    {
        SceneManager.LoadScene("scene11");

    }
    public void WinGame()
    {
        SceneManager.LoadScene("WinScene");

    }
    public void LossGame()
    {
        SceneManager.LoadScene("LossScene");

    }
    public void GotoMainMenu()
    {
        SceneManager.LoadScene("MainMenu");

    }
    #endregion

}
