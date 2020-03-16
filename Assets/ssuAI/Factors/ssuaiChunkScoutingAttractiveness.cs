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

        public override void UpdateUtility()
        {
            float e = (float)Math.E;
            float dSqr = Vector2.SqrMagnitude(
                _chunk - new Vector2(
                    _actor.transform.position.x,
                    _actor.transform.position.z
                )
            );
            float t =
                Time.time - (float)_actor.GetValue("_lastTimeChunkWasScouted").Table.Get(_chunk.ToString()).Number;
            if (t == 0.0f)
            {
                _utility = 0.0f;
            }
            else
            {
                _utility = 1.0f / (1.0f + (100.0f + dSqr) * Mathf.Pow(e, -0.2f * t));
            }
        }
    }
}