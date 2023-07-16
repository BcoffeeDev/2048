using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;

namespace EasyGames
{
    [CreateAssetMenu(fileName = "New CustomizeProfile", menuName = "Create/EasyGames/CustomizeProfile")]
    public class CustomizeProfile : SerializedScriptableObject
    {
        public Dictionary<ColorizeLayer, Color> ColorizeData = new();
    }
}
