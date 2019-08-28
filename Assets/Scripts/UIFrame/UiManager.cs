/****************************************************
    文件：UiManager.cs
	作者：AnderTroy
    邮箱: 1329524041@qq.com
    日期：2019/6/6 13:22:59
    功能：UI窗体管理器脚本（框架核心脚本）
*****************************************************/
using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class UiManager : MonoBehaviour
{
    private static UiManager _instance;
    /// <summary>
    /// 得到实例
    /// </summary>
    public static UiManager Instance()
    {
        if (_instance == null)
        {
            _instance = new GameObject("UiManager").AddComponent<UiManager>();
        }
        return _instance;
    }
    private UiManager() { }

    //UI窗体预设路径(参数1：窗体预设名称，2：表示窗体预设路径)
    private Dictionary<UiWind, string> _dicFormsPaths;
    //缓存所有UI窗体
    private Dictionary<string, BaseUiFrame> _dicAllUiForms;
    //当前显示的UI窗体
    private Dictionary<string, BaseUiFrame> _dicCurrentShowUiForms;
    //定义“栈”集合,存储显示当前所有[反向切换]的窗体类型
    private Stack<BaseUiFrame> _staCurrentUiForms;

    //UI根节点
    private Transform CanvasTrans;
    //全屏幕显示的节点
    private Transform _normalTrans;
    //固定显示的节点
    private Transform _fixedTrans;
    //弹出节点
    private Transform _popUpTrans;

    //UI管理脚本的节点
    private Transform _uiScriptsTrans;

    //初始化核心数据，加载“UI窗体路径”到集合中。
    public void Awake()
    {
        _dicAllUiForms = new Dictionary<string, BaseUiFrame>();
        _dicCurrentShowUiForms = new Dictionary<string, BaseUiFrame>();
        _dicFormsPaths = new Dictionary<UiWind, string>();
        _staCurrentUiForms = new Stack<BaseUiFrame>();
        //初始化加载（根UI窗体）Canvas预设
        InitRootCanvasLoading();
        //得到UI根节点、全屏节点、固定节点、弹出节点
        CanvasTrans = GameObject.FindGameObjectWithTag("Canvas").transform;
        _normalTrans = CanvasTrans.transform.Find("Normal");
        _fixedTrans = CanvasTrans.transform.Find("Fixed");
        _popUpTrans = CanvasTrans.transform.Find("PopUp");
        _uiScriptsTrans = CanvasTrans.transform.Find("Scripts");

        //把本脚本作为“根UI窗体”的子节点。
        gameObject.transform.SetParent(_uiScriptsTrans, false);

        DontDestroyOnLoad(CanvasTrans);//"根UI窗体"在场景转换的时候，不允许销毁
        //初始化“UI窗体预设”路径数据
        InitUiFormsPathData();
    }
    /// <summary>
    /// 初始化“UI窗体预设”路径数据
    /// </summary>
    private void InitUiFormsPathData()
    {
        TextAsset textAsset = Resources.Load<TextAsset>("ResCfgs/UiWind");
        if (!textAsset)
        {
            Debug.Log("Xml file:" + "ResCfgs / UiWind" + "not exist");
        }
        else
        {
            XmlDocument xmlDocument = new XmlDocument();
            xmlDocument.LoadXml(textAsset.text);
            XmlNodeList xmlNodeList = xmlDocument.SelectSingleNode("root")?.ChildNodes;
            if (xmlNodeList != null)
                for (int i = 0; i < xmlNodeList.Count; i++)
                {
                    XmlElement xmlElement = (XmlElement)xmlNodeList[i];
                    if (xmlElement != null && xmlElement.GetAttributeNode("ID") == null)
                    {
                        continue;
                    }
                    int id = Convert.ToInt32(xmlElement?.GetAttributeNode("ID")?.InnerText);
                    UiWindCfg uiWindCfgData = new UiWindCfg
                    {
                        Id = id,
                    };
                    foreach (XmlElement element in xmlNodeList[i].ChildNodes)
                    {
                        switch (element.Name)
                        {
                            case "key":
                                uiWindCfgData.Key = (UiWind)Enum.Parse(typeof(UiWind), element.InnerText);
                                break;
                            case "path":
                                uiWindCfgData.Value = element.InnerText;
                                break;
                        }
                    }
                    _dicFormsPaths.Add(uiWindCfgData.Key, uiWindCfgData.Value);
                }
        }
    }
    //初始化加载（根UI窗体）Canvas预设
    private void InitRootCanvasLoading()
    {
        ResourcesMgr.Instance().LoadAsset("ResUi/Canvas", false);
    }

    /// <summary>
    /// 显示（打开）UI窗体
    /// 功能：
    /// 1: 根据UI窗体的名称，加载到“所有UI窗体”缓存集合中
    /// 2: 根据不同的UI窗体的“显示模式”，分别作不同的加载处理
    /// </summary>
    /// <param name="uiFormName">UI窗体预设的名称</param>
    public void ShowUiForms(string uiFormName)
    {
        //参数的检查
        if (uiFormName == null) return;
        //根据UI窗体的名称，加载到“所有UI窗体”缓存集合中
        var baseUiForms = LoadFormsToAllUiFormsCatch(uiFormName);

        if (baseUiForms == null) return;
        //根据不同的UI窗体的显示模式，分别作不同的加载处理
        switch (baseUiForms.CurrentUiType.UiShowMode)
        {
            case UiShowMode.Normal:                 //“普通显示”窗口模式
                //把当前窗体加载到“当前窗体”集合中。
                LoadUiToCurrentCache(uiFormName);
                break;
            case UiShowMode.ReverseChange:          //需要“反向切换”窗口模式
                PushUiFormToStack(uiFormName);
                break;
            case UiShowMode.HideOther:              //“隐藏其他”窗口模式
                EnterUiFormsAndHideOther(uiFormName);
                break;
        }
    }

    /// <summary>
    /// 关闭（返回上一个）窗体
    /// </summary>
    /// <param name="uiFormName"></param>
    public void CloseUiForms(string uiFormName)
    {
        //参数检查
        //“所有UI窗体”集合中，如果没有记录，则直接返回
        if (uiFormName == null)
        {
            return;
        }
        _dicAllUiForms.TryGetValue(uiFormName, out var baseUiForm);
        if (baseUiForm != null)
        {
            switch (baseUiForm.CurrentUiType.UiShowMode)
            {
                case UiShowMode.Normal:
                    //普通窗体的关闭
                    ExitUiForms(uiFormName);
                    break;
                case UiShowMode.ReverseChange:
                    //反向切换窗体的关闭
                    PopUiForms();
                    break;
                case UiShowMode.HideOther:
                    //隐藏其他窗体关闭
                    ExitUiFormsAndDisplayOther(uiFormName);
                    break;
            }
        }
        if (baseUiForm == null)
        {
            return;
        }
        //根据窗体不同的显示类型，分别作不同的关闭处理

    }
    /// <summary>
    /// 根据UI窗体的名称，加载到“所有UI窗体”缓存集合中
    /// 功能： 检查“所有UI窗体”集合中，是否已经加载过，否则才加载。
    /// </summary>
    /// <param name="uiFormsName">UI窗体（预设）的名称</param>
    private BaseUiFrame LoadFormsToAllUiFormsCatch(string uiFormsName)
    {
        _dicAllUiForms.TryGetValue(uiFormsName, out BaseUiFrame baseUiResult);
        if (baseUiResult == null)
        {
            //加载指定名称的“UI窗体”
            baseUiResult = LoadUiForm(uiFormsName);
        }

        return baseUiResult;
    }
    /// <summary>
    /// 加载指定名称的“UI窗体”
    /// 功能：
    ///    1：根据“UI窗体名称”，加载预设克隆体。
    ///    2：根据不同预设克隆体中带的脚本中不同的“位置信息”，加载到“根窗体”下不同的节点。
    ///    3：隐藏刚创建的UI克隆体。
    ///    4：把克隆体，加入到“所有UI窗体”（缓存）集合中。
    /// </summary>
    /// <param name="uiFormName">UI窗体名称</param>
    private BaseUiFrame LoadUiForm(string uiFormName)
    {
        GameObject cloneUiPrefabs = null;             //创建的UI克隆体预设
        UiWind uiWind = (UiWind)Enum.Parse(typeof(UiWind), uiFormName);
        //根据UI窗体名称，得到对应的加载路径
        _dicFormsPaths.TryGetValue(uiWind, out string strUiFormPaths);
        //根据“UI窗体名称”，加载“预设克隆体”
        if (strUiFormPaths != null)
        {
            cloneUiPrefabs = ResourcesMgr.Instance().LoadAsset(strUiFormPaths, false);
        }
        //设置“UI克隆体”的父节点（根据克隆体中带的脚本中不同的“位置信息”）
        if (CanvasTrans != null && cloneUiPrefabs != null)
        {
            BaseUiFrame baseUiForm = cloneUiPrefabs.GetComponent<BaseUiFrame>();   //窗体基类
            if (baseUiForm == null)
            {
                Debug.Log("baseUiForm==null! ,请先确认窗体预设对象上是否加载了baseUIForm的子类脚本！ 参数 uiFormName=" + uiFormName);
                return null;
            }
            switch (baseUiForm.CurrentUiType.UiWindType)
            {
                case UiWindType.Normal:                 //普通窗体节点
                    cloneUiPrefabs.transform.SetParent(_normalTrans, false);
                    break;
                case UiWindType.Fixed:                  //固定窗体节点
                    cloneUiPrefabs.transform.SetParent(_fixedTrans, false);
                    break;
                case UiWindType.PopUp:                  //弹出窗体节点
                    cloneUiPrefabs.transform.SetParent(_popUpTrans, false);
                    break;
            }
            //设置隐藏
            cloneUiPrefabs.SetActive(false);
            //把克隆体，加入到“所有UI窗体”（缓存）集合中。
            _dicAllUiForms.Add(uiFormName, baseUiForm);
            return baseUiForm;
        }
        else
        {
            Debug.Log("_TraCanvasTransfrom==null Or goCloneUIPrefabs==null!! ,Plese Check!, 参数uiFormName=" + uiFormName);
        }

        Debug.Log("出现不可以预估的错误，请检查，参数 uiFormName=" + uiFormName);
        return null;
    }
    /// <summary>
    /// 把当前窗体加载到“当前窗体”集合中
    /// </summary>
    /// <param name="uiFormName">窗体预设的名称</param>
    private void LoadUiToCurrentCache(string uiFormName)
    {
        //如果“正在显示”的集合中，存在整个UI窗体，则直接返回
        _dicCurrentShowUiForms.TryGetValue(uiFormName, out BaseUiFrame baseWind);
        if (baseWind != null) return;
        //把当前窗体，加载到“正在显示”集合中
        _dicAllUiForms.TryGetValue(uiFormName, out var baseUiWindAllCache);
        if (baseUiWindAllCache != null)
        {
            _dicCurrentShowUiForms.Add(uiFormName, baseUiWindAllCache);
            baseUiWindAllCache.Display();           //显示当前窗体
        }
    }

    /// <summary>
    /// UI窗体入栈
    /// </summary>
    /// <param name="uiFormName">窗体的名称</param>
    private void PushUiFormToStack(string uiFormName)
    {
        //判断“栈”集合中，是否有其他的窗体，有则“冻结”处理。
        if (_staCurrentUiForms.Count > 0)
        {
            BaseUiFrame topUiForm = _staCurrentUiForms.Peek();
            //栈顶元素作冻结处理
            topUiForm.Freeze();
        }
        //判断“UI所有窗体”集合是否有指定的UI窗体，有则处理。
        _dicAllUiForms.TryGetValue(uiFormName, out BaseUiFrame baseWind);
        if (baseWind != null)
        {
            baseWind.Display();//当前窗口显示状态
            //把指定的UI窗体，入栈操作。
            _staCurrentUiForms.Push(baseWind);
        }
        else
        {
            Debug.Log("baseUIForm==null,Please Check, 参数 uiFormName=" + uiFormName);
        }
    }
    /// <summary>
    /// 退出指定UI窗体
    /// </summary>
    /// <param name="strUiFormName"></param>
    private void ExitUiForms(string strUiFormName)
    {
        //"正在显示集合"中如果没有记录，则直接返回。
        _dicCurrentShowUiForms.TryGetValue(strUiFormName, out BaseUiFrame baseUiForm);
        if (baseUiForm == null) return;
        //指定窗体，标记为“隐藏状态”，且从"正在显示集合"中移除。
        baseUiForm.Hiding();
        _dicCurrentShowUiForms.Remove(strUiFormName);
    }

    //（“反向切换”属性）窗体的出栈逻辑
    private void PopUiForms()
    {
        if (_staCurrentUiForms.Count >= 2)
        {
            //出栈处理
            BaseUiFrame topUiForms = _staCurrentUiForms.Pop();
            //做隐藏处理
            topUiForms.Hiding();
            //出栈后，下一个窗体做“重新显示”处理。
            BaseUiFrame nextUiForms = _staCurrentUiForms.Peek();
            nextUiForms.Redisplay();
        }
        else if (_staCurrentUiForms.Count == 1)
        {
            //出栈处理
            BaseUiFrame topUiForms = _staCurrentUiForms.Pop();
            //做隐藏处理
            topUiForms.Hiding();
        }
    }
    /// <summary>
    /// (“隐藏其他”属性)打开窗体，且隐藏其他窗体
    /// </summary>
    /// <param name="strUiName">打开的指定窗体名称</param>
    private void EnterUiFormsAndHideOther(string strUiName)
    {
        //参数检查
        if (strUiName == null) return;

        _dicCurrentShowUiForms.TryGetValue(strUiName, out BaseUiFrame baseWind);
        if (baseWind != null) return;

        //把“正在显示集合”与“栈集合”中所有窗体都隐藏。
        foreach (BaseUiFrame baseUi in _dicCurrentShowUiForms.Values)
        {
            baseUi.Hiding();
        }
        foreach (BaseUiFrame staUi in _staCurrentUiForms)
        {
            staUi.Hiding();
        }

        //把当前窗体加入到“正在显示窗体”集合中，且做显示处理。
        _dicAllUiForms.TryGetValue(strUiName, out BaseUiFrame baseUiWindAllCache);
        if (baseUiWindAllCache != null)
        {
            _dicCurrentShowUiForms.Add(strUiName, baseUiWindAllCache);
            //窗体显示
            baseUiWindAllCache.Display();
        }
    }

    /// <summary>
    /// (“隐藏其他”属性)关闭窗体，且显示其他窗体
    /// </summary>
    /// <param name="strUiName">打开的指定窗体名称</param>
    private void ExitUiFormsAndDisplayOther(string strUiName)
    {
        //参数检查
        if (strUiName == null) return;

        _dicCurrentShowUiForms.TryGetValue(strUiName, out var baseUiForm);
        if (baseUiForm == null) return;

        //当前窗体隐藏状态，且“正在显示”集合中，移除本窗体
        baseUiForm.Hiding();
        _dicCurrentShowUiForms.Remove(strUiName);

        //把“正在显示集合”与“栈集合”中所有窗体都定义重新显示状态。
        foreach (BaseUiFrame baseUi in _dicCurrentShowUiForms.Values)
        {
            baseUi.Redisplay();
        }
        foreach (BaseUiFrame staUi in _staCurrentUiForms)
        {
            staUi.Redisplay();
        }
    }
}