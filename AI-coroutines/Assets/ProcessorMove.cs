using Pixeye.Actors;


public class ProcessorMove : Processor, ITick
{
    
	private Group<ComponentMove> group_Entity;
	
	
	public override void HandleEvents()
	{
		foreach (ent entity in group_Entity.removed)
		{
			//var cMove = entity.ComponentMove();
		}

		foreach (ent entity in group_Entity.added)
		{
			//var cMove = entity.ComponentMove();
		}
	}

	
	public void Tick(float delta)
	{
		foreach (ent entity in group_Entity)
		{
			var cMove = entity.ComponentMove();
			entity.transform.Translate(cMove.direction * delta * 3f);
		}
        
		
		
	}

}
