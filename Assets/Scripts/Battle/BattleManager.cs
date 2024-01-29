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
    public GameObject enemyPrefab;
    public Transform enemyBattleTransform;
    Unit enemyUnit;

    public GameObject playerPrefab;
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
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        enemyHUD.SetHP(enemyUnit, enemyUnit.currentHP);
        dialogueText.text = "The Attack is sucessful!";

        yield return new WaitForSeconds(2f);

        if (isDead)
        {
            state = BattleState.WON;
            EndBattle();
        }
        else
        {
            state = BattleState.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator EnemyAfterAvoidSucess()
    {
        yield return new WaitForSeconds(2f);

        dialogueText.text = enemyUnit.unitName + "attack";
        bool isPlayerDead = playerUnit.TakeDamage(0);
        playerHUD.SetHP(playerUnit, playerUnit.currentHP);
        yield return new WaitForSeconds(2f);

        if (isPlayerDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    IEnumerator EnemyAfterAvoidFailed()
    {
        yield return new WaitForSeconds(2f);

        dialogueText.text = enemyUnit.unitName + "attack!";
        bool isPlayerDead = playerUnit.TakeDamage(enemyUnit.damage * 2);
        playerHUD.SetHP(playerUnit, playerUnit.currentHP);
        yield return new WaitForSeconds(2f);


        if (isPlayerDead)
        {
            state = BattleState.LOST;
            EndBattle();
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

        bool isEnemyDead = playerUnit.TakeDamage(enemyUnit.damage);

        playerHUD.SetHP(playerUnit, playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        if (isEnemyDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.PLAYERTURN;
            PlayerTurn();
        }
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "win!";
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "lose";
        }

        Destroy(this.gameObject);
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
