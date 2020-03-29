using MoonSharp.Interpreter;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

namespace Yeeter
{
    [Preserve, MoonSharpUserData]
    public class SoundManager
    {
        private static bool _initialized = false;
        private static float _musicVolume = 1.0f;
        private static float _effectsVolume = 1.0f;
        private static int _nextId = 0;
        private static Dictionary<int, AudioSource> _audioSources = new Dictionary<int, AudioSource>();
        private static Dictionary<int, AudioSource> _effectSources = new Dictionary<int, AudioSource>();
        private static Dictionary<int, AudioSource> _musicSources = new Dictionary<int, AudioSource>();
        private static Dictionary<AudioSource, float> _unmodifiedAudioSourceVolumes = new Dictionary<AudioSource, float>();


        public static void Initialize()
        {
            if (_initialized) return;
            StreamingAssetsDatabase.AddOnSettingChangedListener("musicVolume", v =>
            {
                var oldVolume = _musicVolume;
                var newVolume = float.Parse(v);
                _musicVolume = newVolume;
                foreach (var source in _musicSources.Values)
                {
                    source.volume = _unmodifiedAudioSourceVolumes[source] * _musicVolume;
                }
            });
            StreamingAssetsDatabase.AddOnSettingChangedListener("effectsVolume", v =>
            {
                var oldVolume = _effectsVolume;
                var newVolume = float.Parse(v);
                _effectsVolume = newVolume;
                foreach (var source in _effectSources.Values)
                {
                    source.volume = _unmodifiedAudioSourceVolumes[source] * _effectsVolume;
                }
            });

            _musicVolume = float.Parse(StreamingAssetsDatabase.GetSetting("musicVolume"));
            _effectsVolume = float.Parse(StreamingAssetsDatabase.GetSetting("effectsVolume"));
            _initialized = true;
        }

        public static int CreateAudioSource(string clipKey)
        {
            var source = GameObject.Instantiate(new GameObject()).AddComponent<AudioSource>();
            var clip = StreamingAssetsDatabase.GetSound(clipKey);
            if (clip == null)
            {
                InGameDebug.Log("Couldn't find clip " + clipKey + ".");
            }
            source.clip = clip;
            _audioSources[_nextId] = source;
            _unmodifiedAudioSourceVolumes[source] = 1.0f;
            return _nextId;
        }

        public static void Set(int id, string key, DynValue value)
        {
            if (!_audioSources.ContainsKey(id))
            {
                InGameDebug.Log("SoundManager.Set(): No AudioSource with id " + id + ".");
                return;
            }
            var audioSource = _audioSources[id];
            switch (key.ToLower())
            {
                case "clip":
                    audioSource.clip = StreamingAssetsDatabase.GetSound(value.String);
                    break;
                case "loop":
                    audioSource.loop = value.Boolean;
                    break;
                case "volume":
                    _unmodifiedAudioSourceVolumes[audioSource] = (float)value.Number;
                    if (_musicSources.ContainsKey(id))
                    {
                        audioSource.volume = _unmodifiedAudioSourceVolumes[audioSource] * _musicVolume;
                    }
                    if (_musicSources.ContainsKey(id))
                    {
                        audioSource.volume = _unmodifiedAudioSourceVolumes[audioSource] * _effectsVolume;
                    }
                    break;
                case "pitch":
                    audioSource.pitch = (float)value.Number;
                    break;
                case "mute":
                    audioSource.mute = value.Boolean;
                    break;
                case "time":
                    audioSource.time = (float)value.Number;
                    break;
            }
        }

        public static void PlayMusic(int id)
        {
            if (!_audioSources.ContainsKey(id))
            {
                InGameDebug.Log("SoundManager.Play(): No AudioSource with id " + id + ".");
                return;
            }
            _audioSources[id].volume = _unmodifiedAudioSourceVolumes[_audioSources[id]] * _musicVolume;
            _audioSources[id].Play();
            _musicSources[id] = _audioSources[id];
        }
        public static void PlayMusic(string file)
        {
            int id = CreateAudioSource(file);
            _audioSources[id].volume = _unmodifiedAudioSourceVolumes[_audioSources[id]] * _musicVolume;
            _audioSources[id].loop = true;
            _audioSources[id].Play();
            _musicSources[id] = _audioSources[id];
        }
        public static void PlayEffect(int id)
        {
            if (!_audioSources.ContainsKey(id))
            {
                InGameDebug.Log("SoundManager.Play(): No AudioSource with id " + id + ".");
                return;
            }
            _audioSources[id].volume = _unmodifiedAudioSourceVolumes[_audioSources[id]] * _effectsVolume;
            _audioSources[id].Play();
            _effectSources[id] = _audioSources[id];
        }
        public static void PlayEffect(string file)
        {
            int id = CreateAudioSource(file);
            _audioSources[id].volume = _unmodifiedAudioSourceVolumes[_audioSources[id]] * _effectsVolume;
            _audioSources[id].Play();
            _musicSources[id] = _audioSources[id];
        }
    }
}