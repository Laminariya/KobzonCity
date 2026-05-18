using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine.Android;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UI;

public class BluetoothManager : MonoBehaviour
{
    public TMP_Text InfoData;
    public GameObject MenuPanel;
    public Transform ButtonsParent;
    public GameObject ButtonPrefab;
    public TMP_Text BluetoothName;
    public Button b_GetPairedDevices;

    public TMP_Text deviceAdd;
    public TMP_Text dataToSend;
    public TMP_Text receivedData;
    public GameObject devicesListContainer;
    public GameObject deviceMACText;
    private bool isConnected;
    public TMP_Text StatusDevice;

    private static AndroidJavaClass unity3dbluetoothplugin;
    private static AndroidJavaObject BluetoothConnector;

    private int _countError;
    private bool _isConnecting;
    private string _keyMac = "MAC";

    private Queue<string> _queue = new Queue<string>();
    private float _delay = 0.1f;
    private float _timer;

    //private SendComPort _sendCom;
    // Start is called before the first frame update
    void Start()
    {
        //_sendCom = FindObjectOfType<SendComPort>(true);
        //_sendCom.Init();
        //Debug.LogWarning("Bluetooth Manager");
        MenuPanel.SetActive(false);
        InitBluetooth();
        _countError = 0;
        isConnected = false;
        b_GetPairedDevices.onClick.AddListener(GetPairedDevices);
        BluetoothName.text = "";

        if (PlayerPrefs.HasKey(_keyMac))
        {
            OnClickConnect(PlayerPrefs.GetString(_keyMac));
        }

        _timer = 0;
        //GetPairedDevices();
    }

    private void Update()
    {
        if (_queue.Count > 0 && Time.time - _timer > _delay)
        {
            _timer = Time.time;
            WriteData(_queue.Dequeue());
        }
    }

    public void AddMessage(string message)
    {
        _queue.Enqueue(message);
    }

    // creating an instance of the bluetooth class from the plugin 
    public void InitBluetooth()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        // Check BT and location permissions
        if (!Permission.HasUserAuthorizedPermission(Permission.CoarseLocation)
            || !Permission.HasUserAuthorizedPermission(Permission.FineLocation)
            || !Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH_ADMIN")
            || !Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH")
            || !Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH_SCAN")
            || !Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH_ADVERTISE")
            || !Permission.HasUserAuthorizedPermission("android.permission.BLUETOOTH_CONNECT"))
        {
            Permission.RequestUserPermissions(new string[]
            {
                Permission.CoarseLocation,
                Permission.FineLocation,
                "android.permission.BLUETOOTH_ADMIN",
                "android.permission.BLUETOOTH",
                "android.permission.BLUETOOTH_SCAN",
                "android.permission.BLUETOOTH_ADVERTISE",
                "android.permission.BLUETOOTH_CONNECT"
            });
        }

        unity3dbluetoothplugin = new AndroidJavaClass("com.example.unity3dbluetoothplugin.BluetoothConnector");
        BluetoothConnector = unity3dbluetoothplugin.CallStatic<AndroidJavaObject>("getInstance");
    }

    // Start device scan
    public void StartScanDevices()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        // Destroy devicesListContainer child objects for new scan display
        foreach (Transform child in devicesListContainer.transform)
        {
            Destroy(child.gameObject);
        }

        BluetoothConnector.CallStatic("StartScanDevices");
    }

    // Stop device scan
    public void StopScanDevices()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        BluetoothConnector.CallStatic("StopScanDevices");
    }

    // This function will be called by Java class to update the scan status,
    // DO NOT CHANGE ITS NAME OR IT WILL NOT BE FOUND BY THE JAVA CLASS
    public void ScanStatus(string status)
    {
        Toast("Scan Status: " + status);
    }

    // This function will be called by Java class whenever a new device is found,
    // and delivers the new devices as a string data="MAC+NAME"
    // DO NOT CHANGE ITS NAME OR IT WILL NOT BE FOUND BY THE JAVA CLASS
    public void NewDeviceFound(string data)
    {
        GameObject newDevice = deviceMACText;
        newDevice.GetComponent<TMP_Text>().text = data;
        Instantiate(newDevice, devicesListContainer.transform);
    }

    // Get paired devices from BT settings
    public void GetPairedDevices()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;
        // This function when called returns an array of PairedDevices as "MAC+Name" for each device found
        string[] data = BluetoothConnector.CallStatic<string[]>("GetPairedDevices");

        // Display the paired devices
        foreach (var d in data)
        {
            GameObject newDevice = Instantiate(ButtonPrefab, ButtonsParent);
            newDevice.GetComponentInChildren<TMP_Text>().text = d;
            newDevice.GetComponent<Button>().onClick.AddListener(() => OnClickConnect(d));
        }
    }

    private void OnClickConnect(string mac)
    {
        BluetoothName.text += mac + "\r\n";
        string[] strings = mac.Split('+');
        StartConnection(strings[0]);
        PlayerPrefs.SetString("MAC", strings[0]);
        MenuPanel.SetActive(false);
        isConnected = true;
    }

    // Start BT connect using device MAC address "deviceAdd"
    public void StartConnection(string address)
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        BluetoothConnector.CallStatic("StartConnection", address.ToUpper());
    }

    // Stop BT connetion
    public void StopConnection()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        if (isConnected)
            BluetoothConnector.CallStatic("StopConnection");
    }

    // This function will be called by Java class to update BT connection status,
    // DO NOT CHANGE ITS NAME OR IT WILL NOT BE FOUND BY THE JAVA CLASS
    public void ConnectionStatus(string status)
    {
        //Toast("Connection Status: " + status);
        isConnected = status == "connected";
        if (isConnected)
        {
            //_sendCom.OffAll();
            //TODO Включаем Демо
        }
    }

    // This function will be called by Java class whenever BT data is received,
    // DO NOT CHANGE ITS NAME OR IT WILL NOT BE FOUND BY THE JAVA CLASS
    public void ReadData(string data)
    {
        Debug.Log("BT Stream: " + data);
        receivedData.text = data;
        _countError--;
    }

    // Write data to the connected BT device
    public void WriteData()
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        if (isConnected)
        {
            BluetoothConnector.CallStatic("WriteData", dataToSend.text.ToString() + "\r\n");
            receivedData.text += "=> " + dataToSend.text.ToString() + "\n\r";
        }
    }

    public void WriteData(string message)
    {
        if (Application.platform != RuntimePlatform.Android)
            return;

        if (isConnected)
        {
            //InfoData.text += " " + message;
            Debug.Log("MESS: " + message);
            BluetoothConnector.CallStatic("WriteData", message + "\r\n"); // 
            //0064010000000000
            //receivedData.text += "=> " + message + "\n\r";
            //OnAddError();
        }
    }

    // This function will be called by Java class to send Log messages,
    // DO NOT CHANGE ITS NAME OR IT WILL NOT BE FOUND BY THE JAVA CLASS
    public void ReadLog(string data)
    {
        Debug.Log(data);
    }


    // Function to display an Android Toast message
    public void Toast(string data)
    {
        if (Application.platform != RuntimePlatform.Android)
            return;
        StatusDevice.text = data;
        BluetoothConnector.CallStatic("Toast", data);
    }
}