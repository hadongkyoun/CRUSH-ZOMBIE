using System;
using System.Data;
using System.IO;
using UnityEngine;



[System.Serializable]
public class PlayerDatas
{
    public int needUpgradeGold_speed = 100;
    public int lv_speed = 1;

    public int needUpgradeGold_durability = 100;
    public int lv_durability = 1;

    public int needUpgradeGold_offline = 100;
    public int lv_offline = 1;

    public float maxVanHP = 20;
    public float boosterSpeedValue = 0f;
    public float increaseMoneyOffline = 0f;

    public int money;
    
    public DateTime lastPlayTime;
}


public class DataManager : Singleton<DataManager>
{

    public PlayerDatas playerData;
    private string path;
    [SerializeField]
    private string fileName = "/save.txt";
    private string keyWord = "sadiqwjwkd#dkwda!! ej2kS@@";

    private float moneyTemp;
    public bool isRewarded;
    

    private void Start()
    {
        path = Application.persistentDataPath + fileName;
        Debug.Log(path);
        LoadData();


        if (playerData.lastPlayTime != DateTime.MinValue)
        {
            UpdateRewardMoney();
            
        }
    }

    public void UpdateRewardMoney()
    {
        TimeSpan elapsedTime = (DateTime.Now).Subtract(playerData.lastPlayTime);
        float calculateElapsedTime = (float)elapsedTime.TotalMinutes / 2;
        if (calculateElapsedTime > 0)
            playerData.money += (int)calculateElapsedTime;
        UIManager.Instance.ActivateRewardPanel((int)calculateElapsedTime);
    }

    public void SaveData()
    {
        string data = JsonUtility.ToJson(playerData);
        File.WriteAllText(path, EncryptAndDecrypt(data));
    }

    public void LoadData()
    {
        if (!File.Exists(path))
        {
            SaveData();
        }
        string data = File.ReadAllText(path);
        playerData = JsonUtility.FromJson<PlayerDatas>(EncryptAndDecrypt(data));
        UIManager.Instance.UpdateMoneyText(playerData.money);
        UIManager.Instance.UpdateBoosterSpeedLevelAndMoney(playerData.lv_speed, playerData.needUpgradeGold_speed);
        UIManager.Instance.UpdateCarHpLevelAndMoney(playerData.lv_durability, playerData.needUpgradeGold_durability);
        UIManager.Instance.UpdateOfflineRewardsLevelAndMoney(playerData.lv_offline, playerData.needUpgradeGold_offline);
    }

    private string EncryptAndDecrypt(string data)
    {
        string result = "";
        for (int i = 0; i < data.Length; i++)
        {
            result += (char)(data[i] ^ keyWord[i % keyWord.Length]);
        }

        return result;
    }


    public void UpgradeBoosterSpeedData()
    {
        if (playerData.money < playerData.needUpgradeGold_speed)
            return;
        else
        {
            playerData.boosterSpeedValue += 0.25f;

            playerData.money -= playerData.needUpgradeGold_speed;
            UIManager.Instance.UpdateMoneyText(playerData.money);
            playerData.needUpgradeGold_speed += 10;
            playerData.lv_speed++;

        }
        UIManager.Instance.UpdateBoosterSpeedLevelAndMoney(playerData.lv_speed, playerData.needUpgradeGold_speed);

        SaveData();
    }
    public void UpgradeDurabilityData()
    {
        if (playerData.money < playerData.needUpgradeGold_durability)
            return;
        else
        {
            playerData.maxVanHP += 2.0f;
            playerData.money -= playerData.needUpgradeGold_durability;
            UIManager.Instance.UpdateMoneyText(playerData.money);
            playerData.needUpgradeGold_durability += 10;
            playerData.lv_durability++;
        }

        UIManager.Instance.UpdateCarHpLevelAndMoney(playerData.lv_durability, playerData.needUpgradeGold_durability);
        SaveData();
    }

    public void UpgradeOfflineMoneyData()
    {
        if (playerData.money < playerData.needUpgradeGold_offline)
            return;
        else
        {
            playerData.increaseMoneyOffline += 0.2f;
            playerData.money -= playerData.needUpgradeGold_offline;
            UIManager.Instance.UpdateMoneyText(playerData.money);
            playerData.needUpgradeGold_offline += 10;
            playerData.lv_offline++;
        }

        UIManager.Instance.UpdateOfflineRewardsLevelAndMoney(playerData.lv_offline, playerData.needUpgradeGold_offline);

        SaveData();
    }

    public void UpdateMoney()
    {
        if (isRewarded)
            playerData.money += (int)moneyTemp * 2;
        else
            playerData.money += (int)moneyTemp;

        UIManager.Instance.UpdateMoneyText(playerData.money);
        SaveData();

        isRewarded = false;
    }

    public void GetMoneyData(float _money)
    {
        moneyTemp = _money;
    }

    public void SetVanData(VanData vanData)
    {
        vanData.maxVanHP = playerData.maxVanHP;
        vanData.boosterSpeedValue = playerData.boosterSpeedValue;
    }

    public void SaveLastTime(DateTime lastTime)
    {
        playerData.lastPlayTime = lastTime;
    }
}
