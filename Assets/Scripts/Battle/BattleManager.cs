using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST, DRAW }

public class BattleManager : MonoBehaviour
{
    public static BattleManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
    }

    public GameObject EnemyManager;
    public GameObject enemyPrefab;
    public Transform enemyBattleTransform;
    public Animator enemyAnim;
    Unit enemyUnit;

    public GameObject playerPrefab;
    public AttackEffect playerEffect;
    Unit playerUnit;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public TextMeshProUGUI dialogueText;
    public BattleState state;

    public Button attackButton;
    public Button avoidButton;
    public Button EscapeButton;

    public GameObject AvoidUI;
    public GameObject BattleUI;

    public BattleResult ResultManager;
    public ShakeObject shakeObject;

    //AR 
    public ChangedCam changeCam;
    public EnemyAR enemyAR;
    public Animator enemyARanim;

    // Start is called before the first frame update
    IEnumerator Start()
    {
        attackButton.interactable = false;
        avoidButton.interactable = false;
        EscapeButton.interactable = false;

        while (!GameManager.instance.ready)
            yield return new WaitForFixedUpdate();

        playerPrefab.GetComponent<Unit>().Init();
        state = BattleState.START;
        StartCoroutine(SetupBattle());

    }
    // Update is called once per frame
    private void Update()
    {
        if (changeCam.isAR)
        {
            enemyAR = GameObject.FindWithTag("ARSession").GetComponent<EnemyAR>();
            enemyARanim = enemyAR.enemy.GetComponent<Animator>();
        }
    }

    IEnumerator SetupBattle()
    {
        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleTransform);
        enemyUnit = enemyGO.GetComponent<Unit>();
        enemyAnim = enemyGO.GetComponent<Animator>();

        playerUnit = playerPrefab.GetComponent<Unit>();

        dialogueText.text = enemyUnit.unitName;

        playerHUD.SetHUD(playerUnit);
        enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        playerEffect.PlayerEffectOn();
        yield return new WaitForSeconds(0.5f);
        shakeObject.VibrationObject(0.02f, 0.2f);
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        enemyHUD.SetHP(enemyUnit, enemyUnit.currentHP);
        dialogueText.text = "공격이 성공했다!";

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            EnemyAnimator("Dead");
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyAfterAvoidSucess()
    {
        yield return new WaitForSeconds(4f);
        dialogueText.text = enemyUnit.unitName + "가 빈틈을 보였다!";

        playerEffect.PlayerEffectOn();
        yield return new WaitForSeconds(0.5f);
        shakeObject.VibrationObject(0.02f, 0.2f);

        yield return new WaitForSeconds(2f);
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        dialogueText.text = "추가 공격에 성공했다!";
        enemyHUD.SetHP(enemyUnit, enemyUnit.currentHP);

        yield return new WaitForSeconds(2f);
        if (isDead)
        {
            state = BattleState.WON;
            EnemyAnimator("Dead");
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    IEnumerator EnemyAfterAvoidFailed()
    {
        yield return new WaitForSeconds(4f);
        dialogueText.text = enemyUnit.unitName + "가 공격했다!";

        yield return new WaitForSeconds(1f);
        EnemyAnimator("Attack");
        bool isPlayerDead = playerUnit.TakeDamage(enemyUnit.damage * 2);
        yield return new WaitForSeconds(1f);
        shakeObject.VibrationCam(10f, 0.3f);
        yield return new WaitForSeconds(0.5f);

        playerHUD.SetHP(playerUnit, playerUnit.currentHP);
        yield return new WaitForSeconds(2f);

        EnemyAnimator("AttackOff");
        if (isPlayerDead)
        {
            state = BattleState.LOST;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    IEnumerator EnemyTurn()
    {
        dialogueText.text = enemyUnit.unitName + "가 공격했다!";
        yield return new WaitForSeconds(1f);

        bool isEnemyDead = playerUnit.TakeDamage(enemyUnit.damage);
        EnemyAnimator("Attack");
        yield return new WaitForSeconds(1f);
        shakeObject.VibrationCam(5f, 0.3f);
        yield return new WaitForSeconds(0.5f);
        playerHUD.SetHP(playerUnit, playerUnit.currentHP);

        yield return new WaitForSeconds(1f);
        EnemyAnimator("AttackOff");

        if (isEnemyDead)
        {
            state = BattleState.LOST;
            StartCoroutine(EndBattle());
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    IEnumerator EndBattle()
    {
        yield return new WaitForSeconds(2f);
        if (state == BattleState.WON)
        {
            ResultManager.ActiveResultUI(1);
            dialogueText.text = "승리했다!";
            PlayerPrefs.SetInt("CatchMob", 1);
        }
        else if (state == BattleState.LOST)
        {
            ResultManager.ActiveResultUI(2);
            dialogueText.text = "패배했다….";
        }
        else if (state == BattleState.DRAW)
        {
            ResultManager.ActiveResultUI(3);
            dialogueText.text = "도주에 성공했다!";
        }
    }

    IEnumerator TryEscaping()
    {
        yield return new WaitForSeconds(1f);
        dialogueText.text = "도주를 시도했다…!";

        int num = Random.Range(0, 3);
        bool trying;
        if (num == 1)
        {
            trying = true;
        }
        else
        {
            trying = false;
        }

        yield return new WaitForSeconds(2f);
        if (trying == true)
        {
            dialogueText.text = "도주에 성공했다!";
            state = BattleState.DRAW;
            EndBattle();
        }
        else
        {
            dialogueText.text = "도주에 실패했다….";
            EnemyTurn();
        }
    }

    void PlayerTurn()
    {
        attackButton.interactable = true;
        avoidButton.interactable = true;
        EscapeButton.interactable = true;

        dialogueText.text = "어떻게 할까?";
    }

    public void OnAvoidButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        if (changeCam.GetComponent<ChangedCam>().isAR == false)
        {
            changeCam.GetComponent<ChangedCam>().AvoidStateCam();
        }
        else
        {
            changeCam.GetComponent<ChangedCam>().OffSessionOrigin();
        }

        BattleUI.SetActive(false);
        AvoidUI.SetActive(true);
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        attackButton.interactable = false;
        avoidButton.interactable = false;
        EscapeButton.interactable = false;

        StartCoroutine(PlayerAttack());
    }

    public void AvoidSuccess()
    {
        StartCoroutine(EnemyAfterAvoidSucess());
    }

    public void AvoidFailed()
    {
        StartCoroutine(EnemyAfterAvoidFailed());
    }

    public void EnemyAnimator(string state)
    {
        switch (state)
        {
            case "Attack":
                if (changeCam.isAR)
                {
                    enemyARanim.SetBool("Attack", true);
                }
                else
                {
                    enemyAnim.SetBool("Attack", true);
                }
                break;

            case "AttackOff":
                if (changeCam.isAR)
                {
                    enemyARanim.SetBool("Attack", false);
                }
                else
                {
                    enemyAnim.SetBool("Attack", false);
                }
                break;

            case "Dead":
                if (changeCam.isAR)
                {
                    enemyARanim.SetTrigger("Die");
                }
                else
                {
                    enemyAnim.SetTrigger("Die");
                }
                break;
        }
    }

    public void OnEscapeButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        attackButton.interactable = false;
        avoidButton.interactable = false;
        EscapeButton.interactable = false;

        TryEscaping();
    }
}