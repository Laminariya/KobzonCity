using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OfficeScrollPrefab : MonoBehaviour
{

    public Image Plan;
    public TMP_Text Area;
    public TMP_Text Number;
    public TMP_Text Price;
    public Button Button;
    public GameObject IconChosen;
    
    private int _floor;
    private int _office;
    private OfficeClass _officeClass;
    
    public void Init(int floor, int office)
    {
        _floor = floor;
        _office = office;
        //Debug.Log(_floor + ", " + _office);
        IconChosen.SetActive(false);
        Button.onClick.AddListener(OnClick);
        _officeClass = GameManager.instance.mainPanel.Floors[floor].OfficeClasses[office - 1];
        Plan.sprite = _officeClass.OfficeSprite;
        Area.text = _officeClass.Area + " м" + GameManager.instance.SymvolQuadro;
        string f = (floor+1).ToString();
        string o = office.ToString();
        if(floor.ToString().Length==1) f = "0" + (floor+1).ToString();
        if(o.ToString().Length==1) o = "0" + _office.ToString();
        Number.text = f + "." + o;
        Price.text = GameManager.instance.GetSplitPrice(_officeClass.MyObject.Price) +
                     " " + GameManager.instance.SymvolRuble;
        if (_officeClass.ChosenText != "")
        {
            IconChosen.SetActive(true);
        }
    }

    private void OnClick()
    {
        //Debug.Log("OnClick");
        GameManager.instance.officePanel.Show(_floor, _office, this);
    }

    public void SetChosen()
    {
        IconChosen.SetActive(false);
        if (_officeClass.ChosenText != "")
        {
            IconChosen.SetActive(true);
        }
    }


}
