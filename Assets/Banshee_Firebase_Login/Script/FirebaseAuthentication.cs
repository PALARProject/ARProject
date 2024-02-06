using Firebase;
using Firebase.Auth;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Text.RegularExpressions;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine.SceneManagement;
using System.Collections.Generic;



//FirebaseAuthentication.SignIn(email,password);
//FirebaseAuthentication.SignUp(email,password);

public class FirebaseAuthentication : MonoBehaviour {

    public JoinPageOpen JPOpen;
    public TMP_InputField LoginEmailInput;
    public TMP_InputField LoginPasswordInput;
    public TMP_InputField JoinEmailInput;
    public TMP_InputField JoinPasswordInput;
    public TMP_InputField PasswordConfirmInput;
    public TMP_InputField NicknameInput;
    public RealtimeDatabase RealtimeDB;
    public TMP_Text ErrorMessage;


  


    private void Start() {
 
    }


    public void SignUp(string email, string password, string Nickname) {
         FirebaseAuth auth;
        auth = FirebaseAuth.DefaultInstance;
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task => {
            if(task.IsCanceled || task.IsFaulted) {
                Debug.LogError($"Failed to sign up: {task.Exception}");
                return;
            }

            Debug.Log("User signed up successfully!");
            SignUp_InventoryMaker(Nickname, email, password);
            });
    }
    //ȸ������ �� �������� SignUp ������ ����
    public void SignUp_InventoryMaker(string username, string email, string password) {
        // ȸ������ ���� ����


        // ȸ�������� �����ϸ� �����ͺ��̽��� �÷��̾� �����͸� ����
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;
        string uid = auth.CurrentUser.UserId;
        string[] playerPath = { "�÷��̾�", uid  }; // �ݽÿ� ���� �̸��� ����ڿ� �ش��ϴ� ��� ����
    
      
        Dictionary<string, object> playerData = new Dictionary<string, object>
        {
        { "��������", new Dictionary<string, object>
            {
                { "�г���", username },
                { "��й�ȣ", password },
                { "�̸���", email }
            }
        },
        { "�κ��丮", new Dictionary<string, object>
            {
                { "box_001", "null" },
                { "box_002", "null" },
                { "box_003", "null" },
                { "box_004", "null" },
                { "box_005", "null" },
                { "box_006", "null" },
                { "box_007", "null" },
                { "box_008", "null" },
                { "box_009", "null" }
            }
        }
    };

        RealtimeDatabase realtimeDatabase = new RealtimeDatabase(); // RealtimeDatabase �ν��Ͻ� ����
        realtimeDatabase.WriteDataToRealtimeDatabase(playerPath, playerData); // �ν��Ͻ��� ����Ͽ� �޼��� ȣ��
    }
    public async Task<int> SignIn(string email, string password) {
        FirebaseAuth auth = FirebaseAuth.DefaultInstance;

        try {
            // �񵿱�� �α��� �õ�
            await auth.SignInWithEmailAndPasswordAsync(email, password);

            // �α��� ���� �� 1 ��ȯ
            return 1;
        } catch(FirebaseException e) {
            // �α��� ���� �� ���� ó��
            Debug.LogError($"Failed to sign in: {e.Message}");
            // �α��� ���� �� 0 ��ȯ
            return 0;
        }
    }
    public void JoinButtonClick(){
        string email = JoinEmailInput.text;
        string password = JoinPasswordInput.text;
        string Nickname = NicknameInput.text;
        // �̸����� ��ȿ���� Ȯ���Ѵ�
         if(!IsValidEmail(email)) {
            // ��ȿ���� ���� �̸����̸� ���� �޽����� ǥ���Ѵ�
            ShowErrorMessage("��ȿ���� ���� �̸����Դϴ�.");
            // ������ �ڵ�� ������� �ʵ��� �����Ѵ�
            return;
        }

        // ��й�ȣ�� �ּ����� �䱸 ������ �����ϴ��� Ȯ���Ѵ�
        if(!IsPasswordValid(password)) {
            // ��ȿ���� ���� ��й�ȣ�� ���� �޽����� ǥ���Ѵ�
            ShowErrorMessage("��й�ȣ�� �ʹ� �����մϴ�. Ư�����ڸ� ������ �ּ��� 8�� �̻��̾�� �մϴ�.");
            // ������ �ڵ�� ������� �ʵ��� �����Ѵ�
            return;
        }

        // ��й�ȣ Ȯ���� ��ġ�ϴ��� Ȯ���Ѵ�
        if(password != PasswordConfirmInput.text) {
            // ��й�ȣ Ȯ���� ��ġ���� ������ ���� �޽����� ǥ���Ѵ�
            ShowErrorMessage("��й�ȣ Ȯ���� ��ġ���� �ʽ��ϴ�.");
            // ������ �ڵ�� ������� �ʵ��� �����Ѵ�
            return;
        }

        // �г����� ��ȿ���� Ȯ���Ѵ�
        if(!IsValidNickname(Nickname)) {
            // ��ȿ���� ���� �г����̸� ���� �޽����� ǥ���Ѵ�
            ShowErrorMessage("��ȿ���� ���� �г����Դϴ�.");
            // ������ �ڵ�� ������� �ʵ��� �����Ѵ�
            return;
        }
        SignUp(email, password, Nickname);
        JPOpen.JoinPageSetActiveChange();
    }

    public async void LoginButtonClick() {
        string email = LoginEmailInput.text;
        string password = LoginPasswordInput.text;

        // �α��� �õ�
        int result = await SignIn(email, password);

        // �α��� ����� ���� ó��
        if(result == 1) {
            // �α��� ����
            Debug.Log("Login successful!");
            LoadScene("SampleScene");
            // ���⿡ �α��� ���� ���� �߰� ������ �߰��� �� �ֽ��ϴ�.
        } else {
            // �α��� ����
            Debug.LogError("Login failed.");
            // ���⿡ �α��� ���� ���� �߰� ������ �߰��� �� �ֽ��ϴ�.
        }
    }






    // �̸����� ��ȿ���� Ȯ���ϴ� �Լ�
    // �̸��� ��ȿ�� �˻�
    private bool IsValidEmail(string email) {
        // ������ ���� ǥ������ ����Ͽ� �̸��� ������ Ȯ���մϴ�
        string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";
        return Regex.IsMatch(email, pattern);
    }

    // ��й�ȣ ��ȿ�� �˻�
    private bool IsPasswordValid(string password) {
        // ��й�ȣ�� �ּ� 8�� �̻��̾�� �ϸ�, ���ڿ� Ư�� ���ڸ� �����ؾ� �մϴ�
        return password.Length >= 8 && Regex.IsMatch(password, @"[0-9]") && Regex.IsMatch(password, @"[^a-zA-Z0-9]");
    }

    // �г��� ��ȿ�� �˻�
    private bool IsValidNickname(string nickname) {
        // �г����� �ּ� 3�� �̻��̾�� �մϴ�
        return nickname.Length >= 3;
    }
    // ���� �޽����� ǥ���ϴ� �Լ�
    private void ShowErrorMessage(string message) {
        ErrorMessage.text = message;
       
    }
    // Ư�� ���� �ε��ϴ� �Լ�
    public void LoadScene(string sceneName) {
        SceneManager.LoadScene(sceneName);
    }
}