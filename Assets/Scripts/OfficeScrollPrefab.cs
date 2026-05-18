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
    
    private int _floor;
    private int _office;
    
    public void Init(int floor, int office)
    {
        _floor = floor;
        _office = office;
        //Debug.Log(_floor + ", " + _office);
        Button.onClick.AddListener(OnClick);
        Plan.sprite = GameManager.instance.mainPanel.Floors[floor].OfficeClasses[office-1].OfficeSprite;
        Area.text = GameManager.instance.mainPanel.Floors[floor].OfficeClasses[office - 1].Area.ToString() + " м" +
                    GameManager.instance.SymvolQuadro;
        string f = (floor+1).ToString();
        string o = office.ToString();
        if(floor.ToString().Length==1) f = "0" + (floor+1).ToString();
        if(o.ToString().Length==1) o = "0" + _office.ToString();
        Number.text = f + "." + o;
        Price.text = GameManager.instance.GetSplitPrice(GameManager.instance.mainPanel.Floors[floor]
                         .OfficeClasses[office - 1].MyObject.Price) +
                     " " + GameManager.instance.SymvolRuble;
    }

    private void OnClick()
    {
        //Debug.Log("OnClick");
        GameManager.instance.officePanel.Show(_floor, _office);
    }


}
