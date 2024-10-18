using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lasso : MonoBehaviour
{
    // Update is called once per frame

    public GameObject cloud;
    public Cowgirl cowgirl; 

    void Start(){
        cowgirl = transform.parent.GetComponent<Cowgirl>();
    }
    void Update()
    {
        if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1)){
            LassoCloud(); 
        }
    }

    void LassoCloud(){

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); 
        RaycastHit hit; 

        if(Physics.Raycast(ray, out hit)){
            if(hit.collider.gameObject == cloud){
                cowgirl.PullToCloud(); 
            }
        }
    }
}
