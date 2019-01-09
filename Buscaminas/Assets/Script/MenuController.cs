using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour {

	public void ExitGame()
    {
        Application.Quit();
    }

    public void EnterGame()
    {
        SceneManager.LoadScene(1);
    }

    public void EnterRecord()
    {
        SceneManager.LoadScene(3);
    }
}
