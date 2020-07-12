using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using System.Linq;

public class MenuController : MonoBehaviour
{
    public GameObject HomeCanvas;
    public GameObject ConsentFormCanvas;
    public GameObject StartApplicationCanvas;
    public GameObject ChoseThemeCanvas;
    public GameObject ChoseObjectsCanvas;
    public GameObject PlaceObjectsCanvas;
    public GameObject MathExerciseCanvas;
    public GameObject CompleteMathCanvas;
    public GameObject SearchObjectsCanvas;
    public GameObject CompleteGameCanvas;
    public GameObject InfosCanvas;
    public GameObject inputText;
    public GameObject saveNicknameButton;
    public GameObject SearchPlaneInstructions;

    public MathController mathController;
    public GameController gameController;
    public ObjectSpawner objectSpawner;
    public SelectTheme selectTheme;
    public SelectObjects selectObjects;
    public SaveUserData saveUserData;
    public TriviaCollection triviaCollection;
    public FirebaseClass firebaseClass;

    public Toggle AgreeToggle;
    public ARPlaneManager arPlaneManager;

    public GameObject EasyButton;
    public GameObject MediumButton;
    public GameObject HardButton;
    public GameObject InfoPanel;
    public GameObject CloseInfoCanvasButton;
    private bool showObjectToFind = false;
    public bool ShowObjectToFind
    {
        get { return this.showObjectToFind; }
        set { this.showObjectToFind = value; }
    }

    private string userFileName;
    public string UserFileName
    {
        get { return this.userFileName; }
        set { this.userFileName = value; }
    }

    public UserData logInformation;
    public UserData LogInformation
    {
        get { return this.logInformation; }
        set { this.logInformation = value; }
    }

    private string playerName;
    public string PlayerName
    {
        get { return this.playerName; }
        set { this.playerName = value; }
    }
    
    private int TimeRoundWasStarted;

    private void Awake()
    {
        ConsentFormCanvas.SetActive(false);
        HomeCanvas.SetActive(true);
        StartApplicationCanvas.SetActive(false);
        ChoseThemeCanvas.SetActive(false);
        ChoseObjectsCanvas.SetActive(false);
        PlaceObjectsCanvas.SetActive(false);
        MathExerciseCanvas.SetActive(false);
        CompleteMathCanvas.SetActive(false);
        SearchObjectsCanvas.SetActive(false);
        CompleteGameCanvas.SetActive(false);
        InfosCanvas.SetActive(false);

        logInformation = new UserData();
        if (PlayerPrefs.GetInt("firstLogin") == 0)
        {
            ConsentFormCanvas.SetActive(true);
            PlayerPrefs.SetInt("AmoutOfObjectsToFind", 3);
        }
        else
        {
            playerName = PlayerPrefs.GetString("Spielername");
            firebaseClass.createRef();
        }
        print("awakefornumber---------------");
        print(objectSpawner.NumberOfObjectsToSpawn);
    }

    void Start()
    {
        logInformation.playedTimeSinceStart = 0;
        logInformation.secondsForThisRound = 0;
        logInformation.skipMathExercise = 0;
        logInformation.mathExerciseWrong = 0;
        logInformation.skipSearchObject = 0;
        logInformation.roundsInARow = 1;
        logInformation.unfinishedRounds = 0;
        logInformation.roindIsComplete = 0;
        logInformation.difficultyLevel = PlayerPrefs.GetInt("AmoutOfObjectsToFind");
        logInformation.agreeConsentForm = PlayerPrefs.GetInt("agreed");

        print("startfornumber---------------");
        print(objectSpawner.NumberOfObjectsToSpawn);
    }

    void Update()
    {
        if(ConsentFormCanvas.activeSelf)
        {
            CheckIfContinueShouldBePossible();
        }
    }

    public void CheckIfContinueShouldBePossible()
    {
        if(AgreeToggle.isOn && inputText.GetComponent<InputField>().text.Length == 4)
        {
            saveNicknameButton.GetComponent<Button>().interactable = true;
        }
        else
        {
            saveNicknameButton.GetComponent<Button>().interactable = false;
        }
    }

    public void StartGameCanvas()
    {
        StartApplicationCanvas.transform.Find("GameExplanationText").GetComponent<TMPro.TextMeshProUGUI>().text =
            "Platzieren Sie " + objectSpawner.NumberOfObjectsToSpawn + " Objekte an verschiedenen Orten in Ihrem Zuhause und merken Sie sich "
            + "diese. Begeben Sie sich anschliessend zurück an den Ort, an welchem Sie sich momentan befinden.";

        HomeCanvas.SetActive(false);
        StartApplicationCanvas.SetActive(true);
        objectSpawner.NumberOfObjectsToSpawn = PlayerPrefs.GetInt("AmoutOfObjectsToFind");
    }

    public void CloseApplication()
    {
        Application.Quit();
    }

    public void ClosePlaceInstructions()
    {
        arPlaneManager.enabled = false;
        gameController.ArComponentsAreActive = false;

        StartApplicationCanvas.SetActive(false);
        ChoseThemeCanvas.SetActive(true);

        selectTheme.resetThemeSelectionInterface();
        selectTheme.resetThemeSelection();
        selectTheme.DestroySpawnableObjectList();

        logInformation.startTime = System.DateTime.UtcNow.ToLocalTime().ToString("dd.MM.yyyy HH:mm");
        TimeRoundWasStarted = (int) Time.realtimeSinceStartup;
    }

    public void CloseThemeSelection()
    {
        selectObjects.generateObjectTextures();

        ChoseThemeCanvas.SetActive(false);
        ChoseObjectsCanvas.SetActive(true);

        //StartApplicationCanvas. transform.Find("GameExplanationText").GetComponent<TMPro.TextMeshProUGUI>().text =
        ChoseObjectsCanvas.transform.Find("TellAmountOfObjects").GetComponent<TMPro.TextMeshProUGUI>().text =
            "Wählen Sie " + objectSpawner.NumberOfObjectsToSpawn + " aus";

        selectObjects.resetObjectSelectionInterface();
        selectObjects.resetObjectSelection();
    }

    public void CloseObjectSelection()
    {
        arPlaneManager.enabled = true;
        gameController.ArComponentsAreActive = true;

        //print("SelectedSpawnableObjects size: " + selectObjects.SelectedSpawnableObjects.Count);
        objectSpawner.FillSpawnableObjectsList(selectObjects.SelectedSpawnableObjects);
        ChoseObjectsCanvas.SetActive(false);
        PlaceObjectsCanvas.SetActive(true);
    }

    public void CloseMathExercise()
    {
        MathExerciseCanvas.SetActive(false);
        mathController.attemptsMathExercise = 0;
        showObjectToFind = true;
        CompleteMathCanvas.SetActive(true);
    }

    public void CloseSearchObjectInstruction()
    {
        arPlaneManager.enabled = true;
        CompleteMathCanvas.SetActive(false);
        SearchObjectsCanvas.SetActive(true);
    }

    public void OpenCompleteGameCanvas()
    {
        arPlaneManager.enabled = false;
        SearchObjectsCanvas.SetActive(false);
        CompleteGameCanvas.SetActive(true);
        DisplayTrivia();

        logInformation.roundsInARow++;
        logInformation.roindIsComplete = 1;

        logInformation.playedTimeSinceStart = (int) Time.realtimeSinceStartup;
        logInformation.secondsForThisRound = (int) Time.realtimeSinceStartup - TimeRoundWasStarted;
        logInformation.endTime = System.DateTime.UtcNow.ToLocalTime().ToString("dd.MM.yyyy HH:mm");
        saveUserData.addDataToFile(playerName);
        logInformation.roindIsComplete = 0;
    }

    public void BackToHome(GameObject canvas)
    {
        canvas.SetActive(false);
        InfosCanvas.SetActive(false);

        if (CompleteGameCanvas.activeSelf == false)
        {
            logInformation.unfinishedRounds++;
            logInformation.roindIsComplete = 0;
        }
        StartNewRound();
    }

    public void RestartGame()
    {
        CompleteGameCanvas.SetActive(false);
        StartNewRound();
    }

    public void StartNewRound()
    {
        HomeCanvas.SetActive(true);
        objectSpawner.CleanSpawnedOjbectList();
        objectSpawner.ShuffleSpawnableObjectList();
        showObjectToFind = false;
        gameController.ArComponentsAreActive = false;
        gameController.restartGameControllerValues();
    }

    public void StartMathExercise()
    {
        mathController.CreateMathExercise();
        mathController.CreateMathText();

        PlaceObjectsCanvas.SetActive(false);
        MathExerciseCanvas.SetActive(true);

        arPlaneManager.enabled = false;
    }

    public void SkipSearchObjects()
    {
        ++logInformation.skipSearchObject;
        if (gameController.CountObjectsFound < objectSpawner.NumberOfObjectsToSpawn)
        {
            gameController.CountObjectsFound++;
            ManageFindObjectInstruction();
            //TODO Manage wih a listener on a Property
        }
        else
        {
            SearchObjectsCanvas.SetActive(false);
            StartApplicationCanvas.SetActive(true);
        }
    }

    public void CheckMathResult(Button button)
    {
        string solution = button.GetComponentInChildren<TMPro.TextMeshProUGUI>().text;

        if (solution == mathController.CalculateNumber().ToString())
        {
            CloseMathExercise();
            gameController.setMathExerciseSolved(true);
        }
        else
        {
            logInformation.mathExerciseWrong++;
            mathController.CreateMathExercise();
            mathController.CreateMathText();
        }
    }

    public bool getPlaceObjectsCanvasStatus()
    {
        if (PlaceObjectsCanvas.activeSelf == true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ManageFindObjectInstruction()
    {

        if (gameController.CountObjectsFound < objectSpawner.NumberOfObjectsToSpawn)
        {
            string nameofObject = objectSpawner.getSpawnedObjectList()[gameController.CountObjectsFound].GetComponent<SpawnableObject>().getModelName();
            print("FindObjectInstructions: " + objectSpawner.getSpawnedObjectList()[0].GetComponent<SpawnableObject>().getModelName());
            SearchObjectsCanvas.transform.Find("FindObjectInstructions").GetComponent<TMPro.TextMeshProUGUI>().text = "Suche nach " + nameofObject;
        }
        SearchObjectsCanvas.transform.Find("FoundObjectMeesage").GetComponent<TMPro.TextMeshProUGUI>().enabled = true;
        //SearchObjectsCanvas.transform.Find("FoundObjectMeesage").GetComponent<TMPro.TextMeshProUGUI>().text = 

        Invoke("DisableFoundObjectText", 3.5f);

    }

    void DisableFoundObjectText()
    {
        SearchObjectsCanvas.transform.Find("FoundObjectMeesage").GetComponent<TMPro.TextMeshProUGUI>().enabled = false;
    }

    public void setSearchPlaneInstructionsState(bool state)
    {
        SearchPlaneInstructions.SetActive(state);
    }

    public void ShowInfo(GameObject canvas)
    {
        InfosCanvas.SetActive(true);
        InfoPanel.SetActive(true);
        CloseInfoCanvasButton.SetActive(true);

        if (canvas.Equals(StartApplicationCanvas))
        {
            GameObject.Find("InfoTextStart").GetComponent<TMPro.TextMeshProUGUI>().text = "in progress...";
        }
        if (canvas.Equals(ChoseObjectsCanvas))
        {
            GameObject.Find("InfoTextStart").GetComponent<TMPro.TextMeshProUGUI>().text = "in progress... again";
        }
        if (canvas.Equals(PlaceObjectsCanvas))
        {
            GameObject.Find("InfoTextStart").GetComponent<TMPro.TextMeshProUGUI>().text = "in progress... again";
        }
        if (canvas.Equals(MathExerciseCanvas))
        {
            GameObject.Find("InfoTextStart").GetComponent<TMPro.TextMeshProUGUI>().text = "in progress... again";
        }
        if (canvas.Equals(SearchObjectsCanvas))
        {
            GameObject.Find("InfoTextStart").GetComponent<TMPro.TextMeshProUGUI>().text = "in progress... again";
        }
        if (canvas.Equals(CompleteGameCanvas))
        {
            GameObject.Find("InfoTextStart").GetComponent<TMPro.TextMeshProUGUI>().text = "in progress... again";
        }
        if (canvas.Equals(HomeCanvas))
        {
            HomeCanvas.SetActive(false);
            InfoPanel.SetActive(false);
            CloseInfoCanvasButton.SetActive(false);
            EasyButton.SetActive(true);
            MediumButton.SetActive(true);
            HardButton.SetActive(true);

            //GameObject.Find("InfoTextStart").GetComponent<TMPro.TextMeshProUGUI>().text = "in progress...";
        }

    }

    public void SetDifficultyLevel(int level)
    {
        objectSpawner.NumberOfObjectsToSpawn = level;
        PlayerPrefs.SetInt("AmoutOfObjectsToFind", level);
        CloseInfo();
        HomeCanvas.SetActive(true);
    }

    public void CloseInfo()
    {
        InfosCanvas.SetActive(false);
        EasyButton.SetActive(false);
        MediumButton.SetActive(false);
        HardButton.SetActive(false);
        InfoPanel.SetActive(false);
        CloseInfoCanvasButton.SetActive(false);
    }

    public void SavePlayerNickname()
    {
        playerName = inputText.GetComponent<InputField>().text;
        PlayerPrefs.SetString("Spielername", playerName);
        saveUserData.SetupFile(playerName);

        print(PlayerPrefs.GetString("Spielername"));
		
        logInformation.agreeConsentForm = 1;
        PlayerPrefs.SetInt("agreed", 1);
        PlayerPrefs.SetInt("firstLogin", 1);
        PlayerPrefs.Save();
        saveUserData.addDataToFile(playerName);

        ConsentFormCanvas.SetActive(false);
        HomeCanvas.SetActive(true);
    }

    void OnApplicationQuit()
    {
        logInformation.unfinishedRounds++;
        logInformation.roindIsComplete = 0;
        logInformation.playedTimeSinceStart = (int) Time.realtimeSinceStartup;
        logInformation.secondsForThisRound = (int) Time.realtimeSinceStartup - TimeRoundWasStarted;
        logInformation.endTime = System.DateTime.UtcNow.ToLocalTime().ToString("dd.MM.yyyy HH:mm");
        saveUserData.addDataToFile(playerName);
    }

    public void DisplayTrivia()
    {
        CompleteGameCanvas.transform.Find("DisplayTriviaText").GetComponent<TMPro.TextMeshProUGUI>().text = triviaCollection.GetTriviaNotShownYet(triviaCollection.getTriviaBasedOnObjectName(selectObjects.SelectedObjectNames[0]));
        CompleteGameCanvas.transform.Find("DisplayTriviaTexture").GetComponent<RawImage>().texture = GetRenderTextureByName(selectObjects.SelectedObjectNames[0]);

        //list.Where(obj => obj.name == "Sword").SingleOrDefault();



    }

    public Texture2D GetRenderTextureByName(string textureName)
    {
        foreach (Texture2D texture in selectObjects.RenderTextureList)
        {
            if (texture.name.Equals(textureName))
            {
                return texture;
            }
        }
        print("could not find correct texture");
        return selectObjects.RenderTextureList[0];
    }
}
