/* Created by and for usage of FF Studios (2021). */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FFStudio
{
    [ CreateAssetMenu( fileName = "event_elephant_custom", menuName = "FF/Event/Elephant Custom Event" ) ]
	public class ElephantCustomEvent : GameEvent
	{
        public string eventValue_key;
		public ElephantParameter eventValue_Param;

        public void Raise( string key, ElephantParameter parameter )
        {
			eventValue_key   = key;
			eventValue_Param = parameter;

			Raise();
		}
	}
}