using System.Collections.Generic;
using UnityEngine;

public class LevelResetManager : MonoBehaviour
{
    public static LevelResetManager Instance { get; private set; }

    //Creates an empty list of resettableObjects that can be set with
    private List<IResettable> resettableObjects = new List<IResettable>();

    //Checking if there's another instance of LRM, if there is one, destroy it.
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    //Function that can be used with MoveTrap(or other future scripts) to allow them to save previous position
    public void Register(IResettable resettable)
    {
        if (!resettableObjects.Contains(resettable))
        {
            resettableObjects.Add(resettable);
        }
    }
    //Uses IResettable to easily reset the states of traps
    public void ResetLevel()
    {
        foreach (var resettable in resettableObjects)
        {
            resettable.ResetState();
        }
    }
}
