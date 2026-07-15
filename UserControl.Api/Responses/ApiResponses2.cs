using UserControl.Core.CustomEntities;

namespace UserControl.Api.Responses
{
    public class ApiResponses2<T>
    {
        public ApiResponses2(T data, ResponseCode responseCode = null)
        {
            Data = data;
            ResponseCode = responseCode;
        }

        public T Data { get; set; }

        public Metadata Meta { get; set; }

        public ResponseCode ResponseCode { get; set; }

    }
}
