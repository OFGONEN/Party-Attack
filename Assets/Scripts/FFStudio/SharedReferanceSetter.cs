using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

public class SharedReferanceSetter : MonoBehaviour
{
    #region Fields
    public SharedReferenceProperty sharedReferanceProperty;
	public Component referanceComponent;
	#endregion

    #region UnityAPI
    private void Awake()
    {
		sharedReferanceProperty.SetValue( referanceComponent );
	}
    #endregion
}
