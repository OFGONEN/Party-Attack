using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;

public class FartBombProjectile : Projectile
{
	#region Implementation
	protected override void TargetReached()
	{
		var particleSpawnPosition = transform.position;
		particleSpawnPosition.y = GameSettings.Instance.projectile_fart_Y_Position;

		particleEvent.changePosition = true;
		particleEvent.particleAlias = "Fart";
		particleEvent.spawnPoint = particleSpawnPosition;
		particleEvent.Raise();

		// movementTween = null;

		gameObject.SetActive( false );

		DOVirtual.DelayedCall( GameSettings.Instance.projectile_fart_delay, MakeHumansRun );
	}

	void MakeHumansRun()
	{
		var hits = Physics.OverlapSphere( transform.position, GameSettings.Instance.projectile_fart_radius, 1 << GameSettings.Instance.projectile_target_triggerLayer /* Human Layer Mask */);

		foreach ( var hit in hits )
        {
			var human = hit.GetComponentInParent<Human>();
			human.Health -= damage;
			human.RunFrom( transform.position );
		}

		// raise ultimate progress  * hits.lenght
		ultimateProgressEvent.eventValue = damage * ultimateProgressCofactor * hits.Length;
		ultimateProgressEvent.Raise();
	}
	#endregion

	private void OnDrawGizmos()
	{
		Gizmos.DrawSphere( transform.position, GameSettings.Instance.projectile_fart_radius );
	}
}
