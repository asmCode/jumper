using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayView : MonoBehaviour
{
    public Text m_platformCountLabel;

    private int m_platformCount = 0;
    private int m_scoredPlatforms = 0;

    public void SetPlatformCount(int count)
    {
        m_platformCount = count;
        UpdateView();
    }

    public void SetScoredPlatform(int count)
    {
        m_scoredPlatforms = count;
        UpdateView();
    }

    private void UpdateView()
    {
        m_platformCountLabel.text = string.Format("{0} / {1}", m_scoredPlatforms, m_platformCount);
    }
}
