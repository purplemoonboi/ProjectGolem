using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicRenderTexture : MonoBehaviour
{

    [SerializeField]
    private RenderTexture renderTexture;
    [SerializeField]
    private int width;
    [SerializeField]
    private int height;

    // Start is called before the first frame update
    void Start()
    {
        width = Screen.width;
        height = Screen.height;
        renderTexture.width = width;
        renderTexture.height = height;
    }

    // Update is called once per frame
    void Update()
    {
        if(renderTexture.width != Screen.width || renderTexture.height != Screen.height)
        {
            width = Screen.width;
            height = Screen.height;
            renderTexture.width = width;
            renderTexture.height = height;
        }
    }
}
