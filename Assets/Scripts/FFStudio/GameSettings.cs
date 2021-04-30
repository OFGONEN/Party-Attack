using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace FFStudio
{
	public class GameSettings : ScriptableObject
    {
        #region Fields

        public int maxLevelCount;
        [Tooltip("Duration of the movement for ui element")] public float uiEntityMoveTweenDuration;
		[Tooltip("Duration of the scaling for ui element")] public float uiEntityScaleTweenDuration;
		[Tooltip("Duration of the movement for floating ui element")] public float uiFloatingEntityTweenDuration;
        [Tooltip("Percentage of the screen to register a swipe")] public int swipeThreshold;

		[Foldout( "Camera Settings" )] public Vector3 camera_RotationVector;

		[Foldout( "Projectile Settings" )] public float projectile_Speed = 20f;
		[Foldout( "Projectile Settings" )] public float projectile_fart_radius = 20f;
		[Foldout( "Projectile Settings" ) ] public int projectile_triggerLayer;

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
