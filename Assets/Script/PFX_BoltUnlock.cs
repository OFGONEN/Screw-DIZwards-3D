/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

public class PFX_BoltUnlock : MonoBehaviour
{
#region Fields
    [ SerializeField ] ParticleEffectPool pool_pfx;
    [ SerializeField ] Transform parent_gfx;
    [ SerializeField ] bool fixPosition = false;

    Vector3 positionDelta;
#endregion

#region Properties
#endregion

#region Unity API
    private void Start()
    {

        if( fixPosition )
        {
			var childCount    = parent_gfx.childCount;
			    positionDelta = Vector3.up * childCount * 0.5f / 2f;

			parent_gfx.localPosition = -positionDelta;
			transform.position       = transform.position + positionDelta;
		}
        else
			positionDelta = Vector3.zero;
	}
#endregion

#region API
    public void Play()
    {
		var childCount = parent_gfx.childCount;

		var particle = pool_pfx.GetEntity();
        FFLogger.Log( "ParticleSpawned", particle );
		particle.PlayParticle( transform.position - positionDelta, new Vector3( 1, childCount, 1 ) );
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
