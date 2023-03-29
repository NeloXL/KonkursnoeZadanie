using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class User
{
    public string login;
    public string password;
    public string role;
    public string name;
    public string surname;
    public string age;

    public User(string login, string password, string role, string name, string surname, string age)
    {
        this.login = login;
        this.password = password;
        this.role = role;
        this.name = name;
        this.surname = surname;
        this.age = age;
    }
}
