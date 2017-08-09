using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hud : MonoBehaviour
{
    public Dude m_dude;
    public Text m_bestTimeLabel;
    public Text m_time;
    public Text m_speed;
    public Text m_speedChange;
    public Text m_jumpAccuracy;
    public Text m_lastTime;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        m_bestTimeLabel.text = m_dude.BestTime.ToString("0.00");
        m_time.text = m_dude.RunTime.ToString("0.00");
        m_speed.text = m_dude.m_jumpSpeed.ToString("0.00");
        m_jumpAccuracy.text = m_dude.JumpAccuracy.ToString("0.00");
        m_speedChange.text = m_dude.SpeedChange.ToString("0.00");
        m_lastTime.text = Dude.LastTime.ToString("0.00");
    }
}
