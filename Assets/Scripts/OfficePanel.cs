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
    public Button b_AddChosen;
    public TMP_Text t_Chosen;
    public Sprite chosen;
    public Sprite unchosen;
    public GameObject ChosenPanel;
    public TMP_InputField InputField;
    public Button b_Save;
    
    private GameManager _manager;
    private int _floor;
    private int _office;
    private OfficeClass officeClass;
    private OfficeScrollPrefab _officeScrollPrefab;
    
    public void Init(GameManager manager)
    {
        _manager = manager;
        b_Back.onClick.AddListener(Hide);
        ChosenPanel.SetActive(false);
        b_AddChosen.onClick.AddListener(OnChosen);
        b_Save.onClick.AddListener(OnSave);
        
        Hide();
    }

    public void Show(int floor, int office, OfficeScrollPrefab officeScrollPrefab)
    {
        gameObject.SetActive(true);
        _floor = floor;
        _office = office;

        _officeScrollPrefab = officeScrollPrefab;
        officeClass = _manager.mainPanel.Floors[floor].OfficeClasses[office - 1];

        if (officeClass.ChosenText != "")
        {
            b_AddChosen.image.sprite = chosen;
            t_Chosen.text = officeClass.ChosenText;
        }
        else
        {
            b_AddChosen.image.sprite = unchosen;
            t_Chosen.text = officeClass.ChosenText;
        }

        b_Image.sprite = officeClass.OfficeSprite;
        t_Floor.text = (floor + 1).ToString();
        if (floor.ToString().Length == 1) t_Floor.text = "0" + (floor + 1).ToString();
        t_Number.text = t_Floor.text + ".0" + office.ToString();
        t_Area.text = officeClass.Area.ToString().Replace(",", ".") +
                      "<size=80%>м" + _manager.SymvolQuadro + "</size>";
        t_Price.text =
            _manager.GetSplitPrice(officeClass.MyObject.Price) + " " +
            _manager.SymvolRuble;
        t_People.text = officeClass.People + " <size=80%>чел.";
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnChosen()
    {
        ChosenPanel.SetActive(true);
        InputField.text = officeClass.ChosenText;
    }

    private void OnSave()
    {
        officeClass.SaveChosenText(InputField.text);
        t_Chosen.text = officeClass.ChosenText;
        
        if (officeClass.ChosenText != "")
        {
            b_AddChosen.image.sprite = chosen;
        }
        else
        {
            b_AddChosen.image.sprite = unchosen;
        }

        if(_officeScrollPrefab != null) _officeScrollPrefab.SetChosen();
        ChosenPanel.SetActive(false);
    }


}
