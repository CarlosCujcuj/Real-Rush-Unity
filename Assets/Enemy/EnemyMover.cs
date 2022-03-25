using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour{


    [SerializeField] List<Waypoint> path = new List<Waypoint>();
    [SerializeField] [Range(0f, 5f)] float speed = 1f;

    Enemy enemy;

    // Called whenever an object is enable  
    void OnEnable(){ 
        FindPath();
        ReturnToStart();
        StartCoroutine(FollowPath());
    }

    void Start(){
        enemy = GetComponent<Enemy>();
    }

    void FindPath(){

        path.Clear();

        GameObject parent = GameObject.FindGameObjectWithTag("Path");


        foreach(Transform child in parent.transform){

            Waypoint waypoint = child.GetComponent<Waypoint>();
            
            if(waypoint != null){
                path.Add(waypoint);
            }
        }
    }

    void ReturnToStart(){
        transform.position = path[0].transform.position;
    }

    void FinishPahth(){

        enemy.StealGold();
        gameObject.SetActive(false);// gameObject Its a reference to itself
    }


    IEnumerator FollowPath(){

        foreach(Waypoint waypoint in path){
            
            Vector3 startPosition = transform.position;
            Vector3 endPosition = waypoint.transform.position;

            float travelPercent = 0f;

            transform.LookAt(endPosition);//move the object to face the next position

            while(travelPercent < 1f){
                travelPercent += Time.deltaTime * speed;
                transform.position = Vector3.Lerp(startPosition, endPosition, travelPercent);
                yield return new WaitForEndOfFrame();
            }
            
        }

        FinishPahth();

    }
}
