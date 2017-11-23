﻿using System.Threading.Tasks;
using MoneyFox.DataAccess.DataServices;
using MoneyFox.Foundation.Interfaces;
using MoneyFox.Service.DataServices;
using MvvmCross.Core.Navigation;
using MvvmCross.Localization;

namespace MoneyFox.Business.ViewModels
{
    /// <summary>
    ///     Reprensentation of the CategoryListView.
    /// </summary>
    public class CategoryListViewModel : AbstractCategoryListViewModel
    {
        /// <summary>
        ///     Creates an CategoryListViewModel for usage when the list including the option is needed.
        /// </summary>
        /// <param name="categoryService">An instance of <see cref="ICategoryService" />.</param>
        /// <param name="modifyDialogService">An instance of <see cref="IModifyDialogService" /></param>
        /// <param name="dialogService">An instance of <see cref="IDialogService" /></param>
        /// <param name="navigationService">An instance of <see cref="IMvxNavigationService" /></param>
        public CategoryListViewModel(ICategoryService categoryService, IModifyDialogService modifyDialogService, IDialogService dialogService, IMvxNavigationService navigationService)
            : base(categoryService, modifyDialogService, dialogService, navigationService)
        {
        }

        /// <summary>
        ///     Provides an TextSource for the translation binding on this page.
        /// </summary>
        public IMvxLanguageBinder TextSource => new MvxLanguageBinder("", GetType().Name);
        
        /// <summary>
        ///     Post selected CategoryViewModel to message hub
        /// </summary>
        protected override async Task ItemClick(CategoryViewModel category)
        {
            await EditCategoryCommand.ExecuteAsync(category);
        }
    }
}