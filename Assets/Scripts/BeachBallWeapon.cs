using System.Collections;
using System.Collections.Generic;
using FFStudio;
using UnityEngine;
using UnityEngine.Events;

public class BeachBallWeapon : MonoBehaviour
{
    #region Fields
	[Header( "Event Listeners" )]
	public EventListenerDelegateResponse aimInputListener;
	public EventListenerDelegateResponse fireInputListener;
	public EventListenerDelegateResponse activateWeaponListener;
	public MultipleEventListenerDelegateResponse disActivateWeaponListener;

	[Header( "UI Interaction" )]
	public UnityEvent activateEvent;
	public UnityEvent disActivateEvent;


	[Header( "Shooting" )]
	public SharedReferenceProperty shootOriginReferance;
	public SharedReferenceProperty mainCameraReferance;
	public GameEvent ultimateUsed;
	public Rigidbody beachBall;
	public GameObject crosshair;


	// private fields
	private Camera mainCamera;
	private Transform shooterTransform;
	private int rayCastLayerMask;
	#endregion

	#region UnityAPI
	private void OnEnable()
	{
		aimInputListener.OnEnable();
		fireInputListener.OnEnable();
		activateWeaponListener.OnEnable();
		disActivateWeaponListener.OnEnable();
	}

	private void OnDisable()
	{
		aimInputListener.OnDisable();
		fireInputListener.OnDisable();
		activateWeaponListener.OnDisable();
		disActivateWeaponListener.OnDisable();
	}

	private void Awake()
	{
		activateWeaponListener.response = ActivateWeapon;
		disActivateWeaponListener.response = DisActivateWeapon;

		DisActivateWeapon();

		mainCameraReferance.changeEvent += () => mainCamera = mainCameraReferance.sharedValue as Camera;
		shootOriginReferance.changeEvent += () => shooterTransform = shootOriginReferance.sharedValue as Transform;

		rayCastLayerMask = LayerMask.GetMask( "Raycast-Only");

		crosshair.gameObject.SetActive( false );
		beachBall.gameObject.SetActive( false );
	}
	#endregion

	#region Implementation
	void ActivateWeapon()
	{
		aimInputListener.response = Aim;
		fireInputListener.response = Shoot;
		crosshair.transform.position = shooterTransform.position;
		crosshair.gameObject.SetActive( true );

		activateEvent.Invoke();
	}
	void DisActivateWeapon()
	{
		aimInputListener.response = ExtensionMethods.EmptyMethod;
		fireInputListener.response = ExtensionMethods.EmptyMethod;
		crosshair.gameObject.SetActive( false );

		disActivateEvent.Invoke();
	}

    void Aim()
    {
		RaycastHit hit;
		Ray ray = mainCamera.ScreenPointToRay( Input.mousePosition );

		if( Physics.Raycast( ray, out hit, 200, rayCastLayerMask ) )
        {
			var position = hit.point;
			position.y += 0.5f;
			crosshair.transform.position = position;
		}
    }

    void Shoot()
    {
		// Neutralize beachball
		beachBall.velocity        = Vector3.zero;
		beachBall.angularVelocity = Vector3.zero;
		beachBall.gameObject.SetActive( true );

		beachBall.transform.position = shooterTransform.position;

		var direction = crosshair.transform.position - shooterTransform.position;

		// set rotation
		var lookRotation = Quaternion.LookRotation( direction ).eulerAngles;

		beachBall.transform.eulerAngles = lookRotation;

		beachBall.AddForce( direction * GameSettings.Instance.projectile_beachBall_force, ForceMode.Force );

		ultimateUsed.Raise();
		DisActivateWeapon();
	}
    #endregion


}
