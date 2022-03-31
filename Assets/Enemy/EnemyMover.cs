using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyMover : MonoBehaviour{


    [SerializeField] [Range(0f, 5f)] float speed = 1f;
    List<Node> path = new List<Node>();

    Enemy enemy;
    GridManager gridManager;
    Pathfinder pathfinder;

    // Called whenever an object is enable  
    void OnEnable(){ 

        ReturnToStart();
        RecalculatePath(true);
        
    }

    void Awake(){
        enemy = GetComponent<Enemy>();
        gridManager = FindObjectOfType<GridManager>();
        pathfinder = FindObjectOfType<Pathfinder>();

    }

    void RecalculatePath(bool resetPath){

        Vector2Int coordinates = new Vector2Int();

        if(resetPath){
            coordinates = pathfinder.StartCoordinates;
        }else{
            coordinates = gridManager.GetCoordinatesFromPosition(transform.position);
        }
        
        StopAllCoroutines();
        path.Clear();
        path = pathfinder.GetNewPath(coordinates);
        StartCoroutine(FollowPath());
    }

    void ReturnToStart(){
        transform.position = gridManager.GetPositionFromCoordinates(pathfinder.StartCoordinates);
    }

    void FinishPahth(){

        enemy.StealGold();
        gameObject.SetActive(false);// gameObject Its a reference to itself
    }


    IEnumerator FollowPath(){

        for(int i = 1; i < path.Count; i++){ // 1 asi empieza con el siguiente Tile y no el current
            
            Vector3 startPosition = transform.position;
            Vector3 endPosition = gridManager.GetPositionFromCoordinates(path[i].coordinates);

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
