using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public bool debugComponent = false;

    void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ClearTouchInstantiatedObjects()
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

    public void QuitApplication()
    {
        Application.Quit();
    }
}
