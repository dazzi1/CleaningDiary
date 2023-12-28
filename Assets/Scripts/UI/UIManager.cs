﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
/// <summary>
/// 菜单场景的UI管理
/// </summary>
public class UIManager : MonoBehaviour
{
    //共用变量
    private int mainChoice; //玩家选择的界面索引
    public GameObject[] panels; //设置，武器，关卡界面
    public Image[] imgSelects;//选择界面的图标
    public Text textMoney;
    private GameManager gameManager;
    //设置界面
    public Image imgJoy;
    public Image imgVol;
    public Sprite volSprite;
    public Sprite muteSprite;
    //武器界面
    public Image[] imgSelectedWeapons;
    private Button[] btnSelectedWeapons;
    private Text[] textSelectedWeapons;
    public int[] weaponsPrice;

    //关卡页面
    public Image[] imgLevels;
    private Button[] btnLevels;
    private Text[] textLevels;

    //UI音频
    public AudioClip[] clickSound;
    private AudioSource audioSource;

    public GameObject gameManagerGo;
    private void Awake()
    {
        if (GameManager.Instance == null)
        {
            Instantiate(gameManagerGo);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        weaponsPrice = new int[] { 0, 100, 200, 350, 400, 500 };
        audioSource = GetComponent<AudioSource>();
        btnLevels = new Button[imgLevels.Length];
        textLevels = new Text[imgLevels.Length];
        for (int i = 0; i < imgLevels.Length; i++)
        {
            textLevels[i] = imgLevels[i].transform.GetChild(0).GetComponent<Text>();
        }
        for (int i = 0; i < imgLevels.Length; i++)
        {
            btnLevels[i] = imgLevels[i].GetComponent<Button>();
        }
        btnSelectedWeapons = new Button[imgSelectedWeapons.Length];
        textSelectedWeapons = new Text[imgSelectedWeapons.Length];
        for (int i = 0; i <imgSelectedWeapons.Length; i++)
        {
            btnSelectedWeapons[i] = imgSelectedWeapons[i].GetComponent<Button>();
        }
        for (int i = 0; i < imgSelectedWeapons.Length; i++)
        {
            textSelectedWeapons[i] = imgSelectedWeapons[i].transform.GetChild(0).GetComponent<Text>();
        }
        UpdateMoneyText();
        mainChoice = 3;
        InitUI();
    }

    #region 初始化UI
    /// <summary>
    /// 初始化所有UI
    /// </summary>
    private void InitUI()
    {
        InitCommon();
        InitSetting();
        InitWeapon();
        InitLevel();
    }
    /// <summary>
    /// 共用界面
    /// </summary>
    private void InitCommon() {
        //隐藏panels
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
        }
        //设置图标为半透明绿色
        for (int i = 0; i < imgSelects.Length; i++)
        {
            imgSelects[i].color = new Color(0, 1, 0, 1.3f);
        }
        imgSelects[mainChoice - 1].color = new Color(0, 1, 0, 1);
        panels[mainChoice - 1].SetActive(true);
    }
    /// <summary>
    /// 初始化设置面板
    /// </summary>
    private void InitSetting() {
        //缩小摇杆
        //imgJoy.rectTransform.localScale = new Vector2(gameManager.joystickSize*2,gameManager.joystickSize*2);
        if (gameManager.volume == 0)
        {
            imgVol.sprite = muteSprite;
            imgVol.color = new Color(1, 0, 0, 0.3f);
        }
        else
        {
            imgVol.sprite = volSprite;
            imgVol.color = new Color(0, 1, 0, gameManager.volume);
        }
    }
    /// <summary>
    /// 初始化武器面板
    /// </summary>
    private void InitWeapon() {
        for (int i = 0; i < imgSelectedWeapons.Length; i++)
        {
            if (gameManager.money>=weaponsPrice[i])
            {
                imgSelectedWeapons[i].color = new Color(0, 1, 0, 0.3f);
                btnSelectedWeapons[i].interactable = true;
                textSelectedWeapons[i].color = new Color(0, 1, 0, 0.3f);

            }
            else
            {
                imgSelectedWeapons[i].color = new Color(1, 0, 0, 0.3f);
                btnSelectedWeapons[i].interactable = false;
                textSelectedWeapons[i].color = new Color(1, 0, 0, 0.3f);
            }
        }
        imgSelectedWeapons[gameManager.gunLevel-1].color = new Color(0, 1, 0, 1f);
        textSelectedWeapons[gameManager.gunLevel - 1].color = new Color(0, 1, 0, 1f);
    }

    private void InitLevel() {
        for(int i = 0; i < imgLevels.Length; i++)
        {
            if (gameManager.unLockedLevel >= i+1)
            {
                imgLevels[i].color = new Color(0, 1, 0, 0.3f);
                btnLevels[i].interactable = true;
                textLevels[i].color = new Color(0, 1, 0, 0.3f);

            }
            else
            {
                imgLevels[i].color = new Color(1, 0, 0, 0.3f);
                btnLevels[i].interactable = false;
                textLevels[i].color = new Color(1, 0, 0, 0.3f);
            }
        }
        imgLevels[gameManager.selectLevel-1].color = new Color(0, 1, 0, 1f);
        textLevels[gameManager.selectLevel-1].color = new Color(0, 1, 0, 1f);
    }

    #endregion

    #region 共用面板
    /// <summary>
    /// 更新金钱的方法
    /// </summary>
    private void UpdateMoneyText() {
        textMoney.text = gameManager.money.ToString();
    }
    /// <summary>
    /// 播放按钮音效
    /// </summary>
    public void PlayButtonSound() {
        audioSource.PlayOneShot(clickSound[0], gameManager.volume);
    }
    public void PlayPanelSound()
    {
        audioSource.PlayOneShot(clickSound[1], gameManager.volume);
    }
    /// <summary>
    /// 显示相关面板
    /// </summary>
    public void ShowPanel(int choice) {
        mainChoice = choice;
        for (int i = 0; i < panels.Length; i++)
        {
            panels[i].SetActive(false);
            imgSelects[i].color = new Color(0, 1, 0, 0.3f);
        }
        panels[mainChoice - 1].SetActive(true);
        imgSelects[mainChoice - 1].color = new Color(0, 1, 0, 1);
    }
    /// <summary>
    /// 开始游戏
    /// </summary>
    public void StartGame() {
        SceneManager.LoadScene(3);
    }

    #endregion

    #region 设置面板
    public void SetJoyStickSize() {
        gameManager.joystickSize += 0.05f;
        if (gameManager.joystickSize > 0.51f) {
            gameManager.joystickSize = 0.3f;
        }
        imgJoy.rectTransform.localScale = new Vector2(gameManager.joystickSize*2,
            gameManager.joystickSize * 2);
        PlayerPrefs.SetFloat("Joystick",gameManager.joystickSize);

    }
    /// <summary>
    /// 设置音量
    /// </summary>
    public void SetVolume() {
        gameManager.volume += 0.2f;
        if (gameManager.volume > 1) {
            gameManager.volume = 0;
            imgVol.color = new Color(1, 0, 0, 0.3f);
            imgVol.sprite = muteSprite;
        }
        else
        {
            imgVol.color = new Color(0, 1, 0, gameManager.volume);
            imgVol.sprite = volSprite;
        }
        PlayerPrefs.SetFloat("Sound", gameManager.volume);
    }
    /// <summary>
    /// 清空存档
    /// </summary>
    public void ClearSave() {
        PlayerPrefs.DeleteAll();
        gameManager.unLockedLevel = 1;
        gameManager.gunLevel = 1;
        gameManager.money = 0;
        SceneManager.LoadScene(0);
    }
    /// <summary>
    /// 退出当前游戏
    /// </summary>
    public void ExitGame() {
        PlayerPrefs.SetFloat("Sound", gameManager.volume);
        PlayerPrefs.SetFloat("Joystick", gameManager.joystickSize);
        PlayerPrefs.SetInt("Money", gameManager.money);
        PlayerPrefs.SetInt("Levels", gameManager.unLockedLevel);
        Application.Quit();
    }
    #endregion

    #region 武器面板
    /// <summary>
    /// 选抢
    /// </summary>
    public void SelectGun(int gunLevel) {
        gameManager.gunLevel = gunLevel;
        InitWeapon();
    }


    #endregion

    #region 关卡面板
    /// <summary>
    /// 选抢
    /// </summary>
    public void SelectLevel(int selectLevel)
    {
        gameManager.selectLevel = selectLevel;
        InitLevel();
    }


    #endregion
}
