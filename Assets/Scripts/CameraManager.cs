using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

public class CameraManager : MonoBehaviour
{
    #region Fields
	[Header( "Event Listeners" )]
	public EventListenerDelegateResponse levelRevealedListener;

	// Private Fields
	private UnityMethod update;
	#endregion

	#region UnityAPI
	private void OnEnable()
    {
		levelRevealedListener.OnEnable();
	}

    private void OnDisable()
    {
		levelRevealedListener.OnDisable();
	}

    private void Awake()
    {
		levelRevealedListener.response = LevelRevealedResponse;
		update = ExtensionMethods.EmptyMethod;
	}

    private void Update()
    {
		update();
	}

    #endregion

    #region Implementation
    void LevelRevealedResponse()
    {
		update = Rotate;
	}

    void Rotate()
    {
		transform.Rotate( GameSettings.Instance.camera_RotationVector );
	}
    #endregion

}
