using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Scripting;
using UnityEngine.UI;

namespace Yeeter
{
    enum AnchorPreset
    {
        TopLeft,
        TopCenter,
        TopRight,

        MiddleLeft,
        MiddleCenter,
        MiddleRight,

        BottomLeft,
        BottomCenter,
        BottomRight,

        VerticalStretchLeft,
        VerticalStretchCenter,
        VerticalStretchRight,

        HorizontalStretchTop,
        HorizontalStretchMiddle,
        HorizontalStretchBottom,

        StretchAll,
    }

    enum PivotPreset
    {
        TopLeft,
        TopCenter,
        TopRight,

        MiddleLeft,
        MiddleCenter,
        MiddleRight,

        BottomLeft,
        BottomCenter,
        BottomRight,
    }

    [Preserve, MoonSharpUserData]
    public class UI
    {
        public static DynValue Instantiate(string path)
        {
            return ObjectBuilder.InstantiateUIElement(path);
        }
        public static DynValue Instantiate(string path, float x, float y)
        {
            DynValue value = Instantiate(path);
            ObjectBuilder.SetPosition((int)value.Number, x, y);
            return value;
        }
        public static DynValue Create(string path)
        {
            return Instantiate(path);
        }
        public static DynValue Create(string path, float x, float y)
        {
            return Instantiate(path, x, y);
        }
        public static int CreateLuaObject(string path)
        {
            var id = ObjectBuilder.InstantiateUIElement();
            var component = ObjectBuilder.Get((int)id.Number).AddComponent<LuaObjectComponent>();
            component.Load(path);
            return (int)id.Number;
        }
        public static int CreateSlider(string settingsKey)
        {
            var id = (int)Instantiate("SliderSetting").Number;
            var go = ObjectBuilder.Get(id);
            var slider = go.GetComponent<SliderSetting>();
            slider.SetKey(settingsKey);
            return id;
        }
        public static void SetParent(int childId, int parentId)
        {
            ObjectBuilder.Get(childId).transform.SetParent(ObjectBuilder.Get(parentId).transform);
        }

        public static void SetAnchor(int id, string presetStr)
        {
            var preset = Enum.Parse(typeof(AnchorPreset), presetStr);
            var go = ObjectBuilder.Get(id);
            var transform = go.GetComponent<RectTransform>();
            switch (preset)
            {
                case AnchorPreset.TopLeft:
                    transform.anchorMin = new Vector2(0.0f, 1.0f);
                    transform.anchorMax = transform.anchorMin;
                    break;
                case AnchorPreset.TopCenter:
                    transform.anchorMin = new Vector2(0.5f, 1.0f);
                    transform.anchorMax = transform.anchorMin;
                    break;
                case AnchorPreset.TopRight:
                    transform.anchorMin = new Vector2(1.0f, 1.0f);
                    transform.anchorMax = transform.anchorMin;
                    break;
                case AnchorPreset.MiddleLeft:
                    transform.anchorMin = new Vector2(0.0f, 0.5f);
                    transform.anchorMax = transform.anchorMin;
                    break;
                case AnchorPreset.MiddleCenter:
                    transform.anchorMin = new Vector2(0.5f, 0.5f);
                    transform.anchorMax = transform.anchorMin;
                    break;
                case AnchorPreset.MiddleRight:
                    transform.anchorMin = new Vector2(1.0f, 0.5f);
                    transform.anchorMax = transform.anchorMin;
                    break;
                case AnchorPreset.BottomLeft:
                    transform.anchorMin = new Vector2(0.0f, 0.0f);
                    transform.anchorMax = transform.anchorMin;
                    break;
                case AnchorPreset.BottomCenter:
                    transform.anchorMin = new Vector2(0.5f, 0.0f);
                    transform.anchorMax = transform.anchorMin;
                    break;
                case AnchorPreset.BottomRight:
                    transform.anchorMin = new Vector2(1.0f, 0.0f);
                    transform.anchorMax = transform.anchorMin;
                    break;
                case AnchorPreset.VerticalStretchLeft:
                    transform.anchorMin = new Vector2(0.0f, 0.0f);
                    transform.anchorMax = new Vector2(0.0f, 1.0f);
                    break;
                case AnchorPreset.VerticalStretchCenter:
                    transform.anchorMin = new Vector2(0.5f, 0.0f);
                    transform.anchorMax = new Vector2(0.5f, 1.0f);
                    break;
                case AnchorPreset.VerticalStretchRight:
                    transform.anchorMin = new Vector2(1.0f, 0.0f);
                    transform.anchorMax = new Vector2(1.0f, 1.0f);
                    break;
                case AnchorPreset.HorizontalStretchTop:
                    transform.anchorMin = new Vector2(0.0f, 1.0f);
                    transform.anchorMax = new Vector2(1.0f, 1.0f);
                    break;
                case AnchorPreset.HorizontalStretchMiddle:
                    transform.anchorMin = new Vector2(0.0f, 0.5f);
                    transform.anchorMax = new Vector2(1.0f, 0.5f);
                    break;
                case AnchorPreset.HorizontalStretchBottom:
                    transform.anchorMin = new Vector2(0.0f, 0.0f);
                    transform.anchorMax = new Vector2(1.0f, 0.0f);
                    break;
                case AnchorPreset.StretchAll:
                    transform.anchorMin = new Vector2(0.0f, 0.0f);
                    transform.anchorMax = new Vector2(1.0f, 1.0f);
                    break;
            }
        }
        public static void SetAnchors(int id, float minX, float maxX, float minY, float maxY)
        {
            var go = ObjectBuilder.Get(id);
            var transform = go.GetComponent<RectTransform>();
            transform.anchorMin = new Vector2(minX, minY);
            transform.anchorMax = new Vector2(maxX, maxY);
        }
        public static void SetPivot(int id, string presetStr)
        {
            var preset = Enum.Parse(typeof(PivotPreset), presetStr);
            var go = ObjectBuilder.Get(id);
            var transform = go.GetComponent<RectTransform>();
            switch (preset)
            {
                case PivotPreset.TopLeft:
                    transform.pivot = new Vector2(0.0f, 1.0f);
                    break;
                case PivotPreset.TopCenter:
                    transform.pivot = new Vector2(0.5f, 1.0f);
                    break;
                case PivotPreset.TopRight:
                    transform.pivot = new Vector2(1.0f, 1.0f);
                    break;
                case PivotPreset.MiddleLeft:
                    transform.pivot = new Vector2(0.0f, 0.5f);
                    break;
                case PivotPreset.MiddleCenter:
                    transform.pivot = new Vector2(0.5f, 0.5f);
                    break;
                case PivotPreset.MiddleRight:
                    transform.pivot = new Vector2(1.0f, 0.5f);
                    break;
                case PivotPreset.BottomLeft:
                    transform.pivot = new Vector2(0.0f, 0.0f);
                    break;
                case PivotPreset.BottomCenter:
                    transform.pivot = new Vector2(0.5f, 0.0f);
                    break;
                case PivotPreset.BottomRight:
                    transform.pivot = new Vector2(1.0f, 0.0f);
                    break;
            }
        }
        public static void SetPivot(int id, float x, float y)
        {
            ObjectBuilder.Get(id).GetComponent<RectTransform>().pivot = new Vector2(x, y);
        }
        public static void SetSize(int id, float x, float y)
        {
            var transform = ObjectBuilder.Get(id).GetComponent<RectTransform>();
            transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, x);
            transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, y);
            Canvas.ForceUpdateCanvases();
        }
        public static void SetWidth(int id, float width)
        {
            var transform = ObjectBuilder.Get(id).GetComponent<RectTransform>();
            transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
            Canvas.ForceUpdateCanvases();
        }
        public static void SetHeight(int id, float height)
        {
            var transform = ObjectBuilder.Get(id).GetComponent<RectTransform>();
            transform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            Canvas.ForceUpdateCanvases();
        }
        public static void SetPosition(int id, float x, float y)
        {
            var transform = ObjectBuilder.Get(id).GetComponent<RectTransform>();
            transform.anchoredPosition = new Vector2(x, y);
        }
        public static void SetSizeDelta(int id, float x, float y)
        {
            var transform = ObjectBuilder.Get(id).GetComponent<RectTransform>();
            transform.sizeDelta = new Vector2(x, y);
        }

        public static void SetText(int id, string str)
        {
            var go = ObjectBuilder.Get(id);
            var text = go.GetComponentInChildren<Text>();
            if (text == null)
            {
                InGameDebug.Log("GameObject has no Text component, nor have any of its children.");
            }
            else
            {
                text.text = str;
            }
        }
        public static void SetTextAlignment(int id, string anchor)
        {
            var go = ObjectBuilder.Get(id);
            var text = go.GetComponent<Text>();
            if (text != null)
            {
                Enum.TryParse(anchor, out TextAnchor result);
                text.alignment = result;
            }
        }
        public static void SetAllTexts(int id, string str)
        {
            var go = ObjectBuilder.Get(id);
            var texts = go.GetComponentsInChildren<Text>();
            foreach (var text in texts) text.text = str;
        }
        public static void SetTextColor(int id, int r, int g, int b, int a)
        {
            var go = ObjectBuilder.Get(id);
            var text = go.GetComponentInChildren<Text>();
            if (text == null)
            {
                InGameDebug.Log("GameObject has no Text component, nor have any of its children.");
            }
            else
            {
                text.color = new Color(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);
            }
        }
        public static void SetFontSize(int id, int size)
        {
            var go = ObjectBuilder.Get(id);
            var text = go.GetComponentInChildren<Text>();
            if (text == null)
            {
                InGameDebug.Log("GameObject has no Text component, nor have any of its children.");
            }
            else
            {
                text.fontSize = size;
            }
        }
        public static void SetFont(int id, string fontName)
        {
            var go = ObjectBuilder.Get(id);
            var text = go.GetComponentInChildren<Text>();
            if (text == null)
            {
                InGameDebug.Log("GameObject has no Text component, nor have any of its children.");
            }
            else
            {
                text.font = Resources.Load<Font>("Fonts/" + fontName);
            }
        }
        public static void SetImage(int id, string key)
        {
            var go = ObjectBuilder.Get(id);
            var image = go.GetComponentInChildren<Image>();
            if (image == null)
            {
                image = go.AddComponent<Image>();
            }
            if (key == null)
            {
                image.sprite = null;
            }
            else
            {
                var texture = StreamingAssetsDatabase.GetTexture(key);
                image.sprite = Sprite.Create(
                    texture,
                    new Rect(0, 0, texture.width, texture.height),
                    new Vector2(0.5f, 0.5f)
                );
            }
        }
        public static void SetColor(int id, int r, int g, int b, int a = 255)
        {
            var go = ObjectBuilder.Get(id);
            var image = go.GetComponentInChildren<Image>();
            if (image != null)
            {
                image.color = new Color(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);
            }
        }

        public static void ForceUpdateCanvases()
        {
            Canvas.ForceUpdateCanvases();
        }

        public static void AddOnClick(int id, DynValue function)
        {
            var go = ObjectBuilder.Get(id);
            var button = go.GetComponent<Button>();
            button.onClick.AddListener(() => { LuaManager.GlobalScript.Call(function); });
        }
        public static void SetOnClick(int id, DynValue function)
        {
            var go = ObjectBuilder.Get(id);
            var button = go.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => { LuaManager.GlobalScript.Call(function); });
        }
        public static void ClearOnClick(int id)
        {
            var go = ObjectBuilder.Get(id);
            var button = go.GetComponent<Button>();
            button.onClick.RemoveAllListeners();
        }
        private static void AddEventTriggerEntry(int id, DynValue function, EventTriggerType type)
        {
            var go = ObjectBuilder.Get(id);
            var trigger = go.GetComponent<EventTrigger>();
            if (trigger == null) trigger = go.AddComponent<EventTrigger>();
            var entry = new EventTrigger.Entry();
            entry.eventID = type;
            entry.callback.AddListener(_ => LuaManager.Call(function));
            trigger.triggers.Add(entry);
        }
        public static void AddOnPointerEnter(int id, DynValue function)
        {
            AddEventTriggerEntry(id, function, EventTriggerType.PointerEnter);
        }
        public static void AddOnPointerExit(int id, DynValue function)
        {
            AddEventTriggerEntry(id, function, EventTriggerType.PointerExit);
        }

        public static Vector3[] GetWorldCorners(int id)
        {
            var go = ObjectBuilder.Get(id);
            var rectTransform = go.GetComponent<RectTransform>();
            var corners = new Vector3[4];
            rectTransform.GetWorldCorners(corners);
            return corners;
        }

        public static InputField GetInputField(int id)
        {
            return ObjectBuilder.Get(id).GetComponentInChildren<InputField>();
        }
    }
}