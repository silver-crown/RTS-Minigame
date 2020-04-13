using System;
using System.Collections;
using UnityEngine;

namespace Yeeter
{
    public class Timer : MonoBehaviour
    {
        public float Interval { get; set; } = 1.0f;
        public Action OnTick { get; set; }

        private void Start()
        {
            StartCoroutine(Tick());
        }

        private IEnumerator Tick()
        {
            while (true)
            {
                yield return new WaitForSeconds(Interval);
                OnTick?.Invoke();
            }
        }
    }
}