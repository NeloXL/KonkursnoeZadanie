using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AdminPanelScript : MonoBehaviour
{
    GameObject PrefabTitleObject;

    [SerializeField] Transform PanelTransform;

    [SerializeField] SceneAsset Scene_CreateUser;

    private void Start()
    {
        PrefabTitleObject = Resources.Load<GameObject>("User");
        List<string> Names = new List<string>();
        List<string> Surnames = new List<string>();
        List<string> Roles = new List<string>();

        MySqlConnection connection = new MySqlConnection(Connection.connectionString);
        string commandText = "SELECT Name, Surname, Role FROM konkursdb.users";
        MySqlCommand command = new MySqlCommand(commandText, connection);

        connection.Open();

        MySqlDataReader reader = command.ExecuteReader();

        while (reader.Read())
        {
            if (reader.GetString(2) != "Admin")
            {
                Names.Add(reader.GetString(0));
                Surnames.Add(reader.GetString(1));
                Roles.Add(reader.GetString(2));
            }
        }

        connection.Close();

        for (int i = 0; i < Names.Count; i++)
        {
            ShowRecord(Names[i], Surnames[i], Roles[i]);
        }
    }

    void ShowRecord(string Name, string Surname, string Role)
    {
        GameObject Record = Instantiate(PrefabTitleObject, PanelTransform);
        Record.GetComponentsInChildren<Text>()[0].text = Name + " " + Surname;
        Record.GetComponentsInChildren<Text>()[1].text = Role;
    }

    public void CreateUser()
    {
        SceneManager.LoadScene(Scene_CreateUser.name);
    }
}
