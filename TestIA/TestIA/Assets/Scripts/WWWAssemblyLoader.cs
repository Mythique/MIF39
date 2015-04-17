using UnityEngine;
using System.Collections;
using System.Reflection;

public class WWWAssemblyLoader : MonoBehaviour
{
    public string m_AssemblyURL;
    private string m_ErrorString = "";
    private WWW m_WWW;
    private bool m_Complete = true;
    
    public void Start()
    {
        if (m_AssemblyURL != "")
        {
            ReloadAssembly(m_AssemblyURL);
        }
    }
    
    public string AssemblyURL
    {
        get
        {
            return m_AssemblyURL;
        }
        set
        {
            if (m_AssemblyURL != value)
            {
                ReloadAssembly(value);
            }
        }
    }
    
    public float Progress
    {
        get
        {
            return m_Complete ? 1.0f : m_WWW.progress;
        }
    }
    
    public string Error
    {
        get
        {
            return m_ErrorString;
        }
    }
    
    public void ReloadAssembly(string url)
    {
        m_Complete = false;
        m_ErrorString = "";
        m_AssemblyURL = url;
        m_WWW = new WWW(m_AssemblyURL);
    }
    
    public void Update()
    {
        if (!m_Complete)
        {
            if (m_WWW.error != null)
            {
                m_ErrorString = m_WWW.error;
                m_Complete = true;
                SendMessage("OnAssemblyLoadFailed", m_AssemblyURL);
            } else if (m_WWW.isDone)
            {
                Assembly assembly = LoadAssembly();
                m_Complete = true;
                if (assembly != null)
                {
                    Debug.Log("Done");
                    SendMessage("OnAssemblyLoaded", new WWWAssembly(m_AssemblyURL, assembly));
                } else
                {
                    Debug.Log("Failed");
                    SendMessage("OnAssemblyLoadFailed", m_AssemblyURL);
                }
            }
        }
    }
    
    private Assembly LoadAssembly()
    {
        try
        {
            return Assembly.Load(m_WWW.bytes);
        } catch (System.Exception e)
        {
            m_ErrorString = e.ToString();
            return null;
        }
    }
}

public class WWWAssembly
{
    private string m_URL;
    private Assembly m_Assembly;
    
    public string URL
    {
        get
        {
            return m_URL;
        }
    }
    
    public Assembly Assembly
    {
        get
        {
            return m_Assembly;
        }
    }
    
    public WWWAssembly(string url, Assembly assembly)
    {
        m_URL = url;
        m_Assembly = assembly;
    }
}