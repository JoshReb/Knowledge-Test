using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHandler : MonoBehaviour {
    
    public Text myText;
    public InputField f_id;
    public InputField f_name;
    public InputField f_email;
    public Dropdown dropdown;

    public List<User> usersList;
    public UserList userListObject;

    List<string> usersString = new List<string>();

    private void Awake() {
        dropdownFill();
    }

    public void ButtonAddPressed(){
        print("Create was pressed");

        //Create User
        User user = new User(f_id.text,f_name.text,f_email.text);

        //Load saved Users into a UserList
        UserList loadedUserList = Load();
        if(!loadedUserList._userListObject.Exists(u => u._id == user._id)){
            //Add new User to the list
            loadedUserList._userListObject.Add(user);

            //Save updated UserList
            Save(loadedUserList);
            clearFields();

            myText.text = "The User was Added Successfully\nId: "+user._id+"\nName: "+user._name+"\nEmail: "+user._email;
        } else {
            myText.text = "A User with that Id already exists";
        }
    }

    public void ButtonSearchPressed(){
        print("Search was pressed");
        if(f_id.text == ""){
            myText.text = "You need to type an Id to Search";
        } 
        else {
            UserList loadedUserList = Load();

            User match = loadedUserList._userListObject.Find(u => u._id == f_id.text);
            if(match != null){
                f_name.text = match._name;
                f_email.text = match._email;
                myText.text = "The User was Found Successfully";
            } 
            else {
                myText.text = "There is no User with that Id";
            }
        }
    }

    public void ButtonUpdatePressed(){
        print("Update was pressed");
        if(f_id.text == "" || f_name.text == "" || f_email.text == ""){
            myText.text = "You need to type in all the fields to Update";
        }
        else {
            UserList loadedUserList = Load();

            int index = loadedUserList._userListObject.FindIndex(u => u._id == f_id.text);
            if(index != -1){
                loadedUserList._userListObject[index]._name = f_name.text;
                loadedUserList._userListObject[index]._email = f_email.text;

                Save(loadedUserList);
                //this is just because i want to copy paste c:
                User user = new User(loadedUserList._userListObject[index]._id, loadedUserList._userListObject[index]._name, loadedUserList._userListObject[index]._email);
                myText.text = "The User was Updated Successfully\n"+user._id+"\nName: "+user._name+"\nEmail: "+user._email;
                clearFields();
            }
            else {
                myText.text = "There is no User with that Id";
            }
        }
    }

    public void ButtonDeletePressed(){
        print("Delete was pressed");

        if(f_id.text == ""){
            myText.text = "You need to type an Id to Delete";
        }
        else {
            UserList loadedUserList = Load();

            int index = loadedUserList._userListObject.FindIndex(u => u._id == f_id.text);
            if(index != -1){
                loadedUserList._userListObject.RemoveAt(index);

                Save(loadedUserList);
                myText.text = "The User was Deleted Successfully";
                clearFields();
            }
            else {
                myText.text = "There is no User with that Id";
            }
        }
    }

    public void Save(UserList toSaveUserList){
        string json = JsonUtility.ToJson(toSaveUserList);
        File.WriteAllText(Application.dataPath +"/save.txt", json);

        dropdownFill();
    }

    public UserList Load(){
        string jsonString = File.ReadAllText(Application.dataPath +"/save.txt");
        UserList loadedUserList = JsonUtility.FromJson<UserList>(jsonString);
        return loadedUserList;
    }

    public void clearFields(){
        f_id.text = "";
        f_name.text = "";
        f_email.text = "";
    }

    public void dropdownFill(){

        UserList loadedUserList = Load();

        usersString.Clear();
        usersString.Add("User List");

        for(int i = 0; i < loadedUserList._userListObject.Count; i++){
            usersString.Add(loadedUserList._userListObject[i]._id+" "+loadedUserList._userListObject[i]._name+" "+loadedUserList._userListObject[i]._email);
        }

        dropdown.options.Clear();
        dropdown.AddOptions(usersString);
    }
}