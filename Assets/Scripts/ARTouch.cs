using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[RequireComponent(typeof(ARRaycastManager))]
public class ARTouch : MonoBehaviour
{
    [Header("Tap Settings:")]
    [SerializeField] private GameObject gameObjectToInstantiate;
    [SerializeField] private GameObject placementIndicator;
    [Range(0, 0.5f)] [SerializeField] private float detectableDistance = 0.5f;

    [Header("Debug Settings:")]
    [SerializeField] private bool debugComponent = false;

    private GameObject selectedGameObject;
    private ARRaycastManager arRaycastManager;
    private static List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private List<GameObject> existingGameObjects = new List<GameObject>();
    private bool isTouching = false;
    private TouchToolMode touchToolMode = TouchToolMode.NONE;

    private enum TouchToolMode
    {
        NONE = 0,
        POSITION = 1,
        ROTATE = 2,
        SCALE = 3,
        ADD = 4,
        REMOVE = 5
    };

    // Start is called before the first frame update
    void Start()
    {
        arRaycastManager = GetComponent<ARRaycastManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;

        if (arRaycastManager.Raycast(touchPosition, hits, TrackableType.PlaneWithinPolygon))
        {
            var hitPose = hits[0].pose;

            if (!isTouching)
            {
                SelectGameObject(hitPose);

                switch (touchToolMode)
                {
                    case TouchToolMode.NONE:
                        break;
                    case TouchToolMode.ADD:
                        AddTool(hitPose);
                        break;
                    case TouchToolMode.REMOVE:
                        RemoveTool(selectedGameObject);
                        break;
                }

                isTouching = true;
            }

            switch (touchToolMode)
            {
                case TouchToolMode.NONE:
                    break;
                case TouchToolMode.POSITION:
                    PositioningTool(hitPose);
                    break;
                case TouchToolMode.ROTATE:
                    RotatingTool();
                    break;
                case TouchToolMode.SCALE:
                    ScaleTool();
                    break;
            }
        }
    }

    void PositioningTool(Pose pose)
    {
        if (selectedGameObject)
        {
            selectedGameObject.transform.position = pose.position;
        }
    }

    void RotatingTool()
    {

    }

    void ScaleTool()
    {

    }

    void AddTool(Pose pose)
    {
        GameObject spawned = Instantiate(gameObjectToInstantiate, pose.position, pose.rotation);
        existingGameObjects.Add(spawned);

        if (debugComponent)
            Debug.Log("Added: " + spawned);
    }

    void RemoveTool(GameObject selectedGameObject)
    {
        if (debugComponent)
            Debug.Log("Removed: " + selectedGameObject);

        Destroy(selectedGameObject);
    }

    public void SwitchTools(int toolState)
    {
        touchToolMode = (TouchToolMode)toolState;

        if (debugComponent)
            Debug.Log("touchToolMode: " + touchToolMode);
    }

    public void RemoveAllTool()
    {
        var allGameObjects = FindObjectsOfType<GameObject>();

        for (int i = 0; i < allGameObjects.Length; i++)
        {
            if (debugComponent)
                Debug.Log("allGameObjects[i]: " + allGameObjects[i].name);

            if (allGameObjects[i].name.Contains("Clone"))
                Destroy(allGameObjects[i]);

        }
    }

    void SelectGameObject(Pose pose)
    {
        List<GameObject> objectsInRange = new List<GameObject>();
        List<float> recordedDistances = new List<float>();

        if (existingGameObjects.Count > 0)
        {
            for (int i = 0; i < existingGameObjects.Count; i++)
            {
                if (Vector3.Distance(pose.position, existingGameObjects[i].transform.position) < detectableDistance)
                {
                    objectsInRange.Add(existingGameObjects[i]);
                    recordedDistances.Add(Vector3.Distance(pose.position, existingGameObjects[i].transform.position));
                }
            }
        }

        if (objectsInRange.Count > 0)
        {
            float smallestDistance = detectableDistance;

            for (int i = 0; i < recordedDistances.Count; i++)
            {
                if (recordedDistances[i] <= smallestDistance)
                {
                    smallestDistance = recordedDistances[i];
                    selectedGameObject = existingGameObjects[i];
                }
            }
        }
    }

    bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }

        touchPosition = default;
        isTouching = false;
        selectedGameObject = null;
        return false;
    }
}
