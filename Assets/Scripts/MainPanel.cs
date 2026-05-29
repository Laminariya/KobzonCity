using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class MainPanel : MonoBehaviour
{

    private GameManager _manager;
    
    public List<FloorClass> Floors = new List<FloorClass>();
    public Image FloorPlan;
    public TMP_Text NumberFloor;
    public TMP_Text CountOffice;
    public TMP_Text CountLot;
    public GameObject PricePanel;
    public TMP_Text Price;
    public Button b_Up;
    public Button b_Down;
    public Button b_Next;
    public Button b_LightKorpus;
    public Color OnColor;
    public Button b_ChosePanel;
    public Button b_OpenBLE;
    
    private int _currentFloor = 0;
    private Color _offColor;
    private int _countClick = 0;
    private float _timer = 0;
    
    public void Init(GameManager manager)
    {
        _manager = manager;
        
        b_Up.onClick.AddListener(OnUp);
        b_Down.onClick.AddListener(OnDown);
        b_Next.onClick.AddListener(OnNext);
        b_LightKorpus.onClick.AddListener(OnLightKorpus);
        b_ChosePanel.onClick.AddListener(OnChosePanel);
        b_OpenBLE.onClick.AddListener(OnOpenBLE);
        _offColor = b_LightKorpus.image.color;
        
        _countClick = 0;
        
        for (int i = 0; i < Floors.Count; i++)
        {
            foreach (var myObject in _manager.MyData.Buildings[0].MyFloors[i].MyObjects)
            {
                foreach (var officeClass in Floors[i].OfficeClasses)   
                {
                    if (myObject.Number != 0)
                    {
                        if (myObject.Number == officeClass.Number)
                        {
                            officeClass.MyObject = myObject;
                            officeClass.Floor = i + 1;
                            officeClass.Init();
                        }
                    }
                    else
                    {
                        if (Mathf.Approximately(myObject.Area, officeClass.Area))
                        {
                            officeClass.MyObject = myObject;
                            officeClass.Floor = i + 1;
                            officeClass.Init();
                        }
                    }
                }
            }
        }
        
        ChangeFloor(_currentFloor);
    }

    private void Update()
    {
        if (Time.time - _timer > 1.0f)
        {
            _countClick = 0;
        }
    }

    private void OnOpenBLE()
    {
        _countClick++;
        _timer = Time.time;
        if (_countClick >= 4)
        {
            _manager.bluetoothManager.MenuPanel.SetActive(true);
        }
    }

    public void Show(int floor)
    {
        _currentFloor = floor;
        ChangeFloor(_currentFloor);
        _manager.MessageOffAllLight();
        _manager.MessageOnDemo();
    }

    private void OnUp()
    {
        _currentFloor++;
        if (_currentFloor >= Floors.Count)
        {
            _currentFloor = 17;
        }
        ChangeFloor(_currentFloor);
    }

    private void OnDown()
    {
        _currentFloor--;
        if (_currentFloor <=0)
        {
            _currentFloor = 0;
        }
        ChangeFloor(_currentFloor);
    }

    private void OnNext()
    {
        _manager.floorPanel.Show(_currentFloor);
    }

    private void ChangeFloor(int floor)
    {
        foreach (var floorClass in Floors)
        {
            floorClass.LightMain.enabled = false;
        }
        Floors[floor].LightMain.enabled = true;
        FloorPlan.sprite = Floors[floor].FloorSprite;
        NumberFloor.text = (floor+1).ToString();
        CountOffice.text = Floors[floor].CountOffice.ToString();
        PricePanel.SetActive(false);
        CountLot.text = "-";
        //Ставим минимальную цену
        if (_manager.MyData.Buildings[0].MyFloors[floor].MyObjects.Count > 0)
        {
            PricePanel.SetActive(true);
            
            Price.text = _manager.GetShortPrice(_manager.MyData.Buildings[0].MyFloors[floor].MinPrice) + " <size=80%>млн. руб";
            CountLot.text = (int)_manager.MyData.Buildings[0].MyFloors[floor].MinArea + "-" +
                            (int)_manager.MyData.Buildings[0].MyFloors[floor].MaxArea + " <size=80%>м" +
                            _manager.SymvolQuadro;

        }
        
        _manager.MessageOffAllLight();
        _manager.MessageOnFloor(1,1,floor+1);
    }

    private void OnLightKorpus()
    {
        if (b_LightKorpus.image.color != _offColor)
        {
            _manager.MessageOffAllLight();
            OffLightKorpus();
            return;
        }

        _manager.MessageOnHouse(1,1);
        b_LightKorpus.image.color = OnColor;
    }
    
    public void OffLightKorpus()
    {
        b_LightKorpus.image.color = _offColor;
    }

    private void OnChosePanel()
    {
        _manager.allChosePanel.Show();
    }


}

[Serializable]
public class FloorClass
{
    public int CountOffice;
    public Image LightMain;
    public Image LightDark;
    public int Number;
    public Sprite FloorSprite;
    public FloorImagePrefab FloorImagePrefab;
    public List<OfficeClass> OfficeClasses = new List<OfficeClass>();
}

[Serializable]
public class OfficeClass
{
    public float Area;
    public Sprite OfficeSprite;
    public int Number;
    [HideInInspector] public MyObject MyObject = null;
    public int People;
    public string ChosenText = "";
    [HideInInspector] public int Floor;

    private string key;

    public void Init()
    {
        key = Floor + "." + Number;
        
        if (PlayerPrefs.HasKey(key))
        {
            ChosenText = PlayerPrefs.GetString(key);
        }
    }

    public void SaveChosenText(string text)
    {
        ChosenText = text;
        PlayerPrefs.SetString(key, text);
    }
}
