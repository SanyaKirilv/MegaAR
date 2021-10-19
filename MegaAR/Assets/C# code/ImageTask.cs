using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class ImageTask : MonoBehaviour
{
    [SerializeField]private Color[] colors;
    [SerializeField]private int isDone;
    [SerializeField]private MenuManager menuManager;

    private void Start()
    {
        Init();
        transform.GetComponent<Image>().color = colors[isDone];
        if(isDone == 1)
        {
            transform.GetComponent<Image>().color = colors[isDone];
        }
    }
    public void LoadTask()
    {
        if (isDone == 0)
        {
            NameToInt();
            menuManager.LoadLevels();
        }
    }

    private void Init()
    {
        if (!PlayerPrefs.HasKey("Task_" + transform.name.ToString()))
        {
            PlayerPrefs.SetInt("Task_" + transform.name.ToString(), 0);
        }
        isDone = PlayerPrefs.GetInt("Task_" + transform.name.ToString());
    }

    private void NameToInt()
    {        
            if(transform.name == "0") PlayerPrefs.SetInt("CurrentScene", 11);
            if(transform.name == "1") PlayerPrefs.SetInt("CurrentScene", 2);
            if(transform.name == "2") PlayerPrefs.SetInt("CurrentScene", 3);
            if(transform.name == "3") PlayerPrefs.SetInt("CurrentScene", 4);
            if(transform.name == "4") PlayerPrefs.SetInt("CurrentScene", 5);
            if(transform.name == "5") PlayerPrefs.SetInt("CurrentScene", 6);
            if(transform.name == "6") PlayerPrefs.SetInt("CurrentScene", 7);
            if(transform.name == "7") PlayerPrefs.SetInt("CurrentScene", 8);
            if(transform.name == "8") PlayerPrefs.SetInt("CurrentScene", 9);
    }
}
