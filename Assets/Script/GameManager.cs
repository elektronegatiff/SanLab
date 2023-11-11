using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static int count;
    [SerializeField] GameObject userInformation;
    private void Start()
    {
        userInformation.SetActive(false);
        count = 0;
    }
    void Update()
    {
        if (count >= 10)
        {
            userInformation.SetActive(true);
        }
    }
    public void Restart()
    {
       
        SceneManager.LoadScene(0);
    }
}
