using System;
using System.Text.RegularExpressions;
using Akka.Actor;

namespace AkkaAccountDemo
{
	public class ReaderActor : UntypedActor
	{
		private readonly IActorRef parserActor;

		public ReaderActor (IActorRef parserActor)
		{
			this.parserActor = parserActor;
		}

		protected override void OnReceive(object message) 
		{	
			var input = Console.ReadLine();

			if (!string.IsNullOrEmpty (input) && string.Equals (input, EnumsAndConstants.ExitCommand, StringComparison.OrdinalIgnoreCase)) 
			{
				Context.System.Terminate ();
				return;
			}

			parserActor.Tell (input);

			Self.Tell ("continue");
		}
	}
}

