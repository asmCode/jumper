using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayViewPresenter : MonoBehaviour
{
    private Dude2 m_dude;
    private PlayView m_playView;

    void Awake()
    {
        var dudeGameObject = GameObject.Find("Dude");
        if (dudeGameObject == null)
            return;

        m_dude = dudeGameObject.GetComponent<Dude2>();

        m_playView = GetComponent<PlayView>();

        m_playView.SetPlatformCount(GetPlatformCount());
    }

    void Update()
    {
        if (m_dude == null)
            return;

        m_playView.SetScoredPlatform(m_dude.PlatformsScored);
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
}
