using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FFStudio;
using DG.Tweening;
using NaughtyAttributes;

public class UIManager : MonoBehaviour
{
	#region Fields

	[Header( "Event Listeners" )]
	public EventListenerDelegateResponse levelLoadedResponse;
	public EventListenerDelegateResponse levelCompleteResponse;
	public EventListenerDelegateResponse levelFailResponse;
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
	public GameEvent loadNewLevelEvent;
	public GameEvent resetLevelEvent;


	#endregion

	#region UnityAPI

	private void OnEnable()
    {
		levelLoadingProgressProperty.changeEvent += LevelLoadingProgressResponse;
		levelProgressProperty.changeEvent        += LevelProgressResponse;

		levelLoadedResponse  .OnEnable();
		levelFailResponse    .OnEnable();
		levelCompleteResponse.OnEnable();
		tapInputListener     .OnEnable();
	}

    private void OnDisable()
    {
		levelLoadingProgressProperty.changeEvent -= LevelLoadingProgressResponse;
		levelProgressProperty.changeEvent        -= LevelProgressResponse;

		levelLoadedResponse  .OnDisable();
		levelFailResponse    .OnDisable();
		levelCompleteResponse.OnDisable();
		tapInputListener     .OnDisable();
	}

    private void Awake()
    {
		levelLoadedResponse.response   = LevelLoadedResponse;
		levelFailResponse.response     = LevelFailResponse;
		levelCompleteResponse.response = LevelCompleteResponse;
		tapInputListener.response      = ExtensionMethods.EmptyMethod;

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

		levelLoadedResponse.response = NewLevelLoaded;
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
		sequence.AppendCallback( () => tapInputListener.response = LoadNewLevel );
		// sequence.Join( informationText.GoPopOut() );

	}

	[Button]
	void LevelFailResponse()
	{
		var sequence = DOTween.Sequence();

		Tween tween = null;

		foreach( var item in weaponButtons )
		{
			tween = item.GoTargetPosition();
		}

		informationText.textRenderer.text = "Level Failed \n\n Tap to Contiune";

		sequence.Append( tween );
		sequence.Append( foreGroundImage.DOFade( 0.5f, 0.1f ) );
		sequence.Append( informationText.GoPopOut() );
		sequence.AppendCallback( () => tapInputListener.response = LoadNewLevel );
		// sequence.Join( informationText.GoPopOut() );

	}

	void NewLevelLoaded()
	{
		levelCountText.textRenderer.text = "Level " + CurrentLevelData.Instance.currentConsecutiveLevel;

		var sequence = DOTween.Sequence();

		sequence.Append( foreGroundImage.DOFade( 0, 0.1f ) );
		sequence.Append( weaponButtons[ 0 ].GoStartPosition() );
		sequence.Join( weaponButtons[ 1 ].GoStartPosition() );
		sequence.Join( weaponButtons[ 2 ].GoStartPosition() );
		sequence.AppendCallback( levelRevealedEvent.Raise );
	}

	void StartLevel()
	{
		foreGroundImage.DOFade( 0, 0.1f );
		informationText.GoPopIn().OnComplete( levelRevealedEvent.Raise );

		tapInputListener.response = ExtensionMethods.EmptyMethod;
	}

	void LoadNewLevel()
	{
		FFLogger.Log( "Load New Level" );
		tapInputListener.response = ExtensionMethods.EmptyMethod;

		var sequence = DOTween.Sequence();

		sequence.Append( foreGroundImage.DOFade( 1f, 0.1f ) );
		sequence.Join( informationText.GoPopIn() );
		sequence.AppendCallback( loadNewLevelEvent.Raise );
	}

	void Resetlevel()
	{
		FFLogger.Log( "Load New Level" );
		tapInputListener.response = ExtensionMethods.EmptyMethod;

		var sequence = DOTween.Sequence();

		sequence.Append( foreGroundImage.DOFade( 1f, 0.1f ) );
		sequence.Join( informationText.GoPopIn() );
		sequence.AppendCallback( resetLevelEvent.Raise );

	}


    #endregion
}
