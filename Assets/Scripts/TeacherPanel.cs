using MySql.Data.MySqlClient;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class TeacherPanel : MonoBehaviour
{

    private GameObject ExercisePrefab;
    [SerializeField] private Transform PanelTransform;

    private void Start()
    {
        Dictionary<int, string> id_Exercise_Dict = new Dictionary<int, string>();
        MySqlConnection connection = new MySqlConnection(Connection.connectionString);
        string getExercisesCommandText = "SELECT id_exercise, exerciseName FROM exercises";
        MySqlCommand getExercisesCommand = new MySqlCommand(getExercisesCommandText, connection);

        connection.Open();

        MySqlDataReader reader = getExercisesCommand.ExecuteReader();

        while (reader.Read())
        {
            id_Exercise_Dict.Add(reader.GetInt32(0), reader.GetString(1));
        }

        connection.Close();

        ExercisePrefab = Resources.Load<GameObject>(@"TeacherPanelPrefabs\Exercise"); 
        
        for (int i = 1; i <= id_Exercise_Dict.Count; i++)
        {
            GameObject Exercise = Instantiate(ExercisePrefab, PanelTransform);
            Exercise.transform.GetComponentInChildren<Text>().text = id_Exercise_Dict[i];
            int id = i;
            Exercise.transform.GetComponentInChildren<Button>().onClick.AddListener(() => {
                Debug.Log($"Редактирование задания номер {id}");
            });
        }

        PanelTransform.GetComponent<RectTransform>().offsetMax = new Vector2(0f, id_Exercise_Dict.Count * 30f);
    }
}
