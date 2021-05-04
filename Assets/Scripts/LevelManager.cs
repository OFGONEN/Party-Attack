using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using NaughtyAttributes;

public class LevelManager : MonoBehaviour
{
	#region Fields
	[Header( "Event Listeners" )]
	public EventListenerDelegateResponse levelLoadedListener;
	public EventListenerDelegateResponse levelRevealedListener;
	public EventListenerDelegateResponse humanNeutralizedListener;
	public EventListenerDelegateResponse ultimateProgressListener;
	public EventListenerDelegateResponse ultimateUsedListener;

	[Header( "Level Releated" )]
	public GameEvent ultimateUnlocked;

	[Header( "Level Releated" )]
	public SharedFloatProperty levelProgress;
	public SharedFloatProperty ultimateProgress;

	[Header( "Fired Events" )]
	public GameEvent defaultWeaponActivate;
	public GameEvent deactivateAllWeapon;
	public GameEvent levelCompleted;

	// Private Variables
	GameObject currentCamera;
	int humanCount;
	int neutralizedHumanCount;
	FloatGameEvent ultimateProgressEvent;
	#endregion

	#region UnityAPI

	private void OnEnable()
    {
		levelLoadedListener     .OnEnable();
		levelRevealedListener   .OnEnable();
		ultimateUsedListener    .OnEnable();
		humanNeutralizedListener.OnEnable();
		ultimateProgressListener.OnEnable();
	}

    private void OnDisable()
    {
		levelLoadedListener     .OnDisable();
		levelRevealedListener   .OnDisable();
		ultimateUsedListener    .OnDisable();
		humanNeutralizedListener.OnDisable();
		ultimateProgressListener.OnDisable();
    }

    private void Awake()
    {
		ultimateProgressEvent = ultimateProgressListener.gameEvent as FloatGameEvent;

		levelLoadedListener.response      = LevelLoadedResponse;
		ultimateUsedListener.response 	  = UltimateUsedResponse;
		levelRevealedListener.response    = defaultWeaponActivate.Raise;
		humanNeutralizedListener.response = HumanNeutralizedResponse;
		ultimateProgressListener.response = UltimateProgressResponse;
	}

	#endregion

	#region Implementation
    void LevelLoadedResponse()
    {
		humanCount = CurrentLevelData.Instance.levelData.humanCount;
		neutralizedHumanCount = 0;
		levelProgress.SetValue( 0 );
		ultimateProgress.SetValue( 0 );

		// Spawn camera and set skybox
		RenderSettings.skybox = CurrentLevelData.Instance.levelData.skyboxMaterial;

        if(currentCamera != null)
			Destroy( currentCamera );

		currentCamera = Instantiate( CurrentLevelData.Instance.levelData.cameraPrefab, transform );
	}

    void HumanNeutralizedResponse()
    {
		neutralizedHumanCount++;
		levelProgress.SetValue( ( float )neutralizedHumanCount / humanCount  );

        if(neutralizedHumanCount == humanCount)
		{
			levelCompleted.Raise();
			deactivateAllWeapon.Raise();
		}
	}

	void UltimateProgressResponse()
	{
		ultimateProgress.SetValue( Mathf.Min(ultimateProgress.sharedValue + ultimateProgressEvent.eventValue, 100) );

		if(ultimateProgress.sharedValue >= 100)
		{
			ultimateUnlocked.Raise();
			ultimateProgressListener.response = ExtensionMethods.EmptyMethod;
		}
	}

	void UltimateUsedResponse()
	{
		ultimateProgress.sharedValue = 0;
		ultimateProgressListener.response = UltimateProgressResponse;
	}

	#endregion
}
