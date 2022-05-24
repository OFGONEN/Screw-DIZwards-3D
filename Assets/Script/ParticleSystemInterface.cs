/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

public class ParticleSystemInterface : MonoBehaviour
{
#region Fields
    [ SerializeField ] ParticleSystem pfx_system;
#endregion

#region Properties
#endregion

#region Unity API
#endregion

#region API
    [ Button() ]
    public void StopAndClear()
    {
		pfx_system.Stop( true, ParticleSystemStopBehavior.StopEmittingAndClear );
	}
#endregion

#region Implementation
#endregion

#region Editor Only
#if UNITY_EDITOR
#endif
#endregion
}