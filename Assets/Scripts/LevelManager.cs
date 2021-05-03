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

	[Header( "Level Releated" )]
	public SharedFloatProperty levelProgress;
	public SharedFloatProperty ultimateProgress;

	// Private Variables
	GameObject currentCamera;
	int humanCount;
	int neutralizedHumanCount;
	#endregion

	#region UnityAPI

	private void OnEnable()
    {
		levelLoadedListener     .OnEnable();
		humanNeutralizedListener.OnEnable();
	}

    private void OnDisable()
    {
		levelLoadedListener     .OnDisable();
		humanNeutralizedListener.OnDisable();
    }

    private void Awake()
    {
		levelLoadedListener.response      = LevelLoadedResponse;
		humanNeutralizedListener.response = HumanNeutralizedResponse;
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
            FFLogger.Log( "Level Finished" );
	}

	#endregion
}
