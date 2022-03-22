using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEditor;

[ExecuteInEditMode]
public class DayNightCycle : MonoBehaviour
{
    private Light mainLight;

    [Range(0.0f, 86400.0f)]
    public int seconds = 34000;


    // Start is called before the first frame update
    void Start()
    {
        this.mainLight = this.GetComponent<Light>();
        if (this.mainLight == null)
        {
            throw new MissingComponentException("Missing Light");
        }
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position =
        Quaternion.Euler(((float)this.seconds / 86400.0f) * 360.0f, 0, 0) *
        new Vector3(0.0f, -300.0f, 0.0f);
        this.transform.position = position;
        this.transform.rotation = Quaternion.LookRotation(-position);
        this.mainLight.intensity = 1.25f - Math.Abs((float)this.seconds / 43200.0f - 1.0f);
        this.mainLight.color = new Color(
            1.0f,
            Math.Min(this.mainLight.intensity + 0.05f, 1.0f),
            Math.Min(this.mainLight.intensity, 1.0f)
        );
        RenderSettings.skybox.SetFloat("_Blend", Math.Abs((float)this.seconds / 43200.0f - 1.0f));
        RenderSettings.skybox.SetFloat("_Rotation", ((float)this.seconds / 86400.0f) * 360.0f);
    }
}
