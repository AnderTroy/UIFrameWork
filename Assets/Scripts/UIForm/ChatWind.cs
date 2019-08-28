using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChatWind : BaseUiFrame
{
    private void Awake()
    {
        CurrentUiType.UiTransparencyType = UiTransparencyType.Penetrable;

        RigisterButtonObjectEvent("CloseBtn", P =>
        {
            CloseUIForm();
        });
    }
}
