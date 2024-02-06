



using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

public class MessageFilter {

    public static List<string> words = new List<string>();
    public static string masking;
    // 비속어 설정 파일 로딩

    public static StreamReader reader = new StreamReader(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + @"\york.ini");
    public static int maxLength = 1;

    public static void FilterLoad() {
        // 파일 읽기
        // 마스킹 처리 길이 계산
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
        // 비속어를 대문자와 소문자 모두 포함하여 비교하기 위해 메시지를 소문자로 변환
        string lowercaseMessage = message.ToLower();

        foreach(string word in words) {
            // 비속어를 소문자로 변환하여 메시지에서 찾음
            string lowercaseWord = word.ToLower();
            // 정규 표현식을 사용하여 부분일치로 비속어를 찾음
            string pattern = "\\b" + Regex.Escape(lowercaseWord) + "\\b";
            // 비속어를 마스킹할 문자열 생성
            string maskedWord = masking[..lowercaseWord.Length];

            // 비속어를 찾아 마스킹 처리
            lowercaseMessage = Regex.Replace(lowercaseMessage, pattern, maskedWord);
        }

        return lowercaseMessage;
    }
}
