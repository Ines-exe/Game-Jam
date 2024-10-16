using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowGirl : MonoBehaviour
{
    public float moveSpeed =5f; 
    private Rigidbody2D rb; 
    private Vector3 ogScale; 

    // Start is called before the first frame update
    void Start()
    {

     rb = GetComponent<Rigidbody2D>(); 
     ogScale = transform.localScale;

    }

    void FixedUpdate(){
        //moving left and right
        float xDirection= Input.GetAxis("Horizontal")*moveSpeed*Time.fixedDeltaTime; 
        float yDirection= Input.GetAxis("Vertical")*moveSpeed*Time.fixedDeltaTime; 

        Vector2 newPos = rb.position + new Vector2(xDirection, yDirection); 

        rb.MovePosition(newPos); 
    }

    // Update is called once per frame
    void Update(){
    // jumping on top of the obstacles
        if(Input.GetKeyDown(KeyCode.Space)){
            rb.AddForce(Vector2.up*500);  
        }
    }

    private void OnMouseDown(){
    //ducking away from obstacles on top
        transform.localScale = new Vector3(ogScale.x,ogScale.y*0.5f,ogScale.z);

        StartCoroutine(Standup());
    }

    private IEnumerator Standup(){
        yield return new WaitForSeconds(0.1f); 
        transform.localScale = ogScale; 
    }
}
