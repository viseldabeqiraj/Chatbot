using Newtonsoft.Json;

namespace Chatbot.Dtos
{
    public class DatasetDto
    {
        [JsonProperty("viewed_doc_titles")]
        public List<string> ViewedDocTitles { get; set; }

        [JsonProperty("used_queries")]
        public List<UsedQueryDto> UsedQueries { get; set; }

        [JsonProperty("annotations")]
        public List<AnnotationDto> Annotations { get; set; }

        [JsonProperty("nq_answer")]
        public List<string> NQAnswer { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("nq_doc_title")]
        public string NQDocTitle { get; set; }

        [JsonProperty("question")]
        public string Question { get; set; }
    }

    public class UsedQueryDto
    {
        [JsonProperty("query")]
        public string Query { get; set; }

        [JsonProperty("results")]
        public List<Result> Results { get; set; }
    }

    public class Result
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("snippet")]
        public string Snippet { get; set; }
    }

    public class AnnotationDto
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("qaPairs")]
        public List<QAPairDto> QAPairs { get; set; }
    }

    public class QAPairDto
    {
        [JsonProperty("question")]
        public string Question { get; set; }

        [JsonProperty("answer")]
        public List<string> Answer { get; set; }
    }

}
