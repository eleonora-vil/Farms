namespace Mock_Project_Net03.Dtos
{
    public class CustomException : Exception
    {
        public int StatusCode { get; private set; }

        public CustomException(string message, int statusCode = 500) : base(message)
        {
            StatusCode = statusCode;
        }
    }
}
