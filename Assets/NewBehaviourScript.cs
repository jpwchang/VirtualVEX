using UnityEngine;
using System.Collections;
using System.Reflection;
using vvIO;

//The Class Assembly loader code is from public domain code created by Emil Johanssen ("AngryAnt") on angryant.com.
//Credits for all loader code go to him.

public class NewBehaviourScript : MonoBehaviour
{
    private string m_MessageString = "Waiting for assembly";

    private GameObject tracker;

    public void OnAssemblyLoaded(WWWAssembly loadedAssembly)
    {
        System.Type sourceType = loadedAssembly.Assembly.GetType("source");
        System.Type type = sourceType.BaseType;

        //FieldInfo field = type.GetField("myString");
        FieldInfo rtData = type.GetField("vexRT");
        float[] vexrt = (rtData.GetValue(null) as float[]);
        MethodInfo setrt = type.GetMethod("setRTValue");
        setrt.Invoke(null, new object[] { 0, Input.GetAxis("Vertical") });
        MethodInfo m = sourceType.GetMethod("driver_control");
        object temp = m.Invoke(null, null);
        m_MessageString = (temp as string) + "\n";
    }

    void OnAssemblyLoadFailed(string url)
    {
        m_MessageString = "Failed to load assembly at " + url;
        vvConsole.println(m_MessageString);
    }

    void Start()
    {
        tracker = GameObject.Find("Mode Tracker");
    }

    void OnGUI()
    {
        GUI.depth = 0;
        GUI.skin = GetComponent<vvRobotBase>().skin;
        if(tracker != null && tracker.GetComponent<ModeTrackingScript>().showStatusBar)
            GUI.Label(new Rect(0, Screen.height - 12, Screen.width, 12), m_MessageString, "labelRight");
    }
}