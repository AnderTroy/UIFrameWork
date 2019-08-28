/********************************************************************
	created:	2019/08/27
	created:	27:8:2019   15:23
	filename: 	D:\UNITY\UIFrameWork\Assets\Scripts\UIFrame\EventTriggerListener.cs
	file path:	D:\UNITY\UIFrameWork\Assets\Scripts\UIFrame
	file base:	EventTriggerListener
	file ext:	cs
	author:		AnderTroy
	
	purpose:	事件触发监听, 实现对于任何对象的监听处理。
*********************************************************************/
using UnityEngine;
using UnityEngine.EventSystems;

public class EventTriggerListener : EventTrigger
{
    public delegate void VoidDelegate(GameObject go);
    public VoidDelegate onClick;
    public VoidDelegate onDown;
    public VoidDelegate onEnter;
    public VoidDelegate onExit;
    public VoidDelegate onUp;
    public VoidDelegate onSelect;
    public VoidDelegate onUpdateSelect;

    /// <summary>
    /// 得到“监听器”组件
    /// </summary>
    /// <param name="go">监听的游戏对象</param>
    /// <returns>
    /// 监听器
    /// </returns>
    public static EventTriggerListener GetLister(GameObject go)
    {
        EventTriggerListener lister = go.GetComponent<EventTriggerListener>();
        if (lister == null)
        {
            lister = go.AddComponent<EventTriggerListener>();
        }
        return lister;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        onClick?.Invoke(gameObject);
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        onDown?.Invoke(gameObject);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        onEnter?.Invoke(gameObject);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        onExit?.Invoke(gameObject);
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        onUp?.Invoke(gameObject);
    }

    public override void OnSelect(BaseEventData eventBaseData)
    {
        onSelect?.Invoke(gameObject);
    }

    public override void OnUpdateSelected(BaseEventData eventBaseData)
    {
        onUpdateSelect?.Invoke(gameObject);
    }

}