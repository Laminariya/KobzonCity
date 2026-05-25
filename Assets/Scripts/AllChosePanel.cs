using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AllChosePanel : MonoBehaviour
{

    public Transform ContentParent;
    public GameObject ChosePrefab;
    
    public GameObject EditPanel;
    public TMP_InputField InputField;
    public Button b_Save;
    
    private List<GameObject> ChoseObjects = new List<GameObject>();
    private ChosenPrefab _chosenPrefab;
    
    public void Init()
    {
        b_Save.onClick.AddListener(OnSave);
        EditPanel.SetActive(false);
        Hide();
    }

    public void Show()
    {
        gameObject.SetActive(true);

        for (int i = 0; i < ChoseObjects.Count; i++)
        {
            Destroy(ChoseObjects[i]);
        }
        ChoseObjects.Clear();
        
        foreach (var floor in GameManager.instance.mainPanel.Floors)
        {
            foreach (var officeClass in floor.OfficeClasses)
            {
                if (officeClass.ChosenText == "") continue;
                GameObject obj = Instantiate(ChosePrefab, ContentParent);
                obj.GetComponent<ChosenPrefab>().Init(officeClass);
                ChoseObjects.Add(obj);
            }
        }
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void OnShowEditPanel(ChosenPrefab chosenPrefab)
    {
        _chosenPrefab = chosenPrefab;
        InputField.text = _chosenPrefab._officeClass.ChosenText;
        EditPanel.SetActive(true);
    }

    private void OnSave()
    {
        _chosenPrefab._officeClass.SaveChosenText(InputField.text);
        _chosenPrefab.OnSave();
        EditPanel.SetActive(false);
    }
    
}
