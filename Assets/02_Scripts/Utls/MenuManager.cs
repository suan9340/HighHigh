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

        // C# ���÷��� ����� ���� �Լ�Ÿ���� ���ͼ� ����
        System.Type _myType = this.GetType();
        BindingFlags _myFlag = BindingFlags.Instance | BindingFlags.Public | BindingFlags.DeclaredOnly;
        FieldInfo[] _fields = _myType.GetFields(_myFlag);

        // ���� ���� �ʵ� Ȯ�ο�
        for (int i = 0; i < _fields.Length; i++)
        {
            Debug.Log($"fields {_fields[i]}");
        }

        foreach (FieldInfo field in _fields)
        {
            Menu _prefab = field.GetValue(this) as Menu;

            if (_prefab != null)
            {
                // ���� �����ϰ� �θ� Ʈ������������ �־��ش�
                Menu _menuInstance = Instantiate(_prefab, menuParaent_transform);

                // ó�� �����ϰ� �θ� Ʈ������������ �־��ش�
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

        // �����ִ� ĵ������ �� ����
        if (menuStack.Count > 0)
        {
            foreach (Menu _menu in menuStack)
            {
                _menu.gameObject.SetActive(false);
            }
        }

        // ������ �ϴ� ĵ������ ���� ���ÿ� �־���
        _menuInstance.gameObject.SetActive(true);
        menuStack.Push(_menuInstance);
    }

    public void CloseMenu()
    {
        if (menuStack.Count == 0)
        {
            Debug.LogWarning("���� ĵ������ ���� ����");
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
