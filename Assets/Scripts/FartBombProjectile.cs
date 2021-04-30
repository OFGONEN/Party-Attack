using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

public class FartBombProjectile : Projectile
{
	#region Implementation
	protected override void TargetReached()
	{
		particleEvent.changePosition = true;
		particleEvent.particleAlias = "Fart";
		particleEvent.spawnPoint = transform.position;

		movementTween = null;

		gameObject.SetActive( false );

		var hits = Physics.OverlapSphere( transform.position, GameSettings.Instance.projectile_fart_radius, GameSettings.Instance.projectile_triggerLayer /* Human Layer Mask */);

        foreach ( var hit in hits )
        {
			// var human = hit.GetComponent<Human>();
			// human.doDamage();
			// human.dorun();
		}
	}
	#endregion
}
