using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Wave", menuName = "ScriptableObjects/Wave")]
public class Wave : ScriptableObject
{
    public Enemy[] enemyPrefabs;
    [Tooltip("in seconds")]
    public float timeToNextEnemy;

    [HideInInspector] public int enemiesLeft;
}
