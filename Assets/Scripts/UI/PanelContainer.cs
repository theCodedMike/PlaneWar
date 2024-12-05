using System.Collections.Generic;
using UI;
using UnityEngine;

[CreateAssetMenu(fileName = "PanelContainer", menuName = "PanelsCreator")]
public class PanelContainer : ScriptableObject
{
    public List<PanelInfo> panelList;
}
