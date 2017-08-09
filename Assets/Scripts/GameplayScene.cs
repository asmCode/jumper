using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayScene : MonoBehaviour
{
    public Platform m_platformPrefab;
    public Dude m_dude;
    public static int m_seed = 10;

    private Track m_track;

    private TrackDebugInfo m_trackDebugInfo = new TrackDebugInfo();

    private Vector3 m_targetLookVector;

    public void UiEvent_SetSeed(int seed)
    {
        m_seed = seed;
    }

    private void Start()
    {
        TrackGenerator trackGenerator = new TrackGenerator();
        m_track = trackGenerator.Generate(m_platformPrefab, m_seed);

        m_dude.SetTrack(m_track);
    }

    private void Update()
    {
        m_trackDebugInfo.DrawTrack(m_track, m_dude.Speed);

        if (Input.GetMouseButtonDown(0))
        {
            m_dude.Jump();
        }

        // m_dude.transform.forward = Vector3.RotateTowards(m_dude.transform.forward, lookVector, 5.0f * Time.deltaTime, 10 * Time.deltaTime);
        //Vector3.SmoothDamp()
    }

    public void Jump()
    {
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void ResetRecord()
    {
        PlayerPrefs.SetFloat("best_time", 0.0f);
    }
}
