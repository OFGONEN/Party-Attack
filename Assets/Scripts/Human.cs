/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using UnityEngine.AI;
using FFStudio;
using NaughtyAttributes;
using DG.Tweening;

public class Human : MonoBehaviour
{
	#region Fields
	[Header ("Fired Events")]
	public GameEvent humanNeutralized;
	public enum State
    {
        Dancing, Running, Neutralized_Ragdoll, Neutralized_Stationary
    }

	[ field: SerializeField ]
	public State CurrentState { get; private set; } = State.Dancing;

	public bool SpawnedBefore { get; private set; } = false;

	public float Health
    {
		get { return health; }
        set
        {
			if( CurrentState == State.Neutralized_Ragdoll || CurrentState == State.Neutralized_Stationary )
				return;

			health = Mathf.Max( 0, value );
			OnHealthChange();
        }
	}
	
/* Private Fields */
	
    [ ShowNonSerializedField, ReadOnly ]
	private float health;

	private float HealthRatio => Health / GameSettings.Instance.human.startingHealth;

	private MaterialPropertyBlock materialPropertyBlock;
    private SkinnedMeshRenderer meshRenderer;
	static private int colorHash = Shader.PropertyToID( "_Color" );
	
	private Animator animator;
	static private int isRunningHash = Animator.StringToHash( "IsRunning" );

	private RagdollToggler ragdoll;
    
	private NavMeshAgent agent;
	private Puppet.Dancer dancer;
    
	private Tween switchToDancingUponReachingTargetTween;
	private Tween delayedRagdollTurnOffCall;
	private bool willJump = false;
	private bool jumpIsAllowed = false;
#endregion

#region Unity API
	private void OnEnable()
    {
        if( SpawnedBefore )
        {
            // Revert back to defaults.
               agent.enabled = false;
            animator.enabled = true;
              dancer.enabled = true;
			ragdoll.Toggle( false );

			willJump = false;

			Health = GameSettings.Instance.human.startingHealth;

			CurrentState = State.Dancing;
		}
	}
    
    private void OnDisable()
    {
		CancelSwitchToDancingUponReachingTargetTween();
		SpawnedBefore = true;

		if(delayedRagdollTurnOffCall != null && delayedRagdollTurnOffCall.IsActive())
		{
			delayedRagdollTurnOffCall.Kill();
		}
	}
    
    private void Start()
    {
        materialPropertyBlock = new MaterialPropertyBlock();
        meshRenderer          = GetComponentInChildren< SkinnedMeshRenderer >();
        agent                 = GetComponent< NavMeshAgent >();
        animator              = GetComponent< Animator >();
        dancer                = GetComponent< Puppet.Dancer >();
		ragdoll               = GetComponent< RagdollToggler >();
        Health                = GameSettings.Instance.human.startingHealth;

		RandomizeDancerProperties();
		
		OnHealthChange();

		jumpIsAllowed = !CurrentLevelData.Instance.levelData.isGroundLevel;
	}
	
	/* Since the child "Neo_Hips" GameObject has the trigger collider AND a Rigidbody, 
	 * OnTriggerEnter is handled on FenceTrigger component of that GameObject. */
	// private void OnTriggerEnter( Collider other )
    // {
	// }
#endregion

#region API
	public void BecomeNeutralized( bool applyHillJumpForce = false )
	{
		CancelSwitchToDancingUponReachingTargetTween();
		
		animator.enabled = false;
		dancer.enabled   = false;
		agent.enabled    = false;
		   
		ragdoll.Toggle( true );
		
		if( applyHillJumpForce )
			ApplyHillJumpingForce();
		else // Make sure body is on Y = 0, so it can be set to stationary after X seconds.
			transform.position = transform.position.SetY( 0 );

		CurrentState = State.Neutralized_Ragdoll;

		humanNeutralized.Raise();

		/* Turn ragdoll off after a pre-determined time passes, IF the character is still resting on Play Area (Y = 0). */
		delayedRagdollTurnOffCall = DOVirtual.DelayedCall( GameSettings.Instance.human.ragdollTurnoffTime,
							   	() =>
									{
										if( Mathf.Approximately( transform.position.y, 0 ) ) // Still on the Play Area.
										{
											ragdoll.Toggle( false );
											CurrentState = State.Neutralized_Stationary;
										}
									} ).OnComplete(() => delayedRagdollTurnOffCall = null);
	}

	public void RunFrom( Vector3 fromThisPosition )
	{
		if( CurrentState != State.Dancing )
			return;

		dancer.Pause();
		animator.SetBool( isRunningHash, true );
		agent.enabled = true;

		var direction = ( transform.position - fromThisPosition ).SetY( 0 ).normalized;
		var destination = transform.position + direction * GameSettings.Instance.human.runDistance;

		if( jumpIsAllowed )
		{
			NavMeshHit hitInfo;
			bool aBoundaryIsHit = agent.Raycast( destination, out hitInfo );
			if( aBoundaryIsHit && hitInfo.mask == 0 /* Meaning the boundary hit is the outermost one. */ )
			{
				/* Means the destination is outside the NavMesh. Agent should go until it hits the border and then jump.
				* Readjust destination to reachable spot. */
				destination = transform.position + direction * hitInfo.distance;

				/* There might also be obsctructing geometry in our way, we don't want to jump in that case. */
				RaycastHit raycastHitInfo;
				if( Physics.SphereCast( transform.position, agent.radius * 1.5f, direction, out raycastHitInfo, Mathf.Infinity, 1 << 10 ) )
					// There IS obstructing geometry in our way. Still going to run, but will stop a little before the obstruction.
					destination = transform.position + direction * ( raycastHitInfo.distance - 0.3f );
				else
					willJump = true;
			}
		}
		
		agent.SetDestination( destination );

		CurrentState = State.Running;

		SwitchToDancingUponReachingTargetTween();
	}
	
	public void ApplyHillJumpingForce()
	{
		ragdoll.ApplyForce( transform.position.normalized * GameSettings.Instance.human.hillJumpForce );
	}
#endregion

#region Implementation
#if UNITY_EDITOR
    [ Button( "[TEST] Apply 30 Damage" ) ]
    private void Apply_30_Damage()
    {
		Health -= 30;
	}
    
	[ Button( "[TEST] Run From <0,0,0>" ) ]
    private void RunFromOrigin()
    {
		RunFrom( Vector3.zero );
	}
#endif

	protected void SwitchToDancingUponReachingTargetTween()
	{
		NavMeshPath path = new NavMeshPath();
		
		switchToDancingUponReachingTargetTween = 
			DOVirtual.DelayedCall( /* Repeat every */ 0.25f /* seconds */, () =>
			{
				if( isActiveAndEnabled && agent.remainingDistance < agent.stoppingDistance )
				{
					CancelSwitchToDancingUponReachingTargetTween();

					if( willJump )
						BecomeNeutralized( /* Apply hill jump force */ true );
					else 
					{
						agent.enabled = false;

						animator.SetBool( isRunningHash, false );
						dancer.Resume();

						CurrentState = State.Dancing;
					}
				}
			} ).SetLoops( -1 );
	}
	
	protected void CancelSwitchToDancingUponReachingTargetTween()
	{
		switchToDancingUponReachingTargetTween.Kill();
		switchToDancingUponReachingTargetTween = null;
	}

	private void OnHealthChange()
	{
		UpdateColor();

		if( Health <= 0.0f )
			BecomeNeutralized();
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
	
	private void RandomizeDancerProperties()
	{
		dancer.footDistance  *= Random.Range( 0.8f, 2.0f );
		dancer.stepFrequency *= Random.Range( 0.4f, 1.6f );
		dancer.stepHeight    *= Random.Range( 0.75f, 1.25f );
		dancer.stepAngle     *= Random.Range( 0.75f, 1.25f );

		dancer.hipHeight        *= Random.Range( 0.75f, 1.25f );
		dancer.hipPositionNoise *= Random.Range( 0.75f, 1.25f );
		dancer.hipRotationNoise *= Random.Range( 0.75f, 1.25f );

		dancer.spineBend           = Random.Range( 4.0f, -16.0f );
		dancer.spineRotationNoise *= Random.Range( 0.75f, 1.25f );

		dancer.handPositionNoise *= Random.Range( 0.5f, 2.0f );
		dancer.handPosition      += Random.insideUnitSphere * 0.25f;

		dancer.headMove       *= Random.Range( 0.2f, 2.8f );
		dancer.noiseFrequency *= Random.Range( 0.4f, 1.8f );
		dancer.randomSeed      = ( uint )Random.Range( 0, 0xffffff );
	}
#endregion
}
