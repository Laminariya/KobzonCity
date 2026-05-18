using System;
using System.Collections;
using System.Collections.Generic;
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
    
    private int _currentFloor = 0;
    
    public void Init(GameManager manager)
    {
        _manager = manager;
        
        b_Up.onClick.AddListener(OnUp);
        b_Down.onClick.AddListener(OnDown);
        b_Next.onClick.AddListener(OnNext);
        
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
                        }
                    }
                    else
                    {
                        if (Mathf.Approximately(myObject.Area, officeClass.Area))
                        {
                            officeClass.MyObject = myObject;
                        }
                    }
                }
            }
        }
        
        ChangeFloor(_currentFloor);
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
}
