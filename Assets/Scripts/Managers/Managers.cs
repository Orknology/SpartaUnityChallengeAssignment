using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers m_instance;
    static Managers Instance { get { Init(); return m_instance; } }

    private GameManager _gameManager = new GameManager();
    private ResourceManager _resourceManager = new ResourceManager();
    private PoolManager _poolManager = new PoolManager();
    private UIManager _uiManger = new UIManager();
    private AdditionalSceneManager _sceneManager = new AdditionalSceneManager();

    public static GameManager GM => Instance?._gameManager;
    public static ResourceManager RM => Instance?._resourceManager;
    public static PoolManager Pool => Instance?._poolManager;
    public static UIManager UI => Instance?._uiManger;
    public static AdditionalSceneManager Scene => Instance?._sceneManager; 


    private void Awake()
    {
        Init();
    }

    private static void Init()
    {
        if (m_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");

            if (go == null)
            {
                go = new GameObject("@Managers");
                go.AddComponent<Managers>();
            }
            DontDestroyOnLoad(go);
            m_instance = go.GetComponent<Managers>();
        }
    }

    public static void Clear()
    {
        UI.Clear();
        GM.Clear();
        Pool.Clear();
    }

}
