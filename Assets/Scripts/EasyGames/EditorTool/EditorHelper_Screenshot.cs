using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EasyGames
{
#if UNITY_EDITOR
    public class EditorHelper_Screenshot : MonoBehaviour
    {
        [Button]
        public void Screenshot()
        {
            var path = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "/";
            var fileName = DateTime.Now.ToString("dd-MM-yy-hh-mm-ss") + ".png";
            ScreenCapture.CaptureScreenshot(path + fileName);
            print($"Screenshot saved: {path + fileName}.");
        }
    }
#endif
}