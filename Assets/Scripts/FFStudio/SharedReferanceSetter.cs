using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

public class SharedReferanceSetter : MonoBehaviour
{
    #region Fields
    public SharedReference sharedReferance;
	public Component referanceComponent;
	#endregion

    #region UnityAPI
    private void Awake()
    {
		sharedReferance.sharedValue = referanceComponent;
	}
    #endregion
}
