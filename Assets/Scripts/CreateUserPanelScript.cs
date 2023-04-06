using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Tls;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreateUserPanelScript : MonoBehaviour
{
    [SerializeField] SceneAsset Scene_AdminPanel;
    //
    [SerializeField] GameObject Panel;
    [SerializeField] Text Error;

    [SerializeField] Color Sucsessful;
    [SerializeField] Color Failure;

    public void Confirm()
    {
        Text FIO = Panel.GetComponentsInChildren<InputField>()[0].GetComponentsInChildren<Text>()[1];
        Text Password = Panel.GetComponentsInChildren<InputField>()[1].GetComponentsInChildren<Text>()[1];
        Text Role = Panel.GetComponentInChildren<Dropdown>().GetComponentsInChildren<Text>()[0];
        string[] initials = FIO.text.Split(' ');

        if (string.IsNullOrEmpty(FIO.text) || string.IsNullOrEmpty(Password.text))
        {
            Error.text = "Введены не все данные!";
            Error.color = Failure;
            Error.gameObject.SetActive(true);
            return;
        }

        if (initials.Length != 3)
        {
            Error.text = "ФИО введено неправильно (правильно: Иванов Иван Иванович)";
            Error.color = Failure;
            Error.gameObject.SetActive(true);
            return;
        }

        Error.gameObject.SetActive(false);

        CreateUser(initials, Password.text, Role.text);
    }

    void CreateUser(string[] initials, string password, string role)
    {
        string login = initials[0] + initials[1].ToUpper()[0] + initials[2].ToUpper()[0];
        string name = initials[1];
        string surname = initials[0];
        string age = Random.Range(0, 99).ToString();

        MySqlConnection connection = new MySqlConnection(Connection.connectionString);
        string commandCreateUserText = $"INSERT INTO konkursdb.users (`Login`, `Password`, `Role`, `Name`, `Surname`, `Age`) VALUES ('{login}', '{password}', '{role}', '{name}', '{surname}', '{age}')";
        MySqlCommand command = new MySqlCommand(commandCreateUserText, connection);

        connection.Open();

        command.ExecuteNonQuery();

        connection.Close();

        Error.text = "Пользователь успешно создан!";
        Error.color = Sucsessful;
        Error.gameObject.SetActive(true);
    }

    public void Back()
    {
        SceneManager.LoadScene(Scene_AdminPanel.name);
    }
}
