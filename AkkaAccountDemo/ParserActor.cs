using System;
using System.Text.RegularExpressions;
using Akka.Actor;

namespace AkkaAccountDemo
{
	public class ParserActor : UntypedActor
	{
		private readonly IActorRef depositActor;
		private readonly IActorRef withdrawActor;
		private readonly IActorRef writerActor;
        private bool isInitialCall = true;

		EnumsAndConstants.TransactionTypes transactionType = EnumsAndConstants.TransactionTypes.None;
		decimal balance = default(decimal);
		decimal transactionValue = default(decimal);
		string transactionCode = null;

		public ParserActor (IActorRef depositActor, IActorRef withdrawActor, IActorRef writerActor)
		{
			this.depositActor = depositActor;
			this.withdrawActor = withdrawActor;
			this.writerActor = writerActor;
		}

		protected override void OnReceive (object message)
		{
			var input = message as String;

			var isValid = validateInput (input);

			if (isValid)
			{
				Messages.Transaction transactionObj = new Messages.Transaction (balance, transactionValue, transactionType);

				if (transactionType == EnumsAndConstants.TransactionTypes.Deposit) 
				{
					var task = depositActor.Ask (transactionObj);
					task.Wait ();

					var result = task.Result as Messages.Transaction;
					balance = result.Balance;
				}

				if (transactionType == EnumsAndConstants.TransactionTypes.Withdraw)
				{
					if (balance >= transactionObj.Amount) 
					{
						var task = withdrawActor.Ask (transactionObj);
						task.Wait ();

						var result = task.Result as Messages.Transaction;
						balance = result.Balance;	
					} else
					{
						writerActor.Tell (new Messages.Error ("Current balance is " + balance + ". Please withdraw an amount less than or equal to current balance."));	
					}
				}
			}
		}

		public bool validateInput(string input)
		{
            if (isInitialCall)
            {
                printInstructions();
                isInitialCall = false;

                return false;
            }

			if (string.IsNullOrEmpty (input))
			{
				writerActor.Tell (new Messages.Error ("Please enter a valid input."));

				return false;
			}

			if (!string.IsNullOrEmpty (input) && string.Equals (input, EnumsAndConstants.StartCommand, StringComparison.OrdinalIgnoreCase)) 
			{
				printInstructions ();

				return false;
			}

			var msg = Regex.Split (input, @"\s+");
			transactionCode = msg [0];

			if (msg.Length != 2) 
			{
				writerActor.Tell (new Messages.Error ("Please enter a valid input."));

				return false;
			}

			if (msg.Length == 2 &&
			    ((string.Equals (transactionCode, EnumsAndConstants.DepositCode, StringComparison.CurrentCultureIgnoreCase) ||
			      string.Equals (transactionCode, EnumsAndConstants.WithdrawCode, StringComparison.CurrentCultureIgnoreCase)) &&
			      Decimal.TryParse (msg [1], out transactionValue)))
			{
				if (string.Equals (transactionCode, EnumsAndConstants.DepositCode, StringComparison.CurrentCultureIgnoreCase)) 
				{
					transactionType = EnumsAndConstants.TransactionTypes.Deposit;
				}
				if (string.Equals (transactionCode, EnumsAndConstants.WithdrawCode, StringComparison.CurrentCultureIgnoreCase)) 
				{
					transactionType = EnumsAndConstants.TransactionTypes.Withdraw;
				}

				return true;
			}
			else
			{
				writerActor.Tell (new Messages.Error ("Please enter a valid input."));

				return false;
			}
		}

		private void printInstructions()
		{
			var information = "                   Akka Account Demo                   " + Environment.NewLine;
			information += "--------------------------------------------------------" + Environment.NewLine;
			information += "For Usage Instructions Enter                  : start" + Environment.NewLine;
			information += "To Deposit to Account Enter Transaction as    : D amount" + Environment.NewLine;
			information += "To Withdraw from Account Enter Transaction as : W amount" + Environment.NewLine;
			information += "To Terminate Session Enter                    : exit" + Environment.NewLine;

			writerActor.Tell (new Messages.Information (information));
		}
	}
}

