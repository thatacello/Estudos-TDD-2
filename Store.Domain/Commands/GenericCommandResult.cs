namespace Store.Domain.Commands
{
    public class GenericCommandResult : ICommandResult
    {
        public GenericCommandResult(bool success, string message, object data)
        {
            this.Success = success;
            this.Message = message;
            this.Data = data;
        }
        public bool Success { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
    }
}