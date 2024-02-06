using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public enum BattleState { START, PLAYERTURN, ENEMYTURN, WON, LOST }

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
    public ShakeObject shakeBackG;

    //AR 
    public GameObject ChangeCam;

    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());

        attackButton.interactable = false;
        avoidButton.interactable = false;
        EscapeButton.interactable = false;
    }

    // Update is called once per frame
    IEnumerator SetupBattle()
    {
        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleTransform);
        enemyUnit = enemyGO.GetComponent<Unit>();
        enemyAnim = enemyGO.GetComponent<Animator>();
        shakeObject = enemyGO.GetComponent<ShakeObject>();

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
        dialogueText.text = "The Attack is sucessful!";

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            enemyAnim.SetTrigger("Die");
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
        yield return new WaitForSeconds(3.5f);
        dialogueText.text = enemyUnit.unitName + "is off its guard";

        playerEffect.PlayerEffectOn();
        yield return new WaitForSeconds(0.5f);
        shakeObject.VibrationObject(0.02f, 0.2f);

        yield return new WaitForSeconds(2f);
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);
        dialogueText.text = "The Attack is sucessful!";
        enemyHUD.SetHP(enemyUnit, enemyUnit.currentHP);

        yield return new WaitForSeconds(2f);
        if (isDead)
        {
            state = BattleState.WON;
            enemyAnim.SetTrigger("Die");
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
        yield return new WaitForSeconds(3.5f);
        enemyAnim.SetBool("Attack", true); 

        yield return new WaitForSeconds(0.5f);
        shakeBackG.VibrationObject(0.03f, 2f);
        dialogueText.text = enemyUnit.unitName + "attack!";
        bool isPlayerDead = playerUnit.TakeDamage(enemyUnit.damage * 2);
        playerHUD.SetHP(playerUnit, playerUnit.currentHP);
        yield return new WaitForSeconds(2f);

        enemyAnim.SetBool("Attack", false);
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
        dialogueText.text = enemyUnit.unitName + "attack!";
        yield return new WaitForSeconds(1f);

        enemyAnim.SetBool("Attack", true);
        bool isEnemyDead = playerUnit.TakeDamage(enemyUnit.damage);
        yield return new WaitForSeconds(0.5f);
        shakeBackG.VibrationObject(0.03f, 0.3f);
        playerHUD.SetHP(playerUnit, playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        enemyAnim.SetBool("Attack", false);
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
            ResultManager.ActiveResultUI(true);
            dialogueText.text = "win!";
        }
        else if (state == BattleState.LOST)
        {
            ResultManager.ActiveResultUI(false);
            dialogueText.text = "lose";
        }
    }

    void PlayerTurn()
    {
        attackButton.interactable = true;
        avoidButton.interactable = true;
        EscapeButton.interactable = true;

        dialogueText.text = "Action :";
    }

    public void OnAvoidButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        if(ChangeCam.GetComponent<ChangedCam>().isAR == false)
        {
            ChangeCam.GetComponent<ChangedCam>().AvoidStateCam();
        }
        else
        {
            ChangeCam.GetComponent<ChangedCam>().OffSessionOrigin();
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
}
