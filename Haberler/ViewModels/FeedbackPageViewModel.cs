using Haberler.Common;
using Library10.Core.Development;
using Library10.Core.UI;
using Prism.Commands;
using Prism.Windows.AppModel;
using Prism.Windows.Mvvm;
using Prism.Windows.Navigation;
using System;
using Windows.ApplicationModel.Email;

namespace Haberler.ViewModels
{
    public class FeedbackPageViewModel : ViewModelBase
    {
        private readonly INavigationService _navigationService;
        private readonly IDialogService _dialogService;
        private readonly IStoreService _storeService;
        private readonly IResourceLoader _resourceLoader;

        private string _comment;
        public string Comment
        {
            get { return _comment; }
            set { SetProperty(ref _comment, value); }
        }

        private string _emailValidation;
        public string EmailValidation
        {
            get { return _emailValidation; }
            set { SetProperty(ref _emailValidation, value); }
        }

        private string _commentValidation;
        public string CommentValidation
        {
            get { return _commentValidation; }
            set { SetProperty(ref _commentValidation, value); }
        }

        public DelegateCommand SendFeedbackCommand { get; private set; }

        public DelegateCommand RateReviewCommand { get; private set; }

        public FeedbackPageViewModel(
            INavigationService navigationService,
            IDialogService dialogService,
            IStoreService storeService,
            IResourceLoader resourceLoader
            )
        {
            _navigationService = navigationService;
            _dialogService = dialogService;
            _storeService = storeService;
            _resourceLoader = resourceLoader;

            SendFeedbackCommand = new DelegateCommand(SendFeedback);
            RateReviewCommand = new DelegateCommand(() => _storeService.RateReview());
        }

        private bool CheckForm()
        {
            bool isCommentValid = false;

            if (!string.IsNullOrWhiteSpace(Comment))
            {
                CommentValidation = string.Empty;
                isCommentValid = true;
            }
            else
                CommentValidation = _resourceLoader.GetString("FeedbackMessageValidation");

            if (isCommentValid)
                return true;

            return false;
        }

        public async void SendFeedback()
        {
            if (CheckForm())
            {
                try
                {
                    EmailRecipient sendTo = new EmailRecipient()
                    {
                        Address = Settings.General.FeedbackUrl
                    };

                    EmailMessage mail = new EmailMessage();
                    mail.Subject = string.Format(_resourceLoader.GetString("FeedbackMailTitle"), Settings.General.AppNameCapitalized);
                    mail.Body = Comment;

                    mail.To.Add(sendTo);

                    await EmailManager.ShowComposeNewEmailAsync(mail);

                    if (DevTools.IsMobile)
                        await _dialogService.ShowAsync(_resourceLoader.GetString("FeedbackSuccessMessage"), _resourceLoader.GetString("FeedbackSuccessTitle"), _resourceLoader.GetString("OK"));
                }
                catch
                {
                    await _dialogService.ShowAsync(_resourceLoader.GetString("FeedbackFailureMessage"), _resourceLoader.GetString("ErrorTitle"), _resourceLoader.GetString("OK"));
                }

                if (_navigationService.CanGoBack())
                    _navigationService.GoBack();
            }
        }
    }
}