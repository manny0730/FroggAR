using UnityEngine;
[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField] private AudioSource buttonTapSound;
    [SerializeField] private AudioSource GroundPlacementSFX;

    void Start()
    {

    }

    // Update is called once per frame
    public void playButtonTapSound()
    {
        buttonTapSound.Play();
    }
    public void playGroundPlacementSFX()
    {
        GroundPlacementSFX.Play();
    }
}
