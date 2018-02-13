using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayViewPresenter : MonoBehaviour
{
    public UnityEvent RetryPressed;

    private Dude2 m_dude;
    private PlayView m_playView;

    void Awake()
    {
        var dudeGameObject = GameObject.Find("Dude");
        if (dudeGameObject == null)
            return;

        m_dude = dudeGameObject.GetComponent<Dude2>();
        m_dude.OnDied.AddListener(() => { m_playView.ShowGameOver(); });

        m_playView = GetComponent<PlayView>();

        m_playView.SetPlatformCount(GetPlatformCount());
    }

    void Update()
    {
        if (m_dude == null)
            return;

        m_playView.SetScoredPlatform(m_dude.PlatformsScored);
    }

    public void ShowWinnerPanel()
    {
        m_playView.ShowWinner();
    }

    private int GetPlatformCount()
    {
        var trackGameObject = GameObject.Find("Track");
        if (trackGameObject == null)
            return 0;

        var track = trackGameObject.GetComponent<Track2>();
        if (track == null)
            return 0;

        return track.GetPlatformCount();
    }

    public void UiEvent_RetryPressed()
    {
        RetryPressed.Invoke();
    }
}
