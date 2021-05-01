﻿using System.Collections;
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
			public float startingHealth        = 100.0f;
			public Color neutralizedColor      = Color.black, fullyDepletedColor = Color.gray, fullHealthColor = Color.red;
			public float runVelocity           = 10.0f;
			public float hillJumpForce_Dancing = 1.0f;
			public float hillJumpForce_Running = 10.0f;
			[ Range( 0.1f, 5.0f ) ]
			public float ragdollTurnoffTime	   = 3.0f;
		}
		
	#region Fields
        public int maxLevelCount;
        [Tooltip("Duration of the movement for ui element")] public float uiEntityMoveTweenDuration;
		[Tooltip("Duration of the scaling for ui element")] public float uiEntityScaleTweenDuration;
		[Tooltip("Duration of the movement for floating ui element")] public float uiFloatingEntityTweenDuration;
        [Tooltip("Percentage of the screen to register a swipe")] public int swipeThreshold;
		
		public PlayerSettings human;

		[Foldout( "Camera Settings" )] public Vector3 camera_RotationVector;

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
