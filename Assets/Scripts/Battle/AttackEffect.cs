using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackEffect : MonoBehaviour
{
    public ChangedCam camInfo;
    public EnemyAR enemyAR;

    private GameObject enemy;

    public GameObject attackEff;
    public GameObject effectLoc;
    private Vector2 loc = new Vector3(0, 0, 0);
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void PlayerEffectOn()
    {
        StartCoroutine(OnEffect());
    }

    IEnumerator OnEffect()
    {
        Vector3 screenCenter = Camera.main.ViewportToScreenPoint(new Vector3(0.5f, 0.5f, 0f));
        GameObject effect = Instantiate(attackEff, screenCenter, Quaternion.identity);
        effect.gameObject.transform.SetParent(gameObject.transform);
        yield return new WaitForSeconds(0.7f);
        Destroy(effect);
    }
}
