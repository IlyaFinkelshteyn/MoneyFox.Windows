﻿using System;
using System.Globalization;
using System.Threading.Tasks;
using MoneyFox.Business.Helpers;
using MoneyFox.Business.Parameters;
using MoneyFox.DataAccess.Pocos;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Foundation.Resources;
using MoneyFox.Service.DataServices;
using MvvmCross.Core.Navigation;
using MvvmCross.Core.ViewModels;
using MvvmCross.Localization;

namespace MoneyFox.Business.ViewModels
{
    public class ModifyAccountViewModel : MvxViewModel<ModifyAccountParameter>
    {
        private readonly IAccountService accountService;
        private readonly ISettingsManager settingsManager;
        private readonly IBackupManager backupManager;
        private readonly IDialogService dialogService;
        private readonly IMvxNavigationService navigationService;

        private bool isEdit;
        private double amount;
        private AccountViewModel selectedAccount;

        public ModifyAccountViewModel(IAccountService accountService,
            ISettingsManager settingsManager,
            IBackupManager backupManager,
            IDialogService dialogService, 
            IMvxNavigationService navigationService)
        {
            this.dialogService = dialogService;
            this.navigationService = navigationService;
            this.settingsManager = settingsManager;
            this.backupManager = backupManager;
            this.accountService = accountService;
        }

        #region Commands
        
        /// <summary>
        ///     Saves all changes to the database
        ///     or creates a new AccountViewModel depending on
        ///     the <see cref="IsEdit" /> property
        /// </summary>
        public MvxAsyncCommand SaveCommand => new MvxAsyncCommand(SaveAccount);

        /// <summary>
        ///     Deletes the selected AccountViewModel from the database
        /// </summary>
        public MvxAsyncCommand DeleteCommand => new MvxAsyncCommand(DeleteAccount);

        /// <summary>
        ///     Cancels the operation and will revert the changes
        /// </summary>
        public MvxAsyncCommand CancelCommand => new MvxAsyncCommand(Cancel);

        #endregion

        #region Properties

        /// <summary>
        ///     Provides an TextSource for the translation binding on this page.
        /// </summary>
        public IMvxLanguageBinder TextSource => new MvxLanguageBinder("", GetType().Name);

        /// <summary>
        ///     indicates if the AccountViewModel already exists and shall
        ///     be updated or new created
        /// </summary>
        public bool IsEdit
        {
            get => isEdit;
            set
            {
                isEdit = value;
                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     Returns the Title based on if the view is in edit mode or not.
        /// </summary>
        public string Title => IsEdit
            ? string.Format(Strings.EditAccountTitle, SelectedAccount.Name)
            : Strings.AddAccountTitle;

        /// <summary>
        ///     Property to format amount string to double with the proper culture.
        ///     This is used to prevent issues when converting the amount string to double
        ///     without the correct culture.
        /// </summary>
        public string AmountString
        {
            get => Utilities.FormatLargeNumbers(amount);
            set
            {
                // we remove all separator chars to ensure that it works in all regions
                string amountstring = Utilities.RemoveGroupingSeparators(value);

                double convertedValue;
                if (double.TryParse(amountstring, NumberStyles.Any, CultureInfo.CurrentCulture, out convertedValue))
                {
                    amount = convertedValue;
                }

                RaisePropertyChanged();
            }
        }

        /// <summary>
        ///     The currently selected AccountViewModel
        /// </summary>
        public AccountViewModel SelectedAccount
        {
            get => selectedAccount;
            set
            {
                selectedAccount = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        private int accountId;

        /// <inheritdoc />
        public override void Prepare(ModifyAccountParameter parameter)
        {
            accountId = parameter.AccountId;
        }

        /// <inheritdoc />
        public override async Task Initialize()
        {
            if (accountId == 0)
            {
                IsEdit = false;
                amount = 0;
                SelectedAccount = new AccountViewModel(new Account());
            } else
            {
                IsEdit = true;
                SelectedAccount = new AccountViewModel(await accountService.GetById(accountId));
                amount = SelectedAccount.CurrentBalance;
            }
        }

        private async Task SaveAccount()
        {
            if (string.IsNullOrEmpty(SelectedAccount.Name))
            {
                await dialogService.ShowMessage(Strings.MandatoryFieldEmptyTitle, Strings.NameRequiredMessage);
                return;
            }

            if (!IsEdit && await accountService.CheckIfNameAlreadyTaken(SelectedAccount.Name))
            {
                await dialogService.ShowMessage(Strings.DuplicatedNameTitle, Strings.DuplicateAccountMessage);
                return;
            }

            SelectedAccount.CurrentBalance = amount;

            await accountService.SaveAccount(SelectedAccount.Account);
            settingsManager.LastDatabaseUpdate = DateTime.Now;
#pragma warning disable 4014
            backupManager.EnqueueBackupTask();
#pragma warning restore 4014
            await navigationService.Close(this);
        }

        private async Task DeleteAccount()
        {
            try
            {
                await accountService.DeleteAccount(SelectedAccount.Account);
                settingsManager.LastDatabaseUpdate = DateTime.Now;
#pragma warning disable 4014
                backupManager.EnqueueBackupTask();
#pragma warning restore 4014
                await navigationService.Close(this);
            } 
            catch(Exception)
            {
                await dialogService.ShowMessage(Strings.ErrorTitleDelete, Strings.ErrorMessageDelete);
            }
        }

        private async Task Cancel()
        {
            await navigationService.Close(this);
        }
    }
}