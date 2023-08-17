using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingMenu : Menu<SettingMenu>
{
    [Header("Setting Switch Sprite")]
    public Sprite[] switchs = new Sprite[1];

    [Header("Setting Switch Btns")]
    public Image soundBtn = null;
    public Image hapTickBtn = null;

    public bool isSoundOn = true;
    public bool isHaptickOn = true;

    private void Start()
    {
        ConnectUI("SOUND_ONOFF", soundBtn);
        ConnectUI("HAPTICK_ONOFF", hapTickBtn);
    }

    private void ConnectUI(string _st, Image _img)
    {
        var _b = PlayerPrefs.GetInt(_st);


        if (_b == 0)
        {
            _img.sprite = switchs[0];
            CheckBoolen(_st, true);
        }
        else
        {
            _img.sprite = switchs[1];
            CheckBoolen(_st, false);
        }

    }

    private void CheckBoolen(string _str, bool _isbool)
    {
        if (_str == "SOUND_ONOFF")
        {
            isSoundOn = _isbool;
        }
        else
        {
            isHaptickOn = _isbool;
        }
    }

    public void OnClickSound()
    {
        if (isSoundOn = !isSoundOn)
        {
            soundBtn.sprite = switchs[0];
        }
        else
        {
            soundBtn.sprite = switchs[1];
        }
        PlayerPrefs.SetInt("SOUND_ONOFF", isSoundOn ? 0 : 1);
    }

    public void OnClickHapTick()
    {
        if (isHaptickOn = !isHaptickOn)
        {
            hapTickBtn.sprite = switchs[0];
        }
        else
        {
            hapTickBtn.sprite = switchs[1];
        }
        PlayerPrefs.SetInt("HAPTICK_ONOFF", isHaptickOn ? 0 : 1);
    }

    public void OnClickBeokWon()
    {

    }

    public void OnClickExitSetting()
    {
        base.OnClickBack();
    }

    private void CheckRemember(int _num, bool _isCheck)
    {
        Debug.Log(_num);

        //switch (_num)
        //{
        //    case 0:
        //        _isCheck = true;
        //        break;

        //    case 1:
        //        _isCheck = false;
        //        break;
        //}
    }
}
