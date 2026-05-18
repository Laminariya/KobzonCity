using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class MyDataClass : MonoBehaviour
{
    
    private GameManager _manager;
    private MyData _myData;
    private string _nameJK = "Шкиперский 19";

    public void Init()
    {
        Debug.Log("Create My Data");
        _manager = GameManager.instance;
        CreateData();
    }

    private void CreateData()
    {
        _myData = new MyData();
        _myData.Buildings = new List<MyBuilding>();
        
        MyBuilding build = new MyBuilding();

        foreach (var objectClass in _manager.Feed.Objects)
        {
            MyObject myObject = new MyObject(objectClass);
            build.MyObjects.Add(myObject);
        }
        
        build.BuildingName = build.MyObjects[0].BuioldingName;
        build.BuildingSection = build.MyObjects[0].BuioldingSection;
        _myData.Buildings.Add(build);
        Debug.Log("Create My Data 1");
        int countFloor = _myData.Buildings[0].MyObjects[0].CountFloor;
        Debug.Log("Create My Data 2");
        for (int i = 0; i < countFloor; i++)
        {
            MyFloor floor = new MyFloor();
            _myData.Buildings[0].MyFloors.Add(floor);
            Debug.Log("Create My Data 4");
            int price = int.MaxValue;
            float minArea = int.MaxValue;
            float maxArea = 0;
            foreach (var myObject in _myData.Buildings[0].MyObjects)
            {
                if (myObject.Floor == i + 1)
                {
                    floor.MyObjects.Add(myObject);
                    Debug.Log("Create My Data 5");
                    if(myObject.Price<price) price = myObject.Price;
                    floor.MinPrice = price;
                    if(myObject.Area<minArea) minArea = myObject.Area;
                    floor.MinArea = minArea;
                    if(myObject.Area>maxArea) maxArea = myObject.Area;
                    floor.MaxArea = maxArea;
                }
            }
        }
        Debug.Log("Create My Data 3");
        foreach (var building in _myData.Buildings)
        {
            int maxPrice = 0;
            int minPrice = int.MaxValue;
            float maxArea = 0;
            float minArea = float.MaxValue;
            int maxFloor = 0;
            int minFloor = int.MaxValue;
            foreach (var myObject in building.MyObjects)
            {
                if (myObject.Price > maxPrice) maxPrice = myObject.Price;
                if (myObject.Price < minPrice) minPrice = myObject.Price;

                if (myObject.Area > maxArea) maxArea = myObject.Area;
                if (myObject.Area < minArea) minArea = myObject.Area;

                if (myObject.Floor > maxFloor) maxFloor = myObject.Floor;
                if (myObject.Floor < minFloor) minFloor = myObject.Floor;
            }

            building.MaxPrice = maxPrice;
            building.MinPrice = minPrice;
            building.MaxArea = maxArea;
            building.MinArea = minArea;
            building.MaxFloor = maxFloor;
            building.MinFloor = minFloor;
        }
        
        _manager.MyData = _myData;

        // foreach (var myFloor in _myData.Buildings[0].MyFloors)
        // {
        //     Debug.Log("Floor " + myFloor.NumberFloor + " " +  myFloor.MyObjects.Count);
        // }
    }

}

[Serializable]
public class MyData
{
    public List<MyBuilding> Buildings = new List<MyBuilding>();
}

[Serializable]
public class MyBuilding
{
    public List<MyObject> MyObjects = new List<MyObject>();
    public List<MyFloor> MyFloors = new List<MyFloor>();
    public int Korpus;
    public int Section;
    public float MinArea;
    public float MaxArea;
    public int MinPrice;
    public int MaxPrice;
    public int MinFloor;
    public int MaxFloor;
    public string BuildingName;
    public string BuildingSection;
}

[Serializable]
public class MyFloor
{
    public List<MyObject> MyObjects = new List<MyObject>();
    public int MinPrice;
    public float MaxArea;
    public float MinArea;
    public int NumberFloor;
}

[Serializable]
public class MyObject
{
    public ObjectClass ObjectClass;
    public float Area;
    public int Korpus;
    public int Floor;
    public int CountFloor;
    public float CeilingHeight; //Находиться в Билдинге
    public int Number;
    public int Price;
    
   
    public bool IsFree;
    public Sprite FlatSprite;
    public Sprite FloorSprite;
    public string Decoration;
    public int NumberOnFloor;
    public int Status;
    public string BuioldingName;
    public string BuioldingSection;

    public int SendHouse;
    public int SendPorch;

    public MyObject(ObjectClass objectClass)
    {
        ObjectClass = objectClass;
        Area = ObjectClass.TotalArea;
        Korpus = 1;
        Floor = ObjectClass.FloorNumber;
        CountFloor = ObjectClass.Building.FloorsCount;
        CeilingHeight = 3.65f;
        Price = ObjectClass.BargainTerms.Price;
        Decoration = ObjectClass.Decoration;
        //Debug.Log("Number " + ObjectClass.JKSchema.Flat.FlatNumber);
        if (ObjectClass.JKSchema.House.Flat != null)
        {
            string[] str = ObjectClass.JKSchema.House.Flat.FlatNumber.Split(".");
            Number = int.Parse(str[1]);
            //Debug.Log("Number " + Number);
        }
        else
        {
            Number = 0;
        }

        
    }

}

[Serializable]
public class SendNumberFlat
{
    public int Korpus;
    public int House;
    public int Porch;
    public int StartFlat;
    public int FinishFlat;
}


