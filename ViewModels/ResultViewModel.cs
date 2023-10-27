namespace EstacionamentoAPI.ViewModels
{
    public class ResultViewModel<T>
    {
        public ResultViewModel(T data, List<string> errors) 
        { 
            Data = data;
            Errors = errors;
        }

        public ResultViewModel(T data)
        {
            Data = data;
        }

        public ResultViewModel(string errors)
        {
            Errors.Add(errors);
        }

        public T Data { get; private set; }
        public List<string> Errors { get; private set; }
    }
}
