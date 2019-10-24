//  Project : ecs
// Contacts : Pix - ask@pixeye.games

using Pixeye.Actors;

namespace Pixeye.Source
{
	public class Tag : ITag
	{
		[TagField] public const int StateAlive = 0;
		[TagField] public const int Hero = 1;
		
		[TagField] public const int AI_Starter = 2;
		[TagField] public const int AI_Death = 3;
		
		
		
		[TagField] public const int AI_Walk = 4;
		[TagField] public const int AI_QQQ = 5;
		[TagField] public const int AI_WWW = 6;
	}
}