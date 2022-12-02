using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    private Text timerText;
    //This code will grab and cache a reference to the Text component on the same GameObject the script exists on.
    void Awake()
    {
        timerText = GetComponent<Text>();
    }
    void Update()
    {
        timerText.text =
        System.Math.Round((decimal)Time.timeSinceLevelLoad,
        2).ToString();
    }
}
