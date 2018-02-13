using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayView : MonoBehaviour
{
    public Text m_platformCountLabel;
    public GameObject m_gameOverPanel;
    public GameObject m_winnerPanel;

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

    public void ShowGameOver()
    {
        m_gameOverPanel.SetActive(true);
    }

    public void ShowWinner()
    {
        m_winnerPanel.SetActive(true);
    }

    private void Awake()
    {
        m_gameOverPanel.SetActive(false);
        m_winnerPanel.SetActive(false);
    }

    private void UpdateView()
    {
        m_platformCountLabel.text = string.Format("{0} / {1}", m_scoredPlatforms, m_platformCount);
    }
}
