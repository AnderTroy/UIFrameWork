using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataText : BaseUiFrame
{
    private void Awake()
    {
        CurrentUiType.UiWindType = UiWindType.PopUp;
        CurrentUiType.UiShowMode = UiShowMode.ReverseChange;
        CurrentUiType.UiTransparencyType = UiTransparencyType.Translucence;

        RigisterButtonObjectEvent("CloseDataBtn", p => { CloseUIForm(); });
    }
}
