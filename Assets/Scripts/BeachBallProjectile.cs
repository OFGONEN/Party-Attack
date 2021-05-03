using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

public class BeachBallProjectile : MonoBehaviour
{
	#region Fields
	[Header( "Fired Events" )]
	public ParticleSpawnEvent particleSpawnEvent;
	#endregion

	#region UnityAPI
	private void OnTriggerEnter( Collider other )
    {
        var human = other.GetComponentInParent< Human >();
		human.Health -= GameSettings.Instance.human.startingHealth;

		var position = transform.position;
		position.y = 0;

		particleSpawnEvent.changePosition = true;
		particleSpawnEvent.spawnPoint = position;
		particleSpawnEvent.particleAlias = "BeachBall";
		particleSpawnEvent.Raise();
	}
    #endregion
}
