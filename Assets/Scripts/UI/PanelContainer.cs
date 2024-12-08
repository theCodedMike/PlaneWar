using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    [CreateAssetMenu(fileName = "PanelContainer", menuName = "PanelsCreator")]
    public class PanelContainer : ScriptableObject
    {
        public List<PanelInfo> panelList;
    }
}
