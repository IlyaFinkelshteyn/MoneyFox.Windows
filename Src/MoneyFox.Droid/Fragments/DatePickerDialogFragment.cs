using System;
using Android.App;
using Android.Content;
using Android.OS;
using MvvmCross.Droid.Support.V4;

namespace MoneyFox.Droid.Fragments
{
    /// <summary>
    ///     Provides an Dialog to select a start and an end date.
    /// </summary>
    public class DatePickerDialogFragment : MvxDialogFragment
    {
        private readonly Context context;
        private readonly DateTime date;
        private readonly DatePickerDialog.IOnDateSetListener listener;

        public DatePickerDialogFragment(Context context, DateTime date, DatePickerDialog.IOnDateSetListener listener)
        {
            this.context = context;
            this.date = date;
            this.listener = listener;
        }

        public override Dialog OnCreateDialog(Bundle savedInstanceState)
            => new DatePickerDialog(context, listener, date.Year, date.Month - 1, date.Day);
    }
}