using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleModifier : MonoBehaviour
{

    public AnimationCurve ScaleProgressionX;
    public AnimationCurve ScaleProgressionY;
    public AnimationCurve ScaleProgressionZ;
    public float Duration;
    public Transform Target;
    public float CurrentTimer { get; private set; }
    public float InverseDuration { get; private set; }
    // Use this for initialization
    void Awake() { this.enabled = false; }
    private void OnEnable()
    {
        CurrentTimer = 0f;
        if (!Target || CurrentTimer >= Duration)
        {
            this.enabled = false;
        }
        InverseDuration = 0f;
        if (!Mathf.Approximately(Duration, 0f))
        {
            InverseDuration = 1f / Duration;
        }
    }
    public void StartScaleMod()
    {
        this.enabled = true;
    }
    public void ResetAndStartScaleMod()
    {
        if (this.enabled)
        {
            OnEnable();
            return;
        }
        StartScaleMod();
    }
    void Update()
    {
        CurrentTimer += Time.deltaTime;
        float curveValue = Mathf.Min(1f, CurrentTimer * InverseDuration);

        Target.localScale = new Vector3(ScaleProgressionX.Evaluate(curveValue), ScaleProgressionY.Evaluate(curveValue), ScaleProgressionZ.Evaluate(curveValue));

        if (CurrentTimer >= 1f)
        {
            this.enabled = false;
        }
    }
}
