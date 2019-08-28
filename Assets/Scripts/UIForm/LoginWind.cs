using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoginWind : BaseUiFrame
{
    public InputField IptAcct;//账号
    public InputField IptPass;//密码

    private void Awake()
    {
        CurrentUiType.UiShowMode = UiShowMode.HideOther;

        if (PlayerPrefs.HasKey("Acct") && PlayerPrefs.HasKey("Pass"))
        {
            IptAcct.text = PlayerPrefs.GetString("Acct");
            IptPass.text = PlayerPrefs.GetString("Pass");
        }
        else
        {
            IptAcct.text = "";
            IptPass.text = "";
        }
        RigisterButtonObjectEvent("Button", p => {
            OpenUIForm(UiWind.CreateWind.ToString());
         });
    }
}