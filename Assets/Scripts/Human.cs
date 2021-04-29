/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using FFStudio;
using NaughtyAttributes;

public class Human : MonoBehaviour
{
#region Fields
    [ Expandable ]
    public SharedFloatProperty health;
    
    private float startHealth;
	private float HealthRatio => health.sharedValue / startHealth;

	private MaterialPropertyBlock materialPropertyBlock;
    private SkinnedMeshRenderer meshRenderer;
    static private int colorHash = Shader.PropertyToID( "_Color" );
#endregion

#region Unity API
    private void OnEnable()
    {
		health.changeEvent += OnHealthChange;
	}
    
    private void OnDisable()
    {
		health.changeEvent -= OnHealthChange;
	}
    
    private void Start()
    {
        meshRenderer          = GetComponentInChildren< SkinnedMeshRenderer >();
        materialPropertyBlock = new MaterialPropertyBlock();
        startHealth           = health.sharedValue = GameSettings.Instance.player.startingHealth;
		OnHealthChange();
	}
#endregion

#region API
#endregion

#region Implementation
#if UNITY_EDITOR
    [ Button() ]
    private void Test_Apply_10_Damage()
    {
		health.SetValue( health.sharedValue - 10 );
	}
#endif

    private void OnHealthChange()
    {
		meshRenderer.GetPropertyBlock( materialPropertyBlock );
		materialPropertyBlock.SetColor( colorHash, ColorBasedOnHealth() );
		meshRenderer.SetPropertyBlock( materialPropertyBlock );
    }
    
    private Color ColorBasedOnHealth()
    {
        if( Mathf.Approximately( health.sharedValue, 0.0f ) )
			return GameSettings.Instance.player.neutralizedColor;

		return Color.Lerp( GameSettings.Instance.player.fullyDepletedColor, GameSettings.Instance.player.fullHealthColor, HealthRatio );
	}
#endregion
}
