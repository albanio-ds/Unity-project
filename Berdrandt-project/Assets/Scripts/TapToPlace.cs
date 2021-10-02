using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.Experimental.XR;
using System;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.Events;

public class TapToPlace : MonoBehaviour
{
    #region Attributes
    public GameObject objectToPlace;
    public GameObject objectToGet;
    public GameObject placementIndicator;
    private ARSessionOrigin arOrigin;
    private Pose placementPose;
    private ARRaycastManager arraycastManager;
    private bool placementPoseIsValid = false;

    private float startTime;
    private UnityEngine.UI.RawImage rawImage;
    public UnityEngine.UI.Button startButton;
    private int started = 0;
    public Canvas canvas;
    public UnityEngine.UI.Text chrono;
    public UnityEngine.UI.Text maxScore;
    public UnityEngine.UI.Text currScore;
    public UnityEngine.UI.Text points;
    public UnityEngine.UI.Text nbObjectsToTake;
    public UnityEngine.UI.Text scoreEvent;
    public UnityEngine.UI.Button moveObject;
    public UnityEngine.UI.Text moveObjectText;
    public UnityEngine.UI.Image backgroundScore;
    public UnityEngine.UI.Image darkImage;
    public UnityEngine.UI.Image helpImage;

    private int maxSpeed = 0;
    private int nbPoints;
    private bool objectMove;
    private static int limitPos = 100;
    private List<ARRaycastHit> avalaiblePos = new List<ARRaycastHit>();
    private float maxSize = 5f;
    #endregion

    void Start()
    {
        rawImage = FindObjectOfType<UnityEngine.UI.RawImage>();
        startButton.onClick.AddListener(startClicked);
    }

    private IEnumerator startFunction()
    {
        started = 2;
        for (int i=0; i< 50; i++)
        {
            darkImage.color = new Color(0,0,0,i/50f); ;
            yield return new WaitForSeconds(0.001f);
        }
        rawImage.gameObject.SetActive(false);
        for (int i = 50; i >= 0; i--)
        {
            darkImage.color = new Color(0, 0, 0, i / 50f); ;
            yield return new WaitForSeconds(0.001f);
        }
        darkImage.enabled = false;
        moveObject.gameObject.SetActive(true);
        backgroundScore.gameObject.SetActive(true);
        arOrigin = FindObjectOfType<ARSessionOrigin>();
        arraycastManager = FindObjectOfType<ARRaycastManager>();
        nbPoints = 0;
        moveObject.onClick.AddListener(moveObjectClicked);
        objectMove = false;
        moveObjectText.GetComponent<UnityEngine.UI.Text>().color = Color.red;
        startTime = Time.time;
        objectToPlace.GetComponentInChildren<Rigidbody>().maxAngularVelocity = 10000;
        if (PlayerPrefs.HasKey("MaxSpeed"))
        {
            maxSpeed = PlayerPrefs.GetInt("MaxSpeed");
        }
        maxScore.text = "Record : " +maxSpeed + " rad/s";
        currScore.text = "Speed : 0 rad/s";
        objectMove = !objectMove;
        helpImage.gameObject.SetActive(!helpImage.gameObject.active);
        started = 3;
    }

    
    private void startClicked()
    {
        startButton.enabled = false;
        started = 1;
    }
    private void moveObjectClicked()
    {
        
        objectMove = !objectMove;


    }

    [Obsolete]
    void Update()
    {
        if (started == 0 || started == 2 || started == 4) return;
        if (started == 1)
        {
            //startFunction();
            StartCoroutine(startFunction());
            return;
        }
        started = 4;
        if (avalaiblePos.Count > 0 && UnityEngine.Random.Range(0, 25) == 1 ) PlaceObjectsToGet();
        UpdateCanvas();
        UpdatePlacementPose();
        UpdatePlacementIndicator();

        if (placementPoseIsValid && objectMove ) PlaceObject();

        if (avalaiblePos.Count == 0)
        {
            UpdateListPositionValid();
        } else
        {
            if (avalaiblePos.Count < 50)
            {
                if (UnityEngine.Random.Range(0, 2) == 0) UpdateListPositionValid();
            }
            else
            {
                if (UnityEngine.Random.Range(0, 3) == 0) UpdateListPositionValid();
            }
        }
        var touched = Input.GetTouch(0);
        if (Input.touchCount > 0 && touched.phase == TouchPhase.Began && touched.position.y > 170 )
        {
            helpImage.gameObject.SetActive(!helpImage.gameObject.active);
        }
        started = 3;
    }
    private void LateUpdate()
    {
        if (started == 4) started = 3;
    }
    private void PlaceObjectsToGet()
    {
        var listValids = GameObject.FindGameObjectsWithTag("Scored");
        if (listValids.Length < 10)
        {
            int indice = UnityEngine.Random.Range(0, avalaiblePos.Count);
            Quaternion rotationRandom = new Quaternion(0,0,0,0);
            Vector3 pos = avalaiblePos[indice].pose.position;
            foreach (GameObject obj in listValids)
            {
                if ( (new Vector3(obj.transform.position.x, 0, obj.transform.position.z)+ new Vector3(pos.x, 0, pos.z)).magnitude < 6) return;
            }
            
            if ((new Vector3(Camera.current.transform.position.x, 0, Camera.current.transform.position.z) + new Vector3(pos.x, 0, pos.z)).magnitude < 2) return;

            GameObject clone = Instantiate(objectToGet);
            Destroy(clone.gameObject, 10f);
            //clone.transform.position = pos;
            clone.transform.SetPositionAndRotation(pos, rotationRandom);
            avalaiblePos.RemoveAt(indice);
        }
    }

    public void testMessage(int msg)
    {
        var e = Instantiate(scoreEvent, canvas.transform);
        Destroy(e, 1f);
        Vector3 translationVar = new Vector3(UnityEngine.Random.Range(100, 400), UnityEngine.Random.Range(100, 400), 0);
        e.GetComponent<UnityEngine.UI.Text>().rectTransform.Translate(translationVar);
        e.GetComponent<UnityEngine.UI.Text>().text = "m : " + msg;
        e.enabled = true;
    }

    public void addPoints(string test, int pointsToAdd = 1)
    {
        nbPoints = nbPoints + pointsToAdd;

        var e = Instantiate(scoreEvent, canvas.transform);

        Vector3 translationVar = new Vector3(UnityEngine.Random.Range(-200, 200), UnityEngine.Random.Range(-200, 200), 0);
        e.GetComponent<UnityEngine.UI.Text>().rectTransform.Translate(translationVar);
        e.GetComponent<UnityEngine.UI.Text>().text = "+1";
        e.enabled = true;
        //yield return new WaitForSeconds(3);
        Destroy(e, 1f);
        
        //var currSize = objectToPlace.transform.localScale;
        //if (currSize.x < maxSize) objectToPlace.transform.localScale.Set( currSize.x + 0.1f, currSize.y + 0.1f, currSize.z + 0.1f);
        objectToPlace.GetComponentInChildren<Rigidbody>().AddTorque(Vector3.one * 100);
        
        //testMessage(((int)objectToPlace.GetComponentInChildren<Rigidbody>().angularVelocity.y));
        //objectToPlace.GetComponent<Rigidbody>().AddForce(new Vector3(0, 1, 0), ForceMode.Impulse);
        // StartCoroutine(CoroutineAddPoints(test));
    }

   /* private IEnumerator CoroutineAddPoints(string test)
    {
        
    }*/

    private void UpdateCanvas()
    {
       
        var newTime = Time.time - startTime;
        string res = "";
        if (newTime > 3600)
        {
            res = (int)(newTime / 3600) + "h ";
            newTime = newTime % 3600;
        }
        if (newTime > 60)
        {
            res = res + (int)(newTime/60) + "min ";
            newTime = newTime % 60;
        }
        if (newTime > 0)
        {
            //Math.Round(newTime, 0);
            newTime = Mathf.Round(newTime * 10f) / 10f;
            res = res + newTime + "s";
        }

        chrono.GetComponent<UnityEngine.UI.Text>().text = "chrono : " + res ;
        points.GetComponent<UnityEngine.UI.Text>().text = "score : " + nbPoints;
        nbObjectsToTake.GetComponent<UnityEngine.UI.Text>().text = "Objets visibles : " + GameObject.FindGameObjectsWithTag("Scored").Length;
        //Mathf.Round(score * 100f) / 100f
        if (objectMove)
        {
            moveObjectText.GetComponent<UnityEngine.UI.Text>().color = Color.green;
        }
        else
        {
            moveObjectText.GetComponent<UnityEngine.UI.Text>().color = Color.red;
        }
        if (!placementPoseIsValid) moveObjectText.GetComponent<UnityEngine.UI.Text>().color = Color.gray;
        int currSpeed = (int)(objectToPlace.GetComponentInChildren<Rigidbody>().angularVelocity.y);
        if (currSpeed > maxSpeed)
        {
            maxSpeed = currSpeed;
            PlayerPrefs.SetInt("MaxSpeed", maxSpeed);
            maxScore.text = "Record : " + maxSpeed + " rad/s";
        }
        currScore.text = "Speed : " + currSpeed + " rad/s";
    }

    [Obsolete]
    private void PlaceObject()
    {
        if (!objectToPlace.active) objectToPlace.SetActive(true);
        objectToPlace.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
    }

    private void UpdatePlacementIndicator()
    {
        if (placementPoseIsValid)
        {
           // placementIndicator.SetActive(true);
            placementIndicator.transform.SetPositionAndRotation(placementPose.position, placementPose.rotation);
        }
        /* else
        {
            placementIndicator.SetActive(false);
        }*/
    }

    private void UpdatePlacementPose()
    {
        var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));
        var hits = new List<ARRaycastHit>();
        arraycastManager.Raycast(screenCenter, hits, TrackableType.Planes);
        placementPoseIsValid = hits.Count > 0;
        if (placementPoseIsValid)
        {
            placementPose = hits[0].pose;
            
            var cameraForward = Camera.current.transform.forward;
            var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
            placementPose.rotation = Quaternion.LookRotation(cameraBearing);
            
        }
    }

    private void UpdateListPositionValid()
    {

        for (int i = UnityEngine.Random.Range(0, 11); i<=10; i++)
        {
            var screenCenter = Camera.current.ViewportToScreenPoint(new Vector3(i/10f, i/10f));
            List<ARRaycastHit> hits = new List<ARRaycastHit>();
            arraycastManager.Raycast(screenCenter, hits, TrackableType.Planes);
            int nbHits = hits.Count;
            if (hits.Count > 0)
            {
                int taille = avalaiblePos.Count;
                if (taille + nbHits < limitPos) avalaiblePos.Add(hits[0]);
                else
                {/*
                    for (int j=taille; j<limitPos; j++)
                    {
                        avalaiblePos[j] = hits[j - taille];
                    }
                    for (int j=0; j<limitPos - taille; j++)
                    {
                        avalaiblePos[j] = hits[j + (limitPos-taille)];
                    }*/
                    for (int j = 0; j < nbHits; j++)
                    {
                        if (UnityEngine.Random.Range(0, 2) == 0)
                        {
                            avalaiblePos[UnityEngine.Random.Range(0, taille)] = hits[j];
                        }
                    }
                }
            }
        }
    }

}
