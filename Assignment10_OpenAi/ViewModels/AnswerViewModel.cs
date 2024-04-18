using YodaOpenAi.Models;

namespace YodaOpenAi.ViewModels
{
    [QueryProperty(nameof(Response), "Response")]
    public class AnswerViewModel : BaseViewModel
    {
        private ChatMessage _questionResponseModel;

        public ChatMessage Response
        {
            get { return _questionResponseModel; }
            set
            {
                _questionResponseModel = value;
                OnPropertyChanged();
            }
        }

        public AnswerViewModel()
        {
        }
    }
}
