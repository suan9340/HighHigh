using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;

public class MenuManager : MonoBehaviour
{
    [Header("---- Menus Prefabs ----")]
    public MainMenu mainMenu_prefab = null;

    public Transform menuParaent_transform = null;



    private Stack<Menu> menuStack = new Stack<Menu>();
    private static MenuManager _instance;
    public static MenuManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            _instance = this;
            InitMenus();
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        if (_instance == this)
        {
            _instance = null;
        }
    }

    private void InitMenus()
    {
        if (menuParaent_transform == null)
        {
            var _menuObj = new GameObject("Menus");
            menuParaent_transform = _menuObj.transform;
        }

        DontDestroyOnLoad(menuParaent_transform.gameObject);

        // C# 리플렉션 기능을 통한 함수타입을 얻어와서 통합
        System.Type _myType = this.GetType();
        BindingFlags _myFlag = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;
        FieldInfo[] _fields = _myType.GetFields(_myFlag);

        // 실제 들어온 필드 확인용
        for (int i = 0; i < _fields.Length; i++)
        {
            Debug.Log($"fields {_fields[i]}");
        }

        foreach (FieldInfo field in _fields)
        {
            Menu _prefab = field.GetValue(this) as Menu;

            if (_prefab != null)
            {
                // 최초 생성하고 부모 트랜스폼안으로 넣어준다
                Menu _menuInstance = Instantiate(_prefab, menuParaent_transform);

                // 처음 생성하고 부모 트랜스폼안으로 넣어준다
                if (_prefab != mainMenu_prefab)
                {
                    _menuInstance.gameObject.SetActive(false);
                }
                else
                {
                    OpenMenu(_menuInstance);
                }
            }
        }
    }

    public void OpenMenu(Menu _menuInstance)
    {
        if (_menuInstance == null)
        {
            Debug.LogWarning("Menu Open Error");
            return;
        }

        // 열려있는 캔버스를 다 닫음
        if (menuStack.Count > 0)
        {
            foreach (Menu _menu in menuStack)
            {
                _menu.gameObject.SetActive(false);
            }
        }

        // 열고자 하는 캔버스를 열고 스택에 넣어줌
        _menuInstance.gameObject.SetActive(true);
        menuStack.Push(_menuInstance);
    }

    public void CloseMenu()
    {
        if (menuStack.Count == 0)
        {
            Debug.LogWarning("닫을 캔버스가 없엉 에러");
            return;
        }

        Menu _topMenu = menuStack.Pop();
        _topMenu.gameObject.SetActive(false);

        if (menuStack.Count > 0)
        {
            Menu _nextMenu = menuStack.Peek();
            _nextMenu.gameObject.SetActive(true);
        }
    }
}
