using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class Settings : MonoBehaviour
{
    public static Settings Instance { get; private set; }
    public float EarthMass { get; private set; }

    public UnityEvent OnClearProjectiles = new UnityEvent();

    public GameObject Earth;

    [HideInInspector] public bool ForceIndicatorsActive;

    private void Start()
    {
        Instance = this;
        EarthMass = 1.0f;
    }

    public void SetMass(float value)
    {
        EarthMass = value;
    }

    public void ClearProjectiles()
    {
        OnClearProjectiles.Invoke();
    }

    public void ToggleForceIndicators()
    {
        ForceIndicatorsActive = !ForceIndicatorsActive;
    }

    public void StartInvokeDestroy(GameObject toDestroy, float delay)
    {
        StartCoroutine(InvokeDestroy(toDestroy, delay));
    }

    public IEnumerator InvokeDestroy(GameObject toDestroy, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(toDestroy);
    }
}
