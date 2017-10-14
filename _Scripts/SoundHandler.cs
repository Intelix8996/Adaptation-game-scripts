using UnityEngine;
using System.Collections;

public class SoundHandler : MonoBehaviour {

    [Header("Sound Banks")]
    public AudioClip[] TerrainSoundsBank;
    public AudioClip[] MaterialSoundsBank;
    public AudioClip[] MusicSoundsBank;
    public AudioClip[] SFXSoundsBank;

    public static AudioSource Source;

    private ThirdPersonCharacter Character;
    private ThirdPersonUserControl Controller;

    public static SoundHandler lib;

    private bool wasFootstepPlayed = false;

    [Header("Examples of Physic Materials")]
    [SerializeField]
    private string[] MaterialExamples;

    [Header("Current Ground Material")]
    [SerializeField]
    private int groundMaterialIndex = 0;

    private void Start()
    {
        Source = GetComponent<AudioSource>();
        lib = this;
        Character = GetComponent<ThirdPersonCharacter>();
        Controller = GetComponent<ThirdPersonUserControl>();
        StartCoroutine("SoundUpdate");
    }

    public static void PlaySound(AudioClip SoundClip) { Source.PlayOneShot(SoundClip); }
    public static void PlaySound(AudioClip SoundClip, AudioSource SoundSource) { SoundSource.PlayOneShot(SoundClip); }

    private IEnumerator SoundUpdate()
    {
        while (true)
        {
            float runCycle = Character.runCycle;

            if (((runCycle >= 0.4f && runCycle <= 0.6f) || (runCycle >= 0.9f && runCycle <= 1)) && Controller.m_Move != Vector3.zero && Character.m_IsGrounded && !Input.GetKey(KeyCode.LeftAlt) && !Console._Console.isConsoleActive)
            {
                RaycastHit hitInfo;

                TerrainMaterial TerrainMat;

                if (Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo))
                {
                    TerrainMat = hitInfo.transform.GetComponent<TerrainMaterial>();

                    if (TerrainMat != null)
                    {
                        groundMaterialIndex = TerrainMat.GetMaterialIndex(hitInfo.point);
                        PlaySound(TerrainSoundsBank[groundMaterialIndex]);
                        yield return new WaitForSeconds(0.1f);
                    }
                    else
                    {
                        for (int i = 0; i < MaterialExamples.Length; ++i)
                        {
                            string[] TerrainMaterialName = hitInfo.collider.material.name.Split(' ');

                            if (TerrainMaterialName[0] == MaterialExamples[i])
                                PlaySound(MaterialSoundsBank[i]);
                        }

                        yield return new WaitForSeconds(0.1f);
                    }
                }
            }
            else if (Input.GetKey(KeyCode.LeftAlt) && !Console._Console.isConsoleActive)
            {
                RaycastHit hitInfo;

                TerrainMaterial TerrainMat;

                runCycle *= 2;

                if (((runCycle >= 0.40f && runCycle <= 0.60f) || (runCycle >= 0.9f && runCycle <= 1.1f) || (runCycle >= 1.40f && runCycle <= 1.60f) || runCycle >= 1.9f) && Controller.m_Move != Vector3.zero && Character.m_IsGrounded && !wasFootstepPlayed && Physics.Raycast(transform.position + (Vector3.up * 0.1f), Vector3.down, out hitInfo))
                {
                    TerrainMat = hitInfo.transform.GetComponent<TerrainMaterial>();

                    if (TerrainMat != null)
                    {
                        groundMaterialIndex = TerrainMat.GetMaterialIndex(hitInfo.point);
                        PlaySound(TerrainSoundsBank[groundMaterialIndex]);
                        yield return new WaitForSeconds(0.1f);
                    }
                    else
                    {
                        for (int i = 0; i < MaterialExamples.Length; ++i)
                        {
                            string[] TerrainMaterialName = hitInfo.collider.material.name.Split(' ');

                            if (TerrainMaterialName[0] == MaterialExamples[i])
                                PlaySound(MaterialSoundsBank[i]);
                        }

                        yield return new WaitForSeconds(0.1f);
                    }
                    wasFootstepPlayed = true;
                }
                else if ((runCycle <= 0.4f || (runCycle >= 0.6f && runCycle <= 0.9f) || (runCycle >= 1.1f && runCycle <= 1.4f) || (runCycle >= 1.6f && runCycle <= 1.9f) || runCycle >= 2) && wasFootstepPlayed)
                    wasFootstepPlayed = false;
            }
            else if ((runCycle <= 0.4f || (runCycle >= 0.6f && runCycle <= 0.9f) || (runCycle >= 1.1f && runCycle <= 1.4f) || (runCycle >= 1.6f && runCycle <= 1.9f) || runCycle >= 2) && wasFootstepPlayed)
                wasFootstepPlayed = false;

            yield return new WaitForSeconds(0.02f);
        }
    }

}
