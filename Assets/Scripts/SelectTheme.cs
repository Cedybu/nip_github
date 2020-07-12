using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectTheme : MonoBehaviour
{
    List<string> themeObjectNameList = new List<string>();
    private Vector3 scaleChange;
    public GameObject liveObject;
    public MenuController menuController;

    private bool themeIsSelected = false;
    public Button acceptThemeSelectionButton;
    public List<GameObject> toggleList = new List<GameObject>();
    private List<GameObject> spawnableObjectList = new List<GameObject>();

    private List<List<string>> themeList = new List<List<string>>();

    private int toggleLastSelectedInt = -1;



    private void Awake()
    {
        /*
        themeObjectNameList.Add("plant");
        themeObjectNameList.Add("Phone");
        themeObjectNameList.Add("cottage");
        themeObjectNameList.Add("modernChair");
        themeObjectNameList.Add("plant");
        themeObjectNameList.Add("Cube");
        themeObjectNameList.Add("EiffelTower");
        themeObjectNameList.Add("cottage");

        */

        fillThemeList();

    }

    public List<string> ThemeObjectNameList
    {
        get { return this.themeObjectNameList; }
        set { this.themeObjectNameList = value; }
    }

    public List<GameObject> SpawnableObjectList
    {
        get { return this.spawnableObjectList; }
        set { this.spawnableObjectList = value; }
    }

    public void fillThemeList()
    {
        for (int i = 0; i < 6; i++) // (6 is fix max number of selectable objects
        {
            List<string> theme1NameList = new List<string>();
            List<string> theme2NameList = new List<string>();
            List<string> theme3NameList = new List<string>();
            List<string> theme4NameList = new List<string>();
            List<string> theme5NameList = new List<string>();
            List<string> theme6NameList = new List<string>();
            //List<string> theme7NameList = new List<string>();
            //List<string> theme8NameList = new List<string>();

            theme1NameList.Add("Eiffelturm");
            theme1NameList.Add("frenchberet");
            theme1NameList.Add("louvre");
            theme1NameList.Add("croissant");
            theme1NameList.Add("Notre Dame");
            theme1NameList.Add("Arc de Triomphe");

            theme2NameList.Add("Phone");
            theme2NameList.Add("Phone");
            theme2NameList.Add("Phone");
            theme2NameList.Add("Phone");
            theme2NameList.Add("Phone");
            theme2NameList.Add("Phone");

            theme3NameList.Add("cottage");
            theme3NameList.Add("cottage");
            theme3NameList.Add("cottage");
            theme3NameList.Add("cottage");
            theme3NameList.Add("cottage");
            theme3NameList.Add("cottage");

            theme4NameList.Add("plant");
            theme4NameList.Add("Phone");
            theme4NameList.Add("cottage");
            theme4NameList.Add("modernChair");
            theme4NameList.Add("plant");
            theme4NameList.Add("Cube");

            theme5NameList.Add("plant");
            theme5NameList.Add("Phone");
            theme5NameList.Add("cottage");
            theme5NameList.Add("modernChair");
            theme5NameList.Add("plant");
            theme5NameList.Add("Cube");

            theme6NameList.Add("fuji");
            theme6NameList.Add("CherryBlossom");
            theme6NameList.Add("cottage");
            theme6NameList.Add("modernChair");
            theme6NameList.Add("plant");
            theme6NameList.Add("Cube");

            /*

            theme7NameList.Add("plant");
            theme7NameList.Add("Phone");
            theme7NameList.Add("cottage");
            theme7NameList.Add("modernChair");
            theme7NameList.Add("plant");
            theme7NameList.Add("Cube");
            theme7NameList.Add("EiffelTower");
            theme7NameList.Add("cottage");

            theme8NameList.Add("Cube");
            theme8NameList.Add("Cube");
            theme8NameList.Add("Cube");
            theme8NameList.Add("Cube");
            theme8NameList.Add("Cube");
            theme8NameList.Add("Cube");
            theme8NameList.Add("Cube");
            theme8NameList.Add("Cube");

    */

            themeList.Add(theme1NameList);
            themeList.Add(theme2NameList);
            themeList.Add(theme3NameList);
            themeList.Add(theme4NameList);
            themeList.Add(theme5NameList);
            themeList.Add(theme6NameList);
            //themeList.Add(theme7NameList);
            //themeList.Add(theme8NameList);

        }

    }

    public void selectionToggle(int index)
    {
        Toggle toggle = toggleList[index].GetComponent<Toggle>();
        Toggle toggleLastSelected;

        themeObjectNameList.Clear();

        
        if (!toggle.isOn)
        {
            themeIsSelected = false;
            //TODO empty listselection

            var colors = toggle.colors;
            colors.normalColor = Color.white;
            toggle.colors = colors;

            toggleLastSelectedInt = -1;
        }
        

        if (toggle.isOn)
        {
            themeIsSelected = true;

            foreach(string objectName in themeList[index])
            {
                themeObjectNameList.Add(objectName);
            }
            //themeObjectNameList = themeList[index];

            var colors = toggle.colors;
            colors.normalColor = Color.green;
            toggle.colors = colors;


            if (toggleLastSelectedInt >= 0)
            {
                toggleLastSelected = toggleList[toggleLastSelectedInt].GetComponent<Toggle>();

                var colorsLastSelected = toggleLastSelected.colors;
                colorsLastSelected.normalColor = Color.white;
                toggleLastSelected.colors = colorsLastSelected;

                Mute(toggleLastSelected.onValueChanged);
                toggleLastSelected.isOn = false;
                Unmute(toggleLastSelected.onValueChanged);
            }

            toggleLastSelectedInt = index;
        }
        checkAcceptSelection();
    }

    public void fillSpawnableObjectList(List<string> objectNames)
    {
        int i = 0;
        foreach (string objectname in objectNames)
        {
            GameObject emptyGameObject = Instantiate(liveObject, transform.position, transform.rotation);

            emptyGameObject.GetComponent<SpawnableObject>().setModelName(objectname);

            emptyGameObject.name = objectname;

            scaleChange = new Vector3(-0.9f, -0.9f, -0.9f);
            emptyGameObject.transform.localScale += scaleChange;



            GameObject go = Instantiate(Resources.Load("Models/" + objectname), transform.position, transform.rotation) as GameObject; 

            go.transform.localScale += scaleChange;

            go.transform.parent = emptyGameObject.transform;
            emptyGameObject.transform.position = new Vector3(1000 + i, 1000, 1000);
            SpawnableObjectList.Add(emptyGameObject);

            i++;
        }
    }

    public void checkAcceptSelection()
    {
        if (themeIsSelected)
        {
            acceptThemeSelectionButton.interactable = true;
        }
        else
        {
            acceptThemeSelectionButton.interactable = false;
        }
    }

    public void deactivateAllUnselectedToggles()
    {
        for (int i = 0; i < 6; i++) // (6 is fix max number of selectable objects
        {
            foreach(GameObject toggle in toggleList)
            {
                if(toggle.GetComponent<Toggle>().isOn == false)
                {
                    toggle.GetComponent<Toggle>().interactable = false;
                }
            }
        }
    }

    public void activateAllToggles()
    {
        foreach (GameObject toggle in toggleList)
        {
            toggle.GetComponent<Toggle>().interactable = true;
        }
    }

    public void acceptSelection()
    {
        fillSpawnableObjectList(themeObjectNameList);

        menuController.CloseThemeSelection();
        //resetThemeSelectionInterface();

    }


    public void resetThemeSelectionInterface()
    {
        resetToggleColour();
        resetAllToggles();
        acceptThemeSelectionButton.interactable = false;
    }

    public void resetThemeSelection()
    {
        themeIsSelected = false;
        themeObjectNameList.Clear();
    }

    public void resetToggleColour()
    {
        foreach (GameObject toggle in toggleList)
        {
            var colors = toggle.GetComponent<Toggle>().colors;
            colors.normalColor = Color.white;
            toggle.GetComponent<Toggle>().colors = colors;
        }
    }

    public void resetAllToggles()
    {
        activateAllToggles();

        foreach (GameObject toggle in toggleList)
        {
            toggle.GetComponent<Toggle>().isOn = false;
        }
    }

    public void DestroySpawnableObjectList()
    {
        if(spawnableObjectList.Count > 0)
        {
            foreach (GameObject spawnableObject in spawnableObjectList)
            {
                Destroy(spawnableObject);
            }
        }
    }

    //https://answers.unity.com/questions/1519824/undesirable-call-of-toggles-on-value-changed-when.html
    public void Mute(UnityEngine.Events.UnityEventBase ev)
    {
        int count = ev.GetPersistentEventCount();
        for (int i = 0; i < count; i++)
        {
            ev.SetPersistentListenerState(i, UnityEngine.Events.UnityEventCallState.Off);
        }
    }

    //https://answers.unity.com/questions/1519824/undesirable-call-of-toggles-on-value-changed-when.html
    public void Unmute(UnityEngine.Events.UnityEventBase ev)
    {
        int count = ev.GetPersistentEventCount();
        for (int i = 0; i < count; i++)
        {
            ev.SetPersistentListenerState(i, UnityEngine.Events.UnityEventCallState.RuntimeOnly);
        }
    }
}
