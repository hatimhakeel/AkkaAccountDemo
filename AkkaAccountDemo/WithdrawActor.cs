using System;
using Akka.Actor;

namespace AkkaAccountDemo
{
	public class WithdrawActor : UntypedActor
	{
		private readonly IActorRef writerActor;

		public WithdrawActor (IActorRef writerActor)
		{
			this.writerActor = writerActor;
		}

		protected override void OnReceive(object message) {
			
			if (message is Messages.Transaction) 
			{
				var msg = message as Messages.Transaction;

				var balance = msg.Balance - msg.Amount;

                Messages.Transaction newMsg = new Messages.Transaction(balance, msg.Amount, msg.TransactionCode);

				writerActor.Tell (newMsg);

				Sender.Tell (newMsg, Self);
			}
		}
	}
}

