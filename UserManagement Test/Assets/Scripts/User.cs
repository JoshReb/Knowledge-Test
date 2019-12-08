using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class User
{
    public string _id;// {get;set;}
    public string _name;// {get;set;}
    public string _email;// {get;set;}

    public User(string id, string name, string email){
        _id = id;
        _name = name;
        _email = email;
    }
}
