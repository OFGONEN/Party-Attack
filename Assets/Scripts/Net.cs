/* Created by and for usage of FF Studios (2021). */

using UnityEngine;

public class Net : MonoBehaviour
{
#region Fields
#endregion

#region Unity API
	private void OnTriggerEnter( Collider other )
	{
		if( other.gameObject.layer != 3 /* Human */ )
			return;

		/* Hitting a Net instantly deactivates a Human.
         * Hierarchy towards parent Human is This -> Neo_Hip -> Neo_Reference -> Human parent. */
		other.gameObject.transform.parent.parent.parent.gameObject.SetActive( false );
	}
#endregion

#region API
#endregion

#region Implementation
#endregion
}
