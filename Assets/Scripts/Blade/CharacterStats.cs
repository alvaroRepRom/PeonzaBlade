
[System.Serializable]
public struct CharacterStats
{
    public float weight;
    public float movementSpeed;
    public float maxRotationSpeed;
    public float attack;
    public float attackSpeed;
    public float defense;
    public float defenseWeight;

    public CharacterStats( CharacterStatsSO characterStatsSO )
    {
        weight = characterStatsSO.weight;
        movementSpeed = characterStatsSO.movementSpeed;
        maxRotationSpeed = characterStatsSO.maxRotationSpeed;
        attack = characterStatsSO.attack;
        attackSpeed = characterStatsSO.defense;
        defense = characterStatsSO.defense;
        defenseWeight = characterStatsSO.defenseWeight;
    }
}
