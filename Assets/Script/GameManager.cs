using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    /// <summary>
    /// Count, Information and Button
    /// </summary>
    #region Definitions
    [Header(" Count ")]
    public static int count;
    [Header(" User Information ")]
    [Tooltip(" User Informations ")]
    [SerializeField] GameObject userInformation;
    [SerializeField] GameObject closeInformation;
    #endregion
    #region Unity Metods
    private void Start()
    {
        userInformation.SetActive(false);
        count = 0;
    }
    /// <summary>
    /// Finish
    /// </summary>
    void Update()
    {
        
        if (count >= 10)
        {
            userInformation.SetActive(true);
        }
    }
    #endregion
    #region Restart
    /// <summary>
    /// Restart Button
    /// </summary>
    public void Restart()
    {
       
        SceneManager.LoadScene(0);
    }
    #endregion
    #region Close Information
    /// <summary>
    /// Close Button
    /// </summary>
    public void CloseInformation()
    {
        closeInformation.SetActive(false);
    }
    #endregion
}
