using System;
using System.Collections;
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
		StartCoroutine(LoadScene1(sceneName));
	}

	IEnumerator LoadScene1(string sceneName)
    {
		TransitionManager.instance.Close();
		yield return new WaitForSeconds(0.75f);
		SceneManager.LoadScene(sceneName);
	}


	public void LoadURL(string url)
	{
		Application.OpenURL(url);
	}
}

