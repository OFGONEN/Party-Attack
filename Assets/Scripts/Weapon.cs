using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

public abstract class Weapon : MonoBehaviour
{
	#region Fields
    [Header ("Event Listeners")]
	public EventListenerDelegateResponse activateWeapon;
	public MultipleEventListenerDelegateResponse disActivateWeapon;

	[Header( "Shooting" )]
	public SharedReference shootOrigin;
	public float coolDown;

	#endregion

	#region UnityAPI
	private void OnEnable()
    {
		activateWeapon.OnEnable();
		disActivateWeapon.OnEnable();
	}

    private void OnDisable()
    {
		activateWeapon.OnDisable();
		disActivateWeapon.OnDisable();
	}

    private void Awake()
    {
		activateWeapon.response    = ActivateWeapon;
		disActivateWeapon.response = DisActivateWeapon;
	}
	#endregion

	#region Implementation
	protected abstract void ActivateWeapon();
	protected abstract void DisActivateWeapon();
	protected abstract void Shoot();
	#endregion
}
