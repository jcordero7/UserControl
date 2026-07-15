using UserControl.Core.CustomEntities;

namespace UserControl.Application.Responses
{
    public class ApiResponses<T>
    {
        public ApiResponses(T data, ResponseCode responseCode = null)
        {
            Data = data;
            ResponseCode = responseCode;
        }

        public T Data { get; set; } = default!;

        public Metadata Meta { get; set; } = default!;

        public ResponseCode ResponseCode { get; set; }

    }
}
