using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

namespace OpenAI
{
    public class GeneratePrompt : MonoBehaviour
    {
        [SerializeField] private Button button;
        [SerializeField] private Text textArea;

        //private float height;
        private OpenAIApi openai = new OpenAIApi();

        private List<ChatMessage> messages = new List<ChatMessage>();

        private void Start()
        {
            button.onClick.AddListener(SendReply);
        }

        private async void SendReply()
        {
            var newMessage = new ChatMessage()
            {
                Role = "user",
                Content = "Give a one-word object, animal, or plant"
            };

            messages.Add(newMessage);
            textArea.text = "...";
            button.enabled = false;

            // Complete the instruction
            var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
            {
                Model = "gpt-3.5-turbo-0613",
                Messages = messages
            });

            if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
            {
                var message = completionResponse.Choices[0].Message;
                message.Content = message.Content.Trim();

                messages.Add(message);
                textArea.text = message.Content;
            }
            else
            {
                Debug.LogWarning("No text was generated from this prompt.");
            }
            button.enabled = true;
            button.GetComponentInChildren<Text>().text = "New Prompt";

        }
    }
}
