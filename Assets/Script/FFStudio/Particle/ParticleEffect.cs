/* Created by and for usage of FF Studios (2021). */

using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace FFStudio
{
	public class ParticleEffect : MonoBehaviour
	{
#region Fields
	[ Title( "Setup" ) ]
		public MultipleEventListenerDelegateResponse level_finish_listener;
		public string alias;
		public UnityEvent onParticleSpawn;

		// Private Fields \\
		private ParticleEffectPool particle_pool;
		private ParticleEffectStopped particleEffectStopped;
		private ParticleSystem particles;

		private Vector3 particle_start_size;
#endregion

#region UnityAPI
		private void OnEnable()
		{
			level_finish_listener.OnEnable();
		}

		private void OnDisable()
		{
			level_finish_listener.OnDisable();
		}

		private void Awake()
		{
			particles = GetComponentInChildren< ParticleSystem >();

			if( particles ) 
			{
				var mainParticle             = particles.main;
				    mainParticle.stopAction  = ParticleSystemStopAction.Callback;
				    mainParticle.playOnAwake = false;
			}

			level_finish_listener.response = OnParticleSystemStopped;
			particle_start_size            = transform.localScale;
		}

		private void OnParticleSystemStopped()
		{
			particleEffectStopped( this );
			particle_pool.ReturnEntity( this );
			transform.localScale = Vector3.one;
		}
#endregion

#region API
		public virtual void InitIntoPool( ParticleEffectPool pool, ParticleEffectStopped effectStoppedDelegate )
		{
			particle_pool         = pool;
			particleEffectStopped = effectStoppedDelegate;
		}

		public void PlayParticle( ParticleSpawnEvent particleEvent )
		{
			gameObject.SetActive( true );
			
			transform.position   = particleEvent.particle_spawn_point;
			transform.localScale = particle_start_size * particleEvent.particle_spawn_size;

			if( particleEvent.particle_spawn_parent != null )
				transform.SetParent( particleEvent.particle_spawn_parent );

			particles?.Play();
			onParticleSpawn.Invoke();
		}

		public void PlayParticle( Vector3 position, Vector3 size, Transform parent = null )
		{
			gameObject.SetActive( true );
			
			transform.position   = position;
			transform.localScale = size;

			if( parent != null )
				transform.SetParent( parent );

			particles?.Play();
			onParticleSpawn.Invoke();
		}
#endregion

	}
}