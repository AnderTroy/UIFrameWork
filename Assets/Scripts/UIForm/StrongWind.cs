using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrongWind : BaseUiFrame
{
    private void Awake()
    {
        CurrentUiType.UiWindType = UiWindType.PopUp;
        CurrentUiType.UiShowMode = UiShowMode.ReverseChange;
        CurrentUiType.UiTransparencyType = UiTransparencyType.Translucence;

        RigisterButtonObjectEvent("CloseBtn", P =>
        {
            CloseUIForm();
        });
    }
}
