using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine;

public class LevelSelectMenu : MonoBehaviour
{
    public void GoToLevelOne()
    {
        SceneManager.LoadScene("BuildRevised");
    }
}
