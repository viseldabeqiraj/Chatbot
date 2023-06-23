using System.Text;

namespace Chatbot.Helpers
{
    public static class StreamExtensions
    {
        public static Stream ToStream(this string input)
        {
            var stream = new MemoryStream(Encoding.UTF8.GetBytes(input));
            stream.Position = 0;
            return stream;
        }
    }
}
