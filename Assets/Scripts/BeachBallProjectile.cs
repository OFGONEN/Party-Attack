using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

public class BeachBallProjectile : MonoBehaviour
{
    #region Fields
    #endregion

    #region UnityAPI
    private void OnTriggerEnter( Collider other )
    {
        var human = other.GetComponentInParent< Human >();
		human.Health -= GameSettings.Instance.human.startingHealth;
	}
    #endregion
}
