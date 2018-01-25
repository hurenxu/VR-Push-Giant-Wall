using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using System;

public class LoadingBar : MonoBehaviour {

    public GameObject LoadingScreen;
    public Slider Slider;
    public float Duration { get; set; }
    public event EventHandler<EventArgs> OnReachMaximum;
    public event EventHandler<EventArgs> OnReset;

    public bool IsLocked { get; set; }

    public void Reset()
    {
        Duration = 0;
        Slider.value = 0;
        IsLocked = false;
        if (OnReset != null)
            OnReset.Invoke(this, EventArgs.Empty);
    }

    public void IncreastTime(float t)
    {        
        if (IsLocked)
        {
            return;
        }
        Duration += t;
        float progress = Mathf.Clamp(Duration, 0, 2);
        Slider.value = progress;
        if (OnReachMaximum != null && progress == 2)
            OnReachMaximum.Invoke(this, EventArgs.Empty);
    }
    
}
