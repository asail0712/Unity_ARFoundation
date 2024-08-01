using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

// 參考資料
// https://www.youtube.com/watch?v=Fpw7V3oa4fs

public class ImageTracker : MonoBehaviour
{
    [SerializeField]private GameObject[] arPrefabArr;
    private ARTrackedImageManager trackedImageMgr;
    private List<GameObject> arObjList;

	private void Awake()
	{
		arObjList		= new List<GameObject>();
		trackedImageMgr = GetComponent<ARTrackedImageManager>();
	}

	private void OnEnable()
	{
		trackedImageMgr.trackedImagesChanged += OnTrakedImgChanged;
	}

	private void OnDisable()
	{
		trackedImageMgr.trackedImagesChanged -= OnTrakedImgChanged;
	}

	private void OnTrakedImgChanged(ARTrackedImagesChangedEventArgs eventArgs)
	{
		foreach (ARTrackedImage imgTracker in eventArgs.added)
		{
			foreach (GameObject arPrefab in arPrefabArr)
			{
				if (imgTracker.referenceImage.name == arPrefab.name)
				{
					GameObject newARPrefab = Instantiate(arPrefab, imgTracker.transform);
					arObjList.Add(newARPrefab);
				}
			}
		}

		foreach (ARTrackedImage imgTracker in eventArgs.updated)
		{
			foreach (GameObject arGO in arObjList)
			{
				if ((imgTracker.referenceImage.name + "(Clone)") == arGO.name)
				{
					arGO.SetActive(imgTracker.trackingState == TrackingState.Tracking);
				}
			}
		}
	}
}
