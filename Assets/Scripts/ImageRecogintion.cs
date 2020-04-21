using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.ARFoundation;

public class ImageRecogintion : MonoBehaviour
{
    private ARTrackedImageManager arTrackedImageManager;

    void OnEnable()
    {
        arTrackedImageManager.trackedImagesChanged += OnImageChange;
    }

    void OnDisable()
    {
        arTrackedImageManager.trackedImagesChanged -= OnImageChange;
    }

    void Awake()
    {
        arTrackedImageManager = FindObjectOfType<ARTrackedImageManager>();
    }

    void Update()
    {
        
    }

    public void OnImageChange(ARTrackedImagesChangedEventArgs args)
    {
        foreach (var trackedImage in args.added)
        {
            Debug.Log("trackedImage.name: " + trackedImage.name);
            
        }
    }
}
