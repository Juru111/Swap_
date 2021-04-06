using UnityEngine;

[CreateAssetMenu(fileName = "NewPrefabDataBase", menuName = "DataBase/PrefabDataBase")]
public class PrefabDataBase : ScriptableObject
{
    [System.Serializable]
    public class ColoredThings
    {
        public GameObject red;
        public GameObject blue;
        public GameObject green;
    }


    public ColoredThings crystal;
    public ColoredThings key;
    [Space]
    public GameObject armor;
}
