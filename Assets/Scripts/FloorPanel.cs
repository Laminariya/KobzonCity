using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloorPanel : MonoBehaviour
{

    public Transform FloorPoint;
    public Button b_Up;
    public Button b_Down;
    public Button b_Back;
    public TMP_Text t_Floor;
    public TMP_Text t_CountOffice;
    public GameObject OfficePrefab;
    public Transform OfficeParent;
    
    
    private GameManager _manager;
    private MainPanel _mainPanel;
    private int _currentFloor;
    private FloorImagePrefab _floorImagePrefab;
    private List<GameObject> _officesPrefabs = new List<GameObject>();
    
    public void Init(GameManager manager)
    {
        _manager = manager;
        _mainPanel = _manager.mainPanel;
        b_Up.onClick.AddListener(OnUp);
        b_Down.onClick.AddListener(OnDown);
        b_Back.onClick.AddListener(OnBack);
        
        Hide();
    }

    public void Show(int floor)
    {
        Debug.Log("Show FloorPanel " + floor);
        gameObject.SetActive(true);
        if(_floorImagePrefab != null)
            Destroy(_floorImagePrefab.gameObject);
        _currentFloor = floor;

        _floorImagePrefab =
            Instantiate(_mainPanel.Floors[_currentFloor].FloorImagePrefab.gameObject, FloorPoint)
                .GetComponent<FloorImagePrefab>();
        _floorImagePrefab.Init(_mainPanel.Floors[_currentFloor]);
        //FloorImage.sprite = _mainPanel.Floors[_currentFloor].FloorSprite;
        ChangeFloor(_currentFloor);

        for (int i = 0; i < _officesPrefabs.Count; i++)
        {
            Destroy(_officesPrefabs[i]);
        }
        _officesPrefabs.Clear();
        
        foreach (var officeClass in _mainPanel.Floors[_currentFloor].OfficeClasses)
        {
            if(officeClass.MyObject.Area<=0) continue;
            GameObject office = Instantiate(OfficePrefab, OfficeParent);
            office.GetComponent<OfficeScrollPrefab>().Init(floor, officeClass.Number);
            _officesPrefabs.Add(office);
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void ShowLight(int floor)
    {
        foreach (var floorClass in _mainPanel.Floors)
        {
            floorClass.LightDark.enabled = false;
        }
        _manager.mainPanel.Floors[floor].LightDark.enabled = true;
    }

    private void ChangeFloor(int floor)
    {
        ShowLight(floor);
        t_Floor.text = (floor+1).ToString();
        t_CountOffice.text = _mainPanel.Floors[floor].CountOffice.ToString();
        //FloorImage.sprite = _mainPanel.Floors[floor].FloorSprite;
    }

    private void OnBack()
    {
        Hide();
        _mainPanel.Show(_currentFloor);
    }

    private void OnUp()
    {
        _currentFloor++;
        if (_currentFloor >= _mainPanel.Floors.Count)
        {
            _currentFloor = 17;
        }
        Show(_currentFloor);
        //ChangeFloor(_currentFloor);
    }

    private void OnDown()
    {
        _currentFloor--;
        if (_currentFloor <=0)
        {
            _currentFloor = 0;
        }
        Show(_currentFloor);
        //ChangeFloor(_currentFloor);
    }

}
