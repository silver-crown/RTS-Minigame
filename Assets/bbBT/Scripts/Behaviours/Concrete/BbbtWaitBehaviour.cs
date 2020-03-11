using Newtonsoft.Json;
using UnityEngine;

namespace Bbbt
{
    /// <summary>
    /// Behaviour which just makes an actor do nothing for x seconds.
    /// </summary>
    [CreateAssetMenu(fileName = "Wait", menuName = "bbBT/Behaviour/Leaf/Wait", order = 0)]
    public class BbbtWaitBehaviour : BbbtLeafBehaviour
    {
        [JsonProperty, SerializeField] private float _waitTime = 5.0f;
        private float _timeToStopWaiting = Mathf.Infinity;

        public override string SaveDataType => "BbbtWaitBehaviour";

        protected override void OnInitialize(GameObject gameObject)
        {
            _timeToStopWaiting = Time.time + _waitTime;
        }

        protected override void OnTerminate(GameObject gameObject, BbbtBehaviourStatus status)
        {
        }

        protected override BbbtBehaviourStatus UpdateBehaviour(GameObject gameObject)
        {
            if (Time.time >= _timeToStopWaiting)
            {
                return BbbtBehaviourStatus.Success;
            }
            else
            {
                return BbbtBehaviourStatus.Running;
            }
        }
    }
}