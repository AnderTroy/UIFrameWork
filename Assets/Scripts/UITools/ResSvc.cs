using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResSvc : MonoBehaviour
{
    private static ResSvc _instance;
    /// <summary>
    /// 得到实例
    /// </summary>
    public static ResSvc Instance()
    {
        if (_instance == null)
        {
            _instance = GameObject.Find("StartGame").GetComponent<ResSvc>();
        }
        return _instance;
    }
    private ResSvc() { }
    public AsyncOperation SceneAsync;

    public Action PrgV { get; set; } = null;

    public void AsyncLoadScene(string sceneName, Action loaded)
    {
        StartGame.Instance().Loading.Display();
        SceneAsync = SceneManager.LoadSceneAsync(sceneName);
        SceneAsync.allowSceneActivation = false;
        PrgV = () =>
        {
            float val = SceneAsync.progress;
            StartGame.Instance().Loading.SetProgress(val);
            if (val == 1)
            {
                loaded?.Invoke(); //if(loaded!=null) loaded();
                PrgV = null;
                SceneAsync = null;
                StartGame.Instance().Loading.Hiding();
            }
        };
    }
    private void Update()
    {
        PrgV?.Invoke();
    }
}
