using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct Question
{
    public string left;
    public string right;
}
namespace Tild.Minigames.BalanceGame
{
    [CreateAssetMenu(fileName = "BalanceGame Question", menuName = "Minigames/BalanceGame/Question", order = 1)]
    public class BalanceQuestionSO : ScriptableObject
    {
        public List<Question> questions;
        public List<Question> GetRandomQuestions(int count)
        {
            
            if (questions == null || questions.Count == 0)
            {
                Debug.LogWarning("질문 리스트가 비어 있습니다!");
                return new List<Question>();
            }

            if (count > questions.Count)
                count = questions.Count;

           
            List<int> indices = new List<int>();
            for (int i = 0; i < questions.Count; i++)
                indices.Add(i);

      
            for (int i = indices.Count - 1; i > 0; i--)
            {
                int rand = UnityEngine.Random.Range(0, i + 1);
                (indices[i], indices[rand]) = (indices[rand], indices[i]);
            }

      
            List<Question> result = new List<Question>();
            for (int i = 0; i < count; i++)
                result.Add(questions[indices[i]]);

            return result;
        }
    }
}