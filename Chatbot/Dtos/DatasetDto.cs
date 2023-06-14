using Newtonsoft.Json;

namespace Chatbot.Dtos
{
    public class DatasetDto
    {
        public List<ChatbotEntry> Entries { get; set; }

    }

    public class ChatbotEntry
    {
        public List<string> ViewedDocTitles { get; set; }
        public List<QueryResult> UsedQueries { get; set; }
        public List<Annotation> Annotations { get; set; }
        public List<string> NQAnswer { get; set; }
        public string Id { get; set; }
        public string NQDocTitle { get; set; }
        public string Question { get; set; }
    }

    public class QueryResult
    {
        public string Query { get; set; }
        public List<Result> Results { get; set; }
    }

    public class Result
    {
        public string Title { get; set; }
        public string Snippet { get; set; }
    }

    public class Annotation
    {
        public string Type { get; set; }
        public List<QAPair> QAPairs { get; set; }
    }

    public class QAPair
    {
        public string Question { get; set; }
        public List<string> Answer { get; set; }
    }
}
