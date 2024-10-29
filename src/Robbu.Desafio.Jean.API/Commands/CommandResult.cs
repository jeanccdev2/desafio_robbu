namespace Robbu.Desafio.Jean.API.Commands
{
    public class CommandResult
    {
        public bool IsSuccessful { get; }
        public bool IsNotFound { get; }

        private CommandResult(bool isSuccessful, bool isNotFound)
        {
            IsSuccessful = isSuccessful;
            IsNotFound = isNotFound;
        }

        public static CommandResult Success() => new CommandResult(true, false);
        public static CommandResult NotFound() => new CommandResult(false, true);
    }
}