using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetLocator : MonoBehaviour{


    [SerializeField] Transform weapon;
    [SerializeField] ParticleSystem projectileParticles;
    [SerializeField] float range = 15;//based on the tiles units, this can be translated into 
                                      // a tile and a half of distance range 
    Transform target;

    //OLD METHOD
    // Start is called before the first frame update
    /*void Start()
    {
        target = FindObjectOfType<Enemy>().transform; // one object
    }*/

    // Update is called once per frame
    void Update(){
        FindClosestTarget();
        AimWeapon();
    }

    void FindClosestTarget(){

        Enemy[] enemies = FindObjectsOfType<Enemy>(); //multiple objects
        Transform closestTarget = null; // to store the closest target
        float maxDistance = Mathf.Infinity;

        foreach(Enemy enemy in enemies){
            float targetDistance = Vector3.Distance(transform.position, enemy.transform.position);

            if(targetDistance < maxDistance){
                closestTarget = enemy.transform;
                maxDistance = targetDistance;
            }
        }

        target = closestTarget;
    }


    void AimWeapon(){

        float targetDistance = Vector3.Distance(transform.position, target.position);
        weapon.LookAt(target);

        if(targetDistance < range){
            Attack(true);
        }else{
            Attack(false);
        }
    }

    void Attack(bool isActive){

        var emissionModule = projectileParticles.emission;
        emissionModule.enabled = isActive;

    }
}
