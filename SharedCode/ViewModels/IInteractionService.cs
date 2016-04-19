namespace Okna.Plugins.ViewModels
{
    public interface IInteractionService
    {
        bool? ShowDialog<T>(T viewModel);

        void ShowError(string message);
    }
}
