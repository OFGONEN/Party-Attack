/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using UnityEngine.AI;
using FFStudio;
using NaughtyAttributes;
using DG.Tweening;

public class Human : MonoBehaviour
{
#region Fields
    public enum State
    {
        Dancing, Running, Neutralized_Ragdoll, Neutralized_Stationary
    }

	public State CurrentState { get; private set; } = State.Dancing;

	public bool SpawnedBefore { get; } = false;

	private float Health
    {
		get { return health; }
        set
        {
			if( CurrentState == State.Neutralized_Ragdoll || CurrentState == State.Neutralized_Stationary )
				return;
                
            health = value; 
            OnHealthChange();
        }
	}
	private float health;

	private float HealthRatio => Health / GameSettings.Instance.human.startingHealth;

	private MaterialPropertyBlock materialPropertyBlock;
    private SkinnedMeshRenderer meshRenderer;
	static private int colorHash = Shader.PropertyToID( "_Color" );
    
	private NavMeshAgent agent;
	private Puppet.Dancer dancer;
    
	private Tween startDancingUponReachingTargetTween;
#endregion

#region Unity API
	private void OnEnable()
    {
        if( SpawnedBefore )
        {
            // Revert back to defaults.
            agent.enabled = false;
			dancer.enabled = true;
			// TODO: ragdoll.enabled = false;

			Health = GameSettings.Instance.human.startingHealth;

			CurrentState = State.Dancing;
		}
	}
    
    private void OnDisable()
    {
		CancelStartDancingUponReachingTargetTween();
	}
    
    private void Start()
    {
        materialPropertyBlock = new MaterialPropertyBlock();
        meshRenderer          = GetComponentInChildren< SkinnedMeshRenderer >();
        agent                 = GetComponent< NavMeshAgent >();
        dancer                = GetComponent< Puppet.Dancer >();
        Health                = GameSettings.Instance.human.startingHealth;
        
		OnHealthChange();
	}
    
    private void OnTriggerEnter( Collider other )
    {
        //if( CurrentState == State.Running )
			// TODO: Handle triggering the Fences.
            
        //if( CurrentState == State.Running || CurrentState == State.Dancing )
			 // TODO: OFG: Handle projectiles.
	}
#endregion

#region API
#endregion

#region Implementation
#if UNITY_EDITOR
    [ Button( "[TEST] Apply 30 Damage" ) ]
    private void Apply_10_Damage()
    {
		Health -= 30;
	}
    
    [ Button( "[TEST] Run From <1,0,0>" ) ]
    private void RunFromVector3Right()
    {
		RunFrom( Vector3.right );
	}
#endif

    private void RunFrom( Vector3 fromThisPosition )
    {
		dancer.enabled = false;
		agent.enabled  = true;
        
		var direction = ( transform.position - fromThisPosition );
        direction.Scale( new Vector3( 1, 0, 1 ) ); // Stay on XZ plane.
		agent.SetDestination( transform.position + direction.normalized * GameSettings.Instance.human.runDistance );
        
		CurrentState = State.Running;
        
		StartDancingUponReachingTargetTween();
	}

    private void OnHealthChange()
    {
		UpdateColor();

		if( Health <= 0.0f )
			OnNeutralize();
    }

	private void OnNeutralize()
	{
		dancer.enabled = agent.enabled = false;
		// TODO: ragdoll.enabled = true;
		CurrentState = State.Neutralized_Ragdoll;
		// TODO: ragdoll.enabled = false in X seconds via DOVirtual.DelayedCall.
	}

	protected void StartDancingUponReachingTargetTween()
	{
		startDancingUponReachingTargetTween = DOVirtual.DelayedCall( /* Repeat every */ 0.5f /* seconds */, () =>
                                            {
												if( isActiveAndEnabled && agent.remainingDistance < agent.stoppingDistance )
                                                {
													agent.enabled = false;
													dancer.enabled = true;
													dancer.SetFeetPosition();
													CurrentState = State.Dancing;
                                                    
													startDancingUponReachingTargetTween.Kill();
                                                    startDancingUponReachingTargetTween = null;
                                                }
                                            } ).SetLoops( -1 );
	}
	
	protected void CancelStartDancingUponReachingTargetTween()
	{
		startDancingUponReachingTargetTween.Kill();
		startDancingUponReachingTargetTween = null;
	}
    
    private void UpdateColor()
    {
		meshRenderer.GetPropertyBlock( materialPropertyBlock );
		materialPropertyBlock.SetColor( colorHash, ColorBasedOnHealth() );
		meshRenderer.SetPropertyBlock( materialPropertyBlock );
    }
    
    private Color ColorBasedOnHealth()
    {
        if( Mathf.Approximately( Health, 0.0f ) )
			return GameSettings.Instance.human.neutralizedColor;

		return Color.Lerp( GameSettings.Instance.human.fullyDepletedColor, GameSettings.Instance.human.fullHealthColor, HealthRatio );
	}
#endregion
}
