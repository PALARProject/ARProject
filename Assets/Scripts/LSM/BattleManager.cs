using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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

    public Transform playerBattleTransform;
    Unit playerUnit;

    //public BattleHUD playerHUD;
    //public BattleHUD enemyHUD;

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

        GameObject playerGO = Instantiate(GameManager.instance.playerPrefab, playerBattleTransform);
        playerUnit = playerGO.GetComponent<Unit>();

        dialogueText.text = enemyUnit.unitName;

        //playerHUD.SetHUD(GameManager.instance.playerUnit);
        // enemyHUD.SetHUD(enemyUnit);

        yield return new WaitForSeconds(2f);

        state = BattleState.PLAYERTURN;
        PlayerTurn();
    }

    IEnumerator PlayerAttack()
    {
        bool isDead = enemyUnit.TakeDamage(playerUnit.damage);

        //enemyHUD.SetHP(enemyUnit.currentHP);
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
        //change state based on what happened
    }

    IEnumerator EnemyTurn()
    {
        dialogueText.text = enemyUnit.unitName + "attack!";

        yield return new WaitForSeconds(1f);

        bool isDead = playerUnit.TakeDamage(enemyUnit.damage);

        //playerHUD.SetHP(GameManager.instance.playerUnit.currentHP);

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

    IEnumerator PlayerHeal()
    {
        playerUnit.Heal(5);

        //playerHUD.SetHP(GameManager.instance.playerUnit.currentHP);
        dialogueText.text = "Heal!";

        yield return new WaitForSeconds(2f);

        state = BattleState.ENEMYTURN;
        StartCoroutine(EnemyTurn());

    }

    public void OnDodgeButton()
    {
        if (state != BattleState.ENEMYTURN)
            return;


    }

    public void OnAttackButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        attackButton.interactable = false;
        healButton.interactable = false;

        StartCoroutine(PlayerAttack());
    }

    public void OnHealButton()
    {
        if (state != BattleState.PLAYERTURN)
            return;

        attackButton.interactable = false;
        healButton.interactable = false;

        StartCoroutine(PlayerHeal());
    }
}
