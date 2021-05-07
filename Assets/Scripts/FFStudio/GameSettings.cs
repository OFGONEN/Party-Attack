using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace FFStudio
{
	public class GameSettings : ScriptableObject
    {
		[ System.Serializable ]
		public class PlayerSettings
		{
			[ Min( 1.0f ) ]
			public float startingHealth   = 100.0f;
			public Color neutralizedColor = Color.black, fullyDepletedColor = Color.gray, fullHealthColor = Color.red;
			public float runDistance      = 10.0f;
			public float hillJumpForce    = 10.0f;
			[ Range( 0.1f, 5.0f ) ]
			public float ragdollTurnoffTime = 3.0f;
		}
		
	#region Fields
        public int maxLevelCount;
        [Tooltip("Duration of the movement for ui element")] public float uiEntityMoveTweenDuration;
		[Tooltip("Duration of the scaling for ui element")] public float uiEntityScaleTweenDuration;
		[Tooltip("Duration of the movement for floating ui element")] public float uiFloatingEntityTweenDuration;
        [Tooltip("Percentage of the screen to register a swipe")] public int swipeThreshold;
		
		public PlayerSettings human;

		[Foldout( "Camera Settings" )] public Vector3 camera_RotationVector;

		[Foldout( "Weapon Settings" )] public float weapon_water_fireRate = 0.15f;
		[Foldout( "Weapon Settings" )] public float weapon_fart_fireRate = 1f;

		[Foldout( "Projectile Settings" )] public float projectile_Speed = 20f;
		[Foldout( "Projectile Settings" )] public float projectile_fart_radius = 1f;
		[Foldout( "Projectile Settings" )] public float projectile_fart_Y_Position = 0.6f;
		[Foldout( "Projectile Settings" )] public float projectile_fart_delay = 1f;
		[Foldout( "Projectile Settings" ) ] public int projectile_target_triggerLayer; // Target == Human
		[Foldout( "Projectile Settings" ) ] public float projectile_beachBall_force; 
		[Foldout( "Projectile Settings" ) ] public float projectile_beachBall_explosion_force = 500f; 
		[Foldout( "Projectile Settings" ) ] public float projectile_beachBall_explosion_radius = 50f; 
		[Foldout( "Projectile Settings" ) ] public float projectile_beachBall_explosion_upwardsModifier = 10f; 
		[Foldout( "Projectile Settings" ), Tooltip("Starts at first human contact") ] public float projectile_beachBall_disableAfterTime; 

		private static GameSettings instance;

		private delegate GameSettings ReturnGameSettings();
		private static ReturnGameSettings returnInstance = LoadInstance;

		public static GameSettings Instance
		{
			get
			{
				return returnInstance();
			}
		}
	#endregion

	#region Implementation
		static GameSettings LoadInstance()
		{
			if( instance == null )
				instance = Resources.Load< GameSettings >( "GameSettings" );

			returnInstance = ReturnInstance;

			return instance;
		}

		static GameSettings ReturnInstance()
		{
			return instance;
		}
	#endregion
	}
}
