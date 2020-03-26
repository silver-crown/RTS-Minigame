using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Progress.InputSystem
{
    public class TestUpdateUI : MonoBehaviour
    {
        [SerializeField]
        private HealthController _target;

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space) && _target != null)
            {
                _target.Stat = _target.Stat - 1;
            }
        }
    }
}
