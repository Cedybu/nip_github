using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveUserData : MonoBehaviour
{
    public UserDataContainer userDataContainer;
    public MenuController menuController;
    public FirebaseClass firebaseClass;

    private string nicknameFile;
    public string NicknameFile
    {
        get { return this.nicknameFile; }
        set { this.nicknameFile = value; }
    }

    public void SetupFile(string nickname)
    {
        nicknameFile = nickname + ".xml";
        //Debug.Log("Your files are located here: " + Application.persistentDataPath);
        XMLManager.Serialize(new UserDataContainer(), Application.persistentDataPath + "/" + nicknameFile);
    }

    public void addDataToFile(string nickname)
    {
        nicknameFile = nickname + ".xml";
        UserDataContainer temp;
        temp = XMLManager.Deserialize<UserDataContainer>(Application.persistentDataPath + "/" + nicknameFile);
        temp.userDataContainers.Add(menuController.LogInformation);
        XMLManager.Serialize(temp, Application.persistentDataPath + "/" + nicknameFile);
        firebaseClass.createRef();
    }
}


