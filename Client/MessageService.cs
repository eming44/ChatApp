using System;

namespace Client
{
    public static class MessageService
    {
        public static Message CreateMessage(string messageFromServer)
        {
            if (string.IsNullOrEmpty(messageFromServer))
            {
                return null;
            }

            string namedText = GetNamedText(messageFromServer);

            return new Message
            {
                FullText = messageFromServer,
                NamedText = namedText,
                Author = GetAuthor(namedText),
                Text = GetText(namedText),
                SentTime = Convert.ToDateTime(GetSentTime(messageFromServer))
            };
        }

        private static string GetNamedText(string fullText)
        {
            int endPointIndex = GetEndPointIndex(fullText, ']');
            return fullText.Substring(++endPointIndex).TrimStart();
        }

        private static string GetSentTime(string fullText)
        {
            int endPointIndex = GetEndPointIndex(fullText, ']');
            return fullText.Substring(1, --endPointIndex);
        }

        private static string GetAuthor(string namedText)
        {
            int endPointIndex = GetEndPointIndex(namedText, ':');
            return namedText.Substring(0, endPointIndex);
        }

        private static string GetText(string namedText)
        {
            int endPointIndex = GetEndPointIndex(namedText, ':');
            return namedText.Substring(++endPointIndex).TrimStart();
        }

        private static int GetEndPointIndex(string text, char endPoint)
        {
            return text.IndexOf(endPoint);
        }
    }
}
