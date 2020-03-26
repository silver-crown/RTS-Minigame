using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

namespace Yeeter
{
    public class AudioLoader : MonoBehaviour
    {
        public class AudioMetaData
        {
            public Module Mod;
            public string Path;
            public string Key;

            public AudioMetaData(Module mod, string path, string key)
            {
                Mod = mod;
                Path = path;
                Key = key;
            }
        }
        private int _count;

        public Dictionary<AudioMetaData, AudioClip> Clips { get; private set; }
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

        private bool HasValidExtension(AudioMetaData file)
        {
            var extension = Path.GetExtension(file.Path);
            return extension == ".ogg" || extension == ".wav";
        }

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