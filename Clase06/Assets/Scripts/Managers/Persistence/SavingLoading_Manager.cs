using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SavingLoading_Manager : MonoBehaviour
{
    private string SavePath => Application.persistentDataPath + "/save.txt";

    public static SavingLoading_Manager instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

       Debug.Log(SavePath);

        //Load();
    }

    private void Start()
    {
        
    }

    [ContextMenu("Save")]
    public void Save()
    {
        var state = LoadFIle();
        CaptureState(state);
        SaveFile(state);
        print(Application.persistentDataPath);
    }


    [ContextMenu("Load")]
    public void Load()
    {
        var state = LoadFIle();
        restoreState(state);
    }


    private void CaptureState(Dictionary<string,object> state)
    {
        foreach (var saveable in FindObjectsOfType<SaveableEntity>())
        {
            state[saveable.Id] = saveable.CaptureState();
        }
    }


    private void restoreState(Dictionary<string, object> state)
    {
        foreach (var saveable in FindObjectsOfType<SaveableEntity>())
        {
           if(state.TryGetValue(saveable.Id, out object value))
           {
                saveable.RestoreState(value);
           }
        }
    }


    private Dictionary<string, object> LoadFIle()
    {
        if (!File.Exists(SavePath))
        {
            return new Dictionary<string, object>();
        }

        using (FileStream stream = File.Open(SavePath, FileMode.Open))
        {
            var formatter = new BinaryFormatter();
            return (Dictionary<string, object>)formatter.Deserialize(stream);
        }
    }



    private void SaveFile(object State)
    {

        using (var stream = File.Open(SavePath, FileMode.Create))
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(stream, State); 
        }


    }
}
