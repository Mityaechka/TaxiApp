namespace TaxiApp.Models
{
    public class ResponseModel<T>
    {
        public Status Status { get; set; }
        public T Data { get; set; }
        public static ResponseModel<T> OkResponse(T Data)
        {
            return new ResponseModel<T>(Status.Ok, Data);
        }
        public static ResponseModel<T> ErrorResponse()
        {
            return new ResponseModel<T>(Status.Error, default);
        }
        public ResponseModel(Status status, T data)
        {
            Status = status;
            Data = data;
        }
    }
    public enum Status
    {
        Ok, Error
    }
}
