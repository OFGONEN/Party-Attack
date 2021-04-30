using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;

public abstract class Projectile : MonoBehaviour
{
    #region Fields
	public ProjectileStack projectileStack;
	public ParticleSpawnEvent particleEvent;

	protected Tween movementTween;
	#endregion

	#region UnityAPI
	protected virtual void OnDisable()
    {
		projectileStack.Push( this );
	}
	#endregion

	#region API
	public void Shoot(Vector3 targetPosition )
	{
		var duration = Vector3.Distance( targetPosition, transform.position ) / GameSettings.Instance.projectile_Speed;

		var tween = transform.DOMoveX( targetPosition.x, duration ).SetEase( Ease.Linear );
		transform.DOMoveZ( targetPosition.z, duration ).SetEase( Ease.Linear );
		transform.DOMoveY( targetPosition.y, duration ).SetEase( Ease.InQuad );

		tween.OnComplete( TargetReached );

		movementTween = tween;
	}
    #endregion

	#region Implementation
	protected abstract void TargetReached();
	#endregion
}
