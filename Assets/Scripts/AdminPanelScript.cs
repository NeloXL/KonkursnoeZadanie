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
    GameObject PrefabUsersTempltaObject;

    [SerializeField] Transform UsersTemplateTransform;

    [SerializeField] SceneAsset Scene_CreateUser;
    [SerializeField] SceneAsset Scene_Auutorization;

    private void Start()
    {
        PrefabUsersTempltaObject = Resources.Load<GameObject>(@"AdminPanelPrefabs\Users");
        PrefabTitleObject = Resources.Load<GameObject>(@"AdminPanelPrefabs\User");

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

        int recordsCount = Names.Count;

        GameObject UsersTemplate = Instantiate(PrefabUsersTempltaObject, UsersTemplateTransform);
        UsersTemplate.GetComponent<ScrollRect>().viewport = UsersTemplateTransform.GetComponent<RectTransform>();
        UsersTemplate.GetComponent<RectTransform>().offsetMax = new Vector2(0f, recordsCount * 100f);

        for (int i = 0; i < recordsCount; i++)
        {
            ShowRecord(Names[i], Surnames[i], Roles[i], UsersTemplate.transform);
        }
    }

    void ShowRecord(string Name, string Surname, string Role, Transform UsersTemplate)
    {
        GameObject Record = Instantiate(PrefabTitleObject, UsersTemplate);
        Record.GetComponentsInChildren<Text>()[0].text = Name + " " + Surname;
        Record.GetComponentsInChildren<Text>()[1].text = Role;
    }

    public void CreateUser()
    {
        SceneManager.LoadScene(Scene_CreateUser.name);
    }
    
    public void Back()
    {
        SceneManager.LoadScene(Scene_Auutorization.name);
    }
}
