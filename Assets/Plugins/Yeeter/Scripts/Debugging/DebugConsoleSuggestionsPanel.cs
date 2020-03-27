using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Yeeter
{
    /// <summary>
    /// Panel which shows suggestions when something is typed into the console input field.
    /// </summary>
    public class DebugConsoleSuggestionsPanel : MonoBehaviour
    {
        private static Dictionary<string, List<string>> _bindings;

        /// <summary>The input field to listen to.</summary>
        [SerializeField] private InputField _inputField = null;
        /// <summary>The prefab to instantiate when we add a suggestion to the panel.</summary>
        [SerializeField] private GameObject _suggestionsPanelEntryPrefab = null;
        /// <summary></summary>
        [SerializeField] private DebugConsoleInputField _debugConsoleInputField = null;

        private void Awake()
        {
            LoadBindings();
            Disable();
            _inputField.onValueChanged.AddListener(Suggest);
            _debugConsoleInputField.OnCommandDone += OnDebugConsoleInputFieldCommandDone;
            // TODO: Add input.
        }

        /// <summary>
        /// Loads the Lua bindings from Bindings.generated.lua
        /// </summary>
        private void LoadBindings()
        {
            // Load the bindings.
            string path = Path.Combine(
                Application.streamingAssetsPath,
                "Data",
                "Core",
                "Scripts",
                "Generated",
                "Bindings.generated.lua");
            var lines = File.ReadAllLines(path).ToList();
            _bindings = new Dictionary<string, List<string>>();
            var aliasNameToClassName = new List<KeyValuePair<string, string>>();

            _bindings.Add("", new List<string>());
            // Sort out lines that aren't bindings and categorise the bindings.
            foreach (var line in lines)
            {
                // Skip comments and empty lines
                if (line.StartsWith("--")) continue;
                if (line == "") continue;
                if (line.Contains("function"))
                {
                    // Method
                    var fullMethodName = line.Split(' ')[0];
                    var fullMethodNameParts = fullMethodName.Split('.');
                    var methodName = fullMethodNameParts[fullMethodNameParts.Length - 1];
                    var className = fullMethodName.Replace("." + methodName, "");
                    var leftParenIndex = line.IndexOf('(');
                    var rightParenIndex = line.IndexOf(')');
                    _bindings[className].Add(methodName);
                }
                else if (line.Contains(" = "))
                {
                    if (line.Contains("= {}"))
                    {
                        // Class
                        var className = line.Split(' ')[0];
                        _bindings.Add(className, new List<string>());
                    }
                    else // if line.Contains("alias = class")
                    {
                        // Alias
                        var parts = line.Split(' ');
                        var aliasName = parts[0];
                        var className = parts[2];
                        _bindings[aliasName] = _bindings[className];
                    }
                }
            }
        }
        /// <summary>
        /// Prints bindings that start with the text in the input field.
        /// </summary>
        /// <param name="text">The text value.</param>
        private void Suggest(string text)
        {
            Clear();
            if (_inputField.caretPosition < text.Length)
            {
                text = text.Remove(_inputField.caretPosition);
            }
            var currentWord = GetLastWord(text);
            // Skip if the text is empty.
            if (currentWord == "")
            {
                Disable();
                return;
            }

            gameObject.SetActive(true);
            // Add entries
            var parts = currentWord.Split('.');
            var entries = new List<string>();
            if (parts.Length == 1)
            {
                // Look for class that starts with text.
                foreach (var className in _bindings.Keys)
                {
                    if (className.StartsWith(currentWord))
                    {
                        entries.Add(className);
                    }
                }
            }
            else
            {
                // Look for method that starts with text.
                foreach (var className in _bindings.Keys)
                {
                    if (className == parts[0])
                    {
                        foreach (var methodName in _bindings[className])
                        {
                            if (methodName.StartsWith(parts[1]))
                            {
                                entries.Add(className + "." + methodName);
                            }
                        }
                    }
                }
            }

            // Instantiate the entries
            foreach (var entry in entries)
            {
                Instantiate(_suggestionsPanelEntryPrefab, transform).GetComponent<Text>().text = entry;
            }

            // Disable if there are no entries
            if (entries.Count == 0)
            {
                Disable();
            }

            Canvas.ForceUpdateCanvases();
        }
        /// <summary>
        /// Removes all children.
        /// </summary>
        private void Clear()
        {
            foreach (Transform child in transform)
            {
                Destroy(child.gameObject);
            }
        }
        /// <summary>
        /// Completes the current word.
        /// </summary>
        private void AutoComplete()
        {
            _inputField.text = _inputField.text.TrimEnd();
            var word = GetLastWord(_inputField.text);
            var parts = word.Split('.');
            var candidates = new List<string>();
            if (parts.Length == 1)
            {
                foreach (var className in _bindings.Keys)
                {
                    if (className.StartsWith(word))
                    {
                        candidates.Add(className.Remove(0, word.Length));
                    }
                }
            }
            else
            {
                // Look for method that starts with text.
                foreach (var className in _bindings.Keys)
                {
                    if (className == parts[0])
                    {
                        foreach (var methodName in _bindings[className])
                        {
                            if (methodName.StartsWith(parts[1]))
                            {
                                candidates.Add(methodName.Remove(0, parts[1].Length));
                            }
                        }
                    }
                }
            }
            if (candidates.Count == 0) return;
            int shortestCandidateLength = 99999999;
            foreach (var candidate in candidates)
            {
                if (candidate.Length < shortestCandidateLength)
                {
                    shortestCandidateLength = candidate.Length;
                }
            }
            for (int characterIndex = 0; characterIndex < shortestCandidateLength; characterIndex++)
            {
                bool allCandidatesMatchAtCharacterIndex = true;
                for (int candidateIndex = 0; candidateIndex < candidates.Count - 1; candidateIndex++)
                {
                    if (candidates[candidateIndex][characterIndex] != candidates[candidateIndex + 1][characterIndex])
                    {
                        allCandidatesMatchAtCharacterIndex = false;
                    }
                }
                if (allCandidatesMatchAtCharacterIndex)
                {
                    _inputField.text += candidates[0][characterIndex];
                }
                else
                {
                    break;
                }
            }
            _inputField.caretPosition = _inputField.text.Length;
            _inputField.ForceLabelUpdate();
            _inputField.onValueChanged?.Invoke(_inputField.text);
        }
        /// <summary>
        /// Disables the suggestion panel.
        /// </summary>
        private void Disable()
        {
            gameObject.SetActive(false);
        }
        /// <summary>
        /// Called when the is done with a command.
        /// </summary>
        /// <param name="command">The command that was run</param>
        private void OnDebugConsoleInputFieldCommandDone(string command)
        {
            // TODO: Checks if any binding aliases were added
            /* This didn't work. I'll try again some other time.
            var trimmed = command.Trim();
            var compacted = trimmed.Replace(" ", "");
            var splitByEquals = compacted.Split(new char[] { '=' });
            // Must be only one equals/two sides in expression.
            if (splitByEquals.Length != 2) return;
            // Must be only one word on each side.
            if (GetWords(splitByEquals[0]).Length != 1) return;
            if (GetWords(splitByEquals[1]).Length != 1) return;
            // Set alias.
            if (!_bindings.ContainsKey(splitByEquals[1])) return;
            _bindings[splitByEquals[0]] = _bindings[splitByEquals[1]];*/
        }
        /// <summary>
        /// Gets the last word in a string.
        /// Class is one word.
        /// Class.Method is one word.
        /// Namespace.Class.Method is one word.
        /// Class.Method(Class.Method) is two words.
        /// </summary>
        /// <param name="text">The text to find words in.</param>
        /// <returns>The words.</returns>
        private string[] GetWords(string text)
        {
            var separators = new char[] { ' ', '(', ')', ';', '-', '+', '*', '/', '%', '\'', '\"', '='};
            return text.Trim().Replace("..", " ").Split(separators);
        }
        /// <summary>
        /// Gets the last word in a string.
        /// Class is one word.
        /// Class.Method is one word.
        /// Namespace.Class.Method is one word.
        /// Class.Method(Class.Method) is two words.
        /// </summary>
        /// <param name="text">The text to find the word in.</param>
        /// <returns>The lsat word.</returns>
        private string GetLastWord(string text)
        {
            var words = GetWords(text);
            return words[words.Length - 1];
        }
    }
}