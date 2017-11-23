using System;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using Android.Widget;
using MoneyFox.Business.ViewModels;
using MoneyFox.Droid.Fragments;
using MoneyFox.Foundation.Resources;
using MvvmCross.Droid.Support.V7.AppCompat;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace MoneyFox.Droid.Activities
{
    [Activity(Label = "ModifyPaymentActivity",
        Name = "moneyfox.droid.activities.ModifyPaymentActivity",
        Theme = "@style/AppTheme",
        LaunchMode = LaunchMode.SingleTop)]
    public class ModifyPaymentActivity : MvxAppCompatActivity<ModifyPaymentViewModel>,
        DatePickerDialog.IOnDateSetListener
    {
        /// <summary>
        ///     Used to determine which button called the date picker
        /// </summary>
        private Button callerButton;

        private Button categoryButton;
        private EditText editTextAmount;
        private Button enddateButton;
        private Button paymentDateButton;

        public void OnDateSet(DatePicker view, int year, int monthOfYear, int dayOfMonth)
        {
            var date = new DateTime(year, monthOfYear + 1, dayOfMonth);

            if (callerButton == paymentDateButton)
            {
                ViewModel.SelectedPayment.Date = date;
            }
            else if (callerButton == enddateButton)
            {
                ViewModel.EndDate = date;
            }

            Title = ViewModel.Title;
        }

        /// <summary>
        ///     Raises the create event.
        /// </summary>
        /// <param name="bundle">Saved instance state.</param>
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            SetContentView(Resource.Layout.activity_modify_payment);

            SetSupportActionBar(FindViewById<Toolbar>(Resource.Id.toolbar));
            SupportActionBar.SetDisplayHomeAsUpEnabled(true);

            categoryButton = FindViewById<Button>(Resource.Id.category);
            paymentDateButton = FindViewById<Button>(Resource.Id.paymentdate);
            enddateButton = FindViewById<Button>(Resource.Id.enddate);

            categoryButton.Click += SelectCategory;
            paymentDateButton.Click += ShowDatePicker;
            enddateButton.Click += ShowDatePicker;

            editTextAmount = FindViewById<EditText>(Resource.Id.editText_amount);
            editTextAmount.FocusChange += EditTextAmountOnFocusChange;
            editTextAmount.Text = ViewModel.AmountString;
            editTextAmount.ClearFocus();

            Title = ViewModel.Title;
        }

        private void EditTextAmountOnFocusChange(object sender, View.FocusChangeEventArgs focusChangeEventArgs)
        {
            if (!focusChangeEventArgs.HasFocus)
            {
                ViewModel.AmountString = editTextAmount.Text;
                editTextAmount.Text = ViewModel.AmountString;
            }
        }

        private void SelectCategory(object sender, EventArgs e)
        {
            ViewModel.GoToSelectCategorydialogCommand.Execute();
        }

        private void ShowDatePicker(object sender, EventArgs eventArgs)
        {
            callerButton = sender as Button;
            var dialog = new DatePickerDialogFragment(this, DateTime.Now, this);
            dialog.Show(SupportFragmentManager.BeginTransaction(), Strings.SelectDateTitle);
        }

        /// <summary>
        ///     Initialize the contents of the Activity's standard options menu.
        /// </summary>
        /// <param name="menu">The options menu in which you place your items.</param>
        /// <returns>To be added.</returns>
        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(ViewModel.IsEdit ? Resource.Menu.menu_modification : Resource.Menu.menu_save, menu);

            return base.OnCreateOptionsMenu(menu);
        }

        /// <summary>
        ///     This hook is called whenever an item in your options menu is selected.
        /// </summary>
        /// <param name="item">The menu item that was selected.</param>
        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            switch (item.ItemId)
            {
                case Android.Resource.Id.Home:
                    Finish();
                    return true;

                case Resource.Id.action_save:
                    ViewModel.AmountString = editTextAmount.Text;
                    ViewModel.SaveCommand.Execute(null);
                    return true;

                case Resource.Id.action_delete:
                    ViewModel.DeleteCommand.Execute(null);
                    return true;

                default:
                    return false;
            }
        }
    }
}