using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Console : MonoBehaviour {

    [SerializeField]
    private InputField Command;
    public Text Body;
    [SerializeField]
    private Image Background;

    [Header("")]
    [SerializeField]
    private string[] Autoexec;

    [Header("")]
    public bool isConsoleActive = false;
    [SerializeField]
    private bool isDeveloperMode = false;
    [Header("Arrays")]
    [SerializeField]
    private string[] substrings;
    [SerializeField]
    private string[] inputHistory;
    [SerializeField]
    private string[] matchesArray;

    [Header("Strings")]
    [SerializeField]
    private string inputHistoryRaw;
    [SerializeField]
    private string commandBuffer = "";
    private string inputBuffer = "";
    private string enumBuffer = "";
    private string matchesString = "";

    private int inputHisCounter = 0;

    public static Console _Console;

    private bool isFreeOn = false;

    [SerializeField]
    private Dictionary<string, string> Binds = new Dictionary<string, string>();

    private void Awake()
    {
        _Console = this;
    }

    private void Start()
    {
        foreach (string str in Autoexec)
        {
            substrings = str.Split(' ');
            executeCommand(substrings);
        }
    }

    private void Update()
    {
        if (Input.anyKeyDown && !isConsoleActive)
        {
            Dictionary<string, string>.KeyCollection Keys = Binds.Keys;
            string[] Buttons = new string[Keys.Count];
            Keys.CopyTo(Buttons, 0);

            foreach (string Button in Buttons)
            {
                try
                {
                    if (Input.GetKeyDown(Button))
                    {
                        string value;
                        string[] substr = { };

                        Binds.TryGetValue(Button, out value);

                        substr = value.Split(' ');
                        executeCommand(substr);
                    }
                }
                catch { ErrorOutput("Key is not valid");  }
            }
        }

        if (Input.GetKeyUp(KeyCode.F1) && !GetComponent<InventoryManadger>().isInventoryOpened)
        {
            isConsoleActive = boolInverser(isConsoleActive);
            inputHisCounter = inputHistory.Length;
        }

        if (Input.GetKeyDown(KeyCode.RightControl) && Input.GetKeyDown(KeyCode.C) && isDeveloperMode)
        {
            isFreeOn = !isFreeOn;
            string[] str = new string[2];
            str[0] = "freeCam";
            if (isFreeOn)
                str[1] = "1";
            if (!isFreeOn)
                str[1] = "0";
            executeCommand(str);
        }

        if (!isConsoleActive)
        {
            GetComponent<UseMenu>().enabled = true;
            Command.gameObject.SetActive(false);
            Body.enabled = false;
            Background.enabled = false;
        }
        if (isConsoleActive)
        {
            GetComponent<UseMenu>().enabled = false;
            GetComponent<Animator>().Play("Idle");
            Command.gameObject.SetActive(true);
            Body.enabled = true;
            Background.enabled = true;
        }

        if (Input.GetKeyDown(KeyCode.UpArrow) && isConsoleActive)
        {
            if (inputHisCounter > 0)
            {
                inputHisCounter--;
                Command.text = inputHistory[inputHisCounter];
            }
        }
        if (Input.GetKeyDown(KeyCode.DownArrow) && isConsoleActive)
        {
            if (inputHisCounter < inputHistory.Length - 1)
            {
                inputHisCounter++;
                Command.text = inputHistory[inputHisCounter];
            }
        }
        if (Input.GetKey(KeyCode.PageDown) && Body.transform.position.y < 40 && isConsoleActive)
        {
            Body.transform.position += new Vector3(0, 40, 0);
        }
        if (Input.GetKey(KeyCode.PageUp) && isConsoleActive)
        {
            Body.transform.position += new Vector3(0, -40, 0);
        }
        if (Input.mouseScrollDelta != Vector2.zero && Body.transform.position.y <= 41 && isConsoleActive)
        {
            Body.transform.position -= new Vector3(0, Input.mouseScrollDelta.y*20, 0);

            if (Body.transform.position.y >= 40)
                Body.transform.position = new Vector3(Body.transform.position.x, 41, Body.transform.position.z);
        }

        if (Input.GetKeyDown(KeyCode.Tab) && isConsoleActive)
        {
            inputBuffer = Command.text;

            for (int i = 0; i < Convert.ToInt16(Commands.endEnum); i++)
            {
                enumBuffer = Convert.ToString(Enum.GetName(typeof(Commands), i));
                matchesString += "\n" + enumBuffer;
            }

            matchesArray = matchesString.Split('\n');
            matchesString = null;

            for (int i = 0; i < matchesArray.Length; i++)
            {
               if (!matchesArray[i].Contains(inputBuffer))
                {
                    matchesArray[i] = null;
                }
            }
            
            if (!string.IsNullOrEmpty(inputBuffer) && !string.IsNullOrEmpty(Command.text))
            {
                for (int i = 0; i < matchesArray.Length; i++)
                {
                    if (matchesArray[i] != null)
                    {
                        Print(matchesArray[i]);
                    }
                }
            }

            enumBuffer = null;
        }

        if (Input.GetKeyDown(KeyCode.Return) && Command.text != "" && isConsoleActive)
        {
            if (!isDeveloperMode)
            {
                commandBuffer = Command.text;

                Body.text = Body.text + "\n<color=#00ff00ff><i><b>User:</b></i></color> <b><i>" + Command.text + "</i></b>";
                Command.text = "";

                inputHistoryRaw += " \n" + commandBuffer;
                inputHistory = inputHistoryRaw.Split('\n');
                substrings = commandBuffer.Split(' ');
                executeCommand(substrings);

                inputHisCounter = inputHistory.Length;
            }else if (isDeveloperMode)
            {
                commandBuffer = Command.text;

                Body.text = Body.text + "\n<color=#008080ff><b><i>$developer$</i></b></color> <color=#00ff00ff><i><b>User:</b></i></color> <b><i>" + Command.text + "</i></b>";
                Command.text = "";

                inputHistoryRaw += " \n" + commandBuffer;
                inputHistory = inputHistoryRaw.Split('\n');
                substrings = commandBuffer.Split(' ');
                executeCommand(substrings);
                
                inputHisCounter = inputHistory.Length;
            }
        }
    }

    private bool boolInverser(bool var)
    {
        if (!var)
            var = true;
        else if (var)
            var = false;

        return var;
    }

    public enum Commands
    {
        developer,
        host_dayNightCycle,
        host_dayNightCycle_speed,
        host_dayNightCycle_angle,
        host_timeScale,
        t_showWater,
        t_waterReflective,
        t_waterStormy,
        clear,
        clearAll,
        help,
        tp,
        getWorldPos,
        c_jumpPower,
        c_gravity,
        c_runSpeed,
        c_normalSpeed,
        c_hp,
        c_stamina,
        c_water,
        c_food,
        c_armor,
        c_radiation,
        c_staminaOut,
        c_staminaRestore,
        c_staminaOutJump,
        c_waterOut,
        c_foodOut,
        c_radiationOut,
        c_hpRestore,
        c_radiationIncome,
        c_hpOutRad,
        c_hpOutRadPlank,
        c_hpOutWaterPlank,
        c_hpOutFoodPlank,
        c_hpOutWater,
        c_hpOutFood,
        spawnWoodDrop,
        setItem,
        setItemInActive,
        setActiveSocket,
        showSpeed,
        freeCam,
        freeCam_Sensitivity,
        freeCam_Speed,
        freeCam_SpeedAdd,
        postFX,
        kill,
        ragdoll,
        attach,
        bind,
        detach,
        unbind,
        playSound,
        endEnum
    }

    void UnDevErrorOutput()
    {
        Body.text = Body.text + "\n<color=#0000ffff><b><i>System Output:</i></b></color> <color=#ff0000ff><b><i>Error: You can't execute this command without developer mode</i></b></color>";
        Command.text = "";
        commandBuffer = "";
    }
    void ErrorOutput (string Out)
    {
        Body.text = Body.text + "\n<color=#0000ffff><b><i>System Output:</i></b></color> <color=#ff0000ff><b><i>Error: " + Out + " </i></b></color>";
        Command.text = "";
        commandBuffer = "";
    }

    void Print (string Out)
    {
        Body.text = Body.text + "\n<color=#0000ffff><b><i>System Output:</i></b></color> <b><i>" + Out + "</i></b>";
        Command.text = "";
        commandBuffer = "";
    }

    public void PrintOther (string Out)
    {
        Body.text = Body.text + "\n<color=#0000ffff><b><i>System Output:</i></b></color> <b><i>" + Out + "</i></b>";
    }

    public void ErrorOutputOther(string Out)
    {
        Body.text = Body.text + "\n<color=#0000ffff><b><i>System Output:</i></b></color> <color=#ff0000ff><b><i>Error: " + Out + " </i></b></color>";
    }

    void executeCommand(string[] Substrings)
    {
        try
        {
            if (Substrings[0].ToLower() == Convert.ToString(Commands.developer).ToLower() && Substrings.Length > 1)
            {
                switch (Convert.ToInt16(Substrings[1]))
                {
                    case 0: isDeveloperMode = false; break;
                    case 1: isDeveloperMode = true; break;
                    default: isDeveloperMode = false; break;
                }
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.developer).ToLower() && Substrings.Length <= 1)
                Print(Convert.ToString(isDeveloperMode));

            if (Substrings[0].ToLower() == Convert.ToString(Commands.host_dayNightCycle).ToLower() && Substrings.Length > 1)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                {
                    switch (Convert.ToInt16(Substrings[1]))
                    {
                        case 0: GameObject.FindGameObjectWithTag("SunLight").GetComponent<Light>().enabled = false; break;
                        case 1: GameObject.FindGameObjectWithTag("SunLight").GetComponent<Light>().enabled = true; break;
                        default: GameObject.FindGameObjectWithTag("SunLight").GetComponent<Light>().enabled = false; break;
                    }
                }
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.host_dayNightCycle).ToLower() && Substrings.Length <= 1)
                Print(Convert.ToString(GameObject.FindGameObjectWithTag("SunLight").GetComponent<Light>().enabled));

            if (Substrings[0].ToLower() == Convert.ToString(Commands.host_dayNightCycle_speed).ToLower() && Substrings.Length > 1)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                    GameObject.FindGameObjectWithTag("SunLight").GetComponent<DayNightCycle>().Speed = Convert.ToSingle(Substrings[1]);
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.host_dayNightCycle_speed).ToLower() && Substrings.Length <= 1)
                Print(Convert.ToString(GameObject.FindGameObjectWithTag("SunLight").GetComponent<DayNightCycle>().Speed));

            if (Substrings[0].ToLower() == Convert.ToString(Commands.host_dayNightCycle_angle).ToLower() && Substrings.Length > 1)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                    GameObject.FindGameObjectWithTag("SunLight").GetComponent<DayNightCycle>().Angle = Convert.ToSingle(Substrings[1]);
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.host_dayNightCycle_angle).ToLower() && Substrings.Length <= 1)
                Print(Convert.ToString(GameObject.FindGameObjectWithTag("SunLight").GetComponent<DayNightCycle>().Angle));

            if (Substrings[0].ToLower() == Convert.ToString(Commands.t_showWater).ToLower() && Substrings.Length > 1)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                {
                    switch (Convert.ToInt16(Substrings[1]))
                    {
                        case 0: GameObject.FindGameObjectWithTag("Water").transform.position = new Vector3(0, -50000, 0); break;
                        case 1: GameObject.FindGameObjectWithTag("Water").transform.position = new Vector3(0, 0, 0); break;
                        default: GameObject.FindGameObjectWithTag("Water").transform.position = new Vector3(0, 0, 0); break;
                    }
                }
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.t_showWater).ToLower() && Substrings.Length <= 1)
                Print(Convert.ToString(GameObject.FindGameObjectWithTag("Water").GetComponent<MeshRenderer>().enabled));

            if (Substrings[0].ToLower() == Convert.ToString(Commands.t_waterReflective).ToLower() && Substrings.Length > 1)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                {
                    switch (Convert.ToInt16(Substrings[1]))
                    {
                        case 0:
                            UnityStandardAssets.Water.PlanarReflection[] B_Scripts = GameObject.FindGameObjectWithTag("Water").GetComponentsInChildren<UnityStandardAssets.Water.PlanarReflection>();

                            foreach (UnityStandardAssets.Water.PlanarReflection B_Script in B_Scripts)
                            {
                                B_Script.enabled = false;
                            }
                            break;
                        case 1:
                            B_Scripts = GameObject.FindGameObjectWithTag("Water").GetComponentsInChildren<UnityStandardAssets.Water.PlanarReflection>();

                            foreach (UnityStandardAssets.Water.PlanarReflection B_Script in B_Scripts)
                            {
                                B_Script.enabled = true;
                            }
                            break;
                        default:
                            B_Scripts = GameObject.FindGameObjectWithTag("Water").GetComponentsInChildren<UnityStandardAssets.Water.PlanarReflection>();

                            foreach (UnityStandardAssets.Water.PlanarReflection B_Script in B_Scripts)
                            {
                                B_Script.enabled = true;
                            }
                            break;
                    }
                }
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.t_waterReflective).ToLower() && Substrings.Length <= 1)
                Print(Convert.ToString(GameObject.FindGameObjectWithTag("Water").GetComponentInChildren<UnityStandardAssets.Water.PlanarReflection>().enabled));

            if (Substrings[0].ToLower() == Convert.ToString(Commands.t_waterStormy).ToLower() && Substrings.Length > 1)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                {
                    Transform[] B_Transforms = GameObject.FindGameObjectWithTag("Water").GetComponentsInChildren<Transform>();

                    foreach (Transform B_Transform in B_Transforms)
                    {
                        B_Transform.localScale = new Vector3(B_Transform.localScale.x, Convert.ToSingle(Substrings[1]) / 10, B_Transform.localScale.z);
                    }
                }
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.t_waterStormy).ToLower() && Substrings.Length <= 1)
                Print(Convert.ToString(GameObject.FindGameObjectWithTag("Water").GetComponentInChildren<Transform>().localScale.y));

            if (Substrings[0].ToLower() == Convert.ToString(Commands.clear).ToLower())
            {
                Body.text = "";
                Command.text = "";
                commandBuffer = "";
            }
            if (Substrings[0].ToLower() == Convert.ToString(Commands.clearAll).ToLower())
            {
                Body.text = "";
                Command.text = "";
                commandBuffer = "";
                Substrings = null;
                substrings = null;
                inputHistory = null;
                inputHisCounter = 0;
                inputHistoryRaw = null;
            }
            if (Substrings[0].ToLower() == Convert.ToString(Commands.help).ToLower())
            {
                for (int i = 0; i < Convert.ToInt16(Commands.endEnum); i++)
                {
                    Print(Convert.ToString(Enum.GetName(typeof(Commands), i)));
                }
            }
            if (Substrings[0].ToLower() == Convert.ToString(Commands.tp).ToLower() && Substrings.Length >= 4)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                    transform.position = new Vector3(Convert.ToSingle(Substrings[1]), Convert.ToSingle(Substrings[2]), Convert.ToSingle(Substrings[3]));
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.tp).ToLower() && Substrings.Length <= 3)
                ErrorOutput("Wrong coordinates input: tp {x} {y} {z}");

            if (Substrings[0].ToLower() == Convert.ToString(Commands.getWorldPos).ToLower())
            {
                Print(Convert.ToString("Your currect position: X: " + transform.position.x + " Y: " + transform.position.y + " Z: " + transform.position.z));
            }
            if (Substrings[0].ToLower() == Convert.ToString(Commands.c_jumpPower).ToLower() && Substrings.Length > 1)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                    GetComponent<ThirdPersonCharacter>().m_JumpPower = Convert.ToSingle(Substrings[1]);
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.c_jumpPower).ToLower() && Substrings.Length <= 1)
                Print(Convert.ToString(GetComponent<ThirdPersonCharacter>().m_JumpPower));

            if (Substrings[0].ToLower() == Convert.ToString(Commands.c_gravity).ToLower() && Substrings.Length > 1)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                    GetComponent<ThirdPersonCharacter>().m_GravityMultiplier = Convert.ToSingle(Substrings[1]);
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.c_gravity).ToLower() && Substrings.Length <= 1)
                Print(Convert.ToString(GetComponent<ThirdPersonCharacter>().m_GravityMultiplier));

            if (Substrings[0].ToLower() == Convert.ToString(Commands.c_runSpeed).ToLower() && Substrings.Length > 1)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                {
                    GetComponent<ThirdPersonCharacter>().m_RunSpeedMultiplier = Convert.ToSingle(Substrings[1]);
                    GetComponent<ThirdPersonCharacter>().m_RunAnimMultiplier = Convert.ToSingle(Substrings[1]);
                }
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.c_runSpeed).ToLower() && Substrings.Length <= 1)
                Print(Convert.ToString(GetComponent<ThirdPersonCharacter>().m_RunSpeedMultiplier));

            if (Substrings[0].ToLower() == Convert.ToString(Commands.c_normalSpeed).ToLower() && Substrings.Length > 1)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                {
                    GetComponent<ThirdPersonCharacter>().m_NormalSpeedMultiplier = Convert.ToSingle(Substrings[1]);
                    GetComponent<ThirdPersonCharacter>().m_NormalAnimMultiplier = Convert.ToSingle(Substrings[1]);
                }
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.c_normalSpeed).ToLower() && Substrings.Length <= 1)
                Print(Convert.ToString(GetComponent<ThirdPersonCharacter>().m_NormalSpeedMultiplier));

            if (Substrings[0].ToLower() == Convert.ToString(Commands.c_hp).ToLower() && Substrings.Length > 1)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                    GetComponent<CharStats>().HealthPoint = Convert.ToSingle(Substrings[1]);
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.c_hp).ToLower() && Substrings.Length <= 1)
                Print(Convert.ToString(GetComponent<CharStats>().HealthPoint));

            if (Substrings[0].ToLower() == Convert.ToString(Commands.c_stamina).ToLower() && Substrings.Length > 1)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                    GetComponent<CharStats>().Stamina = Convert.ToSingle(Substrings[1]);
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.c_stamina).ToLower() && Substrings.Length <= 1)
                Print(Convert.ToString(GetComponent<CharStats>().Stamina));

            if (Substrings[0].ToLower() == Convert.ToString(Commands.c_water).ToLower() && Substrings.Length > 1)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                    GetComponent<CharStats>().Water = Convert.ToSingle(Substrings[1]);
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.c_water).ToLower() && Substrings.Length <= 1)
                Print(Convert.ToString(GetComponent<CharStats>().Water));

            if (Substrings[0].ToLower() == Convert.ToString(Commands.c_food).ToLower() && Substrings.Length > 1)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                    GetComponent<CharStats>().Food = Convert.ToSingle(Substrings[1]);
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.c_food).ToLower() && Substrings.Length <= 1)
                Print(Convert.ToString(GetComponent<CharStats>().Food));

            if (Substrings[0].ToLower() == Convert.ToString(Commands.c_armor).ToLower() && Substrings.Length > 1)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                    GetComponent<CharStats>().Armor = Convert.ToSingle(Substrings[1]);
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.c_armor).ToLower() && Substrings.Length <= 1)
                Print(Convert.ToString(GetComponent<CharStats>().Armor));

            if (Substrings[0].ToLower() == Convert.ToString(Commands.c_radiation).ToLower() && Substrings.Length > 1)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                    GetComponent<CharStats>().Radiation = Convert.ToSingle(Substrings[1]);
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.c_radiation).ToLower() && Substrings.Length <= 1)
                Print(Convert.ToString(GetComponent<CharStats>().Radiation));
            if (Substrings[0].ToLower() == Convert.ToString(Commands.c_staminaOut).ToLower() && Substrings.Length > 1)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                    GetComponent<CharStats>().StaminaOut = Convert.ToSingle(Substrings[1]);
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.c_staminaOut).ToLower() && Substrings.Length <= 1)
                Print(Convert.ToString(GetComponent<CharStats>().StaminaOut));
            if (Substrings[0].ToLower() == Convert.ToString(Commands.c_staminaRestore).ToLower() && Substrings.Length > 1)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                    GetComponent<CharStats>().StaminaRestore = Convert.ToSingle(Substrings[1]);
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.c_staminaRestore).ToLower() && Substrings.Length <= 1)
                Print(Convert.ToString(GetComponent<CharStats>().StaminaRestore));
            if (Substrings[0].ToLower() == Convert.ToString(Commands.c_staminaOutJump).ToLower() && Substrings.Length > 1)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                    GetComponent<CharStats>().StaminaOutJump = Convert.ToSingle(Substrings[1]);
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.c_staminaOutJump).ToLower() && Substrings.Length <= 1)
                Print(Convert.ToString(GetComponent<CharStats>().StaminaOutJump));
            if (Substrings[0].ToLower() == Convert.ToString(Commands.c_waterOut).ToLower() && Substrings.Length > 1)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                    GetComponent<CharStats>().WaterOut = Convert.ToSingle(Substrings[1]);
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.c_waterOut).ToLower() && Substrings.Length <= 1)
                Print(Convert.ToString(GetComponent<CharStats>().WaterOut));
            if (Substrings[0].ToLower() == Convert.ToString(Commands.c_foodOut).ToLower() && Substrings.Length > 1)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                    GetComponent<CharStats>().FoodOut = Convert.ToSingle(Substrings[1]);
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.c_foodOut).ToLower() && Substrings.Length <= 1)
                Print(Convert.ToString(GetComponent<CharStats>().FoodOut));
            if (Substrings[0].ToLower() == Convert.ToString(Commands.c_radiationOut).ToLower() && Substrings.Length > 1)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                    GetComponent<CharStats>().RadiationOut = Convert.ToSingle(Substrings[1]);
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.c_radiationOut).ToLower() && Substrings.Length <= 1)
                Print(Convert.ToString(GetComponent<CharStats>().RadiationOut));
            if (Substrings[0].ToLower() == Convert.ToString(Commands.c_hpRestore).ToLower() && Substrings.Length > 1)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                    GetComponent<CharStats>().HPRestore = Convert.ToSingle(Substrings[1]);
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.c_hpRestore).ToLower() && Substrings.Length <= 1)
                Print(Convert.ToString(GetComponent<CharStats>().HPRestore));
            if (Substrings[0].ToLower() == Convert.ToString(Commands.c_radiationIncome).ToLower() && Substrings.Length > 1)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                    GetComponent<CharStats>().RadIncome = Convert.ToSingle(Substrings[1]);
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.c_radiationIncome).ToLower() && Substrings.Length <= 1)
                Print(Convert.ToString(GetComponent<CharStats>().RadIncome));
            if (Substrings[0].ToLower() == Convert.ToString(Commands.c_hpOutRad).ToLower() && Substrings.Length > 1)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                    GetComponent<CharStats>().HPOutRad = Convert.ToSingle(Substrings[1]);
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.c_hpOutRad).ToLower() && Substrings.Length <= 1)
                Print(Convert.ToString(GetComponent<CharStats>().HPOutRad));
            if (Substrings[0].ToLower() == Convert.ToString(Commands.c_hpOutRadPlank).ToLower() && Substrings.Length > 1)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                    GetComponent<CharStats>().HPOutRadPlank = Convert.ToSingle(Substrings[1]);
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.c_hpOutRadPlank).ToLower() && Substrings.Length <= 1)
                Print(Convert.ToString(GetComponent<CharStats>().HPOutRadPlank));
            if (Substrings[0].ToLower() == Convert.ToString(Commands.c_hpOutWaterPlank).ToLower() && Substrings.Length > 1)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                    GetComponent<CharStats>().HPOutWaterPlank = Convert.ToSingle(Substrings[1]);
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.c_radiation).ToLower() && Substrings.Length <= 1)
                Print(Convert.ToString(GetComponent<CharStats>().HPOutWaterPlank));
            if (Substrings[0].ToLower() == Convert.ToString(Commands.c_hpOutFoodPlank).ToLower() && Substrings.Length > 1)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                    GetComponent<CharStats>().HPOutFoodPlank = Convert.ToSingle(Substrings[1]);
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.c_hpOutFoodPlank).ToLower() && Substrings.Length <= 1)
                Print(Convert.ToString(GetComponent<CharStats>().HPOutFoodPlank));
            if (Substrings[0].ToLower() == Convert.ToString(Commands.c_hpOutWater).ToLower() && Substrings.Length > 1)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                    GetComponent<CharStats>().HPOutWater = Convert.ToSingle(Substrings[1]);
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.c_hpOutWater).ToLower() && Substrings.Length <= 1)
                Print(Convert.ToString(GetComponent<CharStats>().HPOutWater));
            if (Substrings[0].ToLower() == Convert.ToString(Commands.c_hpOutFood).ToLower() && Substrings.Length > 1)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                    GetComponent<CharStats>().HPOutFood = Convert.ToSingle(Substrings[1]);
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.c_hpOutFood).ToLower() && Substrings.Length <= 1)
                Print(Convert.ToString(GetComponent<CharStats>().HPOutFood));
            if (Substrings[0].ToLower() == Convert.ToString(Commands.spawnWoodDrop).ToLower() && Substrings.Length >= 2)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                {
                    SpawnItem spIt = GameObject.Find("ItemSpawner").GetComponent<SpawnItem>();
                    spIt.CustomSpawnWood(Convert.ToInt16(Substrings[1]));
                    spIt = null;
                    Destroy(spIt);
                }
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.spawnWoodDrop).ToLower() && Substrings.Length <= 1)
                ErrorOutput("Wrong wood number: spawnWoodDrop {number of drop}");
            if (Substrings[0].ToLower() == Convert.ToString(Commands.setItem).ToLower() && Substrings.Length >= 4)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                {
                    InventorySocket sc = GetComponent<InventoryManadger>().Inventory[Convert.ToInt16(Substrings[1])];
                    sc.Item = ItemLibrary._ItemGenerator.ItemList[Convert.ToInt16(Substrings[2])];
                    sc.Number = Convert.ToInt16(Substrings[3]);
                    sc = null;
                }
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.setItem).ToLower() && Substrings.Length <= 3)
                ErrorOutput("Wrong input: setItem {Socket Number} {Item ID} {Number} ");
            if (Substrings[0].ToLower() == Convert.ToString(Commands.showSpeed).ToLower() && Substrings.Length > 1)
            {
                if (Substrings[1] == "start")
                    GetComponent<SpeedMeasure>().StartCoroutine("Measurer");
                if (Substrings[1] == "stop")
                    GetComponent<SpeedMeasure>().StopCoroutine("Measurer");
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.showSpeed).ToLower() && Substrings.Length <= 1)
                ErrorOutput("Wrong input: showSpeed start/stop");
            if (Substrings[0].ToLower() == Convert.ToString(Commands.setItemInActive).ToLower() && Substrings.Length >= 4)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                {
                    InventorySocket sc = GetComponent<InventoryManadger>().ActiveSlots[Convert.ToInt16(Substrings[1])].GetComponent<InventorySocket>();
                    sc.Item = ItemLibrary._ItemGenerator.ItemList[Convert.ToInt16(Substrings[2])];
                    sc.Number = Convert.ToInt16(Substrings[3]);
                    sc = null;
                }
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.setItemInActive).ToLower() && Substrings.Length <= 3)
                ErrorOutput("Wrong input: setItemInActive {Socket Number} {Item ID} {Number} ");
            if (Substrings[0].ToLower() == Convert.ToString(Commands.setActiveSocket).ToLower() && Substrings.Length >= 3)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                {
                    ActiveItemSocket sc = GetComponent<InventoryManadger>().ActiveSlots[Convert.ToInt16(Substrings[1])];
                    if (Convert.ToInt16(Substrings[2]) == 1)
                        sc.isActive = true;
                    if (Convert.ToInt16(Substrings[2]) == 0)
                        sc.isActive = false;
                    sc = null;
                }
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.setActiveSocket).ToLower() && Substrings.Length <= 2)
                ErrorOutput("Wrong input: setActiveSocket {Socket Number} {State}");
            if (Substrings[0].ToLower() == Convert.ToString(Commands.freeCam).ToLower() && Substrings.Length > 1)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                {
                    switch (Convert.ToInt16(Substrings[1]))
                    {
                        case 0: GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CamMove>().enabled = true; GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FreeCamMove>().enabled = false; GetComponent<ThirdPersonCharacter>().enabled = true; GetComponent<ThirdPersonUserControl>().enabled = true; GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation; break;
                        case 1: GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CamMove>().enabled = false; GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FreeCamMove>().enabled = true; GetComponent<ThirdPersonCharacter>().enabled = false; GetComponent<ThirdPersonUserControl>().enabled = false; GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation; Cursor.visible = false; break;
                        default: GameObject.FindGameObjectWithTag("MainCamera").GetComponent<CamMove>().enabled = true; GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FreeCamMove>().enabled = false; GetComponent<ThirdPersonCharacter>().enabled = true; GetComponent<ThirdPersonUserControl>().enabled = true; GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation; break;
                    }
                }
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.freeCam).ToLower() && Substrings.Length <= 1)
                Print(Convert.ToString(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FreeCamMove>().enabled));
            if (Substrings[0].ToLower() == Convert.ToString(Commands.freeCam_Sensitivity).ToLower() && Substrings.Length > 1)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                    GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FreeCamMove>().Sensitivity = Convert.ToSingle(Substrings[1]);
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.freeCam_Sensitivity).ToLower() && Substrings.Length <= 1)
                Print(Convert.ToString(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FreeCamMove>().Sensitivity));
            if (Substrings[0].ToLower() == Convert.ToString(Commands.freeCam_Speed).ToLower() && Substrings.Length > 1)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                    GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FreeCamMove>().MoveSpeed = Convert.ToSingle(Substrings[1]);
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.freeCam_Speed).ToLower() && Substrings.Length <= 1)
                Print(Convert.ToString(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FreeCamMove>().MoveSpeed));
            if (Substrings[0].ToLower() == Convert.ToString(Commands.freeCam_SpeedAdd).ToLower() && Substrings.Length > 1)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                    GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FreeCamMove>().MoveSpeedAdd = Convert.ToSingle(Substrings[1]);
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.freeCam_SpeedAdd).ToLower() && Substrings.Length <= 1)
                Print(Convert.ToString(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<FreeCamMove>().MoveSpeedAdd));
            if (Substrings[0].ToLower() == Convert.ToString(Commands.postFX).ToLower() && Substrings.Length > 1)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                {
                    switch (Convert.ToInt16(Substrings[1]))
                    {
                        case 0: GameObject.FindGameObjectWithTag("MainCamera").GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>().enabled = false; break;
                        case 1: GameObject.FindGameObjectWithTag("MainCamera").GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>().enabled = true; break;
                        default: GameObject.FindGameObjectWithTag("MainCamera").GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>().enabled = true; break;
                    }
                }
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.postFX).ToLower() && Substrings.Length <= 1)
                Print(Convert.ToString(GameObject.FindGameObjectWithTag("MainCamera").GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>().enabled));
            if (Substrings[0].ToLower() == Convert.ToString(Commands.kill).ToLower() && Substrings.Length >= 1)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                {
                    GetComponent<CharStats>().PerformDeath(CharStats.DeathReasons.Suicide);
                }
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.kill).ToLower() && Substrings.Length != 1)
                ErrorOutput("Syntax Error: Use kill alone");
            if (Substrings[0].ToLower() == Convert.ToString(Commands.ragdoll).ToLower() && Substrings.Length > 1)
            {
                switch (Convert.ToInt16(Substrings[1]))
                {
                    case 0: GetComponent<CharStats>().StartCoroutine("DisableRagdoll"); break;
                    case 1: GetComponent<CharStats>().StartCoroutine("PerformRagdoll"); break;
                    default: GetComponent<CharStats>().DisableRagdoll(); break;
                }
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.ragdoll).ToLower() && Substrings.Length <= 1)
                ErrorOutput("Wrong input: ragdoll {State}");
            if (Substrings[0].ToLower() == Convert.ToString(Commands.attach).ToLower() && Substrings.Length >= 3 || Substrings[0].ToLower() == Convert.ToString(Commands.bind).ToLower() && Substrings.Length >= 3)
            {
                string cmd = "";

                for (int i = 2; i < Substrings.Length; ++i)
                {
                    if (i == Substrings.Length - 1)
                        cmd += Substrings[i];
                    else
                        cmd += Substrings[i] + " ";
                }

                Binds.Add(Substrings[1], cmd);
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.attach).ToLower() && Substrings.Length < 3 || Substrings[0].ToLower() == Convert.ToString(Commands.bind).ToLower() && Substrings.Length < 3)
                ErrorOutput("Wrong input: attach {Button} {Command}");
            if (Substrings[0].ToLower() == Convert.ToString(Commands.detach).ToLower() && Substrings.Length == 2 || Substrings[0].ToLower() == Convert.ToString(Commands.unbind).ToLower() && Substrings.Length == 2)
            {
                Binds.Remove(Substrings[1]);
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.detach).ToLower() && Substrings.Length < 2 || Substrings[0].ToLower() == Convert.ToString(Commands.unbind).ToLower() && Substrings.Length < 2)
                ErrorOutput("Wrong input: detach {Button}");
            if (Substrings[0].ToLower() == Convert.ToString(Commands.playSound).ToLower() && Substrings.Length >= 3)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                {
                    List<AudioClip[]> Library = new List<AudioClip[]>();
                    Library.Add(SoundHandler.lib.TerrainSoundsBank);
                    Library.Add(SoundHandler.lib.MaterialSoundsBank);
                    Library.Add(SoundHandler.lib.MusicSoundsBank);
                    Library.Add(SoundHandler.lib.SFXSoundsBank);

                    AudioClip[] Bank = Library[Convert.ToInt32(Substrings[1])];

                    SoundHandler.PlaySound(Bank[Convert.ToInt32(Substrings[2])]);

                    Bank = null;
                    Library = null;
                }
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.playSound).ToLower() && Substrings.Length < 3)
                ErrorOutput("Wrong input: playSound {Bank Number} {AudioClip Index}");
            if (Substrings[0].ToLower() == Convert.ToString(Commands.host_timeScale).ToLower() && Substrings.Length >= 2)
            {
                if (!isDeveloperMode)
                    UnDevErrorOutput();
                else
                    Time.timeScale = Convert.ToSingle(Substrings[1].Replace('.', ','));
            }
            else if (Substrings[0].ToLower() == Convert.ToString(Commands.host_timeScale).ToLower() && Substrings.Length < 2)
                Print(Convert.ToString(Time.timeScale));
        }
        catch { ErrorOutput("Syntax Error"); }
    }
}
