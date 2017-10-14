using System;
using System.Collections;
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

    [Header("Ragdoll Colliders")]
    [SerializeField]
    private GameObject[] RagdollColliders = new GameObject[11];

    private UnityEngine.PostProcessing.PostProcessingBehaviour PostProcessingProfile;
    private bool isStaminaRestoringBelowTen = false;
    private bool isCollisionInCooldown = false;

    [SerializeField]
    private Text ApplicationInfo;

    public enum DeathReasons
    {
        Fall,
        Suicide,
        other,
    }

    private void Start()
    {
        Root = GetComponent<ThirdPersonCharacter>();
        PostProcessingProfile = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<UnityEngine.PostProcessing.PostProcessingBehaviour>();
        ApplicationInfo.text = "Early Tech Pre-Alpha: Work in Progress\nVersion: " + Application.version + " Build: " + Application.version.Remove(0,2) + "\nUnity Player: " + Application.unityVersion + "(" + Application.platform + ")";
        Console._Console.Body.text = Console._Console.Body.text + ("\n<b><i><color=#ffff00ff>----------------------------------------------------------\nEarly Tech Pre-Alpha: Work in Progress\nVersion: " + Application.version + " Build: " + Application.version.Remove(0, 2) + "\nUnity Player: " + Application.unityVersion + "(" + Application.platform + ")\n----------------------------------------------------------</color></i></b>");
    }

    public void Update()
    {
        if (HealthPoint <= 0.99f && Root.isAlive)
            PerformDeath(DeathReasons.other);

        if (Input.GetKeyUp(KeyCode.LeftShift) && Stamina <= 10)
            isStaminaRestoringBelowTen = true;

        if (Input.GetKey(KeyCode.LeftShift) && GetComponent<ThirdPersonUserControl>().m_Move != Vector3.zero && !isStaminaRestoringBelowTen)
        {
            Stamina -= StaminaOut;
            Stamina = Clamp(Stamina);
        }
        else
        {
            Stamina += StaminaRestore;
            Stamina = Clamp(Stamina);
        }

        if (Input.GetButtonDown("Jump") && Stamina > StaminaOutJump && Root.m_IsGrounded && !Root.m_Crouching && !Console._Console.isConsoleActive)
        {
            Stamina -= StaminaOutJump;
            SoundHandler.PlaySound(SoundHandler.lib.SFXSoundsBank[2]);
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
        else
            isStaminaRestoringBelowTen = false;

        if (Stamina < 1)
        {
            Root.m_MoveSpeedMultiplier = Root.m_NormalSpeedMultiplier / 2;
            Root.m_AnimSpeedMultiplier = Root.m_NormalAnimMultiplier / 2;
        }
        if (Stamina < StaminaOutJump)
            Root.canJump = false;

        if (Stamina > StaminaOutJump)
            Root.canJump = true;

        if (Radiation >= 1)
            Radiation -= RadiationOut;

        if (Radiation >= HPOutRadPlank)
            HealthPoint -= HPOutRad;


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
            BackgroundHP.GetComponent<Animator>().enabled = true;
        else
        {
            BackgroundHP.GetComponent<Animator>().enabled = false;
            BackgroundHP.color = new Vector4(0, 0, 0, 0.235f);
        }
        if (Food < 15)
            BackgroundFood.GetComponent<Animator>().enabled = true;
        else
        {
            BackgroundFood.GetComponent<Animator>().enabled = false;
            BackgroundFood.color = new Vector4(0, 0, 0, 0.235f);
        }
        if (Water < 30)
            BackgroundWater.GetComponent<Animator>().enabled = true;
        else
        {
            BackgroundWater.GetComponent<Animator>().enabled = false;
            BackgroundWater.color = new Vector4(0, 0, 0, 0.235f);
        }

        if (Radiation > 60)
            RadiationCount.color = new Vector4(1, 0.235f, 0.235f, 1);
        else
            RadiationCount.color = new Vector4(0, 1, 0.13f, 1);

        HealthPoint = Clamp(HealthPoint);
        Food = Clamp(Food);
        Water = Clamp(Water);
        Stamina = Clamp(Stamina);
        Armor = Clamp(Armor);
        Radiation = Clamp(Radiation);

        if (HealthPoint < 50 && HealthPoint >= 0)
            HealthPoint += HPRestore;

        if (Food <= 100 && Food > 0)
            Food -= FoodOut;

        if (Water <= 100 && Water > 0)
            Water -= WaterOut;

        if (Water < HPOutWaterPlank)
            HealthPoint -= HPOutWater;

        if (Food < HPOutFoodPlank)
            HealthPoint -= HPOutFood;


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

    private void OnCollisionEnter(Collision collision)
    {
        if (!isCollisionInCooldown)
        {
            if (collision.relativeVelocity.magnitude > 25f && collision.relativeVelocity.magnitude < 40f)
                HealthPoint -= collision.relativeVelocity.magnitude * 2;
            if (collision.relativeVelocity.magnitude > 40f)
                PerformDeath(DeathReasons.Fall);

            StartCoroutine("CollisionCooldown", 0.15f);
        }
    }

    private void OnDisable()
    {
        PostProcessingProfile.profile.grain.enabled = false;
        PostProcessingProfile.profile.vignette.enabled = false;
        PostProcessingProfile.profile.chromaticAberration.enabled = false;
    }

    public float Clamp(float a)
    {
        if (a < 0)
            a = 0;
        else if (a > 100)
            a = 100;

        return a;
    }

    public void PerformDeath(DeathReasons DeathType)
    {
        Root.isAlive = false;
        Debug.Log("Reason of death:" + DeathType);
        GetComponent<ThirdPersonUserControl>().enabled = false;
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

        foreach (GameObject RagdollCollider in RagdollColliders)
        {
            RagdollCollider.GetComponent<Collider>().enabled = true;
        }

        StartCoroutine("PerformAnimatorDisable", DeathType);
        GameObject.FindGameObjectWithTag("Canvas").GetComponent<Canvas>().enabled = false;
    }

    IEnumerator PerformAnimatorDisable(DeathReasons DeathType)
    {
        if (DeathType == DeathReasons.Fall)
            yield return new WaitForSeconds(0.01f);
        else
            yield return new WaitForSeconds(0.5f);
        GetComponent<Animator>().enabled = false;
    }

    public IEnumerator PerformRagdoll()
    {
        GetComponent<ThirdPersonUserControl>().enabled = false;
        GetComponent<Collider>().enabled = false;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition | RigidbodyConstraints.FreezeRotation;

        foreach (GameObject RagdollCollider in RagdollColliders)
        {
            RagdollCollider.GetComponent<Collider>().enabled = true;
        }

        yield return new WaitForSeconds(1);
        GetComponent<Animator>().enabled = false;
    }

    public IEnumerator DisableRagdoll()
    {
        GetComponent<ThirdPersonUserControl>().enabled = true;
        GetComponent<Animator>().enabled = true;
        GetComponent<Collider>().enabled = true;
        GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;

        foreach (GameObject RagdollCollider in RagdollColliders)
        {
            RagdollCollider.GetComponent<Collider>().enabled = false;
        }

        yield return null;
    }

    private IEnumerator CollisionCooldown(float time)
    {
        isCollisionInCooldown = true;
        yield return new WaitForSeconds(time);
        isCollisionInCooldown = false;
    }
}
