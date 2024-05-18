using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameoverDialog : Dialog
{
    public void Replay()
    {
        Close();
        ReloadCurrentScene();
    }

    public void BackHome()
    {
		Close();
		ReloadCurrentScene();
	}

    public void ExitGame()
    {
		Close();
        Application.Quit();
	}

    void ReloadCurrentScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
