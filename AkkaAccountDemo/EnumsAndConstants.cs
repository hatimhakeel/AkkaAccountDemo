using System;

namespace AkkaAccountDemo
{
	public class EnumsAndConstants
	{
		public const string StartCommand = "start";
		public const string ExitCommand = "exit";
		public const string DepositCode = "D";
		public const string WithdrawCode = "W";

		public enum TransactionTypes 
		{
			None,
			Deposit,
			Withdraw
		}
	}
}

