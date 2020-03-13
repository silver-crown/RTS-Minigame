using RTS;
using System;
using UnityEngine;

namespace ssuai
{
    public class ChunkScoutingAttractiveness : Factor
    {
        private Vector2Int _chunk;
        private Actor _actor;

        public ChunkScoutingAttractiveness(Vector2Int chunk, Actor actor)
        {
            _chunk = chunk;
            _actor = actor;
        }

        public override float GetUtility()
        {
            Validate();
            return _utility;
        }

        public override void UpdateUtility()
        {
            float e = (float)Math.E;
            float d = Vector2.SqrMagnitude(
                _chunk - new Vector2(
                    _actor.transform.position.x,
                    _actor.transform.position.z
                )
            );
            float t = (float)_actor.GetValue("_lastTimeChunkWasScouted").Table.Get(_chunk.ToString()).Number;
            _utility = 1.0f / (1.0f + Mathf.Pow(d, 2) / 5.0f * Mathf.Pow(e, -0.02f * t));
        }
    }
}