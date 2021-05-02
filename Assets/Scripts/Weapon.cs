using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

public class Weapon : MonoBehaviour
{
	#region Fields
	[Header( "Event Listeners" )]
	public EventListenerDelegateResponse inputListener;
	public EventListenerDelegateResponse activateWeaponListener;
	public MultipleEventListenerDelegateResponse disActivateWeaponListener;

	[Header( "Shooting" )]
	public SharedReference shootOrigin;
	public ProjectileStack projectileStack;
	public float fireRate;


	// Private Fields
	private Camera mainCamera;
	private float nextFire;
	private Transform shooterTransform;
	#endregion

	#region UnityAPI
	private void OnEnable()
    {
		inputListener.OnEnable();
		activateWeaponListener.OnEnable();
		disActivateWeaponListener.OnEnable();
	}

    private void OnDisable()
    {
		inputListener.OnDisable();
		activateWeaponListener.OnDisable();
		disActivateWeaponListener.OnDisable();
	}

    private void Awake()
    {
		activateWeaponListener.response    = ActivateWeapon;
		disActivateWeaponListener.response = DisActivateWeapon;
		inputListener.response = ExtensionMethods.EmptyMethod;


		CreateProjectileStack();
	}

    private void Start()
    {
		shooterTransform = shootOrigin.sharedValue as Transform;
		mainCamera = Camera.main;
	}
	#endregion

	#region Implementation
	void ActivateWeapon()
    {
		inputListener.response = Shoot;
	}
	void DisActivateWeapon()
    {
		inputListener.response = ExtensionMethods.EmptyMethod;
	}
	void Shoot()
    {
        if(Time.time < nextFire)
			return;

		RaycastHit hit;
		Ray ray = mainCamera.ScreenPointToRay( Input.mousePosition );

		if( Physics.Raycast( ray, out hit ) )
		{
			// get a projectile
			var projectile = GiveProjectile();

			// set position
			projectile.transform.position = shooterTransform.position;

			// set rotation
			var lookRotation = Quaternion.LookRotation( hit.point - shooterTransform.position );
			projectile.transform.rotation = lookRotation;

			projectile.Fire( hit.point );

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
	#endregion
}
