using System;
using Akka.Actor;

namespace AkkaAccountDemo
{
	public class WriterActor : UntypedActor
	{
		public WriterActor ()
		{
			
		}

		protected override void OnReceive (object message)
		{
			if (message is Messages.Transaction)
			{
				var msg = message as Messages.Transaction;

				if (msg.TransactionCode == EnumsAndConstants.TransactionTypes.Deposit)
				{
					Console.WriteLine ("Successfully deposited " + msg.Amount + ", Current Balance is: " + msg.Balance);	
				}
				if (msg.TransactionCode == EnumsAndConstants.TransactionTypes.Withdraw)
				{
					Console.WriteLine ("Successfully withdrew  " + msg.Amount + ", Current Balance is: " + msg.Balance);
				}
			}

			if (message is Messages.Error)
			{
				var msg = message as Messages.Error;
				Console.WriteLine (msg.ErrorMessage);
			}

			if (message is Messages.Information)
			{
				var msg = message as Messages.Information;
				Console.WriteLine (msg.InfoMessage);
			}
		}
	}
}

