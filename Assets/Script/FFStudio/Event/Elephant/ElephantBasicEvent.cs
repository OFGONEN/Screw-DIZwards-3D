/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFStudio
{
    [ CreateAssetMenu( fileName = "event_elephant_basic", menuName = "FF/Event/Elephant Basic Event" ) ]
	public class ElephantBasicEvent : GameEvent
	{
		public string eventValue_key;

        public void Raise( string key )
        {
			eventValue_key = key;
			Raise();
		}
	}
}