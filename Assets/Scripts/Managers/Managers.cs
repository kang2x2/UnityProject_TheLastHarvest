using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers s_instance = null;
    public static Managers Instance { get { return s_instance; } }

    // Field
    private static GameManager s_gameManager = new GameManager();
    private static PoolManager s_poolManager = new PoolManager();
    private static SceneManagerEx s_sceneManager = new SceneManagerEx();
    private static FadeManager s_fadeManager = new FadeManager();
    private static ResourceManager s_resoruceManager = new ResourceManager();
    private static SoundManager s_soundManager = new SoundManager();
    private static UIManager s_uiManager = new UIManager();
    private CoroutineManager m_coroutineManager;
    // Propertiy
    public static GameManager GameManagerEx { get { Init(); return s_gameManager; } }
    public static PoolManager PoolManager { get { Init(); return s_poolManager; } }
    public static SceneManagerEx SceneManagerEx { get { Init(); return s_sceneManager; } }
    public static FadeManager FadeManager { get { Init(); return s_fadeManager; } }
    public static ResourceManager ResourceManager { get { Init(); return s_resoruceManager; } }
    public static SoundManager SoundManager { get { Init(); return s_soundManager; } }
    public static UIManager UIManager { get { Init(); return s_uiManager; } }
    public static CoroutineManager CoroutineManager { get { return Instance.m_coroutineManager; } }
    void Start()
    {
        Init();
    }

    private static void Init()
    {
        if (s_instance == null)
        {
            GameObject go = GameObject.Find("@Managers");
            if(go == null)
            {
                go = new GameObject { name = "@Managers" };
            }

            s_instance = go.GetComponent<Managers>();
            if (s_instance == null)
            {
                s_instance = go.AddComponent<Managers>();
            }

            DontDestroyOnLoad(go);

            // CoroutineHelper Create
            GameObject coroutineObj = new GameObject("CoroutineObj");
            coroutineObj.transform.parent = GameObject.Find("@Managers").transform;
            s_instance.m_coroutineManager = coroutineObj.AddComponent<CoroutineManager>();

            // Init()
            s_poolManager.Init();
            s_fadeManager.Init();
            s_sceneManager.Init();
            s_soundManager.Init();
        }
    }

    private void Update()
    {
        if(s_gameManager != null && SceneManagerEx.CurScene.SceneType == Define.SceneType.GameScene)
        {
            s_gameManager.Update();
        }
    }

    private void LateUpdate()
    {
        if (s_gameManager != null && SceneManagerEx.CurScene.SceneType == Define.SceneType.GameScene)
        {
            s_gameManager.LateUpdate();
        }
    }

    public static void Clear()
    {
        s_poolManager.Clear();
        s_gameManager.Clear();
        s_sceneManager.Clear();
        s_soundManager.Clear();
        s_uiManager.Clear();
    }
}
