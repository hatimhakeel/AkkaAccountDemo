using System;
using Akka.Actor;

namespace AkkaAccountDemo
{
	class Program
	{
		public static ActorSystem AccountActorSystem;
		
		public static void Main (string[] args)
		{
			AccountActorSystem = ActorSystem.Create ("AccountActorSystem");

			IActorRef writerActor = AccountActorSystem.ActorOf (Props.Create (() => new WriterActor ()));
			IActorRef depositActor = AccountActorSystem.ActorOf (Props.Create(() => new DepositActor(writerActor)));
			IActorRef withdrawActor = AccountActorSystem.ActorOf (Props.Create (() => new WithdrawActor(writerActor)));
			IActorRef parserActor = AccountActorSystem.ActorOf (Props.Create (() => new ParserActor (depositActor, withdrawActor, writerActor)));
			IActorRef readerActor = AccountActorSystem.ActorOf (Props.Create (() => new ReaderActor (parserActor)));

			readerActor.Tell (EnumsAndConstants.StartCommand);

			AccountActorSystem.WhenTerminated.Wait ();
		}
	}
}
