using UnityEngine;
using UnityEngine.VFX;

[CreateAssetMenu(fileName = "EnemyType", menuName = "ScriptableObjects/EnemyType")]
public class EnemyType : ScriptableObject
{
    public float speed = 1f;
    public int damage = 1;
    public int health = 1;
    public int reward = 1;
    public VisualEffect deathEffect;
}
