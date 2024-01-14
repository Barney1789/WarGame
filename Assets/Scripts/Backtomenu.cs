using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Backtomenu : MonoBehaviour
{
    public void LoadScene()
    {
        SceneManager.LoadScene("WelcomeScene");
    }
}