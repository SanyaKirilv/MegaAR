using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using System.Net.Security;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;

public class MenuManager : MonoBehaviour
{
    public bool reset;
    public bool sendAccountsData;
    [Header("Teams")]
    [SerializeField]private string[] teams;
    [SerializeField]private string[] codes;
    [SerializeField]private int[] tasks;
    [SerializeField]private int count;
    [SerializeField]private int teamIndex;
    [Header("UI")]
    [SerializeField] private GameObject[] menu;
    [SerializeField]private int currentScene;
    [SerializeField]private Dropdown dropDown;
    [SerializeField]private InputField inputField;
    [SerializeField]private Text messageText;
    [SerializeField]private string[] message;


    private void Start()
    {
        if(reset) PlayerPrefs.DeleteAll();
        if(sendAccountsData) SendAccountsData();
        Init();
        messageText.text = " ";
    }

    public void Submit()
    {
        if(inputField.text != "")
        {
            for (var i = 0; i < codes.Length; i++)
            {
                if(inputField.text == codes[i])
                {
                    if(teamIndex == i) CorrectPassword();
                    else WrongPassword();
                }
                else WrongPassword();
            }
        }
        else EmptyPassword();
    }

    private void WrongPassword()
    {
        currentScene = 0;
        PlayerPrefs.SetInt("CurrentScene", currentScene);
        messageText.text = message[0];
    }

    private void EmptyPassword()
    {
        currentScene = 0;
        PlayerPrefs.SetInt("CurrentScene", currentScene);
        messageText.text = message[1];
    }

    private void CorrectPassword()
    {
        PlayerPrefs.SetString("Team", teams[teamIndex]);
        Send();
        currentScene = 1;
        PlayerPrefs.SetInt("CurrentScene", currentScene);
        LoadLevels();
    }

    private void Init()
    {
        dropDown.GetComponent<Dropdown>();
        dropDown.options.Clear();
        foreach (var team in teams)
        {
            dropDown.options.Add(new Dropdown.OptionData() {text = team});
        }
        dropDown.onValueChanged.AddListener(delegate {DropdownItemSelected(dropDown);});
        inputField.GetComponent<InputField>();
        LoadLevels(); 
    }

    void DropdownItemSelected(Dropdown dropDown)
    {
        teamIndex = dropDown.value;
    }

    public void TestMode()
    {
        PlayerPrefs.SetInt("CurrentScene", -1);
        LoadLevels();
    }

    public void CloseTestMode()
    {
        PlayerPrefs.SetInt("CurrentScene", 0);
        LoadLevels();
    }

    public void CloseProgram()
    {
        Application.Quit();
    }

    public void LoadLevels()
    {
        EndGame();
        currentScene = PlayerPrefs.GetInt("CurrentScene");
        if(currentScene == -1)
        {
            menu[0].SetActive(false);
            menu[1].SetActive(false);
            menu[2].SetActive(true);
            menu[3].SetActive(false);
        }
        if(currentScene == -2)
        {
            menu[0].SetActive(false);
            menu[1].SetActive(false);
            menu[2].SetActive(false);
            menu[3].SetActive(true);
        }
        else
        {
            switch(currentScene)
            {
                case 0:
                    menu[0].SetActive(true);
                    menu[1].SetActive(false);
                    menu[2].SetActive(false);
                    menu[3].SetActive(false);
                    break;
                case 1:
                    menu[0].SetActive(false);
                    menu[1].SetActive(true);
                    menu[2].SetActive(false);
                    menu[3].SetActive(false);
                    break;
                case 2:
                    SceneManager.LoadScene("Task_1");
                    PlayerPrefs.SetInt("Task", 1);
                    break;
                case 3:
                    SceneManager.LoadScene("Task_2");
                    PlayerPrefs.SetInt("Task", 2);
                    break;
                case 4:
                    SceneManager.LoadScene("Task_3");
                    PlayerPrefs.SetInt("Task", 3);
                    break;
                case 5:
                    SceneManager.LoadScene("Task_4");
                    PlayerPrefs.SetInt("Task", 4);
                    break;
                case 6:
                    SceneManager.LoadScene("Task_5");
                    PlayerPrefs.SetInt("Task", 5);
                    break;
                case 7:
                    SceneManager.LoadScene("Task_6");
                    PlayerPrefs.SetInt("Task", 6);
                    break;
                case 8:
                    SceneManager.LoadScene("Task_7");
                    PlayerPrefs.SetInt("Task", 7);
                    break;
                case 9:
                    SceneManager.LoadScene("Task_8");
                    PlayerPrefs.SetInt("Task", 8);
                    break;
                case 10:
                    SceneManager.LoadScene("AR");
                    break;
                case 11:
                    SceneManager.LoadScene("Task_0");
                    PlayerPrefs.SetInt("Task", 0);
                    break;
            }
        }  
    }

    private void EndGame()
    {
        count = 0;
        for(int i = 0; i < tasks.Length; i++)
        {
            tasks[i] = PlayerPrefs.GetInt("Task_"  + (i + 1).ToString());
            if(tasks[i] == 1)
            {
                count++;
                if(count == tasks.Length) PlayerPrefs.SetInt("CurrentScene", -2);
            } 
        }
    }
    private void Send()
    {
        var team = PlayerPrefs.GetString("Team");
        MailMessage message = new MailMessage
        {
            Subject = "Команда: " + team,
            Body = "Авторизация в приложении в " + DateTime.Now.ToString() + ".",
            From = new MailAddress("alexandrekirilv@icloud.com")
        };
        message.To.Add("nikanikaze@mail.ru");
        message.BodyEncoding = System.Text.Encoding.UTF8;

        SmtpClient client = new SmtpClient
        {
            Host = "smtp.mail.me.com",
            Port = 587,
            Credentials = new NetworkCredential("alexandrekirilv@icloud.com", "ypsr-osbu-cqsf-jkji"),
            EnableSsl = true
        };
        ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        };
        client.Send(message);
    }

    private void SendAccountsData()
    {
        var str = string.Empty;
        for (int i = 0; i < teams.Length; i++)
        {
            str += teams[i] + " - " + codes[i] + "\n";
        }
        MailMessage message = new MailMessage
        {
            Subject = "Команда / пароль: ",
            Body = str,
            From = new MailAddress("alexandrekirilv@icloud.com")
        };
        message.To.Add("nikanikaze@mail.ru");
        message.BodyEncoding = System.Text.Encoding.UTF8;

        SmtpClient client = new SmtpClient
        {
            Host = "smtp.mail.me.com",
            Port = 587,
            Credentials = new NetworkCredential("alexandrekirilv@icloud.com", "ypsr-osbu-cqsf-jkji"),
            EnableSsl = true
        };
        ServicePointManager.ServerCertificateValidationCallback = delegate (object s, X509Certificate certificate, X509Chain chain, SslPolicyErrors sslPolicyErrors)
        {
            return true;
        };
        client.Send(message);
    }
}
