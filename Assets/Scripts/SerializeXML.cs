using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using System.Xml.Serialization;
using UnityEditor;
using UnityEngine.Networking;
using UnityEngine.Serialization;

public class SerializeXML : MonoBehaviour
{

    private HttpClient Client = new HttpClient();
    private string _json;
    [HideInInspector] public FeedClass _feedClass;
    //public string _feedURL;
    
    //ссылка на json
    private string _url = "https://feeds.setevie.su/feeds/october/kobzon_cian.xml";
    //private string _url = "https://feed.storage-s3.ru/feed_flat_regions_new.xml";
    private GameManager _manager;
    
    
    public async Task Init(GameManager manager)
    {
        _manager = manager;
        await LoadJSON(_url);
    }

    private async Task LoadJSON(string url)
    {
        var uri = new Uri(url);
        _manager.InfoStartPanel.text += "Load Json4..."+"\r\n";
        using var result = await Client.GetAsync(_url);
        _manager.InfoStartPanel.text += "Load Json5..."+"\r\n";
        string str = await result.Content.ReadAsStringAsync();
        _manager.InfoStartPanel.text += "Load Json6..."+"\r\n";

        // using (UnityWebRequest request = UnityWebRequest.PostWwwForm(_url,""))
        // {
        //     request.SetRequestHeader("Content-Type", "application/json");
        //     request.timeout = 5;
        //     
        //     
        // }
        
        
        XmlSerializer serializer = new XmlSerializer(typeof(FeedClass));
        
        using (StringReader reader = new StringReader(str))
        {
            Debug.Log("CC");
            try
            {
                _manager.InfoStartPanel.text += "Load Json1..."+"\r\n";
                Debug.Log("Try");
                _feedClass = (FeedClass)serializer.Deserialize(reader);
            }
            catch (Exception e)
            {
                _manager.InfoStartPanel.text += "Load Json2..."+"\r\n";
                Debug.Log(e);
                throw;
            }

            GameManager.instance.Feed = _feedClass;
            Debug.Log("XX "+_feedClass.Objects.Count);
             // foreach (var obj in _feedClass.Objects)
             // {
             //     try
             //     {
             //         Debug.Log(obj.FloorNumber + " " + obj.TotalArea + " " + obj.BargainTerms.Price + " "+ obj.JKSchema.House.Flat.FlatNumber);
             //     }
             //     catch (Exception e)
             //     {
             //         Debug.Log(e);
             //     }
             //    
             // }
            reader.Close();
        }

        _manager.InfoStartPanel.text += "\r\nLoad Feed Complete"+"\r\n";
        result.Dispose();
        serializer = null;

        // foreach (var objectClass in _feedClass.Objects)
        // {
        //     if (objectClass.BuildingSection == "Корпус 1" && objectClass.ApartmentNumber == 167)
        //     {
        //         Debug.Log(objectClass.Name);
        //     }
        // }
        
    }

    private void SaveText(string url, string text)
    {
        File.WriteAllText(url, text);
    }

}


    [XmlRoot("feed"), Serializable]
    public class FeedClass
    {
        [FormerlySerializedAs("CreateDate")] [XmlElement("feed_version")] 
        public string FeedVersion;
        
        [XmlElement("object")] 
        public List<ObjectClass> Objects = new List<ObjectClass>();
    }

    [Serializable]
    public class ObjectClass
    {
        //[XmlAttribute("TotalArea")] public float TotalArea;
        
        [XmlElement("TotalArea")] public float TotalArea;

        [XmlElement("BargainTerms")] public BargainTerms BargainTerms;
        
        [XmlElement("FloorNumber")] public int FloorNumber;

        [XmlElement("Building")] public Building Building;
        
        [XmlElement("Decoration")] public string Decoration;
        
        [XmlElement("JKSchema")] public JKSchema JKSchema;
        
        //[XmlElement("LayoutPhoto")] public JKSchema JKSchema;
        
    }
    
    [Serializable]
    public class BargainTerms
    {
        [XmlElement("Price")] public int Price;
    }
    
    [Serializable]
    public class Building
    {
        [XmlElement("FloorsCount")] public int FloorsCount;
    }

    [Serializable]
    public class JKSchema
    {
        [XmlElement("House")] public House House;
    }

    [Serializable]
    public class Flat
    {
        [XmlElement("FlatNumber")] public string FlatNumber;
    }
    
    [Serializable]
    public class House
    {
        [XmlElement("Flat")] public Flat Flat;
        [XmlElement("Id")] public string Id;
    }