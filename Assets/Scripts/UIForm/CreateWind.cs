using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateWind : BaseUiFrame
{

    private void Awake()
    {
        CurrentUiType.UiShowMode = UiShowMode.HideOther;
        RigisterButtonObjectEvent("Button", p =>
        {
            if (StartGame.Instance().Loading==null)
            {
                StartGame.Instance().Loading = GameObject.Find("LoadingWind").GetComponent<LoadingWind>();
            }
            ResSvc.Instance().AsyncLoadScene("Logoin", () =>
            {
                OpenUIForm(UiWind.MainCityWind.ToString());
            });
            
        });
    }
}
