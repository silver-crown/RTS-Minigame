using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Yeeter
{
    /// <summary>
    /// Loads audio files.
    /// </summary>
    public class AudioLoader : MonoBehaviour
    {
        /// <summary>
        /// Contains information required to load a sound into the StreamingAssetsDatabase.
        /// </summary>
        public class AudioMetaData
        {
            /// <summary>
            /// The module from which the audio was loaded.
            /// </summary>
            public Module Mod;
            /// <summary>
            /// The audio file path.
            /// </summary>
            public string Path;
            /// <summary>
            /// The audio file's key in the StreamingAssetsDatabase.
            /// </summary>
            public string Key;

            /// <summary>
            /// Constructs a new AudioMetaData object.
            /// </summary>
            /// <param name="mod">The module from which the audio was loaded.</param>
            /// <param name="path">The audio file path.</param>
            /// <param name="key">The audio file's key in the StreamingAssetsDatabase.</param>
            public AudioMetaData(Module mod, string path, string key)
            {
                Mod = mod;
                Path = path;
                Key = key;
            }
        }

        private int _count;

        /// <summary>
        /// Maps audio meta data to audio clips.
        /// </summary>
        public Dictionary<AudioMetaData, AudioClip> Clips { get; private set; }
        /// <summary>
        /// Invoked when all audio files are done loading.
        /// </summary>
        public Action OnLoadingDone { get; set; }

        /// <summary>
        /// Loads audio files.
        /// </summary>
        /// <param name="files">The files to load.</param>
        public void LoadFiles(List<AudioMetaData> files)
        {
            Clips = new Dictionary<AudioMetaData, AudioClip>();
            _count = files.Count;
            InGameDebug.Log("\tAudioLoader.LoadFiles(): Count = " + _count + ".");
            foreach (var file in files) LoadFile(file);
        }
        /// <summary>
        /// Loads a file.
        /// </summary>
        /// <param name="file">The file to load.</param>
        private void LoadFile(AudioMetaData file)
        {
            if (HasValidExtension(file))
            {
                InGameDebug.Log("\t\tLoading '" + file.Path + "'.");
                StartCoroutine(StreamFile(file));
            }
            else
            {
                InGameDebug.Log("\t\tAudioLoader: '" + file + "' has invalid extension.");
            }
        }
        /// <summary>
        /// Checks if a file has a valid extension.
        /// </summary>
        /// <param name="file">The file to extension.</param>
        /// <returns>True if the file has a valid extension, false otherwise.</returns>
        private bool HasValidExtension(AudioMetaData file)
        {
            var extension = Path.GetExtension(file.Path);
            return extension == ".ogg" || extension == ".wav";
        }
        /// <summary>
        /// Streams an audio file.
        /// </summary>
        /// <param name="file">The file to stream.</param>
        /// <returns></returns>
        private IEnumerator<UnityWebRequestAsyncOperation> StreamFile(AudioMetaData file)
        {
            var path = file.Path.Replace('\\', '/');
            using (var www = UnityWebRequestMultimedia.GetAudioClip("file://" + path, GetAudioType(path)))
            {
                yield return www.SendWebRequest();
                if (www.isNetworkError)
                {
                    InGameDebug.Log(www.error);
                }
                else
                {
                    var clip = DownloadHandlerAudioClip.GetContent(www);
                    InGameDebug.Log("\t\t<color=green>AudioLoader: '" + path + "' loaded.</color>");
                    Clips.Add(file, clip);
                    if (Clips.Count == _count)
                    {
                        OnLoadingDone?.Invoke();
                    }
                }
            }
        }
        /// <summary>
        /// Gets a file's audio type.
        /// </summary>
        /// <param name="file">The file to get the audio type of.</param>
        /// <returns>The audio type.</returns>
        private AudioType GetAudioType(string file)
        {
            switch (Path.GetExtension(file))
            {
                case ".ogg":
                    return AudioType.OGGVORBIS;
                case ".wav":
                    return AudioType.WAV;
                default:
                    return AudioType.UNKNOWN;
            }
        }
    }
}