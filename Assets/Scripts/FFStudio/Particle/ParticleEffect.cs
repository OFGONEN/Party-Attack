using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFStudio
{
	public class ParticleEffect : MonoBehaviour
	{
		#region Fields
		[Header( "Fired Events" )]
		public ParticleEffectStack particleStack;
		public StringGameEvent particleStoppedEvent;
		public string alias;
		private ParticleSystem particles;
		#endregion

		#region UnityAPI

		private void Awake()
		{
			particles = GetComponent<ParticleSystem>();

			var mainParticle = particles.main;

			mainParticle.stopAction = ParticleSystemStopAction.Callback;
			mainParticle.playOnAwake = false;
			// mainParticle.loop = false;

			particleStoppedEvent.eventValue = alias;

			gameObject.SetActive( false );
		}

		private void OnDisable()
		{
			particleStack.stack.Push( this );
		}

		private void OnParticleSystemStopped()
		{
			gameObject.SetActive( false );
			particleStoppedEvent.Raise();
		}
		#endregion

		#region API
		public void PlayParticle( ParticleSpawnEvent particleEvent )
		{
			gameObject.SetActive( true );

			if( particleEvent.changePosition )
				transform.position = particleEvent.spawnPoint;

			particles.Play();

			FFLogger.Log( "Playing: " + alias + " active:" + gameObject.activeInHierarchy );
		}
		#endregion

	}
}