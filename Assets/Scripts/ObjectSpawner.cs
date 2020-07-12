using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ObjectSpawner : MonoBehaviour
{

    private GameObject objectToSpawn;
    private PlacementIndicatorScript placementIndicator;
    private List<GameObject> SpawnedObjectList = new List<GameObject>();
    public List<GameObject> SpawnableObjectList = new List<GameObject>();

    public Camera gameCamera;

    public MenuController menuController;
    public GameController gameController;
    public SelectObjects selectObjects;

    public GameObject objectToCloseNotice;
    public GameObject placeObjectButton;

    private GameObject Model;

    private int numberOfObjectsToSpawn;
    public int NumberOfObjectsToSpawn
    {
        get { return this.numberOfObjectsToSpawn; }
        set { this.numberOfObjectsToSpawn = value; }
    }

    private float time = 0.0f;
    private float interpolationPeriod = 2.0f;

    private GameObject objAlw;
    private bool notYetSet = true;

    private ARRaycastManager arRaycastManager;

    private Vector3 scaleChange;
    public GameObject liveObject;



    void Awake()
    {
        
        /*
        ObjectNameList.Add("plant");
        ObjectNameList.Add("Phone");
        ObjectNameList.Add("cottage");
        ObjectNameList.Add("modernChair");
        ObjectNameList.Add("plant");
        ObjectNameList.Add("Cube");
        ObjectNameList.Add("EiffelTower");
        */

        time = 1.1f;

        placementIndicator = FindObjectOfType<PlacementIndicatorScript>();
        arRaycastManager = FindObjectOfType<ARRaycastManager>();
    }

    // Start is called before the first frame update
    void Start()
    {
        placeObjectButton.SetActive(false);
        NumberOfObjectsToSpawn = PlayerPrefs.GetInt("AmoutOfObjectsToFind");
    }

    // Update is called once per frame
    void Update()
    {
        time += Time.deltaTime;


        if (gameController.ArComponentsAreActive)
        {

            if (menuController.getPlaceObjectsCanvasStatus() && SpawnedObjectList.Count < NumberOfObjectsToSpawn && menuController.PlaceObjectsCanvas.activeSelf)
            {
                if (notYetSet)
                {
                    SetObjectToSpawn();
                    objAlw = Instantiate(objectToSpawn, placementIndicator.transform.position, placementIndicator.transform.rotation);
                    notYetSet = false;
                }
                if (!notYetSet)
                {
                    RayCastHandling(objAlw);
                }
            }

            //if (menuController.getPlaceObjectsCanvasStatus() && Input.touchCount > 0 && Input.touches[0].phase == TouchPhase.Began && SpawnedObjectList.Count < NumberOfObjcectsToSpawn && NoObjectToClose())
            if (menuController.getPlaceObjectsCanvasStatus() && SpawnedObjectList.Count < NumberOfObjectsToSpawn && NoObjectToClose() && PlaneIsBeingHit()) //&& !menuController.SearchPlaneInstructions.activeSelf)
            {
                placeObjectButton.SetActive(true);
            }
            else
            {
                placeObjectButton.SetActive(false);
            }
            

            if (SpawnedObjectList.Count > 0)
            {
                if (time >= interpolationPeriod)
                {
                    time -= interpolationPeriod;

                }
            }

            if (!NoObjectToClose() && !menuController.SearchPlaneInstructions.activeSelf)
            {
                objectToCloseNotice.SetActive(true);
            }
            else
            {
                objectToCloseNotice.SetActive(false);
            }
        }
    }

    public void PlaceObjectOnCurrentPositon()
    {
        SetObjectToSpawn();

        objAlw.transform.position = placementIndicator.transform.position;
        objAlw.transform.rotation = placementIndicator.transform.rotation;

        SpawnedObjectList.Add(objAlw);
        print(SpawnedObjectList.Count);
        notYetSet = true;
    }

    public bool NoObjectToClose()
    {
        if(SpawnedObjectList.Count > 0)
        {
            foreach (GameObject spawnedObject in SpawnedObjectList)
            {
                if (Vector3.Distance(spawnedObject.transform.position, placementIndicator.transform.position) < 1.0)
                {
                    return false;
                }
            }
        }
        
        return true;
    }

    public List<GameObject> getSpawnedObjectList()
    {
        return SpawnedObjectList;
    }

    public void SetObjectToSpawn()
    {
        print(SpawnableObjectList.Count);
        objectToSpawn = SpawnableObjectList[SpawnedObjectList.Count];
    }

    public void CleanSpawnedOjbectList()
    {
        foreach (GameObject spawnObject in SpawnedObjectList)
        {
            Destroy(spawnObject);
        }

        foreach (GameObject spawnableObject in SpawnableObjectList)
        {
            Destroy(spawnableObject);
        }

        SpawnableObjectList.Clear();
        SpawnedObjectList.Clear();
    }

    public void ShuffleSpawnableObjectList()
    {
        Shuffle(SpawnableObjectList);
    }

    //https://forum.unity.com/threads/clever-way-to-shuffle-a-list-t-in-one-line-of-c-code.241052/
    public void Shuffle<GameObject>( List<GameObject> ts)
    {
        var count = ts.Count;
        var last = count - 1;
        for (var i = 0; i < last; ++i)
        {
            var r = UnityEngine.Random.Range(i, count);
            var tmp = ts[i];
            ts[i] = ts[r];
            ts[r] = tmp;
        }
    }

    public void RayCastHandling(GameObject objectPlace)
    {
        //shoot a raycast from center of screen
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        arRaycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);

        //if we hit an Ar plane, update the position and rotation
        if (hits.Count > 0)
        {
            objectPlace.transform.position = hits[0].pose.position;
            objectPlace.transform.rotation = hits[0].pose.rotation;

            if (!objectPlace.activeInHierarchy)
            {
                objectPlace.SetActive(true);

                Invoke("disableSearchPlaneInstructions", 4f);
            }
        }
        else
        {
            objectPlace.SetActive(false);
        }
    }

    public bool PlaneIsBeingHit()
    {
        //shoot a raycast from center of screen
        List<ARRaycastHit> hits = new List<ARRaycastHit>();

        arRaycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.Planes);

        //if we hit an Ar plane, update the position and rotation
        if (hits.Count > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void disableSearchPlaneInstructions()
    {
        menuController.setSearchPlaneInstructionsState(false);
        
    }

    /*
    public void FillSpawnableObjectsList(List<GameObject> selectedSpawnableObjects)
    {
        //print("Listsize: " + selectedSpawnableObjects.Count);
        //Instantiate(selectedSpawnableObjects[0], new Vector3(0, 0, 0), Quaternion.identity);
        SpawnableObjectList = selectedSpawnableObjects;
        //Fill Spawnable Object List with selected Objecs from SelectObjects own list. Have to figure out how to access the specific Objects spawned in there
    }
    */

    
    public void FillSpawnableObjectsList(List<GameObject> selectedSpawnableObjects)
    {
        //print("Listsize: " + selectedSpawnableObjects.Count);
        //Instantiate(selectedSpawnableObjects[0], new Vector3(0, 0, 0), Quaternion.identity);

        //SpawnableObjectList = selectedSpawnableObjects;


        foreach(string objectName in selectObjects.SelectedObjectNames)
        {
            GameObject emptyGameObject = Instantiate(liveObject, transform.position, transform.rotation);

            emptyGameObject.GetComponent<SpawnableObject>().setModelName(objectName);

            //print("getModelName output: "+emptyGameObject.GetComponent<SpawnableObject>().getModelName());

            emptyGameObject.name = objectName;

            scaleChange = new Vector3(-0.9f, -0.9f, -0.9f);
            emptyGameObject.transform.localScale += scaleChange;



            GameObject go = Instantiate(Resources.Load("Models/" + objectName), transform.position, transform.rotation) as GameObject;

            go.transform.localScale += scaleChange;

            go.transform.parent = emptyGameObject.transform;
            emptyGameObject.transform.position = new Vector3(1000, 1000, 1000);

            SpawnableObjectList.Add(emptyGameObject);


          
        }

        

        
        //Fill Spawnable Object List with selected Objecs from SelectObjects own list. Have to figure out how to access the specific Objects spawned in there
    }

    public void skipButton()
    {
        SetObjectToSpawn();

        objAlw = Instantiate(objectToSpawn, new Vector3(0, 0, 0), Quaternion.identity);

        objAlw.transform.position = placementIndicator.transform.position;
        objAlw.transform.rotation = placementIndicator.transform.rotation;

        SpawnedObjectList.Add(objAlw);

    }

}
