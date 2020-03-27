using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Yeeter
{
    public class CustomGameLoader : MonoBehaviour
    {
        [Tooltip("The text on which to display epic lmao loading messages.")]
        [SerializeField] private Text _text = null;
        [Tooltip("Anchors should be set to min = 0, 0, max = 0, 1. Size delta should be 0." +
        "Should be a rect contained within the area that defines the progress bar's size at max progress.")]
        [SerializeField] private RectTransform _loadingProgressBar;

        private int _step = 0;
        //private int _maxStep = 8;
        private GameObject _console;

        private int Step
        {
            get => _step;
            set 
            { 
                _step = value;
                // Wasn't entirely happy with how the progress bar turned out. I'll probably add it back later.
                //_loadingProgressBar.anchorMax = new Vector2((float)_step / _maxStep, 1.0f); 
            }
        }
        private void Awake()
        {
            _console = ObjectBuilder.Get((int)UI.Create("Console").Number);
            _console.SetActive(false);
        }

        private void Update()
        {
            switch (_step)
            {
                case 0:
                    LuaManager.ActiveLuaManager = new CustomLuaManager();
                    _text.text = "Loading mods...";
                    StreamingAssetsDatabase.LoadModules();
                    Step = 1;
                    return;
                case 1:
                    _text.text = "Throwing out unused mods...";
                    StreamingAssetsDatabase.LoadActiveModules();
                    Step = 2;
                    return;
                case 2:
                    _text.text = "Loading textures...";
                    StreamingAssetsDatabase.LoadTexturesFromActiveModules();
                    Step = 3;
                    return;
                case 3:
                    _text.text = "Loading scripts...";
                    StreamingAssetsDatabase.LoadScriptsFromActiveModules();
                    Step = 4;
                    return;
                case 4:
                    _text.text = "Loading sounds...";
                    bool soundsLoaded = StreamingAssetsDatabase.LoadSoundsFromActiveModules();
                    if (!soundsLoaded)
                    {
                        Step = 5;
                    }
                    else
                    {
                        StreamingAssetsDatabase.OnSoundsDoneLoading += () =>
                        {
                            Step = 5;
                        };
                    }
                    return;
                case 5:
                    _text.text = "Loading defs...";
                    StreamingAssetsDatabase.LoadDefsFromActiveModules();
                    BBInput.LoadProfiles();
                    BBInput.Initialize();
                    FloorBuilder.LoadTileTypes();
                    Step = 6;
                    return;
                case 6:
                    _text.text = "Loading settings...";
                    StreamingAssetsDatabase.LoadSettings();
                    Step = 7;
                    return;
                case 7:
                    SoundManager.Initialize();
                    _text.text = "Loading Main Menu...";
                    Step = 8;
                    if (SceneManager.sceneCountInBuildSettings > 1)
                    {
                        SceneManager.LoadScene(1);
                    }
                    else
                    {
                        InGameDebug.Log(
                            "<color=red><b>No scene with build index 1.</b></color> Go into build settings " +
                            "(Ctrl + Shift + B or 'File > Build Settings...' in the Unity editor) and add " +
                            "the scene you wish to load. " +
                            "If it is not already, the loading scene should also be added with build index 0.");
                    }
                    return;
                case 8:
                    //_loadingProgressBar.GetComponent<Image>().color = new Color(0.9f, 0.2f, 0.3f);
                    _text.text = "Well, this is awkward.\nIt seems the main menu is missing.\nSee console for info.";
                    _console.SetActive(true);
                    return;
            }
        }
    }
}