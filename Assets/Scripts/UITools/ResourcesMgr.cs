/****************************************************
    文件：ResourcesMgr.cs
	作者：AnderTroy
    邮箱: 1329524041@qq.com
    日期：2019/6/6 20:56:38
    功能：资源管理类
*****************************************************/
using System.Collections;
using UnityEngine; 

public class ResourcesMgr : BaseUiFrame
{
    private static ResourcesMgr _instance;
    private Hashtable ht; //容器键值对集合
     
    /// <summary>
    /// 得到实例(单例)
    /// </summary>
    public static ResourcesMgr Instance()
    {
        if (_instance == null)
        {
            _instance = new GameObject("ResourceMgr").AddComponent<ResourcesMgr>();
        }
        return _instance;
    }

    void Awake()
    {
        ht = new Hashtable();
    }
    /// <summary>
    /// 调用资源（带对象缓冲技术）
    /// </summary>
    public GameObject LoadAsset(string path, bool isCatch = true)
    {
        GameObject goObj = LoadResource<GameObject>(path, isCatch);
        GameObject goObjClone = GameObject.Instantiate<GameObject>(goObj, null, false);
        if (goObjClone == null)
        {
            Debug.LogError(GetType() + "/LoadAsset()/克隆资源不成功，请检查。 path=" + path);
        }
        return goObjClone;
    }
    /// <summary>
    /// 调用资源（带对象缓冲技术）
    /// </summary>
    public T LoadResource<T>(string path, bool isCatch = true) where T : Object
    {
        if (ht.Contains(path)) //判断哈希表是否包含特定键
        {
            return ht[path] as T;
        }

        T resource = Resources.Load<T>(path);
        if (resource == null)
        {
            Debug.LogError(GetType() + "/Instance()/TResource 提取的资源找不到，请检查。 path=" + path);
        }
        else if (isCatch)
        {
            ht.Add(path, resource);
        }
        return resource;
    } 
}