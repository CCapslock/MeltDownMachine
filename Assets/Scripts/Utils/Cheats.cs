using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Mans
{
    internal class Cheats : MonoBehaviour
    {
        #region Fields

        [SerializeField] private Texture2D _texButton;
        private bool _isShowCheats;
        GUIStyle stl;
        GUIStyle stl2;
        GUIStyle stlButton;

        #endregion


        #region Util

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ShowMenu();
            }
            if (Input.touches.Length >= 3)
            {
                foreach (var touch in Input.touches)
                {
                    if (touch.phase == TouchPhase.Began)
                    {
                        ShowMenu();
                    }
                }
            }
            if (Input.GetKeyDown("l"))
            {
                ShowLeaks();
            }
        }

        private static void ShowLeaks()
        {
                Debug.Log("***********************************");
                foreach (var item in ControlLeak.DataLeak.OrderByDescending(o => o.Value))
                {
                    Debug.Log($"{item.Key}:{item.Value}");
                }
            
        }

        private void ShowMenu()
        {
            _isShowCheats = !_isShowCheats;
            if (_isShowCheats) Time.timeScale = 0;
            else Time.timeScale = 1;
        }

        private GUIStyle SetStyle(Color clr)
        {
            GUIStyle stl;
            stl = new GUIStyle(GUI.skin.box);
            stl.normal.background = MakeTex(1, 1, clr);
            stl.hover.background = MakeTex(1, 1, clr);
            stl.fontStyle = FontStyle.Bold;
            stl.fontSize = 20;
            return stl;
        }
        private GUIStyle SetStyle(int size)
        {
            GUIStyle stl;
            stl = new GUIStyle(GUI.skin.box);
            stl.fontStyle = FontStyle.Bold;
            stl.fontSize = size;
            return stl;
        }

        private GUIStyle SetStyleTextures(Texture2D tex, Texture2D tex2)
        {
            GUIStyle stl;
            stl = new GUIStyle(GUI.skin.button);
            stl.normal.background = tex2;
            stl.hover.background = tex;
            stl.onNormal.background = tex2;
            stl.onHover.background = tex2;
            stl.onActive.background = tex2;
            stl.active.background = tex2;
            stl.fontStyle = FontStyle.Bold;
            stl.fontSize = 15;
            return stl;
        }

        private Texture2D MakeTex(int width, int height, Color col)
        {
            Color[] pix = new Color[width * height];

            for (int i = 0; i < pix.Length; i++)
                pix[i] = col;

            Texture2D result = new Texture2D(width, height);
            result.SetPixels(pix);
            result.Apply();

            return result;
        }

        #endregion


        #region Gui

        private void OnGUI()
        {
            var ws = Screen.width;
            var hs = Screen.height;
            var w1 = 0.1f * ws;
            var w2 = 0.122f * ws;
            var w4 = 0.4f * ws;
            var h1 = 0.05f * hs;
            var h3 = 0.18f * hs;

            if (stl == null) stl = SetStyle(Color.red);
            if (stl2 == null) stl2 = SetStyle(15);
            if (stlButton == null) stlButton = SetStyleTextures(_texButton, _texButton);

            if (_isShowCheats)
            {
                GUILayout.BeginArea(new Rect(ws / 4, hs / 4 * 2, ws / 2, hs / 2));
                GUILayout.BeginVertical(stl);
                GUILayout.Box("Cheats", stl2);
                GUILayout.Box("", stl, GUILayout.Height(1));
                GUILayout.BeginHorizontal();
                if (GUILayout.Button("Restart", stlButton, GUILayout.Height(h1), GUILayout.Width(w2)))
                {
                    Time.timeScale = 1;
                    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
                }

                GUILayout.EndHorizontal();
                GUILayout.BeginHorizontal();
                GUILayout.Label("Quality:", stl, GUILayout.Width(w1));

                if (GUILayout.Button("Low", stlButton, GUILayout.Height(h1), GUILayout.Width(w1)))
                {
                    SetQuality(0);
                }
                if (GUILayout.Button("Mid", stlButton, GUILayout.Height(h1), GUILayout.Width(w1)))
                {
                    SetQuality(1);
                }
                if (GUILayout.Button("Hight", stlButton, GUILayout.Height(h1), GUILayout.Width(w1)))
                {
                    SetQuality(2);
                }


                GUILayout.EndHorizontal();
                GUILayout.EndVertical();

                GUILayout.EndArea();
            }


        }




        private void SetQuality(int value)
        {
            QualitySettings.SetQualityLevel(value);
        }

        #endregion
    }
}