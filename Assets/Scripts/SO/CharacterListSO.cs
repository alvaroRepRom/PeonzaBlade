using UnityEngine;

[CreateAssetMenu( fileName = "CharacterListSO" , menuName = "CharacterListSO" )]
public class CharacterListSO : ScriptableObject
{
    public CharacterStatsSO[] list;
}
