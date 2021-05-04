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
	public SharedFloatProperty levelProgressProperty;

	[Header( "UI Elements" )]
	public Image loadingScreenImage;
	public UIImage levelLoadingProgressImage;
	public UIText levelCountText;
	public UIImage levelProgressImage;

	#endregion

	#region UnityAPI

	private void OnEnable()
    {
		levelLoadingProgressProperty.changeEvent += LevelLoadingProgressResponse;
		levelProgressProperty.changeEvent        += LevelProgressResponse;

		levelLoadedResponse.OnEnable();
	}

    private void OnDisable()
    {
		levelLoadingProgressProperty.changeEvent -= LevelLoadingProgressResponse;
		levelProgressProperty.changeEvent        -= LevelProgressResponse;

		levelLoadedResponse.OnDisable();
	}

    private void Awake()
    {
		levelLoadedResponse.response = LevelLoadedResponse;
	}
	#endregion

	#region Implementation

    void LevelLoadingProgressResponse()
    {
        levelLoadingProgressImage.imageRenderer.fillAmount = levelLoadingProgressProperty.sharedValue;
    }

	void LevelProgressResponse()
	{
		levelProgressImage.imageRenderer.fillAmount = levelProgressProperty.sharedValue;
	}

	void LevelLoadedResponse()
	{
		var sequance = DOTween.Sequence();

		sequance.Append( levelLoadingProgressImage.GoPopIn() );
		sequance.Append( loadingScreenImage.DOFade( 0, 0.1f  ) ); // GameSettings.Instance.uiEntityMoveTweenDuration

		levelCountText.textRenderer.text = "Level " + CurrentLevelData.Instance.currentConsecutiveLevel;
	}
    #endregion
}
