using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartGame : MonoBehaviour
{
    private static StartGame _instance;
    /// <summary>
    /// 得到实例
    /// </summary>
    public static StartGame Instance()
    {
        if (_instance == null)
        {
            _instance = GameObject.Find("StartGame").GetComponent<StartGame>();
        }
        return _instance;
    }
    private StartGame() { }
    [HideInInspector]
    public LoadingWind Loading = null;
    // Start is called before the first frame update
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        UiManager.Instance().ShowUiForms(UiWind.LoadingWind.ToString());
        Loading = GameObject.Find("LoadingWind(Clone)").GetComponent<LoadingWind>();
        
        ResSvc.Instance().AsyncLoadScene("MainCity", () => 
        {
            UiManager.Instance().ShowUiForms(UiWind.LoginWind.ToString());
        });
    }
}
