using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.Rendering;
using System.Text;
using System;

internal class GameControler : MonoBehaviour
{
    // Home位置
    public Vector3 RedHomePosition;
    public Vector3 BlueHomePosition;

    public Vector3 TerrainPosition;
    // 相机预制体
    public Camera TargetCamera;  // 要调整的相机
    public Vector3 NewPosition = new(140, 580, 0);  // 相机的新位置
    Quaternion Rotation = Quaternion.Euler(60f, 0f, 0f);
    //Usesr管理
    private List<User> BlueUsers;
    private List<User> RedUsers;
    // Home预设体
    public GameObject RedHomePrefab;
    public GameObject RedHomeInitialize;
    public GameObject TerrainPrefab;
    public bool RedSurvival;
    public GameObject BlueHomePrefab;
    public GameObject BlueHomeInitialize;
    public bool BlueSurvival;
    // 兵种预设体及开销
    public UnitData[] RedUnitData;
    public UnitData[] BlueUnitData;
    //建筑管理
    public UnitData[] RedBuildings;
    public UnitData[] BlueBuildings;
    public int[] BlueBuildingCount = new int[10];
    public int[] RedBuildingCount = new int[10];
    public Vector3[] RedBuildingPositions;
    public Vector3[] BlueBuildingPositions;
    // TextMeshPro 组件
    public TMP_Text BlueMoneyText;
    public TMP_Text RedMoneyText;
    // 队伍金钱
    public int RedTeamMoney;
    public int RedMoneyLevel;
    public int BlueTeamMoney;
    public int BlueMoneyLevel;
    public int MoneyLimit;
    public int Money_timer;
    public ConnectViaCode ConnectViaCode;

    void Start()
    {
        ClearScene();
        InitializeTeams();
        InitializeMoney();
        InitializeHomes();
        InitializeBuildings();
    }

    private void InitializeTeams()
    {
        BlueUsers = new List<User>();
        RedUsers = new List<User>();
        RedHomePosition = new(-300, 0, 800);
        BlueHomePosition = new(610, 0, 60);
        TerrainPosition=new Vector3(0,0,0);
    }

    private void InitializeMoney()
    {
        RedTeamMoney = 1000000;
        BlueTeamMoney = 1000000;
        RedMoneyLevel=1;
        BlueMoneyLevel=1;
        MoneyLimit = 20000;
        Money_timer=0;
    }

    private void InitializeCamera()
    {
        if (TargetCamera != null)
        {
            TargetCamera.transform.SetPositionAndRotation(NewPosition, Rotation);
        }
        else
        {
            Debug.LogWarning("Target camera is not assigned.");
        }
    }

    private void InitializeHomes()
    {
        GameObject redHomeInstance = Instantiate(RedHomePrefab, RedHomePosition, Quaternion.Euler(0, 180, 0));
        GameObject blueHomeInstance = Instantiate(BlueHomePrefab, BlueHomePosition, Quaternion.identity);
        GameObject Terrain= Instantiate(TerrainPrefab,TerrainPosition,Quaternion.identity);
        RedHomeInitialize=redHomeInstance;
        BlueHomeInitialize=blueHomeInstance;

        ConnectViaCode = GetComponentInChildren<ConnectViaCode>();
        if (ConnectViaCode == null)
        {
            Debug.Log("fall");
        }
        else
        {
            Debug.Log("Success");
        }

        SetMoneyTextComponents(redHomeInstance, blueHomeInstance);
    }

    private void InitializeBuildings()
    {
        float Z_R = RedHomePosition.z - 100;
        float Z_B = 160;  
        RedBuildingPositions = new Vector3[5];
        BlueBuildingPositions = new Vector3[5];

        RedBuildingPositions[0] = new Vector3(-380, RedHomePosition.y , 700);
        BlueBuildingPositions[0] = new Vector3(670, BlueHomePosition.y , 160);
        
        Z_R = 600 ;
        Z_B=260;
        for(int i=0;i<3;i++){
            RedBuildingPositions[i+1] = new Vector3(-380, RedHomePosition.y , Z_R);
            BlueBuildingPositions[i+1] = new Vector3(670, BlueHomePosition.y , Z_B);
            Z_B += 100;
            Z_R -= 100;
        }
    }

    private void SetMoneyTextComponents(GameObject redHomeInstance, GameObject blueHomeInstance)
    {
        TMP_Text blueMoneyText = blueHomeInstance.GetComponentInChildren<TMP_Text>();
        TMP_Text redMoneyText = redHomeInstance.GetComponentInChildren<TMP_Text>();

        if (blueMoneyText != null)
        {
            BlueMoneyText = blueMoneyText;
        }
        if (redMoneyText != null)
        {
            RedMoneyText = redMoneyText;
        }
    }

    private void Update()
    {
        Money_timer++;
        if (Money_timer == 1000 || Money_timer == 2000)
        {
            BuildingCount();
        }
        if (Money_timer == 2000)
        {
            UpdateMoney();
            Money_timer = 0;
        }
        CheckConnectViaCode();
    }

    private void UpdateMoney()
    {
        if (RedTeamMoney < MoneyLimit)
        {
            RedTeamMoney += 100*RedMoneyLevel;

        }
        if (BlueTeamMoney < MoneyLimit)
        {
            BlueTeamMoney += 100*BlueMoneyLevel;
        }

        UpdateMoneyDisplay();
    }

    private void CheckConnectViaCode()
    {
        if (ConnectViaCode != null && ConnectViaCode.fresh)
        {
            Debug.Log(ConnectViaCode.danmu_msg);
            Debug.Log("refresh");
            ConnectViaCode.fresh = false;
            InformationAnalysis(ConnectViaCode.danmu_msg);
        }
    }

public void BuildingCount()
{
    // 创建不同类型的对象列表
    List<Infantry> infantryList = new();
    List<Vehicle> vehicleList = new(); 
    List<Heavy_industry> heavyIndustryList = new();
    List<Air> airList = new();

    // 筛选出带有 "RedTeam" 或 "BlueTeam" 标签的 Infantry 对象
    GameObjectExtensions.FindGameObjectsWithTagAndType<Infantry>("RedTeam", infantryList);
    int redInfantryCount = infantryList.Count;
    infantryList.Clear();
    GameObjectExtensions.FindGameObjectsWithTagAndType<Infantry>("BlueTeam", infantryList);
    int blueInfantryCount = infantryList.Count;
    RedBuildingCount[0] = redInfantryCount;
    BlueBuildingCount[0] = blueInfantryCount;

    // 筛选出带有 "RedTeam" 或 "BlueTeam" 标签的 Vehicle 对象
    GameObjectExtensions.FindGameObjectsWithTagAndType<Vehicle>("RedTeam", vehicleList);
    int redVehicleCount = vehicleList.Count;
    vehicleList.Clear();
    GameObjectExtensions.FindGameObjectsWithTagAndType<Vehicle>("BlueTeam", vehicleList);
    int blueVehicleCount = vehicleList.Count;
    RedBuildingCount[1] = redVehicleCount;
    BlueBuildingCount[1] = blueVehicleCount;

    // 筛选出带有 "RedTeam" 或 "BlueTeam" 标签的 HeavyIndustry 对象
    GameObjectExtensions.FindGameObjectsWithTagAndType<Heavy_industry>("RedTeam", heavyIndustryList);
    int redHeavyIndustryCount = heavyIndustryList.Count;
    heavyIndustryList.Clear();
    GameObjectExtensions.FindGameObjectsWithTagAndType<Heavy_industry>("BlueTeam", heavyIndustryList);
    int blueHeavyIndustryCount = heavyIndustryList.Count;
    RedBuildingCount[2] = redHeavyIndustryCount;
    BlueBuildingCount[2] = blueHeavyIndustryCount;

    // 筛选出带有 "RedTeam" 或 "BlueTeam" 标签的 Air 对象
    GameObjectExtensions.FindGameObjectsWithTagAndType<Air>("RedTeam", airList);
    int redAirCount = airList.Count;
    airList.Clear();
    GameObjectExtensions.FindGameObjectsWithTagAndType<Air>("BlueTeam", airList);
    int blueAirCount = airList.Count;
    RedBuildingCount[3] = redAirCount;
    BlueBuildingCount[3] = blueAirCount;
}

    void ClearScene()
    {
        // 获取场景中所有的 GameObject
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            // 检查是否为根对象
            if (obj.transform.parent == null)
            {
                if (obj.transform.CompareTag("GameController")||obj.transform.CompareTag("Untagged"))
                {
                    continue;
                }
                // 排除当前脚本所在的 GameObject，避免自己被销毁
                if (obj != this.gameObject && obj.name != "Terrain" && obj.name != "Directional Light")
                {
                    Destroy(obj);
                }
            }
        }
        Debug.Log("clear");
    }

    void ClearArmy()
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>();
        foreach (GameObject obj in allObjects)
        {
            if (obj.transform.CompareTag("BlueTeam") || obj.transform.CompareTag("RedTeam"))
            {
                Destroy(obj);
            }
        }
    }

    public void GenerateUnits(string teamName, string unitName, int quantity)
    {
        int currentMoney = teamName == "RedTeam" ? RedTeamMoney : BlueTeamMoney;
        int num = 0;

        UnitData unitData = GetUnitData(unitName, teamName);
        if (unitData == null)
        {
            Debug.LogError("未知的兵种名称: " + unitName);
            return;
        }

        int totalCost = unitData.Cost * quantity;

        if (totalCost > currentMoney)
        {
            Debug.LogError("资金不足，无法生成兵种。");
            return;
        }
        if (teamName == "RedTeam")
        {
            if (RedBuildingCount[unitData.Index] < quantity)
            {
                Debug.LogError("建筑数量不足，生成兵种受限。");
                num = RedBuildingCount[unitData.Index];
            }
            else{
                num=quantity;
            }
        }
        else
        {
            if (BlueBuildingCount[unitData.Index] < quantity)
            {
                Debug.LogError("建筑数量不足，生成兵种受限。");
                num = BlueBuildingCount[unitData.Index];
            }
             else{
                num=quantity;
            }
        }


        if (teamName == "RedTeam")
            RedTeamMoney -= totalCost;
        else
            BlueTeamMoney -= totalCost;

        UpdateMoneyDisplay();

        for (int i = 0; i < num; i++)
        {
            Vector3 spawnPosition = GetSpawnPosition(teamName);

            Instantiate(unitData.Prefab, spawnPosition, Quaternion.identity);
        }
    }

    public void GenerateBuildingUnits(string teamName, string unitName)
    {
        int currentMoney = teamName == "RedTeam" ? RedTeamMoney : BlueTeamMoney;

        UnitData unitData = GetBuildingUnitData(unitName, teamName);
        if (unitData == null)
        {
            Debug.LogError("未知的兵种名称: " + unitName);
            return;
        }

        int totalCost = unitData.Cost;

        if (totalCost > currentMoney)
        {
            Debug.LogError("资金不足，无法生成兵种。");
            return;
        }

        if (teamName == "RedTeam")
        {
            RedTeamMoney -= totalCost;
            RedBuildingCount[unitData.Index]++;

            Vector3 spawnPosition = new(RedBuildingPositions[unitData.Index].x + RedBuildingCount[unitData.Index] * 40, RedBuildingPositions[unitData.Index].y, RedBuildingPositions[unitData.Index].z);
            Debug.Log(spawnPosition);
            Instantiate(unitData.Prefab, spawnPosition, Quaternion.identity);
            Debug.Log(unitData.Prefab.transform.position);
        }
        else if (teamName == "BlueTeam")
        {
            BlueTeamMoney -= totalCost;
            BlueBuildingCount[unitData.Index]++;
            Vector3 spawnPosition = new(BlueBuildingPositions[unitData.Index].x - BlueBuildingCount[unitData.Index] * 40, BlueBuildingPositions[unitData.Index].y, BlueBuildingPositions[unitData.Index].z);
            Debug.Log(spawnPosition);
            Instantiate(unitData.Prefab, spawnPosition, Quaternion.identity);
            Debug.Log(unitData.Prefab.transform.position);
        }

        UpdateMoneyDisplay();
    }

    private Vector3 GetSpawnPosition(string teamName)
    {
        if (teamName.Equals("RedTeam"))
        {
            Vector3 homePosition = RedHomePosition;
            return new Vector3(homePosition.x + 40, homePosition.y + 5, homePosition.z - 40);
        }
        else if (teamName.Equals("BlueTeam"))
        {
            Vector3 homePosition = BlueHomePosition;
            return new Vector3(homePosition.x - 40, homePosition.y + 5, homePosition.z + 40);
        }
        return new Vector3();
    }

    private UnitData GetUnitData(string unitName, string teamName)
    {
        if (teamName == "RedTeam")
        {
            foreach (var data in RedUnitData)
            {
                if (data.Name == unitName)
                {
                    return data;
                }
            }
        }
        if (teamName == "BlueTeam")
        {
            foreach (var data in BlueUnitData)
            {
                if (data.Name == unitName)
                {
                    return data;
                }
            }
        }

        return null;
    }

    private UnitData GetBuildingUnitData(string unitName, string teamName)
    {
        if (teamName == "RedTeam")
        {
            foreach (var data in RedBuildings)
            {
                if (data.Name == unitName)
                {
                    return data;
                }
            }
        }
        if (teamName == "BlueTeam")
        {
            foreach (var data in BlueBuildings)
            {
                if (data.Name == unitName)
                {
                    return data;
                }
            }
        }

        return null;
    }

    private void UpdateMoneyDisplay()
    {
        Building bluebuilding;
        Building redbuilding;
        RedHomeInitialize.TryGetComponent(out redbuilding);
        BlueHomeInitialize.TryGetComponent(out bluebuilding);
       int redhealth=-1;
       int bluehealth=-1;
       if(redbuilding!=null){
        redhealth=redbuilding.health;
       }
       if(bluebuilding!=null){
        bluehealth=bluebuilding.health;
       }
        if (BlueMoneyText != null)
        {
            BlueMoneyText.text = "Blue Money: " + BlueTeamMoney+"\n"+"Health: "+bluehealth+"\n"+"MoneyLevel: "+BlueMoneyLevel;
        }
        if (RedMoneyText != null)
        {
            RedMoneyText.text = "Red Money: " + RedTeamMoney+"\n"+"Health: "+redhealth+"\n"+"MoneyLevel: "+RedMoneyLevel;
        }
    }

    public void InformationAnalysis(StringBuilder stringBuilder)
    {
        string message = stringBuilder.ToString();

        string title = message.Length >= 5 ? message.Substring(0, 5) : string.Empty;

        if (title.Equals("收到弹幕!"))
        {
            int userIndex = message.IndexOf("用户：");
            if (userIndex != -1)
            {
                int userStartIndex = userIndex + "用户：".Length;
                int userEndIndex = message.IndexOf('\n', userStartIndex);
                if (userEndIndex == -1) userEndIndex = message.Length;
                string userName = message[userStartIndex..userEndIndex].Trim();

                int messageIndex = message.IndexOf("弹幕内容：");
                string msg = message[(messageIndex + "弹幕内容：".Length)..].Trim();
                if (userName == "棕色地球仪")
                {
                    Authority(msg);
                }

                bool exist = false;
                User sender = null;
                if (BlueUsers.Count != 0 || RedUsers.Count != 0)
                {
                    for (int i = 0; i < BlueUsers.Count; i++)
                    {
                        User user = BlueUsers[i];
                        if (user.Name.Equals(userName))
                        {
                            exist = true;
                            sender = user;
                        }
                    }
                    for (int i = 0; i < RedUsers.Count; i++)
                    {
                        User user = RedUsers[i];
                        if (user.Name.Equals(userName))
                        {
                            exist = true;
                            sender = user;
                        }
                    }
                }
                if (exist)
                {
                    DanmuAnalysis(msg, sender);
                }
                else
                {
                    if (msg.Equals("Red"))
                    {
                        sender = new User(userName, "RedTeam");
                        RedUsers.Add(sender);
                        Debug.Log("join");
                    }
                    if (msg.Equals("Blue"))
                    {
                        sender = new User(userName, "BlueTeam");
                        BlueUsers.Add(sender);
                        Debug.Log("join");
                    }

                }
            }
        }
    }

    public void DanmuAnalysis(string msg, User user)
    {
        string[] strings = msg.Split(" ");
        if (strings.Length == 2)
        {
            string team = user.Team;
            string unitName = strings[0];

            if (strings[1] == "build")
            {
                if (unitName.Equals("inf"))
                {
                    GenerateBuildingUnits(team, "inf");
                }
                else if (unitName.Equals("veh"))
                {
                    GenerateBuildingUnits(team, "veh");
                }
                else if (unitName.Equals("hea"))
                {
                    GenerateBuildingUnits(team, "hea");
                }
                else if (unitName.Equals("air"))
                {
                    GenerateBuildingUnits(team, "air");
                }
            }
            else
            {
                int num = int.Parse(strings[1]);
                if (unitName.Equals("FI"))
                {
                    GenerateUnits(team, "FI", num);
                }
                else if (unitName.Equals("AT"))
                {
                    GenerateUnits(team, "AT", num);
                }
                else if (unitName.Equals("Tank"))
                {
                    GenerateUnits(team, "Tank", num);
                }
                else if (unitName.Equals("Miss"))
                {
                    GenerateUnits(team, "Miss", num);
                }
                else if (unitName.Equals("Char"))
                {
                    GenerateUnits(team, "Char", num);
                }else if (unitName.Equals("hel"))
                {
                    GenerateUnits(team, "hel", num);
                }
            }

        }
        else{
            if(msg=="up"){
                if(user.Team=="RedTeam"&&RedTeamMoney>=300*Math.Pow(2,RedMoneyLevel)&&RedMoneyLevel<6){
                    RedTeamMoney-= (int)(300 * Math.Pow(2,RedMoneyLevel));
                    RedMoneyLevel++;
                }
                else{
                    Debug.Log("uplevel fail");
                }
                if(user.Team=="BlueTeam"&&BlueTeamMoney>+300 * Math.Pow(2,BlueMoneyLevel)&&BlueMoneyLevel<6){
                    BlueTeamMoney-=(int)(300 * Math.Pow(2,BlueMoneyLevel));
                    BlueMoneyLevel++;
                }else{
                    Debug.Log("uplevel fail");
                }
            }
        }
    }

    public void Authority(string msg)
    {
        if (msg == "start")
        {
            InitializeCamera();
        }
        else if (msg == "regame")
        {
            Start();
            Debug.Log("Success");
        }
        else if (msg == "clear")
        {
            ClearArmy();
            Debug.Log("Success");
        }
    }

    [System.Serializable]
    public class UnitData
    {
        public string Name;
        public GameObject Prefab;
        public int Cost;
        public int Index;
    }

    public class User
    {
        public string Name;
        public string Team;

        public User(string name, string team)
        {
            Name = name;
            Team = team;
        }
    }
    public static class GameObjectExtensions
    {
        // 通过标签和类型筛选出场景中的物体，并存放在数组中
        public static void FindGameObjectsWithTagAndType<T>(string tag, List<T> result) where T : Component
        {
            GameObject[] gameObjects = GameObject.FindGameObjectsWithTag(tag);
            foreach (GameObject go in gameObjects)
            {
                T component = go.GetComponent<T>();
                if (component != null)
                {
                    result.Add(component);
                }
            }
        }
    }
}