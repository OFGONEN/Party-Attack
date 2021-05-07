using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using FFStudio;

public class Weapon : MonoBehaviour
{
	#region Fields
	[Header( "Event Listeners" )]
	public EventListenerDelegateResponse inputListener;
	public EventListenerDelegateResponse activateWeaponListener;
	public EventListenerDelegateResponse levelLoadedListener;
	public MultipleEventListenerDelegateResponse disActivateWeaponListener;

	[Header( "UI Interaction" )]
	public UnityEvent activateEvent;
	public UnityEvent disActivateEvent;


	[Header( "Shooting" )]
	public IntGameEvent fireEvent;
	public SharedReferenceProperty shootOriginReferance;
	public SharedReferenceProperty mainCameraReferance;
	public ProjectileStack projectileStack;
	public WeaponType weaponType;
	public Transform crosshair;


	// Private Fields
	private float fireRate;
	private float nextFire;
	private int ammoCount;
	private Camera mainCamera;
	private Transform shooterTransform;

	private int rayCastLayerMask;
	#endregion

	#region UnityAPI
	private void OnEnable()
    {
		inputListener            .OnEnable();
		levelLoadedListener      .OnEnable();
		activateWeaponListener   .OnEnable();
		disActivateWeaponListener.OnEnable();
	}

    private void OnDisable()
    {
		inputListener            .OnDisable();
		levelLoadedListener      .OnDisable();
		activateWeaponListener   .OnDisable();
		disActivateWeaponListener.OnDisable();
	}

    private void Awake()
    {
		activateWeaponListener.response    = ActivateWeapon;
		disActivateWeaponListener.response = DisActivateWeapon;
		levelLoadedListener.response 	   = SetWeaponInfo;
		inputListener.response 			   = ExtensionMethods.EmptyMethod;

		mainCameraReferance.changeEvent += () => mainCamera = mainCameraReferance.sharedValue as Camera;
		shootOriginReferance.changeEvent += () => shooterTransform = shootOriginReferance.sharedValue as Transform;

		CreateProjectileStack();

		rayCastLayerMask = LayerMask.GetMask("Raycast-Only", "Net" );

		crosshair.gameObject.SetActive( false );

	}
	#endregion

	#region Implementation
	void ActivateWeapon()
    {
		inputListener.response = Shoot;
		activateEvent.Invoke();

		crosshair.position = Vector3.down * 100;
		crosshair.gameObject.SetActive( true );
	}
	void DisActivateWeapon()
    {
		inputListener.response = ExtensionMethods.EmptyMethod;
		disActivateEvent.Invoke();


		crosshair.gameObject.SetActive( false );
	}
	void Shoot()
    {
        if(Time.time < nextFire || ammoCount == 0)
			return;

		ammoCount--;

		fireEvent.eventValue = ammoCount;
		fireEvent.Raise();

		RaycastHit hit;
		Ray ray = mainCamera.ScreenPointToRay( Input.mousePosition );

		if( Physics.Raycast( ray, out hit, 200, rayCastLayerMask ) )
		{
			var position = hit.point;
			position.y += 0.25f;
			crosshair.position = position;

			// get a projectile
			var projectile = GiveProjectile();

			// set position
			projectile.transform.position = shooterTransform.position;

			// set rotation
			var lookRotation = Quaternion.LookRotation( hit.point - shooterTransform.position ).eulerAngles;

			var temp = lookRotation.x;
			lookRotation.x = 0;

			projectile.transform.eulerAngles = lookRotation;

			lookRotation.x = temp;
			projectile.Fire( hit.point, lookRotation  );

			nextFire = Time.time + fireRate;
		}
	}

    void CreateProjectileStack()
    {
		projectileStack.stack = new Stack<Projectile>( projectileStack.stackSize );

        for (int i = 0; i < projectileStack.stackSize; i++)
        {
			var projectile = GameObject.Instantiate( projectileStack.prefab );
			projectile.transform.SetParent( transform );
			projectile.gameObject.SetActive( false );
		}
	}

	Projectile GiveProjectile()
	{
		Projectile projectile;

		if(projectileStack.stack.Count > 0)
			projectile = projectileStack.stack.Pop();
		else 
		{
			projectile = GameObject.Instantiate( projectileStack.prefab );
			projectile.transform.SetParent( transform );

		}

		projectile.gameObject.SetActive( true );
		return projectile;
	}

	void SetWeaponInfo()
	{
		switch (weaponType)
		{
			case WeaponType.Water: 
				fireRate = GameSettings.Instance.weapon_water_fireRate;
				ammoCount = CurrentLevelData.Instance.levelData.weapon_water_ammoCount;
				break;
			case WeaponType.Fart:  
				fireRate = GameSettings.Instance.weapon_fart_fireRate;
				ammoCount = CurrentLevelData.Instance.levelData.weapon_fart_ammoCount;
				break;
		}

		fireEvent.eventValue = ammoCount;
		fireEvent.Raise();
	}
	#endregion
}
