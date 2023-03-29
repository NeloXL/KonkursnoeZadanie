using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MySql;
using MySql.Data;
using MySql.Data.MySqlClient;

public class AutorizationScript : MonoBehaviour
{
    private GameObject autorizationPanel;

    private Text loginError;
    private Text passwordError;
    private Text loginText;
    private Text passwordText;

    private bool _isCorrect;

    private void Start()
    {
        autorizationPanel = GameObject.Find("PanelAutorization");
        loginError = autorizationPanel.transform.Find("LoginError").GetComponent<Text>();
        passwordError = autorizationPanel.transform.Find("PasswordError").GetComponent<Text>();
    }

    public void LogIn()
    {
        loginText = autorizationPanel.GetComponentsInChildren<InputField>()[0].GetComponentsInChildren<Text>()[1];
        passwordText = autorizationPanel.GetComponentsInChildren<InputField>()[1].GetComponentsInChildren<Text>()[1];

        if (string.IsNullOrEmpty(loginText.text))
        {
            loginError.gameObject.SetActive(true);
            loginError.text = "Введите логин!";
            _isCorrect = false;
        }
        else
        {
            loginError.gameObject.SetActive(false);
            _isCorrect = true;
        }

        if (string.IsNullOrEmpty(passwordText.text))
        {
            passwordError.gameObject.SetActive(true);
            passwordError.text = "Введите пароль!";
            _isCorrect = false;
        }
        else
        {
            passwordError.gameObject.SetActive(false);
            _isCorrect = true;
        }

        if (!_isCorrect) return;

        MySqlConnection connection = new MySqlConnection(Connection.connectionString);
        string commandText = "SELECT Login FROM konkursdb.users";
        MySqlCommand myCommand = new MySqlCommand(commandText, connection);

        connection.Open();

        MySqlDataReader dataReader = myCommand.ExecuteReader();

        List<string> logins = new List<string>();

        while (dataReader.Read())
        {
            logins.Add(dataReader.GetString(0));
        }

        connection.Close();

        foreach (string login in logins)
        {
            if (login == loginText.text)
            {
                PasswordCheck();
            }
        }
    }

    void PasswordCheck()
    {
        MySqlConnection connection = new MySqlConnection(Connection.connectionString);
        string commandText = $"SELECT Password FROM konkursdb.users WHERE Login = '{loginText.text}'";
        MySqlCommand myCommand = new MySqlCommand(commandText, connection);

        connection.Open();

        MySqlDataReader dataReader = myCommand.ExecuteReader();
        dataReader.Read();

        if (dataReader.GetString(0) == passwordText.text)
        {
            passwordError.gameObject.SetActive(false);

            dataReader.Close();

            string getIdCommandStr = $"SELECT id FROM konkursdb.users WHERE Password = '{passwordText.text}'";
            MySqlCommand getIdCommand = new MySqlCommand(getIdCommandStr, connection);

            MySqlDataReader idReader = getIdCommand.ExecuteReader();
            idReader.Read();

            int id = idReader.GetInt32(0);

            connection.Close();

            UserSet(id);
        }
        else
        {
            passwordError.gameObject.SetActive(true);
            passwordError.text = "Неправильный пароль";
        }

        connection.Close();
    }

    void UserSet(int id)
    {
        List<string> userInfo = new List<string>();

        MySqlConnection connection = new MySqlConnection(Connection.connectionString);
        string userInfoCommandStr = $"SELECT * FROM konkursdb.users WHERE id = {id}";
        MySqlCommand userInfoCommand = new MySqlCommand(userInfoCommandStr, connection);

        connection.Open();

        MySqlDataReader userInfoReader = userInfoCommand.ExecuteReader();

        userInfoReader.Read();

        for (int i = 0; i < userInfoReader.VisibleFieldCount; i++)
        {
            userInfo.Add(userInfoReader.GetString(i));
        }
        connection.Close();
    }
}
