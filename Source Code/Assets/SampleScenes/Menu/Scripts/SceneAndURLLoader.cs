using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneAndURLLoader : MonoBehaviour
{
    private PauseMenu m_PauseMenu;


    private void Awake ()
    {
}



    public void SceneLoad(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}


	public void LoadURL(string url)
	{
		Application.OpenURL(url);
	}
}

