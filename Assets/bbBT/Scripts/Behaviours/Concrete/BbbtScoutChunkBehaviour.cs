using MoonSharp.Interpreter;
using RTS;
using ssuai;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Bbbt
{
    [CreateAssetMenu(
        fileName = "Scout Chunk",
        menuName = "bbBT/Behaviour/Leaf/Scout Chunk",
        order = 0)]
    public class BbbtScoutChunkBehaviour : BbbtLeafBehaviour
    {
        public override string SaveDataType { get; } = "BbbtScoutChunkBehaviour";

        private Vector2Int? _chunk;
        private Vector3 _destination;
        private float _lastTimeChunkScouted;

        protected override void OnInitialize(GameObject gameObject)
        {
            var actor = gameObject.GetComponent<Actor>();
            var chunkString = actor.GetValue("_chunkToScout").String.Trim(new char[] { '(', ')' });
            var parts = chunkString.Split(new char[] { ',' });
            int x = int.Parse(parts[0]);
            int y = int.Parse(parts[1]);
            _chunk = new Vector2Int(x, y);
            _destination = new Vector3((float)_chunk?.x, 0.0f, (float)_chunk?.y);
            _lastTimeChunkScouted = (float)actor.GetValue("_lastTimeChunkWasScouted").Table.Get(chunkString).Number;
            gameObject.GetComponent<NavMeshAgent>().SetDestination(_destination);
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {
        }

        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            var actor = gameObject.GetComponent<Actor>();
            if (_chunk != null)
            {
                if ((float)actor.GetValue("_lastTimeChunkWasScouted").Table.Get(_chunk.ToString()).Number !=
                    _lastTimeChunkScouted)
                {
                    actor.GetValue("_lastTimeChunkWasScouted").Table.Set(
                        _chunk.ToString(),
                        DynValue.NewNumber(Time.time));
                    return BbbtBehaviourStatus.Success;
                }
                else
                {
                    return BbbtBehaviourStatus.Running;
                }
            }
            else
            {
                return BbbtBehaviourStatus.Failure;
            }
        }
    }
}