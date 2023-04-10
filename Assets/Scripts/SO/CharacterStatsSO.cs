using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterStatsSO", menuName = "CharacterStatsSO")]
public class CharacterStatsSO :ScriptableObject
{
    [Header("Physical Stats")]
    public float weight;
    public float movementSpeed;
    public float maxRotationSpeed;
    public float attackSpeed;
    public float defenseWeight;
    public float jumpSpeed;

    [Header("Stats")]
    public float normalAttackDamage;
    public float dashAttackDamage;
    public float normalDefense;
    public float specialDefense;
    public float rotationUsedPerSecond;

    [Header("Objects")]
    public Sprite characterImage;
    public GameObject characterPrefab;

    [Header("Show Stats")]
    public float attack;
    public float defense;
    public float agility;
}
