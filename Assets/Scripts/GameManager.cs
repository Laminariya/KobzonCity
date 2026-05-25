using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour
{
    
    public static GameManager instance;

    [HideInInspector] public SerializeXML serializeXML;
    [HideInInspector] public MyDataClass myDataClass;
    [HideInInspector] public BluetoothManager bluetoothManager;
    [HideInInspector] public MainPanel mainPanel;
    [HideInInspector] public FloorPanel floorPanel;
    [HideInInspector] public OfficePanel officePanel;

    public GameObject loadPanel;
    public TMP_Text InfoStartPanel;
    
    [HideInInspector] public FeedClass Feed;
    [HideInInspector] public MyData MyData;
    [HideInInspector] public string SymvolQuadro = "<sup>2</sup>";
    [HideInInspector] public string SymvolRuble = "\u20BD";

    private void Awake()
    {
        if(instance == null)
            instance = this;
        
// #if UNITY_ANDROID
//         // Оптимизация для Android
//         Application.targetFrameRate = 60;
//         QualitySettings.vSyncCount = 0;
//         
//         // Настройка текструр
//         Texture.streamingTextureDiscardUnusedMips = true;
//
//         // Ограничение памяти
//         UnityEngine.Android.AndroidDevice.SetSustainedPerformanceMode(true);
// #endif
    }

    void Start()
    {
        _ = LoadContent();
    }
    
    private async Task LoadContent()
    {
        loadPanel.SetActive(true);
        InfoStartPanel.text = "Load Feed..."+"\r\n";
        
        serializeXML = FindObjectOfType<SerializeXML>(true);
        myDataClass = FindObjectOfType<MyDataClass>(true);
        bluetoothManager = FindObjectOfType<BluetoothManager>(true);
        mainPanel = FindObjectOfType<MainPanel>(true);
        floorPanel = FindObjectOfType<FloorPanel>(true);
        officePanel = FindObjectOfType<OfficePanel>(true);
        
        InfoStartPanel.text += "Load Feed2..."+"\r\n";
        await serializeXML.Init(this);
        InfoStartPanel.text += "Load Feed3..."+"\r\n";
        myDataClass.Init();
        mainPanel.Init(this);
        floorPanel.Init(this);
        officePanel.Init(this);
        
        Debug.Log("XXX");
        StartCoroutine(StartGame());
        Debug.Log("XXX2");
        InfoStartPanel.text += "Load Feed4..."+"\r\n";
        // int countFlat = 0;
        // int countFloor = 0;
        // foreach (var building in MyData.Buildings)
        // {
        //     foreach (var myObject in building.MyObjects)
        //     {
        //         if (myObject.Number == 113)
        //         {
        //             Debug.Log(myObject.ObjectClass.FloorNumber);
        //             Debug.Log(myObject.ObjectClass.TotalArea);
        //             Debug.Log(myObject.ObjectClass.BargainTerms.Price);
        //         }
        //     }
        // }
        
        bluetoothManager.MenuPanel.SetActive(true);
        bluetoothManager.GetPairedDevices();
        InfoStartPanel.text += "Load Feed5..."+"\r\n";
    }

    IEnumerator StartGame()
    {
        
        Debug.Log("Load Panel");
        //yield return StartCoroutine(myDataClass.CreateSprites());
        
        //StartButton.SetActive(true);
        loadPanel.SetActive(false);
        yield break;
    }

    public string GetSplitPrice(int price)
    {
        string result = price.ToString();
        int count = result.Length;

        if (count > 3)
            result = result.Insert(result.Length - 3, " ");
        if(count > 6)
            result = result.Insert(result.Length - 7, " ");
        if(count > 9)
            result = result.Insert(result.Length - 11, " ");
        return result;
    }
    
    public string GetShortPrice(int price)
    {
        string p = (price / 1000000f).ToString();
        if(p.Length>=4)
            p = p.Substring(0, 4);
        return p;
    }

    public void MessageOnHouse(int house, int porch, bool isOn = true)
    {
        //Debug.Log(house+" " + porch);
        //HH02PP0300000000
        string str = house.ToString("X");
        if(str.Length==1) str = "0" + str;
        str += "02";
        string por = porch.ToString("X");
        if(por.Length==1) por = "0" + por;
        str += por;
        if (isOn) str += "0300000000";
        else str += "0000000000";
        Debug.Log("Mess House");
        bluetoothManager.AddMessage(str);
    }

    public void MessageOnFlat(int house, int porch, int flat, bool isOn = true)
    {
        //HH01FFFF03000000
        string str = house.ToString("X");
        if(str.Length==1) str = "0" + str;
        str += "01";
        string f = flat.ToString("X");
        if (f.Length == 1) f = "000" + f;
        else if (f.Length == 2) f = "00" + f;
        else if (f.Length == 3) f = "0" + f;
        if (isOn) f += "03000000";
        else f += "00000000";
        str += f;
        Debug.Log("Mess Flat " + house + " " + porch + " " + flat);
        
        bluetoothManager.AddMessage(str);
        //sendComPort.AddMessage(str);
    }

    public void MessageOnFloor(int house, int porch, int floor)
    {
       /* if (floor == 0)
        {
            bluetoothManager.AddMessage("010A020E00000100");
            return;
        }
        if (floor == 1)
        {
            bluetoothManager.AddMessage("010A020E00000200");
            return;
        }*/

        //HH03SSXX03000000
        string str = house.ToString("X");
        if(str.Length==1) str = "0" + str;
        str += "03";
        string f = floor.ToString("X");
        if (f.Length == 1) f = "0" + f;
        str += f;
        string s = porch.ToString("X");
        if (s.Length == 1) s = "0" + s;
        str += s + "03000000";
        Debug.Log("Mess Floor " +str);
        //LOg.text += str + "\r\n";
        bluetoothManager.AddMessage(str) ;
        //sendComPort.AddMessage(str);
    }

    public void MessageOffAllLight()
    {
        Debug.Log("Mess OffAll");
        //LOg.text += "007F060100000000" + "\r\n";
        bluetoothManager.AddMessage("007F060100000000"); //Погасить всё!!!
    }

    public void MessageOnDemo()
    {
        Debug.Log("Mess Demo");
        //LOg.text += "007F060100000000" + "\r\n";
        bluetoothManager.AddMessage("0064010000000000"); //Включить демо!
    }

}
