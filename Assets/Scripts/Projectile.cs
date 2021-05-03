using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;

public abstract class Projectile : MonoBehaviour
{
	#region Fields
	[Header("Fired Events")]
	public FloatGameEvent ultimateProgressEvent;
	public ParticleSpawnEvent particleEvent;

	[Header("Sets")]
	public ProjectileStack projectileStack;

	[Header ("Damage Related")]
	public float damage;
	public float ultimateProgressCofactor;

	protected Tween[] movementTween = new Tween[3];
	#endregion

	#region UnityAPI
	protected virtual void OnDisable()
    {
		projectileStack.stack.Push( this );
	}
	#endregion

	#region API
	public void Fire(Vector3 targetPosition, Vector3 targetRotation )
	{
		var duration = Vector3.Distance( targetPosition, transform.position ) / GameSettings.Instance.projectile_Speed;

		transform.DORotate( targetRotation, duration ).SetEase(Ease.Linear);

		movementTween[0] = transform.DOMoveX( targetPosition.x, duration ).SetEase( Ease.Linear );
		movementTween[1] = transform.DOMoveZ( targetPosition.z, duration ).SetEase( Ease.Linear );
		movementTween[2] = transform.DOMoveY( targetPosition.y, duration ).SetEase( Ease.InQuad );

		movementTween[2].OnComplete( TargetReached );
	}
    #endregion

	#region Implementation
	protected abstract void TargetReached();
	#endregion
}
