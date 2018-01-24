using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CircleBar : MonoBehaviour {
    public Image Bar;
    public RectTransform Button;
    public float Value = 0;

    void Start()
    {
    }

    // Update is called once per frame
    void Update () {
        Value += 360 * Time.deltaTime / 2.0f;
        Change(Value);
	}

    void Change(float value)
    {
        Bar.fillAmount = value / 360;
        Button.localEulerAngles = new Vector3(0.0f, 0.0f, 
            -value);
    }

}
