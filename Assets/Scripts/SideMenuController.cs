using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rewired;

public class SideMenuController : MonoBehaviour
{
    private int playerId = 0;
    private Player player;
    private GameObject sideMenu;

    private int selectedButtonIndex;
    private List<Button> buttons;
    private ControllerMapEnabler.RuleSet ruleset;

    private bool upKeyPressed;
    private bool downKeyPressed;
    private bool menuKeyPressed;
    // Start is called before the first frame update
    private void Awake()
    {
        player = ReInput.players.GetPlayer(playerId);
        sideMenu = GetComponentInChildren<VerticalLayoutGroup>().gameObject;
        buttons = new List<Button>(GetComponentsInChildren<Button>() as Button[]);
        ruleset = ReInput.mapping.GetControllerMapEnablerRuleSetInstance("SideMenuRules");
        selectedButtonIndex = 0;
        buttons[selectedButtonIndex].Select();
    }

    // Update is called once per frame
    void Update()
    {
        GetInput();
        ProcessInput();
    }

    private void GetInput ()
    {
        upKeyPressed = player.GetButtonDown("Move UI Up");
        downKeyPressed = player.GetButtonDown("Move UI Down");
        menuKeyPressed = player.GetButtonDown("ToggleSideMenu");
    }

    private void ProcessInput ()
    {
        if (upKeyPressed && selectedButtonIndex > 0)
        {
            selectedButtonIndex--;
            buttons[selectedButtonIndex].Select();
        }
        if (downKeyPressed && selectedButtonIndex < (buttons.ToArray().Length - 1))
        {
            selectedButtonIndex++;
            buttons[selectedButtonIndex].Select();
        }
        if (menuKeyPressed)
        {
            if (sideMenu.activeSelf)
            {
                sideMenu.SetActive(false);
                player.controllers.maps.mapEnabler.ruleSets.Remove(ruleset);
                ruleset.rules[0].enable = false;
                player.controllers.maps.mapEnabler.ruleSets.Add(ruleset);
                player.controllers.maps.mapEnabler.Apply();
            }
            else
            {
                sideMenu.SetActive(true);
                player.controllers.maps.mapEnabler.ruleSets.Remove(ruleset);
                ruleset.rules[0].enable = true;
                player.controllers.maps.mapEnabler.ruleSets.Add(ruleset);
                player.controllers.maps.mapEnabler.Apply();
            }
        }
    }
}
