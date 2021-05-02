using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;
using DG.Tweening;

public class WaterBalloonProjectile : Projectile
{
    #region UnityAPI
	private void Awake()
	{
		gameObject.layer = GameSettings.Instance.projectile_triggerLayer;
	}

    private void OnTriggerEnter( Collider other )
    {
        if(movementTween != null)
        {
		    movementTween.Kill();
		}

		//var human = other.GetComponent<Human>();
		// human.DoDamage();

		TargetReached();
	}

    #endregion

	#region Implementation
	protected override void TargetReached()
    {
		particleEvent.changePosition = true;
		particleEvent.particleAlias = "Water";
		particleEvent.spawnPoint = transform.position;
		particleEvent.Raise();

		movementTween = null;

		gameObject.SetActive( false );
	}
	#endregion

}