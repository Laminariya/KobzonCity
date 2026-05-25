using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChosenPanel : MonoBehaviour
{
    
    public TMP_InputField InputField;
    public Button b_Save;

    public void Init()
    {
        b_Save.onClick.AddListener(OnSave);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnSave()
    {
        
    }

}
