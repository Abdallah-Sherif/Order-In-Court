using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public class Objective : MonoBehaviour
{
    [SerializeField] int numberOfObjectivesToComplete;
    private int objectivesCompleted = 0;
    [SerializeField] UnityEvent onObjectiveComplete;
    [SerializeField] bool findEnemies;
    // Start is called before the first frame update
    void Start()
    {
        if(findEnemies) 
        {
            FindNumberOfEnemies();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void FindNumberOfEnemies()
    {
        numberOfObjectivesToComplete = GameObject.FindGameObjectsWithTag("Enemie").Length;
    }
    public void CompleteAnObjective()
    {
        objectivesCompleted++;
        if(objectivesCompleted >=numberOfObjectivesToComplete) 
        {
            onObjectiveComplete?.Invoke();
        }
    }
    public void complete()
    {
        Debug.Log("Complete");
    }
}
