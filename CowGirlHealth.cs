using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 

public class CowGirlHealth : MonoBehaviour
{
    public void LoadScene(string Scene){
        SceneManager.LoadScene(Scene); 
    }
    public void death(){
        LoadScene("lvl1Night1"); 
    }
}
