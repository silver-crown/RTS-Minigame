using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Progress.InputSystem
{
    public class ArmourBarUI : BarUI
    {
        [SerializeField]
        private List<Image> _armourIcons;

        [SerializeField]
        private ArmourController _armourController;

        private void Reset()
        {
            var horizontalGrid = GetComponentInChildren<HorizontalLayoutGroup>();
            _armourIcons = horizontalGrid.GetComponentsInChildren<Image>().ToList();
        }

        public override void OnSetupController(BarController controller)
        {
            _armourController = controller as ArmourController;
            _armourController.RegisterOnChanged(HandleStatChanged);

            var numOfArmourBlocks = _armourController.CurrentValue;
            HandleStatChanged(_armourController.Unit, _armourController.CurrentValue);
        }

        private void HandleStatChanged(Unit unit, int stat)
        {
            for(int i=0; i<_armourIcons.Count; i++)
            {
                var armourIcon = _armourIcons[i];

                if (i <= stat)
                {
                    armourIcon.enabled = true;
                }
                else
                {
                    armourIcon.enabled = false;
                }
            }
        }
    }
}
