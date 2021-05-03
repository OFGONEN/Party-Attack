using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FFStudio;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
	#region Fields

	[Header( "Event Listeners" )]
	public EventListenerDelegateResponse levelLoadedResponse;

	[Header ("Shared Variables")]
	public SharedFloatProperty levelLoadingProgressProperty;

	[Header( "UI Elements" )]
	public Image loadingScreenImage;
	public UIImage levelLoadingProgressImage;

	#endregion

	#region UnityAPI

	private void OnEnable()
    {
		levelLoadingProgressProperty.changeEvent += LevelProgressResponse;
		levelLoadedResponse.OnEnable();
	}

    private void OnDisable()
    {
		levelLoadingProgressProperty.changeEvent -= LevelProgressResponse;
		levelLoadedResponse.OnDisable();
	}

    private void Awake()
    {
		levelLoadedResponse.response = LevelLoadedResponse;
	}
	#endregion

	#region Implementation

    void LevelProgressResponse()
    {
        levelLoadingProgressImage.imageRenderer.fillAmount = levelLoadingProgressProperty.sharedValue;
    }

	void LevelLoadedResponse()
	{
		var sequance = DOTween.Sequence();

		sequance.Append( levelLoadingProgressImage.GoPopIn() );
		sequance.Append( loadingScreenImage.DOFade( 0, GameSettings.Instance.uiEntityMoveTweenDuration ) );
	}
    #endregion
}
