using System;
using System.Collections;
using System.Reflection;
using UnityEditor;
using UnityEngine;
using Ovr;

/// <summary>
/// VDPlaymodeBehavior is an editor behavior that positions the Game window onto the Rift monitor when entering playmode (Ctrl-P).
/// This allows a user using Virtual Desktop to code and playtest while staying in VR the entire time.
/// 
/// Copyright 2015 - Guy Godin
/// http://www.vrdesktop.net/
/// </summary>
[InitializeOnLoad]
public class VDPlaymodeBehavior : MonoBehaviour
{
    #region Static Fields
    private static readonly MethodInfo _getMainGameViewMethod = Type.GetType("UnityEditor.GameView,UnityEditor").GetMethod("GetMainGameView", BindingFlags.NonPublic | BindingFlags.Static);
    private static Hmd _hmd;
    #endregion

    #region Static Constructor
    static VDPlaymodeBehavior()
    {
        EditorApplication.playmodeStateChanged -= OnPlaymodeStateChanged;
        EditorApplication.playmodeStateChanged += OnPlaymodeStateChanged;
        EditorApplication.update -= OnUpdate;
        EditorApplication.update += OnUpdate;
    }
    #endregion
    
    #region Methods
    private static EditorWindow GetMainGameView()
    {
        // Execute the private GetMainGameView method
        return (EditorWindow)_getMainGameViewMethod.Invoke(null, null);
    }

    private static void ShowGameWindow()
    {
        // Make sure an hmd is connected
        if (_hmd == null)
        {
            _hmd = Hmd.Create(0);
            if (_hmd == null)
                return;
        }

        // Make sure it's in extended mode
        HmdDesc desc = _hmd.GetDesc();
        if ((desc.HmdCaps & (uint)HmdCaps.ExtendDesktop) == 0)
            return;

        // Get the hmd monitor position and size
        Vector2i hmdPosition = desc.WindowsPos;
        Sizei hmdResolution = desc.Resolution;

        // Open the game window
        EditorApplication.ExecuteMenuItem("Window/Game");
        EditorWindow gameView = GetMainGameView();
        if (gameView == null)
            return;

        // Adjust the game view position
        Rect newPos = new Rect(hmdPosition.x, hmdPosition.y + 17, hmdResolution.w, hmdResolution.h - 22);

        gameView.position = newPos;
        gameView.minSize = newPos.size;
        gameView.maxSize = newPos.size;
        gameView.position = newPos;

        // Toggle focused window so that Virtual Desktop gets the foreground change event
        EditorUtility.FocusProjectWindow();
        gameView.title = "Game (Stereo)";
        gameView.Focus();
    }

    private static void CloseGameWindow()
    {
        EditorWindow gameView = GetMainGameView();
        if (gameView != null)
        {
            gameView.Close();
        }
    }
    #endregion

    #region Event Handlers
    private static void OnPlaymodeStateChanged()
    {
        if (EditorApplication.isPlaying)
        {
            ShowGameWindow();
        }
        else
        {
            CloseGameWindow();
        }
    }

    private static void OnUpdate()
    {
        if (EditorApplication.isPlaying && Input.GetKey(KeyCode.Escape))
        {
            EditorApplication.isPlaying = false;
        }
    }
    #endregion
}
