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
    Unit playerUnit;

    public BattleHUD playerHUD;
    public BattleHUD enemyHUD;

    public Text dialogueText;
    public BattleState state;

    public Button attackButton;
    public Button healButton;
    // Start is called before the first frame update
    void Start()
    {
        state = BattleState.START;
        StartCoroutine(SetupBattle());

        attackButton.interactable = false;
        healButton.interactable = false;
    }

    // Update is called once per frame
    IEnumerator SetupBattle()
    {
        GameObject enemyGO = Instantiate(enemyPrefab, enemyBattleTransform);
        enemyUnit = enemyGO.GetComponent<Unit>();

        playerUnit = GameManager.instance.playerPrefab.GetComponent<Unit>();

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

        enemyHUD.SetHP(enemyUnit.currentHP);
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

    IEnumerator EnemyAfterDodgeSucess()
    {
        yield return new WaitForSeconds(2f);

        dialogueText.text = enemyUnit.unitName + "'s attack!";
        bool isPlayerDead = playerUnit.TakeDamage(0);
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

    IEnumerator EnemyAfterDodgeFailed()
    {
        yield return new WaitForSeconds(2f);

        dialogueText.text = enemyUnit.unitName + "'s attack!";
        bool isPlayerDead = playerUnit.TakeDamage(enemyUnit.damage * 2);
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

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

        playerHUD.SetHP(playerUnit.currentHP);

        yield return new WaitForSeconds(1f);

        if (isDead)
        {
            state = BattleState.LOST;
            EndBattle();
        }
        else
        {
            state = BattleState.WON;
            EndBattle();
        }
    }

    void EndBattle()
    {
        if (state == BattleState.WON)
        {
            dialogueText.text = "You won!";
        }
        else if (state == BattleState.LOST)
        {
            dialogueText.text = "You lose";
        }

        Destroy(this.gameObject);
    }

    void PlayerTurn()
    {
        attackButton.interactable = true;
        healButton.interactable = true;

        dialogueText.text = "Choose an action:";
    }

    public void OnDodgeButton()
    {
        if (state != BattleState.ENEMYTURN)
            return;

        SceneManager.LoadScene("DodgeScene");
    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        attackButton.interactable = false;
        healButton.interactable = false;

        StartCoroutine(PlayerAttack());
    }

    public void DodgeSuccess()
    {
        StartCoroutine(EnemyAfterDodgeSucess());
    }

    public void DodgeFailed()
    {
        StartCoroutine(EnemyAfterDodgeFailed());
    }
}
