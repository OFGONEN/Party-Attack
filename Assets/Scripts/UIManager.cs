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
	public EventListenerDelegateResponse levelCompleteResponse;
	public EventListenerDelegateResponse tapInputListener;

	[Header ("Shared Variables")]
	public SharedFloatProperty levelLoadingProgressProperty;
	public SharedFloatProperty levelProgressProperty;

	[Header( "UI Elements" )]
	public UIText informationText;
	public Image loadingScreenImage;
	public UIImage levelLoadingProgressImage;
	public UIText levelCountText;
	public UIImage levelProgressImage;
	public Image foreGroundImage;

	public UIEntity[] weaponButtons;

	[Header( "Fired Events" )]
	public GameEvent levelRevealedEvent;


	#endregion

	#region UnityAPI

	private void OnEnable()
    {
		levelLoadingProgressProperty.changeEvent += LevelLoadingProgressResponse;
		levelProgressProperty.changeEvent        += LevelProgressResponse;

		levelLoadedResponse.OnEnable();
		levelCompleteResponse.OnEnable();
		tapInputListener.OnEnable();
	}

    private void OnDisable()
    {
		levelLoadingProgressProperty.changeEvent -= LevelLoadingProgressResponse;
		levelProgressProperty.changeEvent        -= LevelProgressResponse;

		levelLoadedResponse.OnDisable();
		levelCompleteResponse.OnDisable();
		tapInputListener.OnDisable();
	}

    private void Awake()
    {
		levelLoadedResponse.response   = LevelLoadedResponse;
		levelCompleteResponse.response = LevelCompleteResponse;
		tapInputListener.response = ExtensionMethods.EmptyMethod;

		informationText.textRenderer.text = "Tap to Shoot";
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
		sequance.AppendCallback( () => tapInputListener.response = StartLevel );

		levelCountText.textRenderer.text = "Level " + CurrentLevelData.Instance.currentConsecutiveLevel;
	}

	void LevelCompleteResponse()
	{
		var sequence = DOTween.Sequence();

		Tween tween = null;

		foreach (var item in weaponButtons)
		{
			tween = item.GoTargetPosition();
		}

		informationText.textRenderer.text = "Completed \n\n Tap to Contiune";

		sequence.Append( tween );
		sequence.Append( foreGroundImage.DOFade( 0.5f, 0.1f ) );
		sequence.Append( informationText.GoPopOut() );
		// sequence.Join( informationText.GoPopOut() );

	}

	void StartLevel()
	{
		FFLogger.Log( "Start Level" );
		foreGroundImage.DOFade( 0, 0.1f );
		informationText.GoPopIn().OnComplete( levelRevealedEvent.Raise );

		tapInputListener.response = ExtensionMethods.EmptyMethod;
	}
    #endregion
}
