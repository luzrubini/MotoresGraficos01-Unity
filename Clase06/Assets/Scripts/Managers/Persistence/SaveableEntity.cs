using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SaveableEntity : MonoBehaviour
{
   
    [SerializeField] private string id = string.Empty;

    public string Id => id;

    [ContextMenu("Generate id")]

    private void GenerateId() => id = Guid.NewGuid().ToString();

    public object CaptureState()
    {
        var state = new Dictionary<string, object>();

        foreach (var saveable in GetComponents<ISaveable>())
        {
            state[saveable.GetType().ToString()] = saveable.CaptureState();
        }

        return state;
    }


    public object RestoreState(object state)
    {
        var stateDictionary = (Dictionary<string, object>)state;

        foreach(var saveable in GetComponents<ISaveable>())
        {
            string TypeName = saveable.GetType().ToString();

            if (stateDictionary.TryGetValue(TypeName, out object value))
            {
                saveable.RestoreState(value);
            }
        }

        return state;
    }

}
