using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

namespace Progress.InputSystem
{
    public class ColumnMenuState : MenuState
    {
        private int playerId = 0;
        private Player player;

        [SerializeField]
        private GameObject LeftMenu;
        [SerializeField]
        private GameObject BottomMenu;
        [SerializeField]
        private GameObject RightMenu;

        private int selectedColumnIndex;
        private int selectedButtonIndex;
        private List<VerticalLayoutGroup> columns;

        private void Awake()
        {
            player = ReInput.players.GetPlayer(playerId);
            columns = new List<VerticalLayoutGroup>(GetComponentsInChildren<VerticalLayoutGroup>());
            selectedColumnIndex = 0;
            selectedButtonIndex = 0;
            Button button = columns[selectedColumnIndex].GetComponentsInChildren<Button>()[selectedButtonIndex];
            if (button != null && button.IsActive())
            {
                button.Select();
            }
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            //  If any panels should be disabled, do so first
            if (player.GetButtonDown("ToggleLeftMenu"))
            {
                LeftMenu.SetActive(!LeftMenu.activeInHierarchy);
            }
            if (player.GetButtonDown("ToggleBottomMenu"))
            {
                BottomMenu.SetActive(!BottomMenu.activeInHierarchy);
            }
            if (player.GetButtonDown("ToggleRightMenu"))
            {
                RightMenu.SetActive(!RightMenu.activeInHierarchy);
            }

            //  If currently selected column is disabled, go through columns and select first button in first active column found, if any.
            if (!columns[selectedColumnIndex].IsActive())
            {
                System.Predicate<VerticalLayoutGroup> isActive =
                x => x.IsActive();
                int activeColumnIndex = columns.FindIndex(isActive);
                if (activeColumnIndex != -1)
                {
                    selectedColumnIndex = activeColumnIndex;
                    selectedButtonIndex = 0;
                    Button button = columns[selectedColumnIndex].GetComponentsInChildren<Button>()[selectedButtonIndex];
                    if (button != null && button.IsActive())
                    {
                        button.Select();
                    }
                }
            }

            //  Get input and call functions to process it
            if (player.GetButtonDown("Move UI Up"))
            {
                MoveUp();
            }

            if (player.GetButtonDown("Move UI Down"))
            {
                MoveDown();
            }

            if (player.GetButtonDown("Move UI Left"))
            {
                MoveLeft();
            }

            if (player.GetButtonDown("Move UI Right"))
            {
                MoveRight();
            }
        }

        private void MoveUp()
        {
            //  If current column is enabled and current button index > 0, decrement selectedButtonIndex and call Select on the button with that index.
            if (columns[selectedColumnIndex].IsActive() && selectedButtonIndex > 0)
            {
                Button button = columns[selectedColumnIndex].GetComponentsInChildren<Button>()[--selectedButtonIndex];
                if (button != null && button.IsActive())
                {
                    button.Select();
                }
            }
        }

        private void MoveDown()
        {
            //  If current column is enabled and current button index < (column length - 1), increment selectedButtonIndex and call Select on the button with that index
            if (columns[selectedColumnIndex].IsActive() && selectedButtonIndex < (columns[selectedColumnIndex].GetComponentsInChildren<Button>().Length - 1))
            {
                Button button = columns[selectedColumnIndex].GetComponentsInChildren<Button>()[++selectedButtonIndex];
                if (button != null && button.IsActive())
                {
                    button.Select();
                }
            }
        }

        private void MoveLeft()
        {
            //  Search for an enabled column left of this one (with a smaller index). If none is found, nothing happens. If one is found, check that selectedButtonIndex is less than
            //  that column's Length. If it is, select the button in that column with index selectedButtonIndex. If not, select button with index 0.
            for (int i = selectedColumnIndex - 1; i >= 0; i--)
            {
                if (columns[i].IsActive())
                {
                    Button[] column = columns[i].GetComponentsInChildren<Button>();
                    if (selectedButtonIndex < column.Length)
                    {
                        column[selectedButtonIndex].Select();
                        selectedColumnIndex = i;
                        return;
                    }
                    else
                    {
                        column[0].Select();
                        selectedColumnIndex = i;
                        selectedButtonIndex = 0;
                        return;
                    }
                }
            }
        }

        private void MoveRight()
        {
            //  Search for an enabled column right of this one (with a greater index). If none is found, nothing happens. If one is found, check that selectedButtonIndex is less than
            //  that column's Length. If it is, select the button in that column with index selectedButtonIndex. If not, select button with index 0.
            for (int i = selectedColumnIndex + 1; i < columns.ToArray().Length; i++)
            {
                if (columns[i].IsActive())
                {
                    Button[] column = columns[i].GetComponentsInChildren<Button>();
                    if (selectedButtonIndex < column.Length)
                    {
                        column[selectedButtonIndex].Select();
                        selectedColumnIndex = i;
                        return;
                    }
                    else
                    {
                        column[0].Select();
                        selectedColumnIndex = i;
                        selectedButtonIndex = 0;
                        return;
                    }
                }
            }
        }
    }
}
