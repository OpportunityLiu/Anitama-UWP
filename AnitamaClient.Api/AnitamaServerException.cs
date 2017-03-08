using System;

namespace AnitamaClient.Api
{
    public class AnitamaServerException : Exception
    {
        public AnitamaServerException() { }
        public AnitamaServerException(string message) : base(message) { }
        public AnitamaServerException(string message, Exception inner) : base(message, inner) { }

        public static AnitamaServerException Create<T>(Response<T> response, Exception inner)
        {
            return new AnitamaServerException(response.Info, inner)
            {
                Status = response.Status,
                Reterns = response.Data,
                Data =
                    {
                        ["Info"] = response.Info,
                        ["Status"] = response.Status,
                        ["Data"] = response.Data
                    }
            };
        }
        public static AnitamaServerException Create<T>(Response<T> response) => Create(response, null);

        public int Status { get; private set; }
        public object Reterns { get; private set; }
    }
}
