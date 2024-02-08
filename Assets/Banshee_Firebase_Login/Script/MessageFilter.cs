



using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public class MessageFilter {

    public static List<string> words = new List<string>();
    public static string masking;
    // ��Ӿ� ���� ���� �ε�

    public static StreamReader reader = new StreamReader(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + @"\york.ini");
    public static int maxLength = 1;

    public static void FilterLoad() {
        // ���� �б�
        // ����ŷ ó�� ���� ���
        while(!reader.EndOfStream) {
            var line = reader.ReadLine();
            words.Add(line);

            if(maxLength < line.Length)
                maxLength = line.Length;
        }
        for(int i = 0; i <= maxLength; i++)
            masking += "*";
    }


    public static string MessageFilter_use(string message) {
        // ��Ӿ �빮�ڿ� �ҹ��� ��� �����Ͽ� ���ϱ� ���� �޽����� �ҹ��ڷ� ��ȯ
        string lowercaseMessage = message.ToLower();

        foreach(string word in words) {
            // ��Ӿ �ҹ��ڷ� ��ȯ�Ͽ� �޽������� ã��
            string lowercaseWord = word.ToLower();
            // ���� ǥ������ ����Ͽ� �κ���ġ�� ��Ӿ ã��
            string pattern = "\\b" + Regex.Escape(lowercaseWord) + "\\b";
            // ��Ӿ ����ŷ�� ���ڿ� ����
            string maskedWord = masking[..lowercaseWord.Length];

            // ��Ӿ ã�� ����ŷ ó��
            lowercaseMessage = Regex.Replace(lowercaseMessage, pattern, maskedWord);
        }

        return lowercaseMessage;
    }
}
