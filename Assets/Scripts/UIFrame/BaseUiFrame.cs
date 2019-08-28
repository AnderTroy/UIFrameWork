/********************************************************************
	created:	2019/08/27
	created:	27:8:2019   19:02
	filename: 	D:\UNITY\UIFrameWork\Assets\Scripts\UIFrame\BaseUiFrame.cs
	file path:	D:\UNITY\UIFrameWork\Assets\Scripts\UIFrame
	file base:	BaseUiFrame
	file ext:	cs
	author:		AnderTroy
	
	purpose:	UI窗体的父类
    定义四个生命周期
      1：Display 显示状态。
      2：Hiding 隐藏状态
      3：ReDisplay 再显示状态。
      4：Freeze 冻结状态。
*********************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class BaseUiFrame : MonoBehaviour
{
    /* 属性*/
    //当前UI窗体类型
    public UiType CurrentUiType { get; set; } = new UiType();
    #region  窗体的四种(生命周期)状态
    /// <summary>
    /// 显示状态
    /// </summary>
    public virtual void Display()
    {
        gameObject.SetActive(true);
        //设置模态窗体调用(必须是弹出窗体)
        if (CurrentUiType.UiWindType == UiWindType.PopUp)
        {
            UIMaskMgr.Instance().SetMaskWindow(gameObject, CurrentUiType.UiTransparencyType);
        }
    }
    /// <summary>
    /// 隐藏状态
    /// </summary>
    public virtual void Hiding()
    {
        gameObject.SetActive(false);
        //取消模态窗体调用
        if (CurrentUiType.UiWindType == UiWindType.PopUp)
        {
            UIMaskMgr.Instance().CancelMaskWindow();
        }
    }
    /// <summary>
    /// 重新显示状态
    /// </summary>
    public virtual void Redisplay()
    {
        gameObject.SetActive(true);
        //设置模态窗体调用(必须是弹出窗体)
        if (CurrentUiType.UiWindType == UiWindType.PopUp)
        {
            UIMaskMgr.Instance().SetMaskWindow(gameObject, CurrentUiType.UiTransparencyType);
        }
    }
    /// <summary>
    /// 冻结状态
    /// </summary>
    public virtual void Freeze()
    {
        gameObject.SetActive(true);
    }
    #endregion

    #region 封装子类常用的方法
    /// <summary>
    /// 注册按钮事件
    /// </summary>
    /// <param name="buttonName">按钮节点名称</param>
    /// <param name="delHandle">委托：需要注册的方法</param>
    protected void RigisterButtonObjectEvent(string buttonName, EventTriggerListener.VoidDelegate delHandle)
    {
        GameObject Button = UnityHelper.FindTheChildNode(gameObject, buttonName).gameObject;
        //给按钮注册事件方法
        if (Button != null)
        {
            EventTriggerListener.GetLister(Button).onClick = delHandle;
        }
    }
    /// <summary>
    /// 打开UI窗体
    /// </summary>
    /// <param name="uiFormName"></param>
    protected void OpenUIForm(string uiFormName)
    {
        UiManager.Instance().ShowUiForms(uiFormName);
    }
    /// <summary>
    /// 关闭当前UI窗体
    /// </summary>
    protected void CloseUIForm()
    {
        string strUIFromName = GetType().ToString();
        UiManager.Instance().CloseUiForms(strUIFromName);
    }
    protected void SetText(Text txt, string context = "")
    {
        txt.text = context;
    }
    #endregion
}