using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnableObject : MonoBehaviour
{

    //private GameObject model;
    [SerializeField]
    private string modelName;

    private List<string> objectTrivia;


    /*
    public GameObject getModel()
    {
        return model;
    }
    */

    public string getModelName()
    {
        return modelName;
    }

    public void setModelName(string name)
    {
        modelName = name;
    }

    public List<string> getObjectTrivia()
    {
        return objectTrivia;
    }

    public void setObjectTrivia(List<string> objectTriviaList)
    {
        objectTrivia = objectTriviaList;
    }

}
