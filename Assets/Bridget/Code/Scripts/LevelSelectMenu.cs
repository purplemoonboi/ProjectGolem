using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class LevelSelectMenu : MonoBehaviour
{
    [SerializeField]
    private Sprite sprite;

    private void Start()
    {
        GetComponent<Image>().sprite = sprite;
    }

    public void GoToLevelOne()
    {
        SceneManager.LoadScene("Alpha");
    }
}
