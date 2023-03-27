using UnityEngine;

[CreateAssetMenu(fileName = "NewCharacterStatsSO", menuName = "CharacterStatsSO")]
public class CharacterStatsSO :ScriptableObject
{
    public float weight;
    public float movementSpeed;
    public float maxRotationSpeed;
    public float attack;
    public float attackSpeed;
    public float defense;
    public float defenseWeight;
    public float jumpSpeed;
}
