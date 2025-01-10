using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Managers : MonoBehaviour
{
    private static Managers s_instance = null;
    public static Managers Instance { get { return s_instance; } }

    // Field
    private static DataManager s_dataManager = new DataManager();
    private static GameManager s_gameManager = new GameManager();
    private static PoolManager s_poolManager = new PoolManager();
    private static SceneManagerEx s_sceneManager = new SceneManagerEx();
    private static WebManager s_webManager = new WebManager();
    private static FadeManager s_fadeManager = new FadeManager();
    private static ItemCardManager s_itemCardManager = new ItemCardManager();
    private static ResourceManager s_resoruceManager = new ResourceManager();
    private static SoundManager s_soundManager = new SoundManager();
    private static UIManager s_uiManager = new UIManager();
    private CoroutineManager m_coroutineManager;
    // Propertiy
    public static DataManager DataManager { get { Init(); return s_dataManager; } }
    public static GameManager GameManagerEx { get { Init(); return s_gameManager; } }
    public static PoolManager PoolManager { get { Init(); return s_poolManager; } }
    public static SceneManagerEx SceneManagerEx { get { Init(); return s_sceneManager; } }
    public static WebManager WebManager { get { Init(); return s_webManager; } }
    public static FadeManager FadeManager { get { Init(); return s_fadeManager; } }
    public static ItemCardManager ItemCardManager { get { Init(); return s_itemCardManager; } }
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
            Application.targetFrameRate = 60;

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
            s_dataManager.Init();
            s_poolManager.Init();
            s_fadeManager.Init();
            s_sceneManager.Init();
            s_webManager.Init();
            s_soundManager.Init();
        }
    }

    private void Update()
    {
        if (Application.platform == RuntimePlatform.WindowsPlayer || Application.platform == RuntimePlatform.LinuxPlayer || Application.platform == RuntimePlatform.OSXPlayer)
        {
            // 전체화면 전환 후 비율 조정을 강제
            if (Screen.fullScreen == true)
            {
                int setWidth = 1080;
                int setHeight = 1280;
                Screen.SetResolution(setWidth, setHeight, true);
            }
        }

        if (s_gameManager != null && SceneManagerEx.CurScene.SceneType == Define.SceneType.GameScene)
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
