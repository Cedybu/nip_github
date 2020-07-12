using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class PlacementIndicatorScript : MonoBehaviour
{
    private ARRaycastManager arRaycastManager;
    private GameObject visualIndicator;
    public MenuController menuController;
    public GameController gameController;

    private void Start()
    {
        //get components
        arRaycastManager = FindObjectOfType<ARRaycastManager>();
        visualIndicator = transform.GetChild(0).gameObject;

        //hide 

        visualIndicator.SetActive(false);
    }

    private void Update()
    {

        if (gameController.ArComponentsAreActive)
        {
            //shoot a raycast from center of screen
            List<ARRaycastHit> hits = new List<ARRaycastHit>();

            arRaycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), hits, TrackableType.PlaneWithinPolygon);

            //if we hit an Ar plane, update the position and rotation
            if (hits.Count > 0)
            {
                transform.position = hits[0].pose.position;
                transform.rotation = hits[0].pose.rotation;

                if (!visualIndicator.activeInHierarchy)
                {
                    visualIndicator.SetActive(true);

                    Invoke("disableSearchPlaneInstructions", 4f);
                }
            }
            else
            {
                visualIndicator.SetActive(false);
            }
        }

        
    }

    public void disableSearchPlaneInstructions()
    {
        menuController.setSearchPlaneInstructionsState(false);
    }
}
