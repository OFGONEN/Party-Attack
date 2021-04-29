using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FFStudio;

public class UIManager : MonoBehaviour
{
	#region Fields

    [Header ("Event Listeners")]

    [Header ("Shared Variables")]
	public SharedFloatProperty levelProgressProperty;

	[Header( "UI Elements" )]
	public Image levelProgressImage;

	#endregion

	#region UnityAPI

	private void OnEnable()
    {
		levelProgressProperty.changeEvent += LevelProgressResponse;
	}

    private void OnDisable()
    {
		levelProgressProperty.changeEvent -= LevelProgressResponse;
	}

    private void Awake()
    {

	}
	#endregion

	#region Implementation

    void LevelProgressResponse()
    {
        // levelProgressImage.fillAmount = levelProgressProperty.sharedValue;
    }
    #endregion
}
