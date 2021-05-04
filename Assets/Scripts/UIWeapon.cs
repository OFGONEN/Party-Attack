using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using TMPro;

public class UIWeapon : UIEntity
{
	#region Fields
	[Header( "Event Listeners" )]
	public EventListenerDelegateResponse ammoEventListener;
	// public MultipleEventListenerDelegateResponse disactivateWeaponListener;

	[Header( "Fired Events" )]
	public GameEvent activateWeapon;

	[Header( "UI Elements" )]
	public TextMeshProUGUI ammoCountText;

	// Private Fields
	private IntGameEvent ammoEvent;
	#endregion

	#region UnityAPI
	private void OnEnable()
    {
		ammoEventListener.OnEnable();
	}

    private void OnDisable()
    {
		ammoEventListener.OnDisable();
	}

    private void Awake()
    {
		ammoEventListener.response = SetAmmoCount;

		ammoEvent = ammoEventListener.gameEvent as IntGameEvent;
	}
    #endregion

    #region Implementation
    void SetAmmoCount()
    {
		ammoCountText.text = ammoEvent.eventValue.ToString();
	}
        
    #endregion
}
