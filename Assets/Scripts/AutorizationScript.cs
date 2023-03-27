using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutorizationScript : MonoBehaviour
{
    private GameObject autorizationPanel;

    private Text loginError;
    private Text passwordError;
    private Text loginText;
    private Text passwordText;

    private void Start()
    {
        autorizationPanel = GameObject.Find("PanelAutorization");
        loginError = autorizationPanel.transform.Find("LoginError").GetComponent<Text>();
        passwordError = autorizationPanel.transform.Find("PasswordError").GetComponent<Text>();
    }

    public void LogIn()
    {
        loginText = autorizationPanel.GetComponentsInChildren<InputField>()[0].GetComponentsInChildren<Text>()[1];
        passwordText = autorizationPanel.GetComponentsInChildren<InputField>()[0].GetComponentsInChildren<Text>()[1];

        if (string.IsNullOrEmpty(loginText.text))
        {
            loginError.gameObject.SetActive(true);
            loginError.text = "¬ведите логин!";
        }

        if (string.IsNullOrEmpty(passwordText.text))
        {
            Debug.Log(passwordError.text);
            passwordError.gameObject.SetActive(true);
            passwordError.text = "¬ведите пароль!";
        }

        if (!string.IsNullOrEmpty(loginText.text) && !string.IsNullOrEmpty(passwordText.text))
        {
            loginError.gameObject.SetActive(false);
            passwordError.gameObject.SetActive(false);
        }


    }
}
