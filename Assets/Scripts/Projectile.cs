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
	public void Shoot( Tween tween, Vector3 targetPosition )
	{
		movementTween = tween;

		var duration = Vector3.Distance( targetPosition, transform.position ) / GameSettings.Instance.projectile_Speed;

		var bulletTween = transform.DOMoveX( targetPosition.x, duration ).SetEase( Ease.Linear );
		transform.DOMoveZ( targetPosition.z, duration ).SetEase( Ease.Linear );
		transform.DOMoveY( targetPosition.y, duration ).SetEase( Ease.InQuad );

		bulletTween.OnComplete( TargetReached );
	}
    #endregion

	#region Implementation
	protected abstract void TargetReached();
	#endregion
}
