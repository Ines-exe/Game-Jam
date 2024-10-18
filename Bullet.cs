using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public GameObject projectilePrefab;
    public float speed = 3f; 
    public float shootingInterval = 3f; 
    public float bulletForce = 5f; 
    public float bulletReload = 10f; 

    private Vector2 direction = new Vector2(1, 0);
    public Transform Cowgirl; 

    void Start(){
        StartCoroutine(Shooting()); 
    }

    private IEnumerator Shooting(){
        while (true){
            if (Vector2.Distance(transform.position, Cowgirl.position) > bulletReload){
                yield return new WaitForSeconds(shootingInterval);
                continue;
            }

            shootingProjectile(direction);
            yield return new WaitForSeconds(shootingInterval);
        }
    }

    private void shootingProjectile(Vector2 direction){
        GameObject projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        
        Rigidbody2D rb = projectile.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.velocity = direction.normalized * bulletForce; 
        }
    }
}
