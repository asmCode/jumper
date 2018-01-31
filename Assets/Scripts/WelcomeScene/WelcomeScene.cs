using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WelcomeScene : MonoBehaviour
{
    public string m_nextScene = "BoostersEpicLevel";

    public void UiEvent_StartPressed()
    {
        SceneManager.LoadScene(m_nextScene);
    }
}
