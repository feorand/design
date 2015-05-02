using System;

namespace FluentTask
{
	internal class Program
	{
		private static void Main()
		{


			var behaviour = new Behavior()
				.Say("Привет мир!")
				.UntilKeyPressed(b => b
					.Say("Ля-ля-ля!")
					.Say("Тру-лю-лю"))
				.Jump(JumpHeight.High)
				.UntilKeyPressed(b => b
					.Say("Aa-a-a-a-aaaaaa!!!")
					.Say("[набирает воздух в легкие]"))
				.Say("Ой!")
				.Delay(TimeSpan.FromSeconds(1))
				.Say("Кто здесь?!")
				.Delay(TimeSpan.FromMilliseconds(2000));

			behaviour.Execute();

            //var b = new Behavior()
            //.UntilKeyPressed(bb => bb.Say("1111").Say("2222"))
            //    .Say("Hello")
            //    .Say("World")
            //    .Delay(TimeSpan.FromSeconds(1))
            //    .Jump(JumpHeight.High);

            //b.Execute();

		}
	}
}