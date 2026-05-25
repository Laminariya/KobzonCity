using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChosenPrefab : MonoBehaviour
{
    
    public TMP_Text t_Price;
    public TMP_Text t_Area;
    public TMP_Text t_Number;
    public TMP_Text t_Chosen;
    public Image Plan;
    public Button b_Edit;
    
    public OfficeClass _officeClass;
    
    public void Init(OfficeClass officeClass)
    {
        _officeClass = officeClass;
        b_Edit.onClick.AddListener(OnEdit);
        Plan.sprite = _officeClass.OfficeSprite;
        t_Area.text = _officeClass.Area + " м" + GameManager.instance.SymvolQuadro;
        string f = _officeClass.Floor.ToString();
        string o = _officeClass.Number.ToString();
        if(f.Length==1) f = "0" + f;
        if(o.Length==1) o = "0" + o;
        t_Number.text = f + "." + o;
        t_Price.text = GameManager.instance.GetSplitPrice(_officeClass.MyObject.Price) + " " +
                       GameManager.instance.SymvolRuble;
        t_Chosen.text = _officeClass.ChosenText;
    }

    private void OnEdit()
    {
        GameManager.instance.allChosePanel.OnShowEditPanel(this);
    }

    public void OnSave()
    {
        t_Chosen.text = _officeClass.ChosenText;
        
        if (_officeClass.ChosenText == "")
        {
            Destroy(gameObject);
        }
    }


}
