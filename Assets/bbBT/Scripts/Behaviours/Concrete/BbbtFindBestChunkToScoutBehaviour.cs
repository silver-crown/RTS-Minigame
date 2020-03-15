using MoonSharp.Interpreter;
using RTS;
using ssuai;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Bbbt
{
    [CreateAssetMenu(
        fileName = "Find Best Chunk To Scout",
        menuName = "bbBT/Behaviour/Leaf/Find Best Chunk To Scout",
        order = 0)]
    public class BbbtFindBestChunkToScoutBehaviour : BbbtLeafBehaviour
    {
        public override string SaveDataType { get; } = "BbbtFindBestChunkToScoutBehaviour";

        private Vector2Int? _chunk;

        protected override void OnInitialize(GameObject gameObject)
        {
            _chunk = null;
            var actor = gameObject.GetComponent<Actor>();
            var utilityActions = new List<UtilityAction>();

            foreach (var chunk in WorldInfo.Chunks)
            {
                utilityActions.Add(new UtilityAction(
                    new List<Factor>()
                    {
                        new ChunkScoutingAttractiveness(chunk, actor)
                    },
                    () =>
                    {
                        _chunk = chunk;
                    }
                ));
            }

            UtilityAction bestAction = null;
            float highestUtility = -Mathf.Infinity;
            foreach (var action in utilityActions)
            {
                float utility = action.GetUtility();
                if (utility > highestUtility)
                {
                    bestAction = action;
                    highestUtility = utility;
                }
            }

            bestAction?.Behaviour.Invoke();
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {
        }

        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            if (_chunk != null)
            {
                gameObject.GetComponent<Actor>().SetValue("_chunkToScout", DynValue.NewString(_chunk.ToString()));
                return BbbtBehaviourStatus.Success;
            }
            else
            {
                return BbbtBehaviourStatus.Failure;
            }
        }
    }
}