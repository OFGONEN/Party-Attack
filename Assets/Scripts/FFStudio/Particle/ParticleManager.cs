using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

namespace FFStudio
{
	public class ParticleManager : MonoBehaviour
	{
		#region Fields
		[Header( "Event Listeners" )]
		public EventListenerDelegateResponse spawnParticleListener;
		public ParticleEffectStack[] particleEffectStacks;

		private Dictionary<string, ParticleEffectStack> particleStackDictionary;
		#endregion

		#region UnityAPI

		private void OnEnable()
		{
			spawnParticleListener.OnEnable();
		}

		private void OnDisable()
		{
			spawnParticleListener.OnDisable();
		}

		private void Awake()
		{
			spawnParticleListener.response = SpawnParticle;

			particleStackDictionary = new Dictionary<string, ParticleEffectStack>( particleEffectStacks.Length );

			for (int i = 0; i < particleEffectStacks.Length; i++)
			{
				var particleStack = particleEffectStacks[ i ];
				particleStack.stack = new Stack<ParticleEffect>( particleStack.stackSize );

				for( int x = 0; x < particleStack.stackSize; x++ )
				{
					var effect = GameObject.Instantiate( particleStack.prefab );
					effect.transform.SetParent( transform );
				}

				particleStackDictionary.Add( particleStack.prefab.alias, particleStack );
			}
		}
		#endregion

		#region Implementation

		void SpawnParticle()
		{
			var spawnEvent = spawnParticleListener.gameEvent as ParticleSpawnEvent;

			ParticleEffectStack particleStack = null;

			if( !particleStackDictionary.TryGetValue( spawnEvent.particleAlias, out particleStack ) )
			{
				FFLogger.Log( "Particle:" + spawnEvent.particleAlias + " is missing!" );
				return;
			}

			var particleEffect = GiveParticle( particleStack );
			particleEffect.PlayParticle( spawnEvent );
		}

		ParticleEffect GiveParticle(ParticleEffectStack effectStack)
		{
			ParticleEffect particle;

			if( effectStack.stack.Count > 0 )
				particle = effectStack.stack.Pop();
			else
			{
				particle = GameObject.Instantiate( effectStack.prefab );
				particle.transform.SetParent( transform );

			}

			return particle;
		}


		#endregion
	}
}