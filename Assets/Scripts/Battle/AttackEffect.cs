using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    public GameObject attackEff;
    public GameObject effectLoc;
    private Vector2 loc = new Vector3(0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        loc = effectLoc.transform.position;
    }
    
    public void PlayerEffectOn()
    {
        StartCoroutine(OnEffect());
    }

    IEnumerator OnEffect()
    {
        GameObject effect = Instantiate(attackEff, loc, Quaternion.identity);
        effect.gameObject.transform.SetParent(gameObject.transform);
        yield return new WaitForSeconds(1f);
        Destroy(effect);
    }
}
