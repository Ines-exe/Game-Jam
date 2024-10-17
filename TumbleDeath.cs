using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TumbleDeath : MonoBehaviour
{
    // Start is called before the first frame update
    void OnCollisionEnter(Collision col){
        if(col.gameObject.CompareTag("Cowgirl")){
            col.gameObject.GetComponent<CowGirlHealth>().death();
        }
    }
}
