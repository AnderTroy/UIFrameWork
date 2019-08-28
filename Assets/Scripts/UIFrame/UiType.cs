/********************************************************************
	created:	2019/08/27
	created:	27:8:2019   12:19
	filename: 	D:\UNITY\UIFrameWork\Assets\Scripts\UIFrame\UiType.cs
	file path:	D:\UNITY\UIFrameWork\Assets\Scripts\UIFrame
	file base:	UiType
	file ext:	cs
	author:		AnderTroy
	
	purpose:	窗体类型 （引用窗体的重要属性[枚举类型]）
*********************************************************************/
public class UiType
{
    //是否清空“栈集合”
    public bool IsClearStack = false;
    //UI窗体（位置）类型
    public UiWindType UiWindType = UiWindType.Normal;
    //UI窗体显示类型
    public UiShowMode UiShowMode = UiShowMode.Normal;
    //UI窗体透明度类型
    public UiTransparencyType UiTransparencyType = UiTransparencyType.Transparency;
}

/// <summary>
/// UI窗体（位置）类型
/// </summary>
public enum UiWindType
{
    Normal, //普通窗体
    Fixed,  //固定窗体
    PopUp,  //弹出窗体
}
/// <summary>
/// UI窗体的显示类型
/// </summary>
public enum UiShowMode
{
    Normal,         //普通
    ReverseChange,  //反向切换
    HideOther,      //隐藏其他
}
/// <summary>
/// UI窗体透明度类型
/// </summary>
public enum UiTransparencyType
{
    Transparency, //完全透明，不能穿透
    Translucence, //半透明，不能穿透
    ImPenetrable, //低透明度，不能穿透
    Penetrable,   //可以穿透
}

public enum UiWind
{
    BattleEndWind,
    BattleWind,
    BuyWind,
    ChatWind,
    CopyWind,
    CreateWind,
    DataWind,
    DataText,
    DynamicWind,
    GuideWind,
    LoadingWind,
    LoginWind,
    MainCityWind,
    StrongWind,
    TaskWind,
}
/// <summary>
/// 解析xml路径
/// </summary>
public class UiWindCfg
{
    public int Id;
    public UiWind Key;
    public string Value;
}