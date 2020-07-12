using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class SelectObjects : MonoBehaviour
{
    public SelectTheme selectTheme;
    private List<string> selectedObjectNames = new List<string>();
    private List<GameObject> selectedSpawnableObjects = new List<GameObject>();

    private int numberOfObjectsSelected = 0;



    public Camera renderCamera;
    public MenuController menuController;
    public GameController gameController;
    public List<GameObject> toggleList = new List<GameObject>();
    public ObjectSpawner objectSpawner;
    public Button acceptSelectionButton;

    private SortedSet<int> objectsAsNumbers= new SortedSet<int>();

    private List<Texture2D> renderTextureList = new List<Texture2D>();

    public List<Texture2D> RenderTextureList
    {
        get { return this.renderTextureList; }
        set { this.renderTextureList = value; }
    }

    /*
    public void addObjectToList(int index)
    {
        selectedObjectNames.Add(selectTheme.ThemeObjectNameList[index]);
        selectedSpawnableObjects.Add(selectTheme.SpawnableObjectList[index]);
        numberOfObjectsSelected++;

        if (numberOfObjectsSelected >= objectSpawner.NumberOfObjcectsToSpawn)
        {
            menuController.CloseObjectSelection();
        }
    }
    */

    public void selectionToggle(int index)
    {
        Toggle toggle = toggleList[index].GetComponent<Toggle>();

        if (toggle.isOn)
        {
            numberOfObjectsSelected++;
            objectsAsNumbers.Add(index);

            var colors = toggle.colors;
            colors.normalColor = Color.green;
            toggle.colors = colors;
        }

        if (!toggle.isOn)
        {
            numberOfObjectsSelected--;
            objectsAsNumbers.Remove(index);

            var colors = toggle.colors;
            colors.normalColor = Color.white;
            toggle.colors = colors;
        }
        checkAcceptSelection();

    
    }



    Texture2D SnapshotImage(Camera cam)
    {
        //https://answers.unity.com/questions/1197854/1-camera-with-multiple-render-textures.html
        // The camera must has a renderTexture target
        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = cam.targetTexture;
        cam.Render();
        Texture2D image = new Texture2D(cam.targetTexture.width, cam.targetTexture.height);
        image.ReadPixels(new Rect(0, 0, cam.targetTexture.width, cam.targetTexture.height), 0, 0);
        image.Apply();
        RenderTexture.active = currentRT;
        return image;
    }

    public void generateObjectTextures()
    {
        //camera position because of camera tilts
        float positionOfCamera = 998.95f;

        renderCamera.transform.position = new Vector3(positionOfCamera, 1001.15f, 995);

        int counter = 0;
        foreach (GameObject button in toggleList)
        {
            Texture2D renderTexture = SnapshotImage(renderCamera);
            renderTexture.name = selectTheme.ThemeObjectNameList[counter];
            button.GetComponentInChildren<RawImage>().texture = renderTexture;
            button.GetComponentInChildren<Text>().text = selectTheme.ThemeObjectNameList[counter];
            renderTextureList.Add(renderTexture);
            positionOfCamera++;
            renderCamera.transform.position = new Vector3(positionOfCamera, 1001.15f, 995);
            counter++;
        }
    }

    public void checkAcceptSelection()
    {
        print("checkAcceptSelection" + objectSpawner.NumberOfObjectsToSpawn);
        if (numberOfObjectsSelected >= objectSpawner.NumberOfObjectsToSpawn)
        {
            acceptSelectionButton.interactable = true;
            deactivateAllUnselectedToggles();

        }
        else
        {
            acceptSelectionButton.interactable = false;
            activateAllToggles();
        }
    }

    public void acceptSelection()
    {
        foreach (int index in objectsAsNumbers)
        {
            selectedObjectNames.Add(selectTheme.ThemeObjectNameList[index]);
            //selectedSpawnableObjects.Add(selectTheme.SpawnableObjectList[index]);
        }
        
        menuController.CloseObjectSelection();
        resetObjectSelectionInterface();
    }

    public void resetToggleColour()
    {
        foreach(GameObject toggle in toggleList)
        {
            var colors = toggle.GetComponent<Toggle>().colors;
            colors.normalColor = Color.white;
            toggle.GetComponent<Toggle>().colors = colors;
        }
    }

    public void deactivateAllUnselectedToggles()
    {
        for(int i = 0; i < 6; i++) // (6 is fix max number of selectable objects
        {
            if (!objectsAsNumbers.Contains(i))
            {
                toggleList[i].GetComponent<Toggle>().interactable = false;
            }
        }
    }

    public void activateAllToggles()
    {
        foreach(GameObject toggle in toggleList)
        {
            toggle.GetComponent<Toggle>().interactable = true;
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

    public List<GameObject> SelectedSpawnableObjects
    {
        get { return this.selectedSpawnableObjects; }
        set { this.selectedSpawnableObjects = value; }
    }
    

    public List<string> SelectedObjectNames
    {
        get { return this.selectedObjectNames; }
        set { this.selectedObjectNames = value; }
    }

    public void resetObjectSelectionInterface()
    {
        numberOfObjectsSelected = 0;
        resetToggleColour();
        resetAllToggles();
        objectsAsNumbers.Clear();
        acceptSelectionButton.interactable = false;
    }

    public void resetObjectSelection()
    {
        selectedObjectNames.Clear();
        selectedSpawnableObjects.Clear();
    }


}
