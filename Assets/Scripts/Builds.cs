using UnityEngine;

public class Builds : MonoBehaviour
{
    private static Builds INSTANCE;
    void Awake()
    {
        INSTANCE = this;
    }

    public static Builds Get()
    {
        return INSTANCE;
    }

    public static GameObject GetGameObject()
    {
        return INSTANCE.gameObject;
    }
}
