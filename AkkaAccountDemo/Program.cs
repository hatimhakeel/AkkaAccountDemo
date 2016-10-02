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

			IActorRef writerActor = AccountActorSystem.ActorOf (Props.Create (() => new WriterActor ()), "writerActor");
			IActorRef depositActor = AccountActorSystem.ActorOf (Props.Create(() => new DepositActor(writerActor)), "depositActor");
			IActorRef withdrawActor = AccountActorSystem.ActorOf (Props.Create (() => new WithdrawActor(writerActor)), "withdrawActor");
			IActorRef parserActor = AccountActorSystem.ActorOf (Props.Create (() => new ParserActor (depositActor, withdrawActor, writerActor)), "parserActor");
			IActorRef readerActor = AccountActorSystem.ActorOf (Props.Create (() => new ReaderActor (parserActor)), "readerActor");

			readerActor.Tell (EnumsAndConstants.StartCommand);

			AccountActorSystem.WhenTerminated.Wait ();
		}
	}
}
