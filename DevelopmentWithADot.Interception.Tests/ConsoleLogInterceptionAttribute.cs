﻿using System;

namespace DevelopmentWithADot.Interception.Tests
{
	public class ConsoleLogInterceptionAttribute : InterceptionAttribute
	{
		public override void Invoke(InterceptionArgs arg)
		{
			Console.Out.WriteLine("Calling " + arg.Method);
		}
	}
}
