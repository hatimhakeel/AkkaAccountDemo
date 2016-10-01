using System;
using Akka.Actor;

namespace AkkaAccountDemo
{
	public class DepositActor : UntypedActor
	{
		private readonly IActorRef writerActor;
		
		public DepositActor (IActorRef writerActor)
		{
			this.writerActor = writerActor;
		}

		protected override void OnReceive(object message) {

			if (message is Messages.Transaction) 
			{
				var msg = message as Messages.Transaction;

				var balance = msg.Balance + msg.Amount;
				msg.Balance = balance;

				writerActor.Tell (msg);

				Sender.Tell (msg, Self);
			}
		}
	}
}

