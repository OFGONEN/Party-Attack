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
		ultimateUsedListener    .OnEnable();
		humanNeutralizedListener.OnEnable();
		ultimateProgressListener.OnEnable();
	}

    private void OnDisable()
    {
		levelLoadedListener     .OnDisable();
		ultimateUsedListener    .OnDisable();
		humanNeutralizedListener.OnDisable();
		ultimateProgressListener.OnDisable();
    }

    private void Awake()
    {
		ultimateProgressEvent = ultimateProgressListener.gameEvent as FloatGameEvent;

		levelLoadedListener.response      = LevelLoadedResponse;
		ultimateUsedListener.response 	  = UltimateUsedResponse;
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

		defaultWeaponActivate.Raise();
	}

    void HumanNeutralizedResponse()
    {
		neutralizedHumanCount++;
		levelProgress.SetValue( ( float )neutralizedHumanCount / humanCount  );

        if(neutralizedHumanCount == humanCount)
            FFLogger.Log( "Level Finished" );
	}

	void UltimateProgressResponse()
	{
		ultimateProgress.SetValue( ultimateProgress.sharedValue + ultimateProgressEvent.eventValue );

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
