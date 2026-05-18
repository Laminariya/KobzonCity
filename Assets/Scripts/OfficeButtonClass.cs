using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class OfficeButtonClass : MonoBehaviour
{

    public float Area;
    public int Number;

    private Button button;
    private TMP_Text _areaText;
    private int _floor;
    private int _office;


    public void Init(int floor, int office)
    {
        _floor = floor;
        _office = office;
        
        Debug.Log("Office button class init");
        if (GameManager.instance.mainPanel.Floors[floor].OfficeClasses[_office].MyObject.Area <=0 )
        {
            Debug.Log("Init XXX");
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(true);
        button = GetComponent<Button>();
        button.onClick.AddListener(OnNext);
        _areaText = GetComponentInChildren<TMP_Text>();
        _areaText.text = Area.ToString().Replace(",", ".");
    }

    private void OnNext()
    {
        GameManager.instance.officePanel.Show(_floor, _office+1);
    }


}
