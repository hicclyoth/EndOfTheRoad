using System.Collections.Generic;
using UnityEngine;

public class LevelResetManager : MonoBehaviour
{
    public static LevelResetManager Instance { get; private set; }

    private List<IResettable> resettableObjects = new List<IResettable>();

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

    public void Register(IResettable resettable)
    {
        if (!resettableObjects.Contains(resettable))
        {
            resettableObjects.Add(resettable);
        }
    }

    public void ResetLevel()
    {
        foreach (var resettable in resettableObjects)
        {
            resettable.ResetState();
        }
    }
}
