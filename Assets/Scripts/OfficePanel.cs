using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OfficePanel : MonoBehaviour
{

    public Button b_Back;
    public Image b_Image;
    public TMP_Text t_Floor;
    public TMP_Text t_Number;
    public TMP_Text t_Area;
    public TMP_Text t_People;
    public TMP_Text t_Price;
    
    private GameManager _manager;
    private int _floor;
    private int _office;
    
    
    public void Init(GameManager manager)
    {
        _manager = manager;
        b_Back.onClick.AddListener(Hide);
        
        Hide();
    }

    public void Show(int floor, int office)
    {
        gameObject.SetActive(true);
        _floor = floor;
        _office = office;
        b_Image.sprite = _manager.mainPanel.Floors[floor].OfficeClasses[office - 1].OfficeSprite;
        t_Floor.text = (floor + 1).ToString();
        if (floor.ToString().Length == 1) t_Floor.text = "0" + (floor + 1).ToString();
        t_Number.text = t_Floor.text + ".0" + office.ToString();
        t_Area.text = _manager.mainPanel.Floors[floor].OfficeClasses[office - 1].Area.ToString().Replace(",", ".") +
                      "<size=80%>м" + _manager.SymvolQuadro + "</size>";
        t_Price.text =
            _manager.GetSplitPrice(_manager.mainPanel.Floors[floor].OfficeClasses[office - 1].MyObject.Price) + " " +
            _manager.SymvolRuble;
        t_People.text = _manager.mainPanel.Floors[floor].OfficeClasses[office - 1].People + " <size=80%>чел.";
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
    
    


}
