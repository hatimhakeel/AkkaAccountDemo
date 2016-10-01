using System;

namespace AkkaAccountDemo
{
	public class Messages
	{
		public class Transaction
		{
			public Transaction (decimal balance, decimal amount, EnumsAndConstants.TransactionTypes type)
			{
				Balance = balance;
				Amount = amount;
				TransactionCode = type;
			}

			public EnumsAndConstants.TransactionTypes TransactionCode { get; private set; }
			public decimal Balance { get; set; }
			public decimal Amount { get; private set; }
		}

		public class Error
		{
			public Error (string errorMessage)
			{
				ErrorMessage = errorMessage;
			}

			public string ErrorMessage { get; private set; }
		}

		public class Information
		{
			public Information(string infoMessage)
			{
				InfoMessage = infoMessage;
			}

			public string InfoMessage { get; private set; }
		}
	}
}

