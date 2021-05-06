using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using FFStudio;
using TMPro;

public class UIWeapon : UIEntity
{
	#region Fields
	[Header( "Event Listeners" )]
	public EventListenerDelegateResponse levelLoadedListener;
	public EventListenerDelegateResponse ammoEventListener;
	// public MultipleEventListenerDelegateResponse disactivateWeaponListener;


	[Header( "Fired Events" )]
	public GameEvent ammoDepleted;

	[Header( "UI Elements" )]
	public TextMeshProUGUI ammoCountText;
	public Image weaponImage;

	public Sprite[] weaponSprites; // 0 = disactivate, 1 = activate

	// Private Fields
	private IntGameEvent ammoEvent;
	private Button selectionButton;
	#endregion

	#region UnityAPI
	private void OnEnable()
    {
		ammoEventListener.OnEnable();
		levelLoadedListener.OnEnable();
	}

    private void OnDisable()
    {
		ammoEventListener.OnDisable();
		levelLoadedListener.OnDisable();
	}

    private void Awake()
    {
		selectionButton = GetComponent< Button >();

		ammoEventListener.response = SetAmmoCount;
		levelLoadedListener.response = () => selectionButton.interactable = true;

		ammoEvent = ammoEventListener.gameEvent as IntGameEvent;
	}
    #endregion

	#region API
	public void Activated()
	{
		weaponImage.sprite = weaponSprites[ 1 ];
	}

	public void DisActivated()
	{
		weaponImage.sprite = weaponSprites[ 0 ];
	}
	#endregion

    #region Implementation
    void SetAmmoCount()
    {
		ammoCountText.text = ammoEvent.eventValue.ToString();

        if(ammoEvent.eventValue == 0)
		{
			selectionButton.interactable = false;
			ammoDepleted.Raise();
		}

	}
    #endregion
}
