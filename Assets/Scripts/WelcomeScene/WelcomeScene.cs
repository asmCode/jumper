using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class WelcomeScene : MonoBehaviour
{
    public string m_nextScene = "BoostersEpicLevel";

	public GameObject loadingScreen;
	public Slider slider;

	AsyncOperation async;

	public void Update()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit ();
		}
	}

    public void UiEvent_StartPressed()
    {
		StartCoroutine(LoadGame());
    }

	IEnumerator LoadGame()
	{
		loadingScreen.SetActive (true);
		async = SceneManager.LoadSceneAsync(m_nextScene);
		async.allowSceneActivation = false;

		while (async.isDone == false)
		{
			slider.value = async.progress;
			if (async.progress == 0.9f)
			{
				slider.value = 1.0f;
				async.allowSceneActivation = true;
			}
			yield return null;
		}
	}
}
