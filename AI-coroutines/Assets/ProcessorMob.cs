using Pixeye.Actors;
using Pixeye.Source;
using UnityEngine;


public class ProcessorMob : Processor, ITick
{
	private Group<ComponentMob> group_Mob;
	
	public override void HandleEvents()
	{

		foreach (ent entity in group_Mob.added)
		{
			//var cMove = entity.ComponentMove();
			entity.EnableBehaviour(Tag.AI_Starter);
		}
	}

	
	public void Tick(float delta)
	{
		foreach (ent entity in group_Mob)
		{
			//var cMove = entity.ComponentMove();
		}
		
	}


}
