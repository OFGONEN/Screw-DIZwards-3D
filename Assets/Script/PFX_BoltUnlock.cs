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
#endregion

#region Properties
#endregion

#region Unity API
    private void Start()
    {

        if( fixPosition )
        {
			var childCount    = parent_gfx.childCount;
			var positionDelta = Vector3.up * childCount * 0.5f / 2f;

			parent_gfx.localPosition = positionDelta;

            for( var i = 0; i < childCount; i++ )
            {
				var child = parent_gfx.GetChild( i );
				child.position = child.position - positionDelta;
			}
		}
	}
#endregion

#region API
    public void Play()
    {
		var childCount = parent_gfx.childCount;

		var particle = pool_pfx.GetEntity();
		particle.PlayParticle( transform.position, new Vector3( 1, childCount, 1 ) );
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}
