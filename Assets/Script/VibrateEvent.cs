/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FFStudio;

[ CreateAssetMenu( fileName = "event_", menuName = "FF/Event/Vibrate Event" ) ]
public class VibrateEvent : GameEvent
{
	public VibrateMethod vibrateMethod;

	public void Raise( VibrateMethod method )
    {
		vibrateMethod = method;
		Raise();
	}
}