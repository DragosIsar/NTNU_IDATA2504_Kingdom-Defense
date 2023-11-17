using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private List<Transform> pathPos;
    
    [SerializeField] private float speed = 1f;
    [SerializeField] private float damage = 1f;
    [SerializeField] private float health = 1f;

    private Rigidbody rb;
    private int curPosIndex;
    
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
    }
    
    private void Update()
    {
        if (IsAtPathPos())
        {
            GoToNextPos();
        }
        else
        {
            MoveToTarget();
        }
    }

    public void MoveToTarget()
    {   
        Vector3 dir = pathPos[curPosIndex].position - transform.position;
        dir.y = 0;

        transform.rotation = Quaternion.LookRotation(dir);

        rb.velocity = transform.forward * speed;
    }

    private bool IsAtPathPos ()
    {
        return Vector3.Distance(transform.position, pathPos[curPosIndex].position) < 1f;
    }

    private void GoToNextPos () {
        if(curPosIndex < pathPos.Count)
        {
            curPosIndex++;
        }
    }

    public void TestPathPos()
    {
        Vector3 dir = pathPos[1].position - transform.position;
        dir.y = 0;
        GetComponent<Rigidbody>().velocity = dir.normalized * speed;
        
        Debug.Log(transform.position);
    }
    
    public void Damage(float damage)
    {
        if (health - damage <= 0)
        {
            health = 0;
            Die();
        }
        else
        {
            health -= damage;
        }
    }

    public void InitPath (List<Transform> path)
    {
        pathPos = path;
    }
    
    private void Die()
    {
        Destroy(gameObject);
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Base"))
        {
            LevelManager.Instance.DamageBase(damage);
            Die();
        }
    }
}
