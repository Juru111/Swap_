using UnityEngine;

[CreateAssetMenu(fileName = "NewInputConfig", menuName = "Player/InputConfig")]
public class InputConfig : ScriptableObject
{
    [SerializeField] private KeyCode jumpKey = KeyCode.None;
    [SerializeField] private KeyCode leftKey = KeyCode.None;
    [SerializeField] private KeyCode rightKey = KeyCode.None;
    [SerializeField] private KeyCode attackKey = KeyCode.None;

    public KeyCode JumpKey => jumpKey;
    public KeyCode LeftKey => leftKey;
    public KeyCode RightKey => rightKey;
    public KeyCode AttackKey => attackKey;
}
