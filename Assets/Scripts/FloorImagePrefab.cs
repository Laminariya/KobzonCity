using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FloorImagePrefab : MonoBehaviour
{

    public List<OfficeButtonClass> OfficeButtonClasses = new List<OfficeButtonClass>();
    
    private FloorClass _floorClass;
    
    public void Init(FloorClass floorClass)
    {
        Debug.Log("Init FloorImagePrefab");
        _floorClass = floorClass;
        foreach (var officeButton in OfficeButtonClasses)
        {
            officeButton.gameObject.SetActive(false);
            foreach (var officeClass in _floorClass.OfficeClasses)
            {
                if (officeButton.Number == officeClass.Number)
                {
                    officeButton.Init(floorClass.Number-1, officeClass.Number-1);
                    break;
                }
            }
        }
    }

    private void OnNext()
    {
        
    }


}

[Serializable]
public class OfficeImage
{
    public Image image;
    public GameObject TextPanel;
    public TMP_Text AreaText;
    public Button Button;
    public float Area;
}
