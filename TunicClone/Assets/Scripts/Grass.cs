using UnityEngine;

public class Grass : MonoBehaviour
{
    public ParticleSystem fxHit;
    public bool isCut = false;

    public void GetHit()
    {
        if (isCut) return;

        transform.localScale = new Vector3(0.3f,0.3f, 0.3f); 
        isCut = true;
        fxHit.Emit(Random.Range(5,10));
    }
}
