using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Collections;

public class TriviaAPIManager : MonoBehaviour
{
    public static TriviaAPIManager Instance;

    public QuizzQuestion[] _questionArray;

    public int _questionCount = 10;

    private const string BaseUrl = "https://the-trivia-api.com/api/questions";
    public enum Category
    {
        music,
        sport_and_leisure,
        film_and_tv,
        history,
        geography,
        general_knowledge
    }

    public enum Difficulty
    {
        easy,
        medium,
        hard
    }

    public class QuizzQuestion
    {
        public string Category { get; set; }
        public string Id { get; set; }
        public List<string> Tags { get; set; }
        public string Difficulty { get; set; }
        public List<string> Regions { get; set; }
        public bool IsNiche { get; set; }
        public string Question { get; set; }
        public string CorrectAnswer { get; set; }
        public List<string> IncorrectAnswers { get; set; }
        public string Type { get; set; }
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Fetches trivia questions from the API.
    /// </summary>
    /// <param name="category">The category of questions to fetch.</param>
    public void FetchTriviaQuestions(Category _category, Difficulty _difficulty)
    {
        _questionArray = null;
        string url = $"{BaseUrl}?categories={_category}&difficulties={_difficulty}&limit={_questionCount}";
        StartCoroutine(GetTriviaQuestions(url));
    }

    private IEnumerator GetTriviaQuestions(string url)
    {
        using (UnityWebRequest request = UnityWebRequest.Get(url))
        {
            // Send the request
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError($"Error fetching trivia questions: {request.error}");
            }
            else
            {
                // Parse the JSON response
                var jsonResponse = request.downloadHandler.text;

                try
                {
                    _questionArray = JsonConvert.DeserializeObject<QuizzQuestion[]>(jsonResponse);
                }
                catch (Exception e)
                {
                    Debug.LogError($"Error deserializing JSON: {e.Message}");
                }
            }
        }
    }
}