/* Created by and for usage of FF Studios (2021). */

using UnityEngine;

public enum ElephantEvent
{
    LevelStarted,
    LevelCompleted,
    LevelFailed
}

namespace FFStudio
{
    [ CreateAssetMenu( fileName = "event_elephant_level", menuName = "FF/Event/Elephant Level Event" ) ]
    public class ElephantLevelEvent : GameEvent
    {
        public ElephantEvent elephantEventType;
        public int level;
    }
}