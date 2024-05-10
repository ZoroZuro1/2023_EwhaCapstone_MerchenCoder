using UnityEngine;

public class ParticleSystemControl : MonoBehaviour
{

    private ParticleSystem particle;
    public bool isPlay = false;
    public bool hastoInActive;
    private void Awake()
    {

        particle = GetComponent<ParticleSystem>();
    }

    private void Update()
    {

        if (isPlay && !particle.isPlaying)
        {
            Debug.Log("dafaf");
            isPlay = false;
            if (hastoInActive)
                transform.parent.parent.gameObject.SetActive(false);
        }
    }
}