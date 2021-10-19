using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;


public class TaskManager : MonoBehaviour
{
    [Header("Variants")]
    [SerializeField]private string[] variants;
    [SerializeField]private int correctIndex;
    [SerializeField]private int index;
    [Header("UI")]
    [SerializeField]private Dropdown dropDown;
    [SerializeField]private Text errorText;
    [SerializeField]private string[] errors;
    [Header("Timer")]
    [SerializeField]private Text timerText;
    [SerializeField]private bool isPaused = false;
    [SerializeField]private bool isChanged = false;
    [SerializeField]private int timerState;
    [SerializeField]TimeSpan ts;
    [Header("Time")]
    [SerializeField]private float time;
    [SerializeField]private string seconds;
    [SerializeField]private string minutes;

    private void Start()
    {
        Init();
        index = PlayerPrefs.GetInt(SceneManager.GetActiveScene().name);
        errorText.text = " ";
    }

    private void FixedUpdate()
    {
        if(timerState == 1)
        {
            Timer();
        }
        else
        {
            timerText.text = "Выбрать";
        }
    }

    public void Submit()
    {
        timerState = PlayerPrefs.GetInt("Timer");
        if(index == correctIndex && timerState != 1) IsDone();
        else
        {
            timerState = 1;
            PlayerPrefs.SetInt("Timer", timerState);
            errorText.text = "Неправильный ответ, подождите завeршения таймера";
        }
    }

    private void IsDone()
    {
        PlayerPrefs.SetInt("CurrentScene", 10);
        SceneManager.LoadScene("AR");
    }

    private void Init()
    {
        dropDown.GetComponent<Dropdown>();
        dropDown.options.Clear();
        foreach (var variant in variants)
        {
            dropDown.options.Add(new Dropdown.OptionData() {text = variant});
        }
        dropDown.onValueChanged.AddListener(delegate {DropdownItemSelected(dropDown);});
        if(!PlayerPrefs.HasKey("Timer")) PlayerPrefs.SetInt("Timer", 0);
        timerState = PlayerPrefs.GetInt("Timer");
    }

    private void DropdownItemSelected(Dropdown dropDown)
    {
        index = dropDown.value;
    }

    private void Timer()
    {                
        time -= Time.deltaTime;
        seconds = (time % 60).ToString("00");
        minutes = (Mathf.Floor((time / 3600) * 60)).ToString("00");
        timerText.text = minutes + ":" + seconds;
        if(time <= 0)
        {
            timerState = 0;
            PlayerPrefs.SetInt("Timer", timerState);
            time = 299;
        }
    }

    private void PausePlayTime()
    {
        if (isPaused && timerState == 1)
        {
            PlayerPrefs.SetFloat("GameTime", time);
            PlayerPrefs.SetString("PhoneTime", DateTime.Now.ToString());
            isChanged = false;
        }
        if ((!isChanged || !isPaused) && timerState == 1)
        {
            if (PlayerPrefs.HasKey("PhoneTime") && PlayerPrefs.HasKey("GameTime"))
            {
                ts = DateTime.Now - DateTime.Parse(PlayerPrefs.GetString("PhoneTime"));
                time = PlayerPrefs.GetFloat("GameTime") - (ts.Seconds + ts.Minutes*60);
                Debug.Log(PlayerPrefs.GetFloat("GameTime") - ts.Seconds);
            }
            isChanged = true;
        }
    }
    void OnApplicationFocus(bool hasFocus)
    {
        isPaused = !hasFocus;
        PausePlayTime();
    }
    void OnApplicationPause(bool pauseStatus)
    {
        isPaused = pauseStatus;
        PausePlayTime();
    }
}
