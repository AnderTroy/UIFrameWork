using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCityWind : BaseUiFrame
{
    private void Awake()
    {
        CurrentUiType.UiWindType = UiWindType.Fixed;
        CurrentUiType.UiShowMode = UiShowMode.HideOther;

        RigisterButtonObjectEvent("HeadBtn", p =>
        {
            OpenUIForm(UiWind.DataWind.ToString());
        });
        RigisterButtonObjectEvent("ChatBtn", p =>
        {
            OpenUIForm(UiWind.ChatWind.ToString());
        });
        RigisterButtonObjectEvent("TaskArena", p =>
        {
            OpenUIForm(UiWind.CopyWind.ToString());
        });
        RigisterButtonObjectEvent("Task", p =>
        {
            OpenUIForm(UiWind.TaskWind.ToString());
        });
        RigisterButtonObjectEvent("Strong", p =>
        {
            OpenUIForm(UiWind.StrongWind.ToString());
        });
    }
}
