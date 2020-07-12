using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public ObjectSpawner objectSpawner;
    public PlacementIndicatorScript placementIndicator;
    public Camera gameCamera;
    public MenuController menuController;
    public TriviaCollection triviaCollection;
    public SelectObjects selectObjects;

    public GameObject confirmObjectFoundButton;
    public GameObject skipButton;

    private List<GameObject> spawnedObjectList;
    public GameObject gameObjectToFind;

    private bool showObjectsHaveBeenPlaced = true;
    private bool searchObjects = false;
    private bool mathExerciseSolved = false;

    private float time = 0.0f;
    private float interpolationPeriod = 40.0f;

    private int countObjectsFound = 0;

    public int CountObjectsFound
    {
        get { return this.countObjectsFound; }
        set { this.countObjectsFound = value; }
    }

    private bool arComponentsAreActive = false;
    public bool ArComponentsAreActive
    {
        get { return this.arComponentsAreActive; }
        set { this.arComponentsAreActive = value; }
    }

    private void Awake()
    {
        //objectSpawner.NumberOfObjcectsToSpawn = 3;
    }

    void Start()
    {
        objectSpawner.NumberOfObjectsToSpawn = PlayerPrefs.GetInt("AmoutOfObjectsToFind");
        confirmObjectFoundButton.SetActive(false);
        placementIndicator = FindObjectOfType<PlacementIndicatorScript>();
    }

    void Update()
    {
        time += Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (objectSpawner.getSpawnedObjectList().Count >= objectSpawner.NumberOfObjectsToSpawn && menuController.PlaceObjectsCanvas.activeSelf)
        {
            menuController.StartMathExercise();
            menuController.ManageFindObjectInstruction();
        }

        if (objectSpawner.getSpawnedObjectList().Count >= objectSpawner.NumberOfObjectsToSpawn && menuController.SearchObjectsCanvas.activeSelf)
        {
            SearchObjects();
        }

        if (time >= interpolationPeriod)
        {
            skipButton.SetActive(true);
        }
        else
        {
            skipButton.SetActive(false);
        }
    }

    public void SearchObjects()
    {
        print("SearchObjects() called");
        spawnedObjectList = objectSpawner.getSpawnedObjectList();

        if (countObjectsFound >= objectSpawner.NumberOfObjectsToSpawn)
        {
            print("All Objects Found");

            //print(triviaCollection.GetTriviaNotShownYet(triviaCollection.getTriviaBasedOnObjectName(selectObjects.SelectedObjectNames[0])));

            searchObjects = false;
            countObjectsFound = 0;
            menuController.OpenCompleteGameCanvas();
            objectSpawner.CleanSpawnedOjbectList();

            return;
        }

        foreach (GameObject spawnedObject in spawnedObjectList)
        {
            if (Vector3.Distance(spawnedObjectList[countObjectsFound].transform.position, gameCamera.transform.position) < 1.5 && countObjectsFound == spawnedObjectList.IndexOf(spawnedObject))
            {
                spawnedObject.SetActive(true);
            }
            else
            {
                spawnedObject.SetActive(false);
                
            }
        }
        

        if (Vector3.Distance(spawnedObjectList[countObjectsFound].transform.position, gameCamera.transform.position) < 1.5 && 
            Vector3.Distance(spawnedObjectList[countObjectsFound].transform.position, placementIndicator.transform.position) < 0.2 ||
            Vector3.Distance(spawnedObjectList[countObjectsFound].transform.position, gameCamera.transform.position) < 0.3)
        {
            confirmObjectFoundButton.SetActive(true);
        }
        else
        {
            confirmObjectFoundButton.SetActive(false);
        }
    }

    public void confirmObjectFound()
    {
        countObjectsFound++;
        menuController.ManageFindObjectInstruction();
        time = 0.0f;
    }

    public void restartGameControllerValues()
    {
        mathExerciseSolved = false;
        showObjectsHaveBeenPlaced = true;
    }

    public void setMathExerciseSolved(bool state)
    {
        mathExerciseSolved = state;
    }

    public void skipSearchButton()
    {
        countObjectsFound++;
        print(countObjectsFound);
        menuController.ManageFindObjectInstruction();
    }

}
