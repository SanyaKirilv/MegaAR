using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using System.Net.Security;
using System.Net.Mail;
using System.Net;
using System.Security.Cryptography.X509Certificates;
public class ARManager : MonoBehaviour
{
    [SerializeField]private int index;
    [SerializeField]private GameObject[] images;

    private void Start()
    {
        Init();
    }
    public void Complete()
    {
        Send();
        PlayerPrefs.SetInt("Task_" + index.ToString(), 1);
        if(index == 0) PlayerPrefs.SetInt("CurrentScene", 0);
        else PlayerPrefs.SetInt("CurrentScene", 1);
        SceneManager.LoadScene("Menu");
    }

    private void Init()
    {
        index = PlayerPrefs.GetInt("Task");
        for(int i = 0; i < images.Length; i++)
        {
            images[i].SetActive(false);
        }
        images[index].SetActive(true);
    }

    private void Send()
    {
        var team = PlayerPrefs.GetString("Team");
        MailMessage message = new MailMessage
        {
            Subject = "Команда: " + team,
            Body = "Завершила " + index + " задание в " + DateTime.Now.ToString() + ".",
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
