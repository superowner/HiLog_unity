﻿/*////////////////////////////////////////////////////////////////////////////////////
 *  * Description: hidebug's view logic
 *  
 * Author: hiramtan@live.com
 *////////////////////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public partial class HiDebug : MonoBehaviour
{
    private static float _buttonWidth = 0.2f;
    private static float _buttonHeight = 0.1f;
    private static float _panelHeight = 0.7f;//0.3 is for stack

    private enum EDisplay
    {
        Button,//switch button
        Panel,//log panel
    }
    private EDisplay _eDisplay = EDisplay.Button;

    void OnGUI()
    {
        Button();
        Panel();
    }
}

//button
public partial class HiDebug : MonoBehaviour
{
    private enum EMouse
    {
        Up,
        Down,
    }
    private readonly float _mouseClickTime = 0.2f;//less than this is click
    private EMouse _eMouse = EMouse.Up;
    private float _mouseDownTime;
    private Rect _rect = new Rect(0, 0, Screen.width * _buttonWidth, Screen.height * _buttonHeight);
    void Button()
    {
        if (_eDisplay != EDisplay.Button)
        {
            return;
        }
        if (_rect.Contains(Event.current.mousePosition))
        {
            if (Event.current.type == EventType.MouseDown)
            {
                _eMouse = EMouse.Down;
                _mouseDownTime = Time.realtimeSinceStartup;
            }
            else if (Event.current.type == EventType.MouseUp)
            {
                _eMouse = EMouse.Up;
                if (Time.realtimeSinceStartup - _mouseDownTime < _mouseClickTime)//click
                { _eDisplay = EDisplay.Panel; }
            }
        }
        if (_eMouse == EMouse.Down && Event.current.type == EventType.mouseDrag)
        {
            _rect.x = Event.current.mousePosition.x - _rect.width / 2f;
            _rect.y = Event.current.mousePosition.y - _rect.height / 2f;
        }
        GUI.Button(_rect, "On", GetGUISkin(GUI.skin.button, Color.white, TextAnchor.MiddleCenter));
    }
}

//panel
public partial class HiDebug : MonoBehaviour
{
    private bool _isLogOn = true;
    private bool _isWarnningOn = true;
    private bool _isErrorOn = true;

    void Panel()
    {
        if (_eDisplay != EDisplay.Panel)
            return;

        GUI.Window(0, new Rect(0, 0, Screen.width, Screen.height * _panelHeight), LogWindow, "HiDebug", GetGUISkin(GUI.skin.window, Color.white, TextAnchor.UpperCenter));
        GUI.Window(1, new Rect(0, Screen.height * _panelHeight, Screen.width, Screen.height * (1 - _panelHeight)), StackWindow, "Stack", GetGUISkin(GUI.skin.window, Color.white, TextAnchor.UpperCenter));
    }
    private Vector2 _scrollLogPosition;
    void LogWindow(int windowID)
    {
        if (GUI.Button(new Rect(0, 0, Screen.width * _buttonWidth, Screen.height * _buttonHeight), "Clear", GetGUISkin(GUI.skin.button, Color.white, TextAnchor.MiddleCenter)))
        {
            logInfos.Clear();
            _stackInfo = null;
        }
        if (GUI.Button(new Rect(Screen.width * (1 - _buttonWidth), 0, Screen.width * _buttonWidth, Screen.height * _buttonHeight), "Close", GetGUISkin(GUI.skin.button, Color.white, TextAnchor.MiddleCenter)))
        {
            _eDisplay = EDisplay.Button;
        }
        var headHeight = GUI.skin.window.padding.top;//height of head
        var logStyle = GetGUISkin(GUI.skin.toggle, Color.white, TextAnchor.UpperLeft);
        _isLogOn = GUI.Toggle(new Rect(Screen.width * 0.3f, headHeight, Screen.width * _buttonWidth, Screen.height * _buttonHeight - headHeight), _isLogOn, "Log", logStyle);
        var WarnningStyle = GetGUISkin(GUI.skin.toggle, Color.yellow, TextAnchor.UpperLeft);
        _isWarnningOn = GUI.Toggle(new Rect(Screen.width * 0.5f, headHeight, Screen.width * _buttonWidth, Screen.height * _buttonHeight - headHeight), _isWarnningOn, "Warnning", WarnningStyle);
        var errorStyle = GetGUISkin(GUI.skin.toggle, Color.red, TextAnchor.UpperLeft);
        _isErrorOn = GUI.Toggle(new Rect(Screen.width * 0.7f, headHeight, Screen.width * _buttonWidth, Screen.height * _buttonHeight - headHeight), _isErrorOn, "Error", errorStyle);

        GUILayout.Space(Screen.height * _buttonHeight - headHeight);
        _scrollLogPosition = GUILayout.BeginScrollView(_scrollLogPosition);
        LogItem();
        GUILayout.EndScrollView();
    }

    void LogItem()
    {
        for (int i = 0; i < logInfos.Count; i++)
        {
            if (logInfos[i].Type == LogType.Log)
            {
                if (!_isLogOn)
                    continue;
            }
            else if (logInfos[i].Type == LogType.Warning)
            {
                if (!_isWarnningOn)
                    continue;
            }
            else if (logInfos[i].Type == LogType.Error)
            {
                if (!_isErrorOn)
                    continue;
            }
            if (GUILayout.Button(logInfos[i].Condition, GetGUISkin(GUI.skin.button, GetColor(logInfos[i].Type), TextAnchor.MiddleLeft)))
            {
                _stackInfo = logInfos[i];
            }
        }
    }
    private Vector2 _scrollStackPosition;
    void StackWindow(int windowID)
    {
        _scrollStackPosition = GUILayout.BeginScrollView(_scrollStackPosition);
        StackItem();
        GUILayout.EndScrollView();
    }

    void StackItem()
    {
        if (_stackInfo == null)
            return;
        var strs = _stackInfo.StackTrace.Split('\n');
        for (int i = 0; i < strs.Length; i++)
        {
            GUILayout.Label(strs[i], GetGUISkin(GUI.skin.label, GetColor(_stackInfo.Type), TextAnchor.MiddleLeft));
        }
    }

    List<LogInfo> logInfos = new List<LogInfo>();
    private LogInfo _stackInfo;
    public void UpdateLog(LogInfo logInfo)
    {
        logInfos.Add(logInfo);
    }

    GUIStyle GetGUISkin(GUIStyle guiStyle, Color color, TextAnchor style)
    {
        guiStyle.normal.textColor = color;
        guiStyle.hover.textColor = color;
        guiStyle.active.textColor = color;
        guiStyle.onNormal.textColor = color;
        guiStyle.onHover.textColor = color;
        guiStyle.onActive.textColor = color;
        guiStyle.margin = new RectOffset(0, 0, 0, 0);
        guiStyle.alignment = style;
        guiStyle.fontSize = Debuger.FontSize;
        return guiStyle;
    }

    Color GetColor(LogType type)
    {
        if (type == LogType.Log)
            return Color.white;
        if (type == LogType.Warning)
            return Color.yellow;
        if (type == LogType.Error)
            return Color.red;
        if (type == LogType.Exception)
            return Color.red;
        return Color.white;
    }
}



