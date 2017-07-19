using System;
using UnityEngine;
using UnityEngine.UI;

public class CharStats : MonoBehaviour
{

    [SerializeField]
    private ThirdPersonCharacter Root;

    [Header("Global Stats")]
    [Range(0, 100)]
    public float HealthPoint = 75;
    [Range(0, 100)]
    public float Stamina = 100;
    [Range(0, 100)]
    public float Water = 100;
    [Range(0, 100)]
    public float Food = 100;
    [Range(0, 100)]
    public float Armor = 100;
    [Range(0.01f, 100f)]
    public float Radiation = 0f;

    [Header("Speeds Out/Restore")]
    [Range(0.0001f, 2.5f)]
    public float StaminaOut = 0.2f;
    [Range(0.0001f, 2.5f)]
    public float StaminaRestore = 0.12f;
    [Range(0.1f, 100f)]
    public float StaminaOutJump = 15f;
    [Range(0.0001f, 2.5f)]
    public float WaterOut = 0.01f;
    [Range(0.0001f, 2.5f)]
    public float FoodOut = 0.01f;
    [Range(0.0001f, 2.5f)]
    public float RadiationOut = 0.01f;
    [Range(0.0001f, 2.5f)]
    public float HPRestore = 0.1f;
    [Range(0.0001f, 10f)]
    public float RadIncome = 0.1f;
    [Range(0.0001f, 10f)]
    public float HPOutRad = 0.1f;
    [Range(0.01f, 100f)]
    public float HPOutRadPlank = 30f;
    [Range(0.01f, 100f)]
    public float HPOutWaterPlank = 10f;
    [Range(0.01f, 100f)]
    public float HPOutFoodPlank = 5f;
    [Range(0.0001f, 5f)]
    public float HPOutWater = 0.1f;
    [Range(0.0001f, 5f)]
    public float HPOutFood = 0.1f;

    [Header("HUD References")]
    [SerializeField]
    private Image HealthBar;
    [SerializeField]
    private Image FoodBar;
    [SerializeField]
    private Image WaterBar;
    [SerializeField]
    private Image ArmorBar;
    [SerializeField]
    private Image StaminaBar;
    [SerializeField]
    private Text HPCount;
    [SerializeField]
    private Text FoodCount;
    [SerializeField]
    private Text WaterCount;
    [SerializeField]
    private Text ArmorCount;
    [SerializeField]
    private Text StaminaConut;
    [SerializeField]
    private Image BackgroundHP;
    [SerializeField]
    private Image BackgroundWater;
    [SerializeField]
    private Image BackgroundFood;

    [Header("HUD Radiation References")]
    [SerializeField]
    private Image BackgroundRad1; //RadGroup
    [SerializeField]
    private Image BackgrounRad2; //RadGroup
    [SerializeField]
    private RawImage RadiationIcon; //RadGroup
    [SerializeField]
    private Text RadiationText; //RadGroup
    [SerializeField]
    private Text RadiationCount; //RadGroup

    private UnityEngine.PostProcessing.PostProcessingBehaviour PostProcessingProfile;

    private void Start()
    {
        Root = GetComponent<ThirdPersonCharacter>();
        PostProcessingProfile = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>();
    }

    public void Update()
    {
        if (Input.GetKey(KeyCode.LeftShift) && GetComponent<ThirdPersonUserControl>().m_Move != Vector3.zero)
        {
            Stamina -= StaminaOut;
            Stamina = Clamp(Stamina);
        }
        else
        {
            Stamina += StaminaRestore;
            Stamina = Clamp(Stamina);
        }

        if (Input.GetButtonDown("Jump") && Stamina > StaminaOutJump && Root.m_IsGrounded && !Root.m_Crouching)
        {
            Stamina -= StaminaOutJump;
        }

        if (Stamina < 100)
        {
            PostProcessingProfile.profile.chromaticAberration.enabled = true;
            PostProcessingProfile.profile.chromaticAberration.settings = new UnityEngine.PostProcessing.ChromaticAberrationModel.Settings
            {
                intensity = 1 - (Stamina / 100),
            };
            PostProcessingProfile.profile.vignette.enabled = true;
            PostProcessingProfile.profile.vignette.settings = new UnityEngine.PostProcessing.VignetteModel.Settings
            {
                mode = UnityEngine.PostProcessing.VignetteModel.Mode.Classic,
                color = new Color(0f, 0f, 0f, 1f),
                center = new Vector2(0.5f, 0.5f),
                intensity = .5f - (Stamina / 200),
                smoothness = 0.2f,
                roundness = 1f,
                mask = null,
                opacity = 1f,
                rounded = false
            };
        }
        if (Stamina < 10)
        {
            Root.m_MoveSpeedMultiplier = Root.m_NormalSpeedMultiplier;
            Root.m_AnimSpeedMultiplier = Root.m_NormalAnimMultiplier;
        }
        if (Stamina < 1)
        {
            Root.m_MoveSpeedMultiplier = Root.m_NormalSpeedMultiplier / 2;
            Root.m_AnimSpeedMultiplier = Root.m_NormalAnimMultiplier / 2;
        }
        if (Stamina < StaminaOutJump)
        {
            Root.canJump = false;
        }
        if (Stamina > StaminaOutJump)
        {
            Root.canJump = true;
        }
        if (Radiation >= 1)
        {
            Radiation -= RadiationOut;
        }
        if (Radiation >= HPOutRadPlank)
        {
            HealthPoint -= HPOutRad;
        }

        if (Radiation >= 1)
        {
            BackgroundRad1.enabled = true;
            BackgrounRad2.enabled = true;
            RadiationIcon.enabled = true;
            RadiationText.enabled = true;
            RadiationCount.enabled = true;

            PostProcessingProfile.profile.grain.enabled = true;
            PostProcessingProfile.profile.grain.settings = new UnityEngine.PostProcessing.GrainModel.Settings
            {
                colored = true,
                intensity = Radiation / 100,
                size = 1.5f,
                luminanceContribution = 1 - (Radiation / 100)
            };
        }
        else
        {
            BackgroundRad1.enabled = false;
            BackgrounRad2.enabled = false;
            RadiationIcon.enabled = false;
            RadiationText.enabled = false;
            RadiationCount.enabled = false;

            PostProcessingProfile.profile.grain.enabled = false;
        }


        if (HealthPoint < 20)
        {
            BackgroundHP.GetComponent<Animator>().enabled = true;
        }
        else
        {
            BackgroundHP.GetComponent<Animator>().enabled = false;
            BackgroundHP.color = new Vector4(0, 0, 0, 0.235f);
        }
        if (Food < 15)
        {
            BackgroundFood.GetComponent<Animator>().enabled = true;
        }
        else
        {
            BackgroundFood.GetComponent<Animator>().enabled = false;
            BackgroundFood.color = new Vector4(0, 0, 0, 0.235f);
        }
        if (Water < 30)
        {
            BackgroundWater.GetComponent<Animator>().enabled = true;
        }
        else
        {
            BackgroundWater.GetComponent<Animator>().enabled = false;
            BackgroundWater.color = new Vector4(0, 0, 0, 0.235f);
        }

        if (Radiation > 60)
        {
            RadiationCount.color = new Vector4(1, 0.235f, 0.235f, 1);
        }
        else
        {
            RadiationCount.color = new Vector4(0, 1, 0.13f, 1);
        }

        HealthPoint = Clamp(HealthPoint);
        Food = Clamp(Food);
        Water = Clamp(Water);
        Stamina = Clamp(Stamina);
        Armor = Clamp(Armor);
        Radiation = Clamp(Radiation);

        if (HealthPoint < 50 && HealthPoint >= 0)
        {
            HealthPoint += HPRestore;
        }
        if (Food <= 100 && Food > 0)
        {
            Food -= FoodOut;
        }
        if (Water <= 100 && Water > 0)
        {
            Water -= WaterOut;
        }

        if (Water < HPOutWaterPlank)
        {
            HealthPoint -= HPOutWater;
        }
        if (Food < HPOutFoodPlank)
        {
            HealthPoint -= HPOutFood;
        }

        HealthBar.rectTransform.localScale = new Vector3(HealthPoint / 100, 1, 1);
        WaterBar.rectTransform.localScale = new Vector3(Water / 100, 1, 1);
        FoodBar.rectTransform.localScale = new Vector3(Food / 100, 1, 1);
        StaminaBar.rectTransform.localScale = new Vector3(Stamina / 100, 1, 1);
        ArmorBar.rectTransform.localScale = new Vector3(Armor / 100, 1, 1);

        HPCount.text = Convert.ToString(Convert.ToInt16(HealthPoint));
        FoodCount.text = Convert.ToString(Convert.ToInt16(Food));
        WaterCount.text = Convert.ToString(Convert.ToInt16(Water));
        StaminaConut.text = Convert.ToString(Convert.ToInt16(Stamina));
        ArmorCount.text = Convert.ToString(Convert.ToInt16(Armor));
        RadiationCount.text = Convert.ToString(Convert.ToInt16(Radiation));
    }

    public float Clamp(float a)
    {
        if (a <= 0)
        {
            a = 0;
        }
        if (a >= 100)
        {
            a = 100;
        }
        return a;
    }
}
