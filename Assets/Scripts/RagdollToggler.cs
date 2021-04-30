/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public class RagdollToggler : MonoBehaviour
{
#region Fields
    [ SerializeField ]
    private Rigidbody[] ragdollRigidbodies;
#endregion

#region Unity API
#endregion

#region API
#if UNITY_EDITOR
    [ Button() ]
    private void Toggle()
    {
		var currentState = ragdollRigidbodies[ 0 ].isKinematic;

		for( var i = 0; i < ragdollRigidbodies.Length; i++ )
			ragdollRigidbodies[ i ].isKinematic = !currentState;
    }
#endif

    public void Toggle( bool activate )
    {
        for( var i = 0; i < ragdollRigidbodies.Length; i++ )
			ragdollRigidbodies[ i ].isKinematic = !activate;
    }
#endregion

#region Implementation
#endregion
}
