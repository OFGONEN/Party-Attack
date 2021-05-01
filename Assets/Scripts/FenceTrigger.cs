/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using FFStudio;

public class FenceTrigger : MonoBehaviour
{
#region Fields
    [ SerializeField ]
    private Human human;
#endregion

#region Unity API
    private void OnTriggerEnter( Collider other )
    {
		if( other.gameObject.layer != 8 /* Fence */ )
			return;

		/* Only times a Fence can trigger a Human is when the Human is running or dancing.
	     * Hitting a Fence instantly Neutralizes a Human. */
		if( human.CurrentState == Human.State.Running || human.CurrentState == Human.State.Dancing )
			human.BecomeNeutralized( /* Apply hill jump force */ true );

		/* Trigger behaviour between Human and The Net are handled in The Net side. */
	}
#endregion

#region API
#endregion

#region Implementation
#endregion
}
